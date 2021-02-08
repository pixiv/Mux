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

using Mux.Markup;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using UnityEngine.TestTools.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mux.Tests.Markup
{
    [TestFixture]
    public static class RectTransform
    {
        private sealed class DataTemplateSelector : Xamarin.Forms.DataTemplateSelector
        {
            public DataTemplate template;

            protected override DataTemplate OnSelectTemplate(object source, BindableObject container)
            {
                return template;
            }
        }

        private sealed class NotTransform : Mux.Markup.Component<UnityEngine.Component>
        {
            public UnityEngine.GameObject gameObject;
            public int awakeCount = 0;

            protected internal override void AddToInMainThread(UnityEngine.GameObject gameObject)
            {
                this.gameObject = gameObject;
            }

            protected internal override void AwakeInMainThread()
            {
                awakeCount++;
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

        internal sealed class WithoutXaml : Mux.Markup.RectTransform
        {
        }

        private sealed class WithProperty : Mux.Markup.RectTransform
        {
            public static readonly BindableProperty PropertyProperty = BindableProperty.Create(
                "Property",
                typeof(int),
                typeof(WithProperty));

            public int Property
            {
                get
                {
                    return (int)GetValue(PropertyProperty);
                }

                set
                {
                    SetValue(PropertyProperty, value);
                }
            }
        }

        [Test]
        public static void ClearTransformsSource()
        {
            var bindingSource = new Source { Property = 0 };
            var binding = new Binding("Property", BindingMode.OneWay, null, null, null, bindingSource);
            var rect = new Mux.Markup.RectTransform();

            try
            {
                var templated = new WithProperty();
                var transformsSource = new ObservableCollection<int> { 1 };
                templated.SetBinding(WithProperty.PropertyProperty, binding);

                rect.TransformsSource = transformsSource;
                rect.TransformTemplate = new DataTemplate(() => templated);
                transformsSource.Clear();
                bindingSource.Property = 2;

                Assert.AreEqual(0, templated.Property);
            }
            finally
            {
                rect.Destroy();
            }
        }

        [Test]
        public static void InsertTransformsSourceWithParent()
        {
            var gameObject = new UnityEngine.GameObject();

            try
            {
                var rect = new Mux.Markup.RectTransform();
                rect.AddTo(gameObject);
                var templated = new Mux.Markup.RectTransform();
                var transformsSource = new ObservableCollection<int>();

                rect.TransformsSource = transformsSource;
                rect.TransformTemplate = new DataTemplate(() => templated);

                SynchronizationContextWaiter.Execute(() => transformsSource.Add(2));
                Assert.AreEqual(2, templated.BindingContext);
                Assert.AreEqual(0, templated.Body.GetSiblingIndex());

                templated = new Mux.Markup.RectTransform();
                SynchronizationContextWaiter.Execute(() => transformsSource.Add(3));
                Assert.AreEqual(3, templated.BindingContext);
                Assert.AreEqual(1, templated.Body.GetSiblingIndex());
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        [Test]
        public static void InsertTransformsSourceWithoutParent()
        {
            var rect = new Mux.Markup.RectTransform();

            try
            {
                var templated = new Mux.Markup.RectTransform();
                var transformsSource = new ObservableCollection<int>();

                rect.TransformsSource = transformsSource;
                rect.TransformTemplate = new DataTemplate(() => templated);
                transformsSource.Add(0);

                Assert.AreEqual(0, templated.BindingContext);
            }
            finally
            {
                rect.Destroy();
            }
        }

        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public static void MoveTransformsSource(int from, int to)
        {
            var gameObject = new UnityEngine.GameObject();

            try
            {
                var rect = new Mux.Markup.RectTransform();
                var transformsSource = new ObservableCollection<int> { 2, 3 };

                rect.TransformsSource = transformsSource;
                rect.TransformTemplate = new DataTemplate(() => new Mux.Markup.RectTransform());
                rect.AddTo(gameObject);
                var transforms = rect.Cast<Mux.Markup.RectTransform>().ToArray();
                transformsSource.Move(from, to);

                Assert.AreEqual(1, transforms[0].Body.GetSiblingIndex());
                Assert.AreEqual(0, transforms[1].Body.GetSiblingIndex());
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        [Test]
        public static void RemoveTransformsSource()
        {
            var bindingSource = new Source { Property = 1 };
            var binding = new Binding("Property", BindingMode.OneWay, null, null, null, bindingSource);
            var rect = new Mux.Markup.RectTransform();

            try
            {
                var templated = new WithProperty();
                var transformsSource = new ObservableCollection<int> { 2 };
                templated.SetBinding(WithProperty.PropertyProperty, binding);

                rect.TransformsSource = transformsSource;
                rect.TransformTemplate = new DataTemplate(() => templated);
                transformsSource.RemoveAt(0);
                bindingSource.Property = 3;

                Assert.AreEqual(1, templated.Property);
            }
            finally
            {
                rect.Destroy();
            }
        }

        [Test]
        public static void ReplaceTransformsSource()
        {
            var bindingSource = new Source { Property = 1 };
            var binding = new Binding("Property", BindingMode.OneWay, null, null, null, bindingSource);
            var rect = new Mux.Markup.RectTransform();

            try
            {
                var selector = new DataTemplateSelector();
                var templated = new WithProperty();
                var transformsSource = new ObservableCollection<int> { 2 };
                templated.SetBinding(WithProperty.PropertyProperty, binding);

                selector.template = new DataTemplate(() => templated);
                rect.TransformsSource = transformsSource;
                rect.TransformTemplate = selector;
                var oldTemplated = templated;
                templated = new WithProperty();
                selector.template = new DataTemplate(() => templated);
                transformsSource[0] = 3;
                bindingSource.Property = 4;

                Assert.AreEqual(1, oldTemplated.Property);
                Assert.AreEqual(3, templated.BindingContext);
            }
            finally
            {
                rect.Destroy();
            }
        }

        [Test]
        public static void SetNullToTransformsSource()
        {
            var rect = new Mux.Markup.RectTransform();

            try
            {
                rect.TransformTemplate = new DataTemplate(() => new Mux.Markup.RectTransform());
                rect.TransformsSource = new[] { 0 };
                rect.TransformsSource = null;
            }
            finally
            {
                rect.Destroy();
            }
        }

        [Test]
        public static void X()
        {
            var former = new Mux.Markup.Sized();
            var later = new Mux.Markup.Sized();
            var rect = new Mux.Markup.RectTransform { BindingContext = 0 };

            try
            {
                Assert.AreEqual(0, ((Mux.Markup.Sized)rect.X).Index);
                Assert.AreEqual(100, ((Mux.Markup.Sized)rect.X).SizeDelta);

                rect.X = former;
                Assert.AreEqual(rect.Body, former.Body);
                Assert.AreEqual(0, former.BindingContext);

                rect.X = later;
                Assert.IsNull(former.Body);
                Assert.AreEqual(rect.Body, later.Body);
                Assert.AreEqual(0, later.BindingContext);
            }
            finally
            {
                rect.Destroy();
            }
        }

        [Test]
        public static void Y()
        {
            var former = new Mux.Markup.Sized();
            var later = new Mux.Markup.Sized();
            var rect = new Mux.Markup.RectTransform { BindingContext = 0 };

            try
            {
                Assert.AreEqual(1, ((Mux.Markup.Sized)rect.Y).Index);
                Assert.AreEqual(100, ((Mux.Markup.Sized)rect.Y).SizeDelta);

                rect.Y = former;
                Assert.AreEqual(rect.Body, former.Body);
                Assert.AreEqual(0, former.BindingContext);

                rect.Y = later;
                Assert.IsNull(former.Body);
                Assert.AreEqual(rect.Body, later.Body);
                Assert.AreEqual(0, later.BindingContext);
            }
            finally
            {
                rect.Destroy();
            }
        }

        [Test]
        public static void Name()
        {
            var gameObject = new UnityEngine.GameObject();

            try
            {
                var rect = new Mux.Markup.RectTransform();
                Assert.AreEqual("Mux.Markup.RectTransform", rect.Name);

                rect.AddTo(gameObject);
                Assert.AreEqual("Mux.Markup.RectTransform", rect.Body.name);

                SynchronizationContextWaiter.Execute(() =>
                {
                    rect.Name = "";
                    Assert.AreEqual("", rect.Name);
                });

                Assert.AreEqual("", rect.Body.name);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        [Test]
        public static void Tag()
        {
            var gameObject = new UnityEngine.GameObject();

            try
            {
                var rect = new Mux.Markup.RectTransform();
                Assert.AreEqual("Untagged", rect.Tag);
                rect.AddTo(gameObject);
                Assert.AreEqual("Untagged", rect.Body.tag);

                SynchronizationContextWaiter.Execute(() =>
                {
                    rect.Tag = "Finish";
                    Assert.AreEqual("Finish", rect.Tag);
                });

                Assert.AreEqual("Finish", rect.Body.tag);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        [Test]
        public static void DefaultLayer()
        {
            var gameObject = new UnityEngine.GameObject { layer = 1 };

            try
            {
                var rect = new Mux.Markup.RectTransform();
                rect.AddTo(gameObject);

                Assert.AreEqual(1, rect.Layer);
                Assert.AreEqual(1, rect.Body.gameObject.layer);

                SynchronizationContextWaiter.Execute(() =>
                {
                    rect.Layer = 2;
                    Assert.AreEqual(2, rect.Layer);
                });

                Assert.AreEqual(2, rect.Body.gameObject.layer);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        [Test]
        public static void PresetLayer()
        {
            var gameObject = new UnityEngine.GameObject { layer = 1 };

            try
            {
                var rect = new Mux.Markup.RectTransform { Layer = 2 };
                rect.AddTo(gameObject);

                Assert.AreEqual(2, rect.Layer);
                Assert.AreEqual(2, rect.Body.gameObject.layer);

                SynchronizationContextWaiter.Execute(() =>
                {
                    rect.Layer = 3;
                    Assert.AreEqual(3, rect.Layer);
                });

                Assert.AreEqual(3, rect.Body.gameObject.layer);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        [Test]
        public static void LocalPositionZ()
        {
            var gameObject = new UnityEngine.GameObject();

            try
            {
                var rect = new Mux.Markup.RectTransform();

                Assert.AreEqual(0, rect.LocalPositionZ);
                rect.AddTo(gameObject);
                Assert.AreEqual(new UnityEngine.Vector3(0, 0, 0), rect.Body.localPosition);

                SynchronizationContextWaiter.Execute(() =>
                {
                    rect.LocalPositionZ = 1;
                    Assert.AreEqual(1, rect.LocalPositionZ);
                });

                Assert.AreEqual(new UnityEngine.Vector3(0, 0, 1), rect.Body.localPosition);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        [Test]
        public static void LocalScale()
        {
            var gameObject = new UnityEngine.GameObject();

            try
            {
                var rect = new Mux.Markup.RectTransform();

                Assert.AreEqual(UnityEngine.Vector3.one, rect.LocalScale);
                rect.AddTo(gameObject);
                Assert.AreEqual(UnityEngine.Vector3.one, rect.Body.localScale);

                SynchronizationContextWaiter.Execute(() =>
                {
                    rect.LocalScale = new UnityEngine.Vector3(0, 2, 3);
                    Assert.AreEqual(new UnityEngine.Vector3(0, 2, 3), rect.LocalScale);
                });

                Assert.AreEqual(new UnityEngine.Vector3(0, 2, 3), rect.Body.localScale);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        [Test]
        public static void LocalEulerAngles()
        {
            var rect = new Mux.Markup.RectTransform();

            try
            {
                SynchronizationContextWaiter.Execute(() =>
                {
                    Assert.AreEqual(UnityEngine.Vector3.zero, rect.LocalEulerAngles);

                    rect.LocalEulerAngles = new UnityEngine.Vector3(7, 8, 9);
                    var expectedEuler = new UnityEngine.Vector3(7, 8, 9);
                    Assert.That(rect.LocalEulerAngles, Is.EqualTo(expectedEuler).Using(Vector3EqualityComparer.Instance));
                });

                var expectedRotation = new UnityEngine.Quaternion(0.1f, 0.1f, 0.1f, 1f);
                Assert.That(rect.LocalRotation, Is.EqualTo(expectedRotation).Using(QuaternionEqualityComparer.Instance));
            }
            finally
            {
                rect.Destroy();
            }
        }

        [Test]
        public static void LocalRotation()
        {
            var newRotation = new UnityEngine.Quaternion(0, 0, 1, 0);
            var isNewRotation = Is.EqualTo(newRotation).Using(QuaternionEqualityComparer.Instance);
            var gameObject = new UnityEngine.GameObject();

            try
            {
                var rect = new Mux.Markup.RectTransform();
                Assert.AreEqual(UnityEngine.Quaternion.identity, rect.LocalRotation);
                Assert.AreEqual(UnityEngine.Vector3.zero, rect.LocalEulerAngles);

                rect.AddTo(gameObject);
                Assert.AreEqual(UnityEngine.Quaternion.identity, rect.Body.localRotation);

                SynchronizationContextWaiter.Execute(() =>
                {
                    rect.LocalRotation = newRotation;
                    Assert.That(rect.LocalRotation, isNewRotation);
                    Assert.AreEqual(new UnityEngine.Vector3(0, 0, 180), rect.LocalEulerAngles);
                });

                Assert.That(rect.Body.localRotation, isNewRotation);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        [Test]
        public static void ActiveSelf()
        {
            var rect = new Mux.Markup.RectTransform();

            Assert.IsTrue(rect.ActiveSelf);
            rect.ActiveSelf = false;
            Assert.IsFalse(rect.ActiveSelf);

            var gameObject = new UnityEngine.GameObject();

            try
            {
                rect.AddTo(gameObject);
                Assert.IsFalse(rect.Body.gameObject.activeSelf);

                SynchronizationContextWaiter.Execute(() =>
                {
                    rect.ActiveSelf = true;
                    Assert.IsTrue(rect.ActiveSelf);
                });

                Assert.IsTrue(rect.Body.gameObject.activeSelf);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        [Test]
        public static void SetInheritedBindingContextToX()
        {
            var rect = new Mux.Markup.RectTransform { BindingContext = 0 };

            try
            {
                Assert.AreEqual(0, rect.X.BindingContext);
                rect.BindingContext = 1;
                Assert.AreEqual(1, rect.X.BindingContext);
            }
            finally
            {
                rect.Destroy();
            }
        }

        [Test]
        public static void SetInheritedBindingContextToY()
        {
            var rect = new Mux.Markup.RectTransform { BindingContext = 0 };

            try
            {
                Assert.AreEqual(0, rect.Y.BindingContext);
                rect.BindingContext = 1;
                Assert.AreEqual(1, rect.Y.BindingContext);
            }
            finally
            {
                rect.Destroy();
            }
        }

        [Test]
        public static void SetInheritedBindingContextToChildren()
        {
            var rect = new Mux.Markup.RectTransform { BindingContext = 0 };

            try
            {
                var children = new[]
                {
                    new Mux.Markup.RectTransform(),
                    new Mux.Markup.RectTransform()
                };

                children[0].BindingContextChanged += (sender, args) =>
                {
                    if ((int)((Mux.Markup.RectTransform)sender).BindingContext == 1)
                    {
                        children[1].Destroy();
                    }
                };

                foreach (var child in children)
                {
                    rect.Add(child);
                }

                foreach (var child in children)
                {
                    Assert.AreEqual(0, child.BindingContext);
                }

                rect.BindingContext = 1;
                CollectionAssert.AreEqual(new[] { children[0] }, rect);
                Assert.AreEqual(1, children[0].BindingContext);
                Assert.AreEqual(1, children[1].BindingContext);
            }
            finally
            {
                rect.Destroy();
            }
        }

        [Test]
        public static void AddTransform()
        {
            var rect = new Mux.Markup.RectTransform { BindingContext = 0 };
            var afterHavingParent = new Mux.Markup.RectTransform();
            var beforeHavingParent = new Mux.Markup.RectTransform();

            rect.Add(beforeHavingParent);
            Assert.AreEqual(rect, ((IInternalTransform)beforeHavingParent).Parent);
            CollectionAssert.Contains(rect, beforeHavingParent);

            var gameObject = new UnityEngine.GameObject();

            try
            {
                rect.AddToInMainThread(gameObject);
                Assert.AreEqual(gameObject.transform, rect.Body.parent);
                Assert.AreEqual(rect.Body, beforeHavingParent.Body.parent);
                Assert.False(beforeHavingParent.Body.gameObject.activeSelf);

                rect.Add(afterHavingParent);
                Assert.AreEqual(rect, ((IInternalTransform)afterHavingParent).Parent);
                Assert.AreEqual(rect.Body, afterHavingParent.Body.parent);
                Assert.False(afterHavingParent.Body.gameObject.activeSelf);

                rect.AwakeInMainThread();
                Assert.True(beforeHavingParent.Body.gameObject.activeSelf);
                Assert.True(afterHavingParent.Body.gameObject.activeSelf);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        /// <remarks>
        /// <see cref="UnityEngine.UI.Slider.handleRect" /> is expected to have a parent,
        /// which is usually set via data binding between
        /// <see cref="Mux.Markup.Slider.HandleRect" /> and
        /// <see cref="Mux.Markup.RectTransform.Body" />.
        /// This ensures <see cref="Mux.Markup.RectTransform.Body" /> meets the requirement.
        /// <remarks>
        [Test]
        public static void AddTransformToGameObject()
        {
            var gameObject = new UnityEngine.GameObject();

            try
            {
                var child = new Mux.Markup.RectTransform();

                child.PropertyChanged += (sender, args) =>
                {
                    Assert.AreEqual("Body", args.PropertyName);
                    Assert.AreEqual(gameObject.transform, child.Body.parent);
                };

                child.AddToInMainThread(gameObject);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        [Test]
        public static void AddNotTransformAfterHavingParent()
        {
            var gameObject = new UnityEngine.GameObject();

            try
            {
                var rect = new Mux.Markup.RectTransform { BindingContext = 0 };
                rect.AddTo(gameObject);
                var child = new NotTransform();
                SynchronizationContextWaiter.Execute(() => rect.Add(child));

                Assert.AreEqual(0, child.BindingContext);
                Assert.AreEqual(rect.Body.gameObject, child.gameObject);
                Assert.AreEqual(1, child.awakeCount);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        [Test]
        public static void AddNotTransformBeforeHavingParent()
        {
            var rect = new Mux.Markup.RectTransform { BindingContext = 0 };
            var beforeHavingParent = new NotTransform();
            var afterHavingParent = new NotTransform();
            SynchronizationContextWaiter.Execute(() => rect.Add(beforeHavingParent));

            Assert.AreEqual(0, beforeHavingParent.BindingContext);
            Assert.Null(beforeHavingParent.gameObject);
            Assert.AreEqual(0, beforeHavingParent.awakeCount);

            var gameObject = new UnityEngine.GameObject();

            try
            {
                rect.AddToInMainThread(gameObject);
                Assert.AreEqual(rect.Body.gameObject, beforeHavingParent.gameObject);
                Assert.AreEqual(0, beforeHavingParent.awakeCount);

                rect.Add(afterHavingParent);
                Assert.AreEqual(rect.Body.gameObject, afterHavingParent.gameObject);
                Assert.AreEqual(0, afterHavingParent.awakeCount);

                rect.AwakeInMainThread();
                Assert.AreEqual(1, beforeHavingParent.awakeCount);
                Assert.AreEqual(1, afterHavingParent.awakeCount);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        [Test]
        public static void DestroyMuxInMainThreadUnappliesBindings()
        {
            var rect = new Mux.Markup.RectTransform();
            var source = new Source { Property = 0 };
            var binding = new Binding("Property", BindingMode.OneWay, null, null, null, source);
            rect.SetBinding(Mux.Markup.RectTransform.BindingContextProperty, binding);
            rect.DestroyMuxInMainThread();
            source.Property = 1;
            Assert.AreEqual(0, rect.BindingContext);
        }

        [Test]
        public static void DestroyXMuxInMainThread()
        {
            var rect = new Mux.Markup.RectTransform();
            var source = new Source { Property = 0 };
            var binding = new Binding("Property", BindingMode.OneWay, null, null, null, source);
            rect.X.SetBinding(Mux.Markup.RectTransformLayout.BindingContextProperty, binding);
            rect.DestroyMuxInMainThread();
            source.Property = 1;
            Assert.AreEqual(0, rect.X.BindingContext);
        }

        [Test]
        public static void DestroyYMuxInMainThread()
        {
            var rect = new Mux.Markup.RectTransform();
            var source = new Source { Property = 0 };
            var binding = new Binding("Property", BindingMode.OneWay, null, null, null, source);
            rect.Y.SetBinding(Mux.Markup.RectTransformLayout.BindingContextProperty, binding);
            rect.DestroyMuxInMainThread();
            source.Property = 1;
            Assert.AreEqual(0, rect.Y.BindingContext);
        }

        [Test]
        public static void DestroyNodeMuxInMainThread()
        {
            var rect = new Mux.Markup.RectTransform();
            var source = new Source { Property = 0 };
            var binding = new Binding("Property", BindingMode.OneWay, null, null, null, source);
            var child = new Mux.Markup.RectTransform();
            rect.Add(child);
            child.SetBinding(Mux.Markup.RectTransform.BindingContextProperty, binding);
            rect.DestroyMuxInMainThread();
            source.Property = 1;
            Assert.AreEqual(0, child.BindingContext);
        }

        [Test]
        public static void Destroy()
        {
            var source = new Source { Property = 0 };
            var binding = new Binding("Property", BindingMode.OneWay, null, null, null, source);
            var gameObject = new UnityEngine.GameObject();

            try
            {
                SynchronizationContextWaiter.Execute(() =>
                {
                    var parent = new Mux.Markup.RectTransform();
                    parent.AddTo(gameObject);
                    var rect = new Mux.Markup.RectTransform();
                    parent.Add(rect);

                    rect.SetBinding(Mux.Markup.RectTransform.BindingContextProperty, binding);
                    rect.Destroy();
                    source.Property = 1;
                    Assert.AreEqual(0, rect.BindingContext);
                    CollectionAssert.DoesNotContain(parent, rect);
                });
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        [Test]
        public static void DestroyImmediate()
        {
            var source = new Source { Property = 0 };
            var binding = new Binding("Property", BindingMode.OneWay, null, null, null, source);
            var gameObject = new UnityEngine.GameObject();

            try
            {
                var parent = new Mux.Markup.RectTransform();
                parent.AddTo(gameObject);
                var rect = new Mux.Markup.RectTransform();
                parent.Add(rect);

                SynchronizationContextWaiter.Execute(() =>
                {
                    rect.SetBinding(Mux.Markup.RectTransform.BindingContextProperty, binding);
                    rect.DestroyImmediate();
                    source.Property = 1;
                    Assert.AreEqual(0, rect.BindingContext);
                });

                Assert.AreEqual(0, parent.Body.childCount);
                CollectionAssert.DoesNotContain(parent, rect);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

#if UNITY_EDITOR
        [Test]
        public static void ReloadWithTransformsSource()
        {
            var gameObject = new UnityEngine.GameObject();

            try
            {
                var rect = new Mux.Markup.RectTransform();
                rect.AddTo(gameObject);
                rect.TransformTemplate = new DataTemplate(() => new Mux.Markup.RectTransform());
                rect.TransformsSource = new[] { 1 };
                var oldNodes = rect.ToArray();

                ((IInternalTransform)rect).Clear();

                var newNodes = rect.ToArray();
                Assert.AreEqual(oldNodes.Length, newNodes.Length);
                Assert.AreNotSame(oldNodes[0], newNodes[0]);
            }
            finally
            {
                UnityEngine.GameObject.Destroy(gameObject);
            }
        }

        [Test]
        public static void ReloadWithoutTransformsSource()
        {
            var rect = new Mux.Markup.RectTransform();

            try
            {
                rect.Add(new Mux.Markup.RectTransform());
                ((IInternalTransform)rect).Clear();
                CollectionAssert.IsEmpty(rect);
            }
            finally
            {
                rect.Destroy();
            }
        }

        [Test]
        public static void ClearToDestroyNotTransform()
        {
            var source = new Source { Property = 0 };
            var notTransform = new NotTransform();
            var binding = new Binding("Property", BindingMode.OneWay, null, null, null, source);
            var rect = new Mux.Markup.RectTransform();

            try
            {
                notTransform.SetBinding(NotTransform.BindingContextProperty, binding);
                rect.Add(notTransform);

                ((IInternalTransform)rect).Clear();
                CollectionAssert.IsEmpty(rect);

                source.Property = 1;
                Assert.AreEqual(0, notTransform.BindingContext);
            }
            finally
            {
                rect.Destroy();
            }
        }

        [Test]
        public static void ResetBodyWithParent()
        {
            var parent = new UnityEngine.GameObject();

            try
            {
                var rects = new[]
                {
                    new Mux.Markup.RectTransform(),
                    new Mux.Markup.RectTransform()
                };

                SynchronizationContextWaiter.Execute(() =>
                {
                    var oldBody = rects[0].Body;

                    rects[0].AddTo(parent);
                    rects[1].AddTo(parent);

                    rects[0].ActiveSelf = false;
                    rects[0].X = new Mux.Markup.Sized { Anchor = 0 };
                    rects[0].Y = new Mux.Markup.Sized { Anchor = 1 };

                    ((IInternalTransform)rects[0]).Clear();

                    Assert.AreNotSame(oldBody, rects[0].Body);
                });

                Assert.AreEqual(false, rects[0].Body.gameObject.activeSelf);
                Assert.AreEqual(new UnityEngine.Vector2(0, 1), rects[0].Body.anchorMin);
                Assert.AreEqual(parent.transform, rects[0].Body.parent);
                Assert.AreEqual(0, rects[0].Body.GetSiblingIndex());
            }
            finally
            {
                UnityEngine.Object.Destroy(parent);
            }
        }

        [Test]
        public static void ResetBodyWithoutParent()
        {
            var gameObject = new UnityEngine.GameObject();

            try
            {
                var rect = new Mux.Markup.RectTransform();
                rect.AddTo(gameObject);

                SynchronizationContextWaiter.Execute(() =>
                {
                    var oldBody = rect.Body;

                    rect.ActiveSelf = false;
                    rect.X = new Mux.Markup.Sized { Anchor = 0 };
                    rect.Y = new Mux.Markup.Sized { Anchor = 1 };

                    ((IInternalTransform)rect).Clear();

                    Assert.AreNotSame(oldBody, rect.Body);
                });

                Assert.AreEqual(false, rect.Body.gameObject.activeSelf);
                Assert.AreEqual(new UnityEngine.Vector2(0, 1), rect.Body.anchorMin);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
#endif

        [Test]
        public static void GetEnumerator()
        {
            var rect = new Mux.Markup.RectTransform();

            try
            {
                var nodes = new Mux.Markup.Node[] {
                    new NotTransform(),
                    new Mux.Markup.RectTransform()
                };

                rect.Add(nodes[1]);
                rect.Add(nodes[0]);
                CollectionAssert.AreEqual(nodes, rect);
            }
            finally
            {
                rect.Destroy();
            }
        }

        [Test]
        public static void SetXAfterAddBeforeAwake()
        {
            var localPosition = new UnityEngine.Vector3(6, 0, 0);
            var rect = new Mux.Markup.RectTransform();
            var gameObject = new UnityEngine.GameObject();

            try
            {
                rect.AddToInMainThread(gameObject);
                rect.X = new Mux.Markup.Sized { AnchoredPosition = 6 };
                Assert.AreEqual(localPosition, rect.Body.localPosition);

                rect.AwakeInMainThread();
                Assert.AreEqual(localPosition, rect.Body.localPosition);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
    }
}
