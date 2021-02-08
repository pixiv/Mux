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

using Microsoft.Build.Framework;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Xamarin.Forms.Build.Tasks;

namespace Mux.Editor.Assembly
{
    internal partial class Modifier
    {
        private Dictionary<Slot, Type> _types = new Dictionary<Slot, Type>();
        public HashSet<string> pendings = new HashSet<string>();

        public Modifier()
        {
            var assets = AssetDatabase.FindAssets("t:DefaultAsset").Select(AssetDatabase.GUIDToAssetPath);
            Import(assets);
        }

        public void Modify(IEnumerable<UnityEditor.Compilation.Assembly> lockedAssemblies, IEnumerable<string> sources = null)
        {
            var modifiers = new List<ILModifier>();

            EditorApplication.LockReloadAssemblies();

            try
            {
                using (var resolver = new Resolver())
                {
                    var xamarin = new ILXamarin(resolver);

                    foreach (var assembly in lockedAssemblies)
                    {
                        var definition = resolver.Add(assembly.outputPath, true);

                        if (definition != null)
                        {
                            modifiers.Add(new ILModifier(assembly, definition));
                        }
                    }

                    var engine = new Build.BuildEngine();
                    var logger = new Build.Logger();

                    logger.Verbosity = Settings.Verbosity;
                    logger.Initialize(engine);

                    while (modifiers.Count > 0)
                    {
                        var modifier = modifiers.Last();

                        try
                        {
                            foreach (var reference in modifier.compilation.allReferences)
                            {
                                resolver.Add(reference, false);
                            }

                            var modified = modifier.Modify(xamarin, _types, sources);

                            if (modified)
                            {
                                modifier.Write();
                            }

                            if (modified)
                            {
                                resolver.DisposeAssembly(modifier.compilation.outputPath, modifier.definition);

                                var task = new XamlCTask();
                                task.BuildEngine = engine;
                                task.Assembly = modifier.compilation.outputPath;
                                task.DebugSymbols = Settings.DebugSymbols.value;
                                task.DebugType = "portable";
                                task.OptimizeIL = Settings.OptimizeIL.value;
                                task.CompileByDefault = true;
                                task.DefaultAssemblyResolver = resolver;
                                engine.ProjectFileOfTaskNode = task.Assembly;
                                task.Execute();
                            }
                        }
                        catch (Exception exception)
                        {
                            Debug.LogException(exception);
                        }
                        finally
                        {
                            modifiers.RemoveAt(modifiers.Count - 1);
                            modifier.Dispose();
                        }
                    }
                }
            }
            finally
            {
                EditorApplication.UnlockReloadAssemblies();

                foreach (var modifier in modifiers)
                {
                    modifier.Dispose();
                }
            }
        }

        public void Import(IEnumerable<string> assets)
        {
            Import(assets, _types);
        }

        private static void Import(IEnumerable<string> assets, Dictionary<Slot, Type> xamlTypes)
        {
            foreach (var asset in assets)
            {
                Slot lastSlot;
                Type xamlType;

                try
                {
                    if (!asset.EndsWith(".xaml", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var xaml = Xaml.Parse(asset);
                    var slots = Slot.Parse(xaml.classFullName).ToArray();
                    lastSlot = slots.Last();

                    foreach (var xamlSlot in slots.Take(slots.Length - 1))
                    {
                        if (!xamlTypes.TryGetValue(xamlSlot, out xamlType))
                        {
                            xamlType.nested = new Dictionary<Slot, Type>();
                            xamlTypes[xamlSlot] = xamlType;
                        }

                        xamlTypes = xamlType.nested;
                    }

                    if (!xamlTypes.TryGetValue(lastSlot, out xamlType))
                    {
                        xamlType.nested = new Dictionary<Slot, Type>();
                    }

                    xamlType.names = xaml.names;
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                    continue;
                }

                xamlType.asset = asset;
                xamlTypes[lastSlot] = xamlType;
            }
        }

        public void Recompile(IEnumerable<UnityEditor.Compilation.Assembly> lockedAssemblies, IEnumerable<string> sources)
        {
            using (var resolver = new Resolver())
            {
                var xamarin = new ILXamarin(resolver);

                foreach (var assembly in lockedAssemblies)
                {
                    using (var definition = AssemblyDefinition.ReadAssembly(assembly.outputPath))
                    {
                        var modifier = new ILModifier(assembly, definition);
                        modifier.Recompile(xamarin, _types, sources);
                    }
                }
            }
        }

        public void RecompileForPendings(IEnumerable<UnityEditor.Compilation.Assembly> lockedAssemblies)
        {
            Recompile(lockedAssemblies, pendings);
            pendings = new HashSet<string>();
        }
    }
}
