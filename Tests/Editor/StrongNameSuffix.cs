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
using NUnit.Framework;
using System;

namespace Mux.Editor.Tests
{
    [TestFixture]
    public static class StrongNameSuffix
    {
        [TestCase(0, 1, ExpectedResult = false)]
        [TestCase(1, 0, ExpectedResult = false)]
        [TestCase(0, 0, ExpectedResult = true)]
        public static bool Equals(byte publicKeyToken, int minor)
        {
            var s = new Mux.Editor.StrongNameSuffix(
                new AssemblyNameReference("Mux", new Version(0, 0))
                { PublicKeyToken = new byte[] { 0 } });

            var t = new Mux.Editor.StrongNameSuffix(
                new AssemblyNameReference("Mux", new Version(0, minor))
                { PublicKeyToken = new byte[] { publicKeyToken } });

            return s.Equals(t);
        }
    }
}
