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
using Xamarin.Forms;

namespace Mux.Markup
{
    /// <summary>
    /// An abstract class that represents layout of <see cref="T:UnityEngine.RectTransform" />.
    /// </summary>
    public abstract class RectTransformLayout : RectTransform.Modifier
    {
        internal static readonly BindableProperty IndexProperty = BindableProperty.Create(
            "Index",
            typeof(int),
            typeof(RectTransformLayout),
            0,
            BindingMode.OneWay,
            null,
            OnIndexChanged);

        /// <summary>Backing store for the <see cref="Pivot" /> property.</summary>
        public static readonly BindableProperty PivotProperty = BindableProperty.Create(
            "Pivot",
            typeof(float),
            typeof(RectTransformLayout),
            0.5f,
            BindingMode.OneWay,
            null,
            OnPivotChanged);

        private static void OnIndexChanged(BindableObject sender, object oldValue, object newValue)
        {
            Forms.mainThread.Send(state =>
            {
                var layout = (RectTransformLayout)state;

                if (layout.Body != null)
                {
                    layout.InitializeBodyInMainThread();
                }
            }, sender);
        }

        private static void OnPivotChanged(BindableObject sender, object oldValue, object newValue)
        {
            Forms.mainThread.Send(state =>
            {
                var layout = (RectTransformLayout)state;
                var body = layout.Body;

                if (body != null)
                {
                    body.pivot = layout.Set(body.pivot, layout.Pivot);
                }
            }, sender);
        }

