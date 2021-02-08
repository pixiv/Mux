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

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using static System.Collections.Specialized.NotifyCollectionChangedAction;

namespace Mux.Tests.Markup
{
    [TestFixture]
    public static class Gradient
    {
        private sealed class ListeningList<T> : List<T>, INotifyCollectionChanged
        {
            public event NotifyCollectionChangedEventHandler CollectionChanged;

            public void AssertNotNull()
            {
                Assert.NotNull(CollectionChanged);
            }

            public void AssertNull()
            {
                Assert.Null(CollectionChanged);
            }

            public void Invoke()
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(Reset));
            }

            public void Replace(int index, T item)
            {
                var replace = NotifyCollectionChangedAction.Replace;
                var args = new NotifyCollectionChangedEventArgs(replace, item, this[index], index);
                this[index] = item;
                CollectionChanged?.Invoke(this, args);
            }
        }

        [Test]
        public static void AddAlphaKey()
        {
            var keys = new[]
            {
                new UnityEngine.GradientAlphaKey(0.1f, 0),
                new UnityEngine.GradientAlphaKey(0.2f, 1)
            };

            var gradient = new Mux.Markup.Gradient { keys[0], keys[1] };

            CollectionAssert.AreEqual(keys, gradient);
            CollectionAssert.AreEqual(keys, gradient.Body.alphaKeys);
        }

        [Test]
        public static void AddColorKey()
        {
            var colors = new[]
            {
                new UnityEngine.Color(0.1f, 0.2f, 0.3f),
                new UnityEngine.Color(0.4f, 0.5f, 0.6f)
            };

            var keys = new[]
            {
                new UnityEngine.GradientColorKey(colors[0], 0),
                new UnityEngine.GradientColorKey(colors[1], 1),
            };

            var gradient = new Mux.Markup.Gradient { keys[0], keys[1] };

            CollectionAssert.AreEqual(keys, gradient);
            CollectionAssert.AreEqual(keys, gradient.Body.colorKeys);
        }

        [Test]
        public static void SetNotNotifyingAlphaKeys()
        {
            var keys = new[]
            {
                new UnityEngine.GradientAlphaKey(0.1f, 0),
                new UnityEngine.GradientAlphaKey(0.2f, 1)
            };

            var gradient = new Mux.Markup.Gradient { AlphaKeys = keys };

            Assert.AreEqual(keys, gradient.AlphaKeys);
            CollectionAssert.AreEqual(keys, gradient);
            CollectionAssert.AreEqual(keys, gradient.Body.alphaKeys);
        }

        [Test]
        public static void SetNotNotifyingColorKeys()
        {
            var colors = new[]
            {
                new UnityEngine.Color(0.1f, 0.2f, 0.3f),
                new UnityEngine.Color(0.4f, 0.5f, 0.6f)
            };

            var keys = new[]
            {
                new UnityEngine.GradientColorKey(colors[0], 0),
                new UnityEngine.GradientColorKey(colors[1], 1)
            };

            var gradient = new Mux.Markup.Gradient { ColorKeys = keys };

            Assert.AreEqual(keys, gradient.ColorKeys);
            CollectionAssert.AreEqual(keys, gradient);
            CollectionAssert.AreEqual(keys, gradient.Body.colorKeys);
        }

        [Test]
        public static void SetNotifyingAlphaKeys()
        {
            var keys = new ListeningList<UnityEngine.GradientAlphaKey>();
            keys.Add(new UnityEngine.GradientAlphaKey(0.1f, 0));
            keys.Add(new UnityEngine.GradientAlphaKey(0.2f, 1));
            var gradient = new Mux.Markup.Gradient { AlphaKeys = keys };

            keys.AssertNotNull();
            Assert.AreEqual(keys, gradient.AlphaKeys);
            CollectionAssert.AreEqual(keys, gradient);
            CollectionAssert.AreEqual(keys, gradient.Body.alphaKeys);

            keys.Replace(0, new UnityEngine.GradientAlphaKey(0.3f, 0));
            CollectionAssert.AreEqual(keys, gradient);
            CollectionAssert.AreEqual(keys, gradient.Body.alphaKeys);

            gradient.AlphaKeys = null;
            keys.AssertNull();
        }

        [Test]
        public static void SetNotifyingColorKeys()
        {
            var colors = new[]
            {
                new UnityEngine.Color(0.1f, 0.2f, 0.3f),
                new UnityEngine.Color(0.4f, 0.5f, 0.6f),
                new UnityEngine.Color(0.7f, 0.8f, 0.9f)
            };

            var keys = new ListeningList<UnityEngine.GradientColorKey>();
            keys.Add(new UnityEngine.GradientColorKey(colors[0], 0));
            keys.Add(new UnityEngine.GradientColorKey(colors[1], 1));
            var gradient = new Mux.Markup.Gradient { ColorKeys = keys };

            keys.AssertNotNull();
            Assert.AreEqual(keys, gradient.ColorKeys);
            CollectionAssert.AreEqual(keys, gradient);
            CollectionAssert.AreEqual(keys, gradient.Body.colorKeys);

            keys.Replace(0, new UnityEngine.GradientColorKey(colors[2], 0));
            CollectionAssert.AreEqual(keys, gradient);
            CollectionAssert.AreEqual(keys, gradient.Body.colorKeys);

            gradient.ColorKeys = null;
            keys.AssertNull();
        }

        [Test]
        public static void SetNullAlphaKeys()
        {
            var gradient = new Mux.Markup.Gradient { AlphaKeys = null };
            Assert.Null(gradient.AlphaKeys);
        }

        [Test]
        public static void SetNullColorKeys()
        {
            var gradient = new Mux.Markup.Gradient { ColorKeys = null };
            Assert.Null(gradient.ColorKeys);
        }
    }
}
