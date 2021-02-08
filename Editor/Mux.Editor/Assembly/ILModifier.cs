// Copyright 2019 pixiv Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using Xamarin.Forms.Xaml;

namespace Mux.Editor.Assembly
{
    internal sealed class ILModifier : System.IDisposable
    {
        private readonly List<EmbeddedResource> _resources = new List<EmbeddedResource>();
        public readonly UnityEditor.Compilation.Assembly compilation;
        public readonly AssemblyDefinition definition;

        public ILModifier(UnityEditor.Compilation.Assembly compilation, AssemblyDefinition definition)
        {
            this.compilation = compilation;
            this.definition = definition;
        }

        public void Dispose()
        {
            foreach (var resource in _resources)
            {
                resource.GetResourceStream().Dispose();
            }
        }

        public bool Modify(ILXamarin xamarin, Dictionary<Slot, Type> types, IEnumerable<string> sources)
        {
            var modified = false;
            var assemblyIsToCompile = IsToCompile(true, definition);

            foreach (var module in definition.Modules)
            {
                var moduleIsToCompile = IsToCompile(assemblyIsToCompile, module);

                foreach (var type in types.ToArray())
                {
                    var ilType = module.GetType(type.Key.muxNamespace, type.Key.muxName);

                    if (ilType != null && Modify(xamarin, types, type.Key, type.Value, ilType, moduleIsToCompile, sources))
                    {
                        modified = true;
                    }
                }
            }

            return modified;
        }

        public void Recompile(ILXamarin xamarin, Dictionary<Slot, Type> types, IEnumerable<string> sources)
        {
            foreach (var module in definition.Modules)
            {
                foreach (var type in types)
                {
                    var ilType = module.GetType(type.Key.muxNamespace, type.Key.muxName);

                    if (ilType != null && ShouldRecompile(xamarin, type.Value, ilType, sources))
                    {
                        AssetDatabase.ImportAsset(compilation.sourceFiles[0], ImportAssetOptions.ForceUpdate);
                        return;
                    }
                }
            }
        }

        private static bool ShouldRecompile(ILXamarin xamarin, Type type, TypeDefinition ilType, IEnumerable<string> sources)
        {
            foreach (var nestedIlType in ilType.NestedTypes)
            {
                Type nestedType;
                var nestedSlot = new Slot { muxNamespace = nestedIlType.Namespace, muxName = nestedIlType.Name };

                if (type.nested.TryGetValue(nestedSlot, out nestedType) &&
                    ShouldRecompile(xamarin, nestedType, nestedIlType, sources))
                {
                    return true;
                }
            }

            if (type.asset == null || (sources != null && !sources.Contains(type.asset)))
            {
                return false;
            }

            var initializer = FindInitializeComponent(ilType);
            if (initializer == null)
            {
                return false;
            }

            return Validate(FindXamlResourceIdAttribute(xamarin, ilType), initializer);
        }

        private bool Modify(ILXamarin xamarin, Dictionary<Slot, Type> siblings, Slot slot, Type type, TypeDefinition ilType, bool compile, IEnumerable<string> sources)
        {
            var modified = false;

            foreach (var nestedIlType in ilType.NestedTypes)
            {
                Type nestedType;
                var nestedSlot = new Slot { muxNamespace = nestedIlType.Namespace, muxName = nestedIlType.Name };

                if (type.nested.TryGetValue(nestedSlot, out nestedType) &&
                    Modify(xamarin, type.nested, nestedSlot, nestedType, nestedIlType, compile, sources))
                {
                    modified = true;
                }
            }

            if (type.asset == null || (sources != null && !sources.Contains(type.asset)))
            {
                return modified;
            }

            var initializer = FindInitializeComponent(ilType);
            if (initializer == null)
            {
                return modified;
            }

            compile = IsToCompile(compile, ilType);

            var stringType = ilType.Module.TypeSystem.String;
            var typeType = new TypeReference("System", "Type", ilType.Module, ilType.Module.TypeSystem.CoreLibrary);
            var voidType = ilType.Module.TypeSystem.Void;

            var attribute = FindXamlResourceIdAttribute(xamarin, ilType);

            if (!Validate(attribute, initializer))
            {
                return modified;
            }

            try
            {
                UpdateOrAddResource(ilType.Module.Resources, type);
            }
            catch (DirectoryNotFoundException)
            {
                goto NotFound;
            }
            catch (FileNotFoundException)
            {
                goto NotFound;
            }

            if (compile)
            {
                initializer = ilType.Methods.FirstOrDefault(method => method.Name == "__InitComponentRuntime");
            }

            if (initializer == null)
            {
                initializer = new MethodDefinition("__InitComponentRuntime", MethodAttributes.Private, voidType);
                ilType.Methods.Add(initializer);
                FillInitializeComponent(initializer, type, typeType, xamarin);
                modified = true;
            }

            if (attribute == null)
            {
                attribute = new CustomAttribute(ilType.Module.ImportReference(xamarin.xamlResourceIdAttribute));
                ilType.Module.Assembly.CustomAttributes.Add(attribute);
                FillXamlResourceIdAttributeArguments(attribute.ConstructorArguments, type, ilType, stringType, typeType);
                modified = true;
            }

            return modified;

        NotFound:
            if (type.nested.Count > 0)
            {
                type.asset = null;
                siblings[slot] = type;
            }
            else
            {
                siblings.Remove(slot);
            }

            return modified;
        }

