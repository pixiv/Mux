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
using System.Linq;

namespace Mux.Editor.Assembly
{
    internal sealed class Resolver : IAssemblyResolver
    {
        private static KeyValuePair<StrongNameSuffix, AssemblyDefinition> SelectGreaterVersion(KeyValuePair<StrongNameSuffix, AssemblyDefinition> s, KeyValuePair<StrongNameSuffix, AssemblyDefinition> t)
        {
            return s.Key.version < t.Key.version ? t : s;
        }

        private readonly HashSet<string> _paths = new HashSet<string>();
        private readonly Dictionary<string, Dictionary<string, Dictionary<StrongNameSuffix, AssemblyDefinition>>> _nameMap =
            new Dictionary<string, Dictionary<string, Dictionary<StrongNameSuffix, AssemblyDefinition>>>();

        public AssemblyDefinition Add(string path, bool readWrite)
        {
            AssemblyDefinition assembly;
            Dictionary<string, Dictionary<StrongNameSuffix, AssemblyDefinition>> cultureMap;
            Dictionary<StrongNameSuffix, AssemblyDefinition> suffixMap;

            if (_paths.Contains(path))
            {
                return null;
            }

            try
            {
                var parameters = new ReaderParameters();
                parameters.AssemblyResolver = this;
                parameters.ReadSymbols = readWrite;
                parameters.ReadWrite = readWrite;
                assembly = AssemblyDefinition.ReadAssembly(path, parameters);
            }
            catch (System.IO.FileNotFoundException)
            {
                return null;
            }

            var suffix = new StrongNameSuffix(assembly.Name);

            if (_nameMap.TryGetValue(assembly.Name.Name, out cultureMap))
            {
                if (cultureMap.TryGetValue(assembly.Name.Culture, out suffixMap))
                {
                    if (suffixMap.ContainsKey(suffix))
                    {
                        assembly.Dispose();
                        return null;
                    }
                }
                else
                {
                    suffixMap = new Dictionary<StrongNameSuffix, AssemblyDefinition>();
                    cultureMap[assembly.Name.Culture] = suffixMap;
                }
            }
            else
            {
                suffixMap = new Dictionary<StrongNameSuffix, AssemblyDefinition>();
                cultureMap = new Dictionary<string, Dictionary<StrongNameSuffix, AssemblyDefinition>>();
                cultureMap[assembly.Name.Culture] = suffixMap;
                _nameMap[assembly.Name.Name] = cultureMap;
            }

            _paths.Add(path);
            suffixMap[suffix] = assembly;

            return assembly;
        }

        public void DisposeAssembly(string path, AssemblyDefinition assembly)
        {
            assembly.Dispose();
            _paths.Remove(path);
            _nameMap[assembly.Name.Name][assembly.Name.Culture].Remove(new StrongNameSuffix(assembly.Name));
        }

        public AssemblyDefinition Resolve(AssemblyNameReference name)
        {
            if (!_nameMap.TryGetValue(name.Name, out var cultureMap))
            {
                return null;
            }

            if (name.Name == "mscorlib" || string.IsNullOrEmpty(name.Culture))
            {
                return cultureMap.Values
                    .Select(s => s.Aggregate(SelectGreaterVersion))
                    .Aggregate(SelectGreaterVersion).Value;
            }

            if (!cultureMap.TryGetValue(name.Culture, out var versionMap))
            {
                return null;
            }

            if (name.PublicKeyToken == null || name.PublicKeyToken.Length <= 0)
            {
                return versionMap.Aggregate(SelectGreaterVersion).Value;
            }

            if (!versionMap.TryGetValue(new StrongNameSuffix(name), out var assembly))
            {
                return null;
            }

            return assembly;
        }

        public AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            foreach (var cultureMap in _nameMap.Values)
            {
                foreach (var suffixMap in cultureMap.Values)
                {
                    foreach (var assembly in suffixMap.Values)
                    {
                        assembly.Dispose();
                    }

                    suffixMap.Clear();
                }

                cultureMap.Clear();
            }

            _paths.Clear();
            _nameMap.Clear();
        }
    }
}
