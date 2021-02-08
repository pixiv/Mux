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
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Mux.Tests.Markup
{
    [TestFixture]
    public static class Transforms
    {
        internal sealed class Reloadable : Mux.Markup.Transform, Mux.Markup.IReloadable
        {
            public bool reloaded;

            public void Reload()
            {
                reloaded = true;
            }
        }

        [Test]
        public static void Unreloadable()
        {
            var target = new Mux.Markup.Transform();

            try
            {
                var reference = new WeakReference<Mux.Markup.IInternalTransform>(target);

                Mux.Markup.Transforms.Add("path", reference);
                Mux.Markup.Transforms.Reload(new[] { "path" });
                LogAssert.Expect(LogType.Log, "Reloadable Mux ancestor not found for Mux.Markup.Transform.");
                LogAssert.NoUnexpectedReceived();

                Mux.Markup.Transforms.Remove("path", reference);
                Mux.Markup.Transforms.Reload(new[] { "path" });
            }
            finally
            {
                target.Destroy();
            }
        }

        [Test]
        public static void ReloadableParent()
        {
            var parent = new Reloadable();
            var child = new Mux.Markup.Transform();

            parent.Add(child);

            try
            {
                var reference = new WeakReference<Mux.Markup.IInternalTransform>(child);

                Mux.Markup.Transforms.Add("path", reference);
                Mux.Markup.Transforms.Reload(new[] { "path" });
                Assert.IsEmpty(parent);
                Assert.True(parent.reloaded);
                LogAssert.Expect(LogType.Log, "Reloaded Mux");
                LogAssert.NoUnexpectedReceived();
                parent.reloaded = false;

                Mux.Markup.Transforms.Remove("path", reference);
                Mux.Markup.Transforms.Reload(new[] { "path" });
                Assert.False(parent.reloaded);
            }
            finally
            {
                parent.Destroy();
            }
        }

        [Test]
        public static void AddRemoveGetTwo()
        {
            var nodes = new[] { new Reloadable(), new Reloadable() };

            try
            {
                var references =
                    new WeakReference<Mux.Markup.IInternalTransform>[nodes.Length];

                for (var index = 0; index < nodes.Length; index++)
                {
                    references[index] =
                        new WeakReference<Mux.Markup.IInternalTransform>(nodes[index]);
                }

                Mux.Markup.Transforms.Add("path", references[0]);
                Mux.Markup.Transforms.Reload(new[] { "path" });
                Assert.True(nodes[0].reloaded);
                LogAssert.Expect(LogType.Log, "Reloaded Mux");
                nodes[0].reloaded = false;

                Mux.Markup.Transforms.Add("path", references[1]);
                Mux.Markup.Transforms.Reload(new[] { "path" });
                Assert.True(nodes[0].reloaded);
                Assert.True(nodes[1].reloaded);
                LogAssert.Expect(LogType.Log, "Reloaded Mux");
                nodes[0].reloaded = false;
                nodes[1].reloaded = false;

                Mux.Markup.Transforms.Remove("path", references[0]);
                Mux.Markup.Transforms.Reload(new[] { "path" });
                Assert.False(nodes[0].reloaded);
                Assert.True(nodes[1].reloaded);
                LogAssert.Expect(LogType.Log, "Reloaded Mux");
                nodes[1].reloaded = false;

                Mux.Markup.Transforms.Remove("path", references[1]);
                Mux.Markup.Transforms.Reload(new[] { "path" });
                Assert.False(nodes[0].reloaded);
                Assert.False(nodes[1].reloaded);
            }
            finally
            {
                foreach (var node in nodes)
                {
                    node.Destroy();
                }
            }

            LogAssert.NoUnexpectedReceived();
        }
    }
}