        public void Write()
        {
            definition.Write(new WriterParameters { WriteSymbols = true });
        }

        private static MethodDefinition FindInitializeComponent(TypeDefinition ilType)
        {
            foreach (var method in ilType.Methods)
            {
                if (!method.CustomAttributes.Any(candidate =>
                    candidate.AttributeType.Namespace == "Mux" &&
                    candidate.AttributeType.Name == "XamlImportAttribute"))
                {
                    continue;
                }

                if (method.Name == "InitializeComponent")
                {
                    return method;
                }

                UnityEngine.Debug.LogWarning($"{method} has Mux.XamlImportAttribute but its name is not InitializeComponent. Ignoring.");
            }

            return null;
        }

        private static CustomAttribute FindXamlResourceIdAttribute(ILXamarin xamarin, TypeDefinition type)
        {
            var stringType = type.Module.TypeSystem.String;
            var typeType = new TypeReference("System", "Type", type.Module, type.Module.TypeSystem.CoreLibrary);
            var voidType = type.Module.TypeSystem.Void;

            foreach (var attribute in type.Module.Assembly.CustomAttributes)
            {
                if (
                    FullNameEquals(attribute.AttributeType, xamarin.xamlResourceIdAttribute.DeclaringType) &&
                    attribute.Constructor.Parameters.Count == 3 &&
                    FullNameEquals(attribute.Constructor.Parameters[0].ParameterType, stringType) &&
                    FullNameEquals(attribute.Constructor.Parameters[1].ParameterType, stringType) &&
                    FullNameEquals(attribute.Constructor.Parameters[2].ParameterType, typeType) &&
                    attribute.ConstructorArguments.Count == 3 &&
                    FullNameEquals(attribute.ConstructorArguments[2].Type, typeType) &&
                    FullNameEquals((TypeReference)attribute.ConstructorArguments[2].Value, type))
                {
                    return attribute;
                }
            }

            return null;
        }

        private static bool Validate(CustomAttribute xamlResourceIdAttribute, MethodDefinition initializer)
        {
            if (xamlResourceIdAttribute == null && (initializer.IsAbstract || initializer.RVA != 0))
            {
                UnityEngine.Debug.LogWarning($"{initializer} has Mux.XamlImportAttribute but it is abstract, or its implementation is already provided. Skipping.");
                return false;
            }

            return true;
        }

        private void UpdateOrAddResource(Collection<Resource> resources, Type type)
        {
            var stream = new FileStream(type.asset, FileMode.Open, FileAccess.Read);

            try
            {
                var resource = new EmbeddedResource(type.asset, ManifestResourceAttributes.Private, stream);
                _resources.Add(resource);

                for (var index = 0; index < resources.Count; index++)
                {
                    if (resources[index].Name == resource.Name)
                    {
                        resources[index] = resource;
                        return;
                    }
                }

                resources.Add(resource);
            }
            catch (System.Exception)
            {
                stream.Dispose();
                throw;
            }
        }

