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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mux.Markup
{
    internal interface IInternalTransform : ITransform
    {
        void DestroyBody(float delay = 0);
        void RemoveTransform(Node transform);
        int GetSiblingIndex();
        void SetSiblingIndex(int index);
        IInternalTransform Parent { get; set; }

#if UNITY_EDITOR
        void Clear();
        void BindTransforms();
        void ReleaseTransforms();
#endif
    }

    /// <summary>
    /// An interface that represents <see cref="T:UnityEngine.Transform" /> or its derivatives,
    /// and <xref href="UnityEngine.Component.gameObject?text=the game object it is attached to" />.
    /// </summary>
    /// <remarks>
    /// This is useful as an interface common both for <see cref="Transform" /> and
    /// <see cref="RectTransform" />
    /// </remarks>
    public interface ITransform : IEnumerable<Node>
    {
        /// <summary>
        /// Gets or sets the <see cref="DataTemplate" /> to apply to the <see cref="TransformsSource" />.
        /// </summary>
        DataTemplate TransformTemplate { get; set; }

        /// <summary>Gets or sets the source of transforms to template and display.</summary>
        IEnumerable TransformsSource { get; set; }

        /// <summary>A property that represents <see cref="P:UnityEngine.Transform.localRotation" />.</summary>
        UnityEngine.Quaternion LocalRotation { get; set; }

        /// <summary>A property that represents <see cref="P:UnityEngine.Transform.localScale" />.</summary>
        /// <seealso cref="Vector3" />
        UnityEngine.Vector3 LocalScale { get; set; }

        /// <summary>A property that represents <see cref="P:UnityEngine.GameObject.activeSelf" />.</summary>
        bool ActiveSelf { get; set; }

        /// <summary>A property that represents <see cref="P:UnityEngine.GameObject.layer" />.</summary>
        int Layer { get; set; }

        /// <summary>The name of the <see cref="T:UnityEngine.Transform" />.</summary>
        /// <seealso cref="P:UnityEngine.Object.name" />
        string Name { get; set; }

        /// <summary>The tag of the <see cref="T:UnityEngine.Transform" />.</summary>
        /// <seealso cref="P:UnityEngine.Component.tag" />
        string Tag { get; set; }

        /// <summary>A method to add <see cref="Node" />.</summary>
        /// <remarks>
        /// You can add <see cref="Node" /> as child elements in XAML thanks to this method.
        ///
        /// The order of components except transforms are not guranteed. Transforms will be
        /// ordered in the order of addition.
        ///
        /// Transforms cannot be added when templating is enabled.
        /// </remarks>
        void Add(Node node);

        /// <summary>
        /// A method to unapply bindings, remove itself from the markup tree, and destroy the underlaying
        /// <see cref="T:UnityEngine.GameObject" />.
        /// </summary>
        /// <remarks>
        /// The <paramref name="delay" /> only applies to the destruction of the underlaying
        /// <see cref="T:UnityEngine.GameObject" />.
        /// Bindings will synchrnously be unapplied and the markup tree will be updated at the same time.
        /// </remarks>
        /// <seealso cref="M:UnityEngine.Object.Destroy(UnityEngine.Object,System.Single)" />
        void Destroy(float delay = 0);

        /// <summary>
        /// A method to unapply bindings, remove itself from the markup tree, and destroy the underlaying
        /// <see cref="T:UnityEngine.GameObject" />.
        /// </summary>
        /// <seealso cref="M:UnityEngine.Object.DestroyImmediate(UnityEngine.Object,System.Boolean)" />
        void DestroyImmediate(bool destroyAssets = false);
    }

    /// <summary>
    /// A <see cref="Component{T}" /> that represents <see cref="T:UnityEngine.Transform" /> or its derivatives,
    /// and <xref href="UnityEngine.Component.gameObject?text=the game object it is attached to" />.
    /// </summary>
    public abstract class Transform<T> : Component<T>, IInternalTransform where T : UnityEngine.Transform
    {
        private sealed class TransformCollection : TemplatableCollection<Node>
        {
            private readonly ImmutableList<TemplatedItem<Node>>.Builder _builder =
                ImmutableList.CreateBuilder<TemplatedItem<Node>>();

            public TransformCollection(Node container) : base(container)
            {
            }

            protected override IList<TemplatedItem<Node>> GetList()
            {
                return _builder;
            }

            public override void ClearList()
            {
                foreach (var item in _builder)
                {
                    ((IInternalTransform)item.Content).DestroyBody();
                }

                base.ClearList();
            }

            public override void InsertListRange(int index, IEnumerable<TemplatedItem<Node>> enumerable)
            {
                var count = _builder.Count;
                _builder.InsertRange(index, enumerable);
                count = _builder.Count - count;
                ((Transform<T>)container).InsertRangeSideEffect(index, count);
            }

            public override void MoveListRange(int from, int to, int count)
            {
                if (from < to)
                {
                    var index = count;
                    while (index > 0)
                    {
                        index--;
                        var toTransform = (IInternalTransform)_builder[to + index].Content;
                        var fromTransform = (IInternalTransform)_builder[from + index].Content;
                        toTransform.SetSiblingIndex(fromTransform.GetSiblingIndex());
                    }
                }
                else
                {
                    for (var index = 0; index < count; index++)
                    {
                        var toTransform = (IInternalTransform)_builder[to + index].Content;
                        var fromTransform = (IInternalTransform)_builder[from + index].Content;
                        toTransform.SetSiblingIndex(fromTransform.GetSiblingIndex());
                    }
                }

                base.MoveListRange(from, to, count);
            }

            public override void RemoveListRange(int index, int count)
            {
                foreach (var item in _builder.Skip(index).Take(count))
                {
                    ((IInternalTransform)item.Content).DestroyBody();
                }

                while (count > 0)
                {
                    _builder.RemoveAt(index);
                    count--;
                }
            }

            public override void ReplaceListRange(int index, int count, IEnumerable<TemplatedItem<Node>> enumerable)
            {
                foreach (var item in _builder.Skip(index).Take(count))
                {
                    ((IInternalTransform)item.Content).DestroyBody();
                }

                base.ReplaceListRange(index, count, enumerable);
                ((Transform<T>)container).InsertRangeSideEffect(index, count);
            }

            public IEnumerable<Node> ToImmutable()
            {
                foreach (var item in _builder.ToImmutable())
                {
                    yield return item.Content;
                }
            }

            public Node this[int index] => _builder[index].Content;
        }

        /// <summary>Backing store for the <see cref="TransformsSource" /> property.</summary>
        public static readonly BindableProperty TransformsSourceProperty = BindableProperty.Create(
            "TransformsSource",
            typeof(IEnumerable),
            typeof(Transform<T>),
            null,
            BindingMode.OneWay,
            null,
            OnTransformsSourceChanged);

        /// <summary>Backing store for the <see cref="TransformTemplate" /> property.</summary>
        public static readonly BindableProperty TransformTemplateProperty = BindableProperty.Create(
            "TransformTemplate",
            typeof(DataTemplate),
            typeof(Transform<T>),
            null,
            BindingMode.OneWay,
            null,
            OnTransformTemplateChanged);

        /// <summary>Backing store for the <see cref="Name" /> property.</summary>
        public static readonly BindableProperty NameProperty = CreateBindableBodyProperty<string>(
            "Name",
            typeof(Transform<T>),
            (body, value) => body.name = value,
            null,
            BindingMode.OneWay,
            sender => sender.GetType().FullName);

        /// <summary>Backing store for the <see cref="Tag" /> property.</summary>
        public static readonly BindableProperty TagProperty = CreateBindableBodyProperty<string>(
            "Tag",
            typeof(Transform<T>),
            (body, value) => body.tag = value,
            "Untagged");

        /// <summary>Backing store for the <see cref="Layer" /> property.</summary>
        public static readonly BindableProperty LayerProperty = CreateBindableBodyProperty<int>(
            "Layer",
            typeof(Transform<T>),
            (body, value) => body.gameObject.layer = value,
            0,
            BindingMode.OneWay);

        /// <summary>Backing store for the <see cref="LocalScale" /> property.</summary>
        public static readonly BindableProperty LocalScaleProperty = CreateBindableBodyProperty<UnityEngine.Vector3>(
            "LocalScale",
            typeof(Transform<T>),
            (body, value) => body.localScale = value,
            UnityEngine.Vector3.one);

        /// <summary>Backing store for the <see cref="LocalRotation" /> property.</summary>
        public static readonly BindableProperty LocalRotationProperty = BindableProperty.Create(
            "LocalRotation",
            typeof(UnityEngine.Quaternion),
            typeof(Transform<T>),
            UnityEngine.Quaternion.identity,
            BindingMode.OneWay,
            null,
            OnLocalRotationChanged);

        /// <summary>Backing store for the <see cref="LocalEulerAngles" /> property.</summary>
        public static readonly BindableProperty LocalEulerAnglesProperty = BindableProperty.Create(
            "LocalEulerAngles",
            typeof(UnityEngine.Vector3),
            typeof(Transform<T>),
            null,
            BindingMode.OneWay,
            null,
            OnLocalEulerAnglesChanged,
            null,
            null,
            CreateLocalEulerAngles);

        /// <summary>Backing store for the <see cref="ActiveSelf" /> property.</summary>
        public static readonly BindableProperty ActiveSelfProperty = CreateBindableBodyProperty<bool>(
            "ActiveSelf",
            typeof(Transform<T>),
            (body, value) => body.gameObject.SetActive(value),
            true);

        private T _sleeping = null;

        private static void OnTransformsSourceChanged(BindableObject sender, object oldValue, object newValue)
        {
            if (newValue != null)
            {
                ((Transform<T>)sender)._transforms.ChangeSource((IEnumerable)newValue);
            }
        }

        private static void OnTransformTemplateChanged(BindableObject sender, object oldValue, object newValue)
        {
            ((Transform<T>)sender)._transforms.ChangeTemplate((DataTemplate)newValue);
        }

        private static void OnLocalRotationChanged(BindableObject sender, object oldValue, object newValue)
        {
            Forms.mainThread.Send(state =>
            {
                var rect = (Transform<T>)state;

                if (rect.Body != null)
                {
                    rect.Body.localRotation = rect.LocalRotation;
                }
            }, sender);

            sender.ClearValue(LocalEulerAnglesProperty);
        }

        private static void OnLocalEulerAnglesChanged(BindableObject sender, object oldValue, object newValue)
        {
            sender.SetValueCore(LocalRotationProperty, UnityEngine.Quaternion.Euler((UnityEngine.Vector3)newValue));
        }

        private static object CreateLocalEulerAngles(BindableObject sender)
        {
            return ((Transform<T>)sender).LocalRotation.eulerAngles;
        }

        private readonly TransformCollection _transforms;
        private readonly List<Node> _components = new List<Node>();

        /// <inheritdoc />
        public IEnumerable TransformsSource
        {
            get
            {
                return (IEnumerable)GetValue(TransformsSourceProperty);
            }

            set
            {
                SetValue(TransformsSourceProperty, value);
            }
        }

        /// <inheritdoc />
        public DataTemplate TransformTemplate
        {
            get
            {
                return (DataTemplate)GetValue(TransformTemplateProperty);
            }

            set
            {
                SetValue(TransformTemplateProperty, value);
            }
        }

        /// <inheritdoc />
        public string Name
        {
            get
            {
                return (string)GetValue(NameProperty);
            }

            set
            {
                SetValue(NameProperty, value);
            }
        }

        /// <inheritdoc />
        public string Tag
        {
            get
            {
                return (string)GetValue(TagProperty);
            }

            set
            {
                SetValue(TagProperty, value);
            }
        }

        /// <inheritdoc />
        [TypeConverter(typeof(LayerTypeConverter))]
        public int Layer
        {
            get
            {
                return (int)GetValue(LayerProperty);
            }

            set
            {
                SetValue(LayerProperty, value);
            }
        }

        /// <inheritdoc />
        public UnityEngine.Vector3 LocalScale
        {
            get
            {
                return (UnityEngine.Vector3)GetValue(LocalScaleProperty);
            }

            set
            {
                SetValue(LocalScaleProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Transform.localEulerAngles" />.</summary>
        public UnityEngine.Vector3 LocalEulerAngles
        {
            get
            {
                return (UnityEngine.Vector3)GetValue(LocalEulerAnglesProperty);
            }

            set
            {
                SetValue(LocalEulerAnglesProperty, value);
            }
        }

        /// <inheritdoc />
        public UnityEngine.Quaternion LocalRotation
        {
            get
            {
                return (UnityEngine.Quaternion)GetValue(LocalRotationProperty);
            }

            set
            {
                SetValue(LocalRotationProperty, value);
            }
        }

        /// <inheritdoc />
        public bool ActiveSelf
        {
            get
            {
                return (bool)GetValue(ActiveSelfProperty);
            }

            set
            {
                SetValue(ActiveSelfProperty, value);
            }
        }

        private IInternalTransform _parent;

        IInternalTransform IInternalTransform.Parent
        {
            get
            {
                return _parent;
            }

            set
            {
                _parent = value;
            }
        }

        public Transform()
        {
            _transforms = new TransformCollection(this);
            _transforms.ChangeSource(TransformsSource);
            _transforms.ChangeTemplate(TransformTemplate);

#if UNITY_EDITOR
            _this = new WeakReference<IInternalTransform>(this);

            for (var type = GetType(); type != typeof(Transform<T>); type = type.BaseType)
            {
                var path = GetXamlPath(type);

                if (path != null)
                {
                    Transforms.Add(path, _this);
                }
            }
#endif
        }

#if UNITY_EDITOR
        ~Transform()
        {
            for (var type = GetType(); type != typeof(Transform<T>); type = type.BaseType)
            {
                var path = GetXamlPath(type);

                if (path != null)
                {
                    Transforms.Remove(path, _this);
                }
            }
        }

        private readonly WeakReference<IInternalTransform> _this;

        private string GetXamlPath(Type type)
        {
            foreach (var attribute in type.Assembly.GetCustomAttributes<XamlResourceIdAttribute>())
            {
                if (attribute.Type == type)
                {
                    return attribute.Path;
                }
            }

            return null;
        }
#endif

        /// <inheritdoc />
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            foreach (var node in _components.Concat(_transforms.ToImmutable()))
            {
                SetInheritedBindingContext(node, BindingContext);
            }
        }

        /// <inheritdoc />
        public void Add(Node node)
        {
            if (node is IInternalTransform)
            {
                _transforms.Add(node);
            }
            else
            {
                _components.Add(node);
                SetInheritedBindingContext(node, BindingContext);

                Forms.mainThread.Send(state =>
                {
                    if (_sleeping != null)
                    {
                        node.AddToInMainThread(_sleeping.gameObject);
                    }
                    else if (Body != null)
                    {
                        node.AddToInMainThread(Body.gameObject);
                        node.AwakeInMainThread();
                    }
                }, null);
            }
        }

        private void InsertRangeSideEffect(int index, int count)
        {
            Forms.mainThread.Send(state =>
            {
                if (_sleeping == null && Body == null)
                {
                    while (count > 0)
                    {
                        var item = _transforms[index];
                        SetInheritedBindingContext(item, BindingContext);
                        ((IInternalTransform)item).Parent = this;
                        count--;
                        index++;
                    }
                }
                else
                {
                    var siblingIndex = index > 0 ?
                        ((IInternalTransform)_transforms[index - 1]).GetSiblingIndex() + 1 : 0;

                    while (count > 0)
                    {
                        var node = _transforms[index];
                        var transform = (IInternalTransform)node;
                        SetInheritedBindingContext(node, BindingContext);
                        transform.Parent = this;
                        node.AddToInMainThread((_sleeping ?? Body).gameObject);
                        transform.SetSiblingIndex(siblingIndex);
                        count--;
                        index++;
                        siblingIndex++;

                        if (_sleeping == null)
                        {
                            node.AwakeInMainThread();
                        }
                    }
                }
            }, null);
        }

        /// <inheritdoc />
        protected internal override void AddToInMainThread(UnityEngine.GameObject parent)
        {
            var gameObject = new UnityEngine.GameObject(Name);
            gameObject.SetActive(false);
            _sleeping = AddComponentToInMainThread(gameObject);

            foreach (var node in this)
            {
                node.AddToInMainThread(gameObject);
            }

            _sleeping.SetParent(parent.transform, false);
            Body = _sleeping;
        }

        private protected abstract T AddComponentToInMainThread(UnityEngine.GameObject gameObject);

        /// <inheritdoc />
        protected internal override void AwakeInMainThread()
        {
            if (!IsSet(LayerProperty))
            {
                Layer = Body.parent.gameObject.layer;
            }

            Body.gameObject.layer = Layer;
            Body.name = Name;
            Body.tag = Tag;
            Body.localScale = LocalScale;
            Body.localRotation = LocalRotation;
            Body.gameObject.SetActive(true);
            _sleeping = null;

            foreach (var node in this)
            {
                node.AwakeInMainThread();
            }

            Body.gameObject.SetActive(ActiveSelf);
        }

        /// <inheritdoc />
        protected internal override void DestroyMuxInMainThread()
        {
            base.DestroyMuxInMainThread();

            foreach (var item in this)
            {
                item.DestroyMuxInMainThread();
            }
        }

        /// <inheritdoc />
        public void Destroy(float delay = 0)
        {
            ((IInternalTransform)this).DestroyBody(delay);
            _parent?.RemoveTransform(this);
        }

        private void DestroyBodyImmediate(bool destroyAssets = false)
        {
            Forms.mainThread.Send(state =>
            {
                DestroyMuxInMainThread();

                if (Body != null)
                {
                    UnityEngine.Object.DestroyImmediate(Body.gameObject, destroyAssets);
                }
            }, null);
        }

        /// <inheritdoc />
        public void DestroyImmediate(bool destroyAssets = false)
        {
            DestroyBodyImmediate(destroyAssets);
            _parent?.RemoveTransform(this);
        }

#if UNITY_EDITOR
        void IInternalTransform.Clear()
        {
            if (Body == null)
            {
                if (TransformsSource == null)
                {
                    _transforms.Clear();
                }

                Forms.mainThread.Send(state =>
                {
                    foreach (var node in this)
                    {
                        node.DestroyMuxInMainThread();
                    }
                }, null);

                _components.Clear();
            }
            else
            {
                if (TransformsSource == null)
                {
                    _transforms.Clear();
                }
                else
                {
                    _transforms.ChangeSource(new object[0]);
                }

                Forms.mainThread.Send(state =>
                {
                    var parent = Body.parent;
                    var siblingIndex = Body.GetSiblingIndex();

                    foreach (var node in this)
                    {
                        node.DestroyMuxInMainThread();
                    }

                    UnityEngine.Object.Destroy(Body.gameObject);
                    _components.Clear();
                    AddToInMainThread(parent.gameObject);
                    Body.SetSiblingIndex(siblingIndex);
                    AwakeInMainThread();
                }, null);

                _transforms.ChangeSource(TransformsSource);
            }

            SetValue(Xamarin.Forms.Internals.NameScope.NameScopeProperty, null);
        }

        private readonly HashSet<Node> _boundTransforms = new HashSet<Node>();

        void IInternalTransform.BindTransforms()
        {
            _boundTransforms.UnionWith(_transforms);
        }

        void IInternalTransform.ReleaseTransforms()
        {
            _boundTransforms.Clear();
        }
#endif

        IEnumerator<Node> IEnumerable<Node>.GetEnumerator()
        {
            return _components.Concat(_transforms).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _components.Concat(_transforms).GetEnumerator();
        }

        void IInternalTransform.DestroyBody(float delay)
        {
            Forms.mainThread.Send(state =>
            {
                DestroyMuxInMainThread();

                if (Body != null)
                {
                    UnityEngine.Object.Destroy(Body.gameObject, delay);
                }
            }, null);
        }

        void IInternalTransform.RemoveTransform(Node transform)
        {
            _transforms.Remove(transform);
        }

        int IInternalTransform.GetSiblingIndex()
        {
            return Body.GetSiblingIndex();
        }

        void IInternalTransform.SetSiblingIndex(int index)
        {
            (Body ?? _sleeping)?.SetSiblingIndex(index);
        }
    }

    /// <summary>
    /// A <see cref="Component{T}" /> that represents <see cref="T:UnityEngine.Transform" /> and
    /// <xref href="UnityEngine.Component.gameObject?text=the game object it is attached to" />.
    /// </summary>
    public class Transform : Transform<UnityEngine.Transform>
    {
        /// <summary>Backing store for the <see cref="LocalPosition" /> property.</summary>
        public static readonly BindableProperty LocalPositionProperty = CreateBindableBodyProperty<UnityEngine.Vector3>(
            "LocalPosition",
            typeof(Transform),
            (body, value) => body.localPosition = value);

        /// <summary>
        /// A property that represents <see cref="P:UnityEngine.Transform.localPosition" />.
        /// </summary>
        public UnityEngine.Vector3 LocalPosition
        {
            get
            {
                return (UnityEngine.Vector3)GetValue(LocalPositionProperty);
            }

            set
            {
                SetValue(LocalPositionProperty, value);
            }
        }

        /// <inheritdoc />
        protected internal override void AwakeInMainThread()
        {
            Body.localPosition = LocalPosition;
            base.AwakeInMainThread();
        }

        private protected sealed override UnityEngine.Transform AddComponentToInMainThread(UnityEngine.GameObject gameObject)
        {
            return gameObject.transform;
        }
    }
}
