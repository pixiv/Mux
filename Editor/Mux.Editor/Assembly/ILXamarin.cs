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
using System;
using System.Collections.Generic;
using System.IO;

namespace Mux.Editor.Assembly
{
    internal readonly struct ILXamarin
    {
        public readonly MethodDefinition xamlResourceIdAttribute;
        public readonly MethodDefinition loadFromXaml;
        public readonly MethodDefinition getNameScope;
        public readonly MethodDefinition findByName;

        public ILXamarin(Resolver resolver)
        {
            var core = resolver.Add(Path.GetFullPath("Packages/com.pixiv.mux/Runtime/Plugins/Xamarin.Forms.Core.dll"), false).Modules;
            xamlResourceIdAttribute = FindXamlResourceIdAttribute(core);
            getNameScope = FindGetNameScope(core);
            findByName = FindFindByName(core);

            var xaml = resolver.Add(Path.GetFullPath("Packages/com.pixiv.mux/Runtime/Plugins/Xamarin.Forms.Xaml.dll"), false).Modules;
            loadFromXaml = FindLoadFromXaml(xaml);
        }

        private static MethodDefinition FindXamlResourceIdAttribute(IEnumerable<ModuleDefinition> modules)
        {
            foreach (var module in modules)
            {
                var type = module.GetType("Xamarin.Forms.Xaml", "XamlResourceIdAttribute");
                if (type == null)
                {
                    continue;
                }

                foreach (var method in type.Methods)
                {
                    if (!method.IsPrivate &&
                        method.IsConstructor &&
                        method.Parameters.Count == 3 &&
                        method.Parameters[0].ParameterType.Namespace == "System" &&
                        method.Parameters[0].ParameterType.Name == "String" &&
                        method.Parameters[1].ParameterType.Namespace == "System" &&
                        method.Parameters[1].ParameterType.Name == "String" &&
                        method.Parameters[2].ParameterType.Namespace == "System" &&
                        method.Parameters[2].ParameterType.Name == "Type")
                    {
                        return method;
                    }
                }
            }

            throw new Exception("Xamarin.Forms.Xaml.XamlResourceIdAttribute not found");
        }

        private static MethodDefinition FindGetNameScope(IEnumerable<ModuleDefinition> modules)
        {
            foreach (var module in modules)
            {
                var type = module.GetType("Xamarin.Forms.Internals", "NameScope");
                if (type == null)
                {
                    continue;
                }

                foreach (var method in type.Methods)
                {
                    if (method.Name == "GetNameScope" &&
                        !method.IsPrivate &&
                        method.IsStatic &&
                        method.Parameters.Count == 1 &&
                        method.Parameters[0].ParameterType.Namespace == "Xamarin.Forms" &&
                        method.Parameters[0].ParameterType.Name == "BindableObject")
                    {
                        return method;
                    }
                }
            }

            throw new Exception("Xamarin.Forms.Internals.NameScope.GetNameScope not found");
        }

        private static MethodDefinition FindFindByName(IEnumerable<ModuleDefinition> modules)
        {
            foreach (var module in modules)
            {
                var type = module.GetType("Xamarin.Forms.Internals", "INameScope");
                if (type == null)
                {
                    continue;
                }

                foreach (var method in type.Methods)
                {
                    if (method.Name == "FindByName" &&
                        method.Parameters.Count == 1 &&
                        method.Parameters[0].ParameterType.Namespace == "System" &&
                        method.Parameters[0].ParameterType.Name == "String")
                    {
                        return method;
                    }
                }
            }

            throw new Exception("Xamarin.Forms.Internals.INameScope.FindByName not found");
        }

        private static MethodDefinition FindLoadFromXaml(IEnumerable<ModuleDefinition> modules)
        {
            foreach (var module in modules)
            {
                var type = module.GetType("Xamarin.Forms.Xaml", "Extensions");
                if (type == null)
                {
                    continue;
                }

                foreach (var method in type.Methods)
                {
                    if (method.Name == "LoadFromXaml" &&
                        !method.IsPrivate &&
                        method.IsStatic &&
                        method.GenericParameters.Count == 1 &&
                        method.Parameters.Count == 2 &&
                        (method.Parameters[0].ParameterType as GenericParameter)?.Position == 0 &&
                        method.Parameters[1].ParameterType.Namespace == "System" &&
                        method.Parameters[1].ParameterType.Name == "Type")
                    {
                        return method;
                    }
                }
            }

            throw new Exception("Xamarin.Forms.Xaml.Extensions.LoadFromXaml not found");
        }
    }
}
