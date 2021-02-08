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
    public static class Node
    {
        private sealed class Derived : Mux.Markup.Node
        {
            public static readonly BindableProperty GameObjectProperty = BindableProperty.Create(
                "GameObject",
                typeof(UnityEngine.GameObject),
                typeof(Derived));

            public static readonly BindableProperty MainThreadProperty = BindableProperty.Create(
                "MainThread",
                typeof(SynchronizationContext),
                typeof(Derived));

            public int awakeCount;

            public UnityEngine.GameObject GameObject
            {
                get
                {
                    return (UnityEngine.GameObject)GetValue(GameObjectProperty);
                }

                set
                {
                    SetValue(GameObjectProperty, value);
                }
            }
            public SynchronizationContext MainThread
            {
                get
                {
                    return (SynchronizationContext)GetValue(MainThreadProperty);
                }

                set
                {
                    SetValue(MainThreadProperty, value);
                }
            }

            protected internal override void AddToInMainThread(UnityEngine.GameObject gameObject)
            {
                Assert.AreEqual(GameObject, gameObject);
                Assert.AreEqual(MainThread, SynchronizationContext.Current);
                GameObject = null;
                MainThread = null;
            }

            protected internal override void AwakeInMainThread()
            {
                awakeCount++;
            }
        }

        private sealed class Source : INotifyPropertyChanged
        {
            private UnityEngine.GameObject _gameObject;
            private SynchronizationContext _mainThread;

            public UnityEngine.GameObject GameObject
            {
                get
                {
                    return _gameObject;
                }

                set
                {
                    _gameObject = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GameObject"));
                }
            }

            public SynchronizationContext MainThread
            {
                get
                {
                    return _mainThread;
                }

                set
                {
                    _mainThread = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MainThread"));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        [Test]
        public static void AddTo()
        {
            var gameObject = new UnityEngine.GameObject();

            try
            {
                var derived = new Derived { GameObject = gameObject, MainThread = Mux.Forms.mainThread };

                derived.AddTo(gameObject);

                Assert.IsNull(derived.GameObject);
                Assert.IsNull(derived.MainThread);
                Assert.AreEqual(1, derived.awakeCount);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        [Test]
        public static void DestroyMuxInMainThread()
        {
            var derived = new Derived();
            var gameObject = new UnityEngine.GameObject();

            try
            {
                var source = new Source { GameObject = gameObject, MainThread = Mux.Forms.mainThread };
                var gameObjectBinding = new Binding("GameObject", BindingMode.OneWay, null, null, null, source);
                var mainThreadBinding = new Binding("MainThread", BindingMode.OneWay, null, null, null, source);
                derived.SetBinding(Derived.GameObjectProperty, gameObjectBinding);
                derived.SetBinding(Derived.MainThreadProperty, mainThreadBinding);

                derived.DestroyMuxInMainThread();
                source.GameObject = null;
                source.MainThread = null;

                Assert.AreEqual(gameObject, derived.GameObject);
                Assert.AreEqual(Mux.Forms.mainThread, derived.MainThread);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
    }
}
