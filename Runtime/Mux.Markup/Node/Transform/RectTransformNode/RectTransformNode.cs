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

using Xamarin.Forms;

namespace Mux.Markup
{
    /// <summary>
    /// A <see cref="Transform{T}" /> that represents <see cref="T:UnityEngine.RectTransform" /> and
    /// <xref href="UnityEngine.Component.gameObject?text=the game object it is attached to" />.
    /// </summary>
    /// <example>
    /// <code language="xaml">
    /// <![CDATA[
    /// <m:RectTransform
    ///     xmlns="http://xamarin.com/schemas/2014/forms"
    ///     xmlns:m="clr-namespace:Mux.Markup;assembly=Mux.Markup"
    ///     xmlns:mu="clr-namespace:Mux.Markup;assembly=Mux.Markup.UI"
    ///     xmlns:mue="clr-namespace:Mux.Markup.Extras;assembly=Mux.Markup.UI"
    ///     xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    ///     xmlns:playgroundMarkup="clr-namespace:Mux.Playground.Markup;assembly=Assembly-CSharp">
    ///     <!--
    ///       Note that you can use "using" scheme instead of "clr-namespace" to omit assembly
    ///       specification if:
    ///       - the referenced type is in an assembly already loaded. (interpreter)
    ///       - the referenced type is in the assembly containing the compiled XAML. (compiler)
    ///     -->
    ///     <mu:StandaloneInputModule />
    ///     <mu:Canvas />
    ///     <mu:CanvasScaler UiScale="{mu:ConstantPhysicalSize}" />
    ///     <mu:GraphicRaycaster />
    ///     <mu:Image Color="{m:Color R=0, G=0, B=1}" />
    ///     <m:RectTransform X="{m:Stretch}" Y="{m:Sized SizeDelta=99}">
    ///         <mu:Image Color="{m:Color R=0, G=1, B=0}" />
    ///         <playgroundMarkup:TextTransform X="{m:Stretch}" Y="{m:Stretch}">
    ///             <playgroundMarkup:TextTransform.Text>
    /// The green RectTransform has Stretch X;
    /// the width is determined by the blue RectTransform.
    ///
    /// The green RectTransform has Sized Y;
    /// the height is fixed to 99.
    ///             </playgroundMarkup:TextTransform.Text>
    ///         </playgroundMarkup:TextTransform>
    ///     </m:RectTransform>
    /// </m:RectTransform>
    /// ]]>
    /// </code>
    /// </example>
    public class RectTransform : Transform<UnityEngine.RectTransform>
    {
        /// <summary>Backing store for the <see cref="LocalPositionZ" /> property.</summary>
        public static readonly BindableProperty LocalPositionZProperty = CreateBindableBodyProperty<float>(
            "LocalPositionZ",
            typeof(RectTransform),
            (body, value) => body.localPosition = new UnityEngine.Vector3(
                body.localPosition.x,
                body.localPosition.y,
                value),
            0f);

        /// <summary>Backing store for the <see cref="X" /> property.</summary>
        /// <remarks>
        /// Setting <see cref="RectTransformLayout" /> to this property binds its
        /// lifetime to the lifetime of this object.
        /// </remarks>
        public static readonly BindableProperty XProperty = BindableProperty.Create(
            "X",
            typeof(RectTransformLayout),
            typeof(RectTransform),
            null,
            BindingMode.OneWay,
            null,
            OnXChanged,
            null,
            null,
            sender => new Sized { Index = 0, SizeDelta = 100 });

        /// <summary>Backing store for the <see cref="Y" /> property.</summary>
        /// <remarks>
        /// Setting <see cref="RectTransformLayout" /> to this property binds its
        /// lifetime to the lifetime of this object.
        /// </remarks>
        public static readonly BindableProperty YProperty = BindableProperty.Create(
            "Y",
            typeof(RectTransformLayout),
            typeof(RectTransform),
            null,
            BindingMode.OneWay,
            null,
            OnYChanged,
            null,
            null,
            sender => new Sized { Index = 1, SizeDelta = 100 });

        private static void OnXChanged(BindableObject sender, object oldValue, object newValue)
        {
            if (oldValue != null)
            {
                var layout = (RectTransformLayout)oldValue;
                layout.DestroyMux();
            }

            if (newValue != null)
            {
                var layout = (RectTransformLayout)newValue;
                SetInheritedBindingContext(layout, sender.BindingContext);
                layout.Index = 0;
                layout.Body = ((RectTransform)sender).Body;
            }
        }

        private static void OnYChanged(BindableObject sender, object oldValue, object newValue)
        {
            if (oldValue != null)
            {
                var layout = (RectTransformLayout)oldValue;
                layout.DestroyMux();
            }

            if (newValue != null)
            {
                var layout = (RectTransformLayout)newValue;
                SetInheritedBindingContext(layout, sender.BindingContext);
                layout.Index = 1;
                layout.Body = ((RectTransform)sender).Body;
            }
        }

        /// <summary>A property that specifies the position in X axis and the width.</summary>
        /// <seealso cref="Sized" />
        /// <seealso cref="Stretch" />
        public RectTransformLayout X
        {
            get
            {
                return (RectTransformLayout)GetValue(XProperty);
            }

            set
            {
                SetValue(XProperty, value);
            }
        }

        /// <summary>A property that specifies the position in Y axis and the height.</summary>
        /// <seealso cref="Sized" />
        /// <seealso cref="Stretch" />
        public RectTransformLayout Y
        {
            get
            {
                return (RectTransformLayout)GetValue(YProperty);
            }

            set
            {
                SetValue(YProperty, value);
            }
        }

        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.Vector3.z" /> of
        /// <see cref="P:UnityEngine.Transform.localPosition" />.
        /// </summary>
        public float LocalPositionZ
        {
            get
            {
                return (float)GetValue(LocalPositionZProperty);
            }

            set
            {
                SetValue(LocalPositionZProperty, value);
            }
        }

        public RectTransform()
        {
            ClearValue(XProperty);
            ClearValue(YProperty);
        }

        /// <inheritdoc />
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (X != null)
            {
                SetInheritedBindingContext(X, BindingContext);
            }

            if (Y != null)
            {
                SetInheritedBindingContext(Y, BindingContext);
            }
        }

        /// <inheritdoc />
        protected internal override void AwakeInMainThread()
        {
            var localPosition = Body.localPosition;
            localPosition.z = LocalPositionZ;
            Body.localPosition = localPosition;

            if (X != null)
            {
                X.Body = Body;
            }

            if (Y != null)
            {
                Y.Body = Body;
            }

            base.AwakeInMainThread();
        }

        /// <inheritdoc />
        protected internal override void DestroyMuxInMainThread()
        {
            base.DestroyMuxInMainThread();

            X.DestroyMux();
            Y.DestroyMux();
        }

        private protected sealed override UnityEngine.RectTransform AddComponentToInMainThread(UnityEngine.GameObject gameObject)
        {
            return gameObject.AddComponent<UnityEngine.RectTransform>();
        }
    }
}
