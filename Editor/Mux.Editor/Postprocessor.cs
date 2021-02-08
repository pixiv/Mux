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

using Mux.Editor.Assembly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using Xamarin.Forms.Internals;

namespace Mux.Editor
{
    /// <summary>A class to process managed code and XAMLs.</summary>
    public sealed class Postprocessor : AssetPostprocessor
    {
        private static Func<ResourceLoader.ResourceLoadingQuery, ResourceLoader.ResourceLoadingResponse> s_resourceProviderValue = (query) =>
        {
            if (s_modifier.pendings.Contains(query.ResourcePath))
            {
                using (var reader = new System.IO.StreamReader(query.ResourcePath))
                {
                    return new ResourceLoader.ResourceLoadingResponse { ResourceContent = reader.ReadToEnd() };
                }
            }

            return null;
        };

        private static Modifier s_modifier = new Modifier();

        private static void RenewModifier()
        {
            s_modifier = new Modifier();
        }

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.playModeStateChanged += state =>
            {
                ResourceLoader.ResourceProvider2 = null;
                s_modifier.RecompileForPendings(GetLockedAssemblies());
            };

            CompilationPipeline.assemblyCompilationFinished += (assemblyOutputPath, message) =>
            {
                RenewModifier();
                s_modifier.Modify(GetLockedAssembliesByPath(assemblyOutputPath));
            };

            Settings.Backend.afterSettingsSaved += () => s_modifier.Recompile(GetLockedAssemblies(), null);

            s_modifier.Modify(GetLockedAssemblies());
        }


        private static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] moved, string[] movedFrom)
        {
            var assets = imported.Concat(moved);
            var sources = assets;

            foreach (var asset in assets)
            {
                if (asset.StartsWith("Packages/com.pixiv.mux/"))
                {
                    sources = null;
                }
            }

            s_modifier.Import(imported);

            if (sources != null && Settings.EnableLiveReload && Application.isPlaying)
            {
                ResourceLoader.ResourceProvider2 = s_resourceProviderValue;
                s_modifier.pendings.UnionWith(sources);
                Markup.Transforms.Reload(sources);
            }
            else
            {
                s_modifier.Recompile(GetLockedAssemblies(), sources);
            }
        }

        private static IEnumerable<UnityEditor.Compilation.Assembly> GetLockedAssembliesByPath(string path)
        {
            var assemblies = CompilationPipeline.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                if (assembly.outputPath == path)
                {
                    yield return assembly;
                }
            }
        }

        private static IEnumerable<UnityEditor.Compilation.Assembly> GetLockedAssemblies()
        {
            var assemblies = CompilationPipeline.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                yield return assembly;
            }
        }

        /// <summary>A method to recompile all xamls.</summary>
        /// <remarks>It is useful in automated tests where postprocessors are disabled.</remarks>
        [MenuItem("Tools/[Mux] Recompile")]
        public static void Recompile()
        {
            RenewModifier();
            s_modifier.Recompile(GetLockedAssemblies(), null);
        }
    }
}
