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
using System.Threading;
using Xamarin.Forms;

namespace Mux.Tests.Markup
{
    [TestFixture]
    public static class Color
    {
        [Test]
        public static void ProvideValue()
        {
            var instance = new Mux.Markup.Color { R = 0, G = 0.1f, B = 0.2f };
            var value = instance.ProvideValue(null);

            Assert.AreEqual(new UnityEngine.Color(0, 0.1f, 0.2f, 1), value);
        }
    }

    [TestFixture]
    public static class Color32
    {
        [Test]
        public static void ProvideValue()
        {
            var instance = new Mux.Markup.Color32 { R = 0, G = 1, B = 2 };
            var value = instance.ProvideValue(null);

            Assert.AreEqual(new UnityEngine.Color32(0, 1, 2, 255), value);
        }
    }

    [TestFixture]
    public static class ResourceRequestAndBaseResourceProvider
    {
        private sealed class ResourceProvider : Mux.Markup.BaseResourceProvider<UnityEngine.Object>
        {
            private readonly SynchronizationContext _mainThread;
            private readonly UnityEngine.ResourceRequest _request;

            public ResourceProvider(SynchronizationContext mainThread, UnityEngine.ResourceRequest request)
            {
                _mainThread = mainThread;
                _request = request;
            }

            internal override UnityEngine.ResourceRequest ProvideRequest()
            {
                Assert.AreEqual(_mainThread, SynchronizationContext.Current);
                return _request;
            }
        }

        private sealed class Source
        {
            public UnityEngine.Object Asset { get; set; }
            public bool IsDone { get; set; }
            public UnityEngine.ResourceRequest Request { get; set; }
        }

        [Test]
        public static void ProvideValueWithIsDone()
        {
            var request = UnityEngine.Resources.LoadAsync("Mux/Tests");
            var provider = new ResourceProvider(Mux.Forms.mainThread, request);
            var source = new Source { IsDone = true };

            provider.IsDone = new Binding("IsDone", BindingMode.OneWayToSource, null, null, null, source);
            SynchronizationContextWaiter.Execute(() => provider.ProvideValue(null));
            Assert.IsFalse(source.IsDone);
        }

        [Test]
        public static void ProvideValueWithRequest()
        {
            var request = UnityEngine.Resources.LoadAsync("Mux/Tests");
            var provider = new ResourceProvider(Mux.Forms.mainThread, request);
            var source = new Source();

            provider.Request = new Binding("Request", BindingMode.OneWayToSource, null, null, null, source);
            SynchronizationContextWaiter.Execute(() => provider.ProvideValue(null));
            Assert.AreEqual(request, source.Request);
        }
    }

    [TestFixture]
    public static class ResourceProvider
    {
        [Test]
        public static void ProvideRequest()
        {
            var provider = new Mux.Markup.ResourceProvider { Path = "Mux/Tests" };
            Assert.Null(provider.ProvideRequest().asset);
        }
    }

    [TestFixture]
    public static class GenericResourceProvider
    {
        [Test]
        public static void ProvideRequest()
        {
            var provider = new Mux.Markup.ResourceProvider<UnityEngine.GameObject>();
            provider.Path = "Mux/Tests";
            Assert.Null(provider.ProvideRequest().asset);
        }
    }

    [TestFixture]
    public static class Vector2
    {
        [Test]
        public static void ProvideValue()
        {
            var vector2 = new Mux.Markup.Vector2 { X = 0, Y = 1 };
            Assert.AreEqual(new UnityEngine.Vector2(0, 1), vector2.ProvideValue(null));
        }
    }

    [TestFixture]
    public static class Vector3
    {
        [Test]
        public static void ProvideValue()
        {
            var vector3 = new Mux.Markup.Vector3 { X = 0, Y = 1, Z = 2 };
            Assert.AreEqual(new UnityEngine.Vector3(0, 1, 2), vector3.ProvideValue(null));
        }
    }

    [TestFixture]
    public static class Vector4
    {
        [Test]
        public static void ProvideValue()
        {
            var vector4 = new Mux.Markup.Vector4 { X = 0, Y = 1, Z = 2, W = 3 };
            Assert.AreEqual(new UnityEngine.Vector4(0, 1, 2, 3), vector4.ProvideValue(null));
        }
    }

    [TestFixture]
    public static class Vector3Int
    {
        [Test]
        public static void ProvideValue()
        {
            var vector3 = new Mux.Markup.Vector3Int { X = 0, Y = 1, Z = 2 };
            Assert.AreEqual(new UnityEngine.Vector3Int(0, 1, 2), vector3.ProvideValue(null));
        }
    }
}
