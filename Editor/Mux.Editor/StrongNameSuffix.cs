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

using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Mux.Editor
{
    [StructLayout(LayoutKind.Auto)]
    internal readonly struct StrongNameSuffix : IEquatable<StrongNameSuffix>
    {
        public readonly byte[] publicKeyToken;
        public readonly Version version;

        public StrongNameSuffix(Mono.Cecil.AssemblyNameReference name)
        {
            this.publicKeyToken = name.PublicKeyToken;
            this.version = name.Version;
        }

        public bool Equals(StrongNameSuffix another)
        {
            return publicKeyToken.SequenceEqual(another.publicKeyToken) && version == another.version;
        }
    }
}
