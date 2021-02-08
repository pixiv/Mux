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
using System.ComponentModel;
using System.Threading;
using Xamarin.Forms;

namespace Mux.Tests.Markup
{
    [TestFixture]
    public static class Component
    {
        private sealed class WithBindableBodyProperties : Mux.Markup.Component<UnityEngine.Transform>
        {
            public static readonly BindableProperty WithDefaultValueProperty = CreateBindableBodyProperty<int>(
                "WithDefaultValue",
                typeof(WithBindableBodyProperties),
                OnWithDefaultValueChanged,
                1,
                BindingMode.TwoWay);

            public static readonly BindableProperty WithDefaultValueCreatorProperty = CreateBindableBodyProperty<int>(
                "WithDefaultValueCreator",
                typeof(WithBindableBodyProperties),
                null,
                0,
                BindingMode.OneWay,
                CreateDefaultValue);

            public static readonly BindableProperty BindableModifierProperty = CreateBindableModifierProperty(
                "BindableModifier",
                typeof(WithBindableBodyProperties),
                CreateDefaultBindableModifier);

            private static object CreateDefaultValue(BindableObject sender)
            {
                return 3;
            }

            private static object CreateDefaultBindableModifier(BindableObject sender)
            {
                return ((WithBindableBodyProperties)sender).defaultModifier;
            }

            private static void OnWithDefaultValueChanged(UnityEngine.Transform body, int value)
            {
                body.gameObject.layer = value;
            }

            public Modifier defaultModifier;

            public int WithDefaultValue
            {
                get
                {
                    return (int)GetValue(WithDefaultValueProperty);
                }

                set
                {
                    SetValue(WithDefaultValueProperty, value);
                }
            }

            public int WithDefaultValueCreator
            {
                get
                {
                    return (int)GetValue(WithDefaultValueCreatorProperty);
                }

                set
                {
                    SetValue(WithDefaultValueCreatorProperty, value);
                }
            }

            public Modifier BindableModifier
            {
                get
                {
                    return (Modifier)GetValue(BindableModifierProperty);
                }

                set
                {
                    SetValue(BindableModifierProperty, value);
                }
            }

            protected internal override void AddToInMainThread(UnityEngine.GameObject gameObject)
            {
                Body = gameObject.transform;
            }
        }

        private sealed class Modifier : Mux.Markup.Component<UnityEngine.Transform>.Modifier
        {
            public SynchronizationContext context;

            protected override void InitializeBodyInMainThread()
            {
                context = SynchronizationContext.Current;
            }
        }

        private sealed class Source : INotifyPropertyChanged
        {
            private int _field;