        public static bool IsToCompile(bool fallback, ICustomAttributeProvider provider)
        {
            foreach (var attribute in provider.CustomAttributes)
            {
                var type = attribute.AttributeType;

                if (type.Namespace == "Xamarin.Forms.Xaml" && type.Name == "XamlCompilationAttribute")
                {
                    var options = (XamlCompilationOptions)attribute.ConstructorArguments[0].Value;

                    if ((options & XamlCompilationOptions.Compile) == XamlCompilationOptions.Compile)
                    {
                        return true;
                    }

                    if ((options & XamlCompilationOptions.Skip) == XamlCompilationOptions.Skip)
                    {
                        return false;
                    }

                    break;
                }
            }

            return fallback;
        }

        private static bool FullNameEquals(TypeReference t, TypeReference u)
        {
            while (t.Namespace == u.Namespace && t.Name == u.Name)
            {
                t = t.DeclaringType;
                u = u.DeclaringType;

                if (t == u)
                {
                    return true;
                }
            }

            return false;
        }

        private static void FillInitializeComponent(MethodDefinition initializer, Type type, TypeReference typeType, ILXamarin xamarin)
        {
            var module = initializer.Module;

            var runtimeTypeHandle = new TypeReference("System", "RuntimeTypeHandle", module, module.TypeSystem.CoreLibrary);
            runtimeTypeHandle.IsValueType = true;

            var getTypeFromHandle = new MethodReference("GetTypeFromHandle", typeType, typeType);
            getTypeFromHandle.Parameters.Add(new ParameterDefinition(runtimeTypeHandle));

            var loadFromXamlInstance = new GenericInstanceMethod(xamarin.loadFromXaml);
            loadFromXamlInstance.GenericArguments.Add(initializer.DeclaringType);

            var fields = new List<FieldDefinition>();
            TypeReference baseType = initializer.DeclaringType;

            while (baseType != null)
            {
                if (baseType.Namespace == "Xamarin.Forms" && baseType.Name == "BindableObject")
                {
                    foreach (var name in type.names)
                    {
                        foreach (var field in initializer.DeclaringType.Fields)
                        {
                            if (field.Name == name)
                            {
                                fields.Add(field);
                                break;
                            }
                        }
                    }

                    break;
                }

                baseType = baseType.Resolve().BaseType;
            }

            initializer.Body = new MethodBody(initializer);
            var il = initializer.Body.GetILProcessor();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldtoken, initializer.DeclaringType);
            il.Emit(OpCodes.Call, module.ImportReference(getTypeFromHandle));
            il.Emit(OpCodes.Call, module.ImportReference(loadFromXamlInstance));

            if (fields.Count > 0)
            {
                var getNameScope = module.ImportReference(xamarin.getNameScope);
                var iNameScope = module.ImportReference(xamarin.getNameScope.ReturnType);
                var findByName = module.ImportReference(xamarin.findByName);
                var loc = new VariableDefinition(iNameScope);
                initializer.Body.Variables.Add(loc);

                il.Emit(OpCodes.Call, getNameScope);
                il.Emit(OpCodes.Stloc_0);

                foreach (var field in fields)
                {
                    var fieldType = field.FieldType;

                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldloc_0);
                    il.Emit(OpCodes.Ldstr, field.Name);
                    il.Emit(OpCodes.Callvirt, findByName);

                    if (fieldType.Namespace != "System" || fieldType.Name != "Object")
                    {
                        il.Emit(fieldType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, fieldType);
                    }

                    il.Emit(OpCodes.Stfld, field);
                }
            }
            else
            {
                il.Emit(OpCodes.Pop);
            }

            il.Emit(OpCodes.Ret);
        }

        private static void FillXamlResourceIdAttributeArguments(Collection<CustomAttributeArgument> arguments, Type type, TypeReference ilType, TypeReference stringType, TypeReference typeType)
        {
            arguments.Add(new CustomAttributeArgument(stringType, type.asset));
            arguments.Add(new CustomAttributeArgument(stringType, type.asset));
            arguments.Add(new CustomAttributeArgument(typeType, ilType));
        }
    }
}