        internal int Index
        {
            get
            {
                return (int)GetValue(IndexProperty);
            }

            set
            {
                SetValue(IndexProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.RectTransform.pivot" />.</summary>
        public float Pivot
        {
            get
            {
                return (float)GetValue(PivotProperty);
            }

            set
            {
                SetValue(PivotProperty, value);
            }
        }

        /// <summary>A method to set the component represented by this object to the given vector.</summary>
        protected UnityEngine.Vector2 Set(UnityEngine.Vector2 vector, float value)
        {
            vector[Index] = value;
            return vector;
        }

        /// <inheritdoc />
        protected override void InitializeBodyInMainThread()
        {
            Body.pivot = Set(Body.pivot, Pivot);
        }
    }

    /// <summary>
    /// A class that represents layout of <see cref="T:UnityEngine.RectTransform" /> with fixed size in an axis.
    /// </summary>
    /// <remarks>
    /// Internally, it sets the same value for <see cref="P:UnityEngine.RectTransform.anchorMin" />
    /// and <see cref="P:UnityEngine.RectTransform.anchorMax" />.
    /// </remarks>
    public class Sized : RectTransformLayout
    {
        /// <summary>Backing store for the <see cref="Anchor" /> property.</summary>
        public static readonly BindableProperty AnchorProperty = BindableProperty.Create(
            "Anchor",
            typeof(float),
            typeof(Sized),
            0.5f,
            BindingMode.OneWay,
            null,
            OnAnchorChanged);

        /// <summary>Backing store for the <see cref="AnchoredPosition" /> property.</summary>
        public static readonly BindableProperty AnchoredPositionProperty = BindableProperty.Create(
            "AnchoredPosition",
            typeof(float),
            typeof(Sized),
            0f,
            BindingMode.OneWay,
            null,
            OnAnchoredPositionChanged);

        /// <summary>Backing store for the <see cref="SizeDelta" /> property.</summary>
        public static readonly BindableProperty SizeDeltaProperty = BindableProperty.Create(
            "SizeDelta",
            typeof(float),
            typeof(Sized),
            0f,
            BindingMode.OneWay,
            null,
            OnSizeDeltaChanged);

        private static void OnAnchorChanged(BindableObject sender, object oldValue, object newValue)
        {
            Forms.mainThread.Send(state =>
            {
                var sized = (Sized)state;
                var body = sized.Body;

                if (body != null)
                {
                    var anchor = sized.Anchor;
                    body.anchorMin = sized.Set(body.anchorMin, anchor);
                    body.anchorMax = sized.Set(body.anchorMax, anchor);
                }
            }, sender);
        }

        private static void OnAnchoredPositionChanged(BindableObject sender, object oldValue, object newValue)
        {
            Forms.mainThread.Send(state =>
            {
                var layout = (Sized)state;
                var body = layout.Body;

                if (body != null)
                {
                    body.anchoredPosition = layout.Set(body.anchoredPosition, layout.AnchoredPosition);
                }
            }, sender);
        }

        private static void OnSizeDeltaChanged(BindableObject sender, object oldValue, object newValue)
        {
            Forms.mainThread.Send(state =>
            {
                var layout = (Sized)state;
                var body = layout.Body;

                if (body != null)
                {
                    body.sizeDelta = layout.Set(body.sizeDelta, layout.SizeDelta);
                }
            }, sender);
        }

        /// <summary>
        /// A property that represents <see cref="P:UnityEngine.RectTransform.anchorMin" />
        /// and <see cref="P:UnityEngine.RectTransform.anchorMax" />.
        /// </summary>
        public float Anchor
        {
            get
            {
                return (float)GetValue(AnchorProperty);
            }

            set
            {
                SetValue(AnchorProperty, value);
            }
        }

        /// <summary>
        /// A property that represents <see cref="P:UnityEngine.RectTransform.anchoredPosition" />.
        /// </summary>
        public float AnchoredPosition
        {
            get
            {
                return (float)GetValue(AnchoredPositionProperty);
            }

            set
            {
                SetValue(AnchoredPositionProperty, value);
            }
        }

        /// <summary>
        /// A property that represents <see cref="P:UnityEngine.RectTransform.sizeDelta" />.
        /// </summary>
        public float SizeDelta
        {
            get
            {
                return (float)GetValue(SizeDeltaProperty);
            }

            set
            {
                SetValue(SizeDeltaProperty, value);
            }
        }

        /// <inheritdoc />
        protected sealed override void InitializeBodyInMainThread()
        {
            base.InitializeBodyInMainThread();

            Body.anchorMin = Set(Body.anchorMin, Anchor);
            Body.anchorMax = Set(Body.anchorMax, Anchor);
            Body.anchoredPosition = Set(Body.anchoredPosition, AnchoredPosition);
            Body.sizeDelta = Set(Body.sizeDelta, SizeDelta);
        }
    }

    /// <summary>
    /// A <xref href="Xamarin.Forms.Xaml.IMarkupExtension`1?text=markup extension" /> that represents layout of
    /// <see cref="T:UnityEngine.RectTransform" /> with variable size in an axis.
    /// </summary>
    public class Stretch : RectTransformLayout
    {
        /// <summary>Backing store for the <see cref="AnchorMin" /> property.</summary>
        public static readonly BindableProperty AnchorMinProperty = BindableProperty.Create(
            "AnchorMin",
            typeof(float),
            typeof(Stretch),
            0f,
            BindingMode.OneWay,
            null,
            OnAnchorMinChanged);

        /// <summary>Backing store for the <see cref="AnchorMax" /> property.</summary>
        public static readonly BindableProperty AnchorMaxProperty = BindableProperty.Create(
            "AnchorMax",
            typeof(float),
            typeof(Stretch),
            1f,
            BindingMode.OneWay,
            null,
            OnAnchorMaxChanged);

        /// <summary>Backing store for the <see cref="OffsetMin" /> property.</summary>
        public static readonly BindableProperty OffsetMinProperty = BindableProperty.Create(
            "OffsetMin",
            typeof(float),
            typeof(Stretch),
            0f,
            BindingMode.OneWay,
            null,
            OnOffsetMinChanged);

        /// <summary>Backing store for the <see cref="OffsetMax" /> property.</summary>
        public static readonly BindableProperty OffsetMaxProperty = BindableProperty.Create(
            "OffsetMax",
            typeof(float),
            typeof(Stretch),
            0f,
            BindingMode.OneWay,
            null,
            OnOffsetMaxChanged);

        private static void OnAnchorMinChanged(BindableObject sender, object oldValue, object newValue)
        {
            Forms.mainThread.Send(state =>
            {
                var stretch = (Stretch)state;
                var body = stretch.Body;

                if (body != null)
                {
                    body.anchorMin = stretch.Set(body.anchorMin, stretch.AnchorMin);
                }
            }, sender);
        }

        private static void OnAnchorMaxChanged(BindableObject sender, object oldValue, object newValue)
        {
            Forms.mainThread.Send(state =>
            {
                var stretch = (Stretch)state;
                var body = stretch.Body;

                if (body != null)
                {
                    body.anchorMax = stretch.Set(body.anchorMax, stretch.AnchorMax);
                }
            }, sender);
        }

        private static void OnOffsetMinChanged(BindableObject sender, object oldValue, object newValue)
        {
            Forms.mainThread.Send(state =>
            {
                var stretch = (Stretch)state;
                var body = stretch.Body;

                if (stretch.Body != null)
                {
                    stretch.UpdateOffset();
                }
            }, sender);
        }

        private static void OnOffsetMaxChanged(BindableObject sender, object oldValue, object newValue)
        {
            Forms.mainThread.Send(state =>
            {
                var stretch = (Stretch)state;

                if (stretch.Body != null)
                {
                    stretch.UpdateOffset();
                }
            }, sender);
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.RectTransform.anchorMin" />.</summary>
        public float AnchorMin
        {
            get
            {
                return (float)GetValue(AnchorMinProperty);
            }

            set
            {
                SetValue(AnchorMinProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.RectTransform.anchorMax" />.</summary>
        public float AnchorMax
        {
            get
            {
                return (float)GetValue(AnchorMaxProperty);
            }

            set
            {
                SetValue(AnchorMaxProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.RectTransform.offsetMin" />.</summary>
        public float OffsetMin
        {
            get
            {
                return (float)GetValue(OffsetMinProperty);
            }

            set
            {
                SetValue(OffsetMinProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.RectTransform.offsetMax" />.</summary>
        public float OffsetMax
        {
            get
            {
                return (float)GetValue(OffsetMaxProperty);
            }

            set
            {
                SetValue(OffsetMaxProperty, value);
            }
        }

        /// <inheritdoc />
        protected sealed override void InitializeBodyInMainThread()
        {
            base.InitializeBodyInMainThread();

            Body.anchorMin = Set(Body.anchorMin, AnchorMin);
            Body.anchorMax = Set(Body.anchorMax, AnchorMax);
            UpdateOffset();
        }

        protected override void OnPropertyChanged(string name)
        {
            base.OnPropertyChanged(name);

            if (name == "Pivot" && Body != null)
            {
                UpdateOffset();
            }
        }

        private void UpdateOffset()
        {
            var sizeDelta = OffsetMax - OffsetMin;
            Body.anchoredPosition = Set(Body.anchoredPosition, OffsetMin + sizeDelta * Pivot);
            Body.sizeDelta = Set(Body.sizeDelta, sizeDelta);
        }
    }
}