            public int Property
            {
                get
                {
                    return _field;
                }

                set
                {
                    _field = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Property"));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        [Test]
        public static void SetNullBodyToModifier()
        {
            var modifier = new Modifier { context = null };
            modifier.Body = null;

            Assert.Null(modifier.Body);
            Assert.Null(modifier.context);
        }

        [Test]
        public static void SetNonNullBodyToModifier()
        {
            var gameObject = new UnityEngine.GameObject();

            try
            {
                var body = gameObject.transform;
                var modifier = new Modifier { context = null };
                SynchronizationContextWaiter.Execute(() => modifier.Body = body);

                Assert.AreEqual(body, modifier.Body);
                Assert.AreEqual(Mux.Forms.mainThread, modifier.context);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        [Test]
        public static void DestroyModifierMuxUnappliesBindings()
        {
            var modifier = new Modifier();
            var source = new Source { Property = 0 };
            var binding = new Binding("Property", BindingMode.OneWay, null, null, null, source);
            modifier.SetBinding(Modifier.BindingContextProperty, binding);
            modifier.DestroyMux();
            source.Property = 1;

            Assert.AreEqual(0, modifier.BindingContext);
        }

        [Test]
        public static void DestroyModifierMuxBody()
        {
            var body = new UnityEngine.GameObject();

            try
            {
                var modifier = new Modifier { Body = body.transform };
                modifier.DestroyMux();

                Assert.Null(modifier.Body);
            }
            finally
            {
                UnityEngine.Object.Destroy(body);
            }
        }

        [Test]
        public static void CreateBindableBodyPropertyWithDefaultValue()
        {
            var instance = new WithBindableBodyProperties();
            Assert.AreEqual(1, instance.WithDefaultValue);
        }

        [Test]
        public static void CreateBindableBodyPropertyWithDefaultBindingMode()
        {
            var instance = new WithBindableBodyProperties();
            var source = new Source { Property = 0 };
            var binding = new Binding("Property", BindingMode.Default, null, null, null, source);
            instance.SetBinding(WithBindableBodyProperties.WithDefaultValueProperty, binding);
            instance.WithDefaultValue = 2;
            Assert.AreEqual(2, source.Property);
        }

        [Test]
        public static void CreateBindableBodyPropertyWithDefaultValueCreator()
        {
            var instance = new WithBindableBodyProperties();
            Assert.AreEqual(3, instance.WithDefaultValueCreator);
        }

        [Test]
        public static void OnBindablePropertyChangedWithBody()
        {
            var instance = new WithBindableBodyProperties();
            var gameObject = new UnityEngine.GameObject();

            try
            {
                instance.AddToInMainThread(gameObject);
                SynchronizationContextWaiter.Execute(() => instance.WithDefaultValue = 4);
                Assert.AreEqual(4, gameObject.layer);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        [Test]
        public static void OnBindablePropertyChangedWithDestroyedBody()
        {
            var instance = new WithBindableBodyProperties();
            var gameObject = new UnityEngine.GameObject();

            try
            {
                instance.AddToInMainThread(gameObject);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }

            Assert.DoesNotThrow(() =>
                SynchronizationContextWaiter.Execute(() =>
                    instance.WithDefaultValue = 4));
        }

        [Test]
        public static void SetNullToBindableModifierProperty()
        {
            var source = new Source { Property = 0 };
            var body = new UnityEngine.GameObject();

            try
            {
                var modifier = new Modifier { Body = body.transform };
                var instance = new WithBindableBodyProperties { defaultModifier = modifier };
                var binding = new Binding("Property", BindingMode.OneWay, null, null, null, source);

                modifier.SetBinding(Modifier.BindingContextProperty, binding);
                Assert.AreEqual(modifier, instance.BindableModifier);
                Assert.AreEqual(0, modifier.BindingContext);

                instance.BindableModifier = null;
                Assert.Null(instance.BindableModifier);

                source.Property = 1;
                Assert.AreEqual(0, modifier.BindingContext);
                Assert.Null(modifier.Body);
            }
            finally
            {
                UnityEngine.Object.Destroy(body);
            }
        }

        [Test]
        public static void SetNonNullToBindableModifierProperty()
        {
            var source = new Source { Property = 0 };
            var gameObject = new UnityEngine.GameObject();

            try
            {
                var modifier = new Modifier { Body = gameObject.transform };
                var newModifier = new Modifier();
                var instance = new WithBindableBodyProperties { defaultModifier = modifier };
                instance.AddToInMainThread(gameObject);
                instance.BindingContext = 2;
                var binding = new Binding("Property", BindingMode.OneWay, null, null, null, source);

                modifier.SetBinding(Modifier.BindingContextProperty, binding);
                Assert.AreEqual(modifier, instance.BindableModifier);
                Assert.AreEqual(0, modifier.BindingContext);

                SynchronizationContextWaiter.Execute(() => instance.BindableModifier = newModifier);
                Assert.AreEqual(newModifier, instance.BindableModifier);
                Assert.AreEqual(gameObject.transform, newModifier.Body);
                Assert.AreEqual(2, newModifier.BindingContext);

                source.Property = 1;
                Assert.AreEqual(0, modifier.BindingContext);
                Assert.Null(modifier.Body);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
    }
}
