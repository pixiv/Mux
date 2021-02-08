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
using Xamarin.Forms.Xaml;

namespace Mux.Markup
{
    /// <summary>
    /// A <xref href="Xamarin.Forms.Xaml.IMarkupExtension`1?text=markup extension" />
    /// that represents <see cref="T:UnityEngine.Vector2" />.
    /// </summary>
    [AcceptEmptyServiceProvider]
    public class Vector2 : IMarkupExtension<UnityEngine.Vector2>
    {
        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.Vector2.x" />.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.Vector2.y" />.
        /// </summary>
        public float Y { get; set; }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        public UnityEngine.Vector2 ProvideValue(IServiceProvider serviceProvider)
        {
            return new UnityEngine.Vector2(X, Y);
        }
    }

    /// <summary>
    /// A <xref href="Xamarin.Forms.Xaml.IMarkupExtension`1?text=markup extension" />
    /// that represents <see cref="T:UnityEngine.Vector3" />.
    /// </summary>
    /// <example>
    /// <code language="xaml">
    /// <![CDATA[
    /// <m:RectTransform
    ///     xmlns="http://xamarin.com/schemas/2014/forms"
    ///     xmlns:m="clr-namespace:Mux.Markup;assembly=Mux.Markup"
    ///     xmlns:mu="clr-namespace:Mux.Markup;assembly=Mux.Markup.UI"
    ///     xmlns:mue="clr-namespace:Mux.Markup.Extras;assembly=Mux.Markup.UI"
    ///     xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">
    ///     <mu:StandaloneInputModule />
    ///     <mu:Canvas />
    ///     <mu:CanvasScaler UiScale="{mu:ConstantPhysicalSize}" />
    ///     <mu:GraphicRaycaster />
    ///     <m:RectTransform LocalEulerAngles="{m:Vector3 X=45, Y=30, Z=15}">
    ///         <mu:Image />
    ///     </m:RectTransform>
    /// </m:RectTransform>
    /// ]]>
    /// </code>
    /// </example>
    [AcceptEmptyServiceProvider]
    public class Vector3 : IMarkupExtension<UnityEngine.Vector3>
    {
        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.Vector3.x" />.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.Vector3.y" />.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.Vector3.z" />.
        /// </summary>
        public float Z { get; set; }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        public UnityEngine.Vector3 ProvideValue(IServiceProvider serviceProvider)
        {
            return new UnityEngine.Vector3(X, Y, Z);
        }
    }

    /// <summary>
    /// A <xref href="Xamarin.Forms.Xaml.IMarkupExtension`1?text=markup extension" />
    /// that represents <see cref="T:UnityEngine.Vector4" />.
    /// </summary>
    [AcceptEmptyServiceProvider]
    public class Vector4 : IMarkupExtension<UnityEngine.Vector4>
    {
        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.Vector4.x" />.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.Vector4.y" />.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.Vector4.z" />.
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.Vector4.w" />.
        /// </summary>
        public float W { get; set; }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        public UnityEngine.Vector4 ProvideValue(IServiceProvider serviceProvider)
        {
            return new UnityEngine.Vector4(X, Y, Z, W);
        }
    }

    /// <summary>
    /// A <xref href="Xamarin.Forms.Xaml.IMarkupExtension`1?text=markup extension" />
    /// that represents <see cref="T:UnityEngine.Vector3Int" />.
    /// </summary>
    /// <example>
    /// <code language="xaml">
    /// <![CDATA[
    /// <m:RectTransform
    ///     xmlns="http://xamarin.com/schemas/2014/forms"
    ///     xmlns:m="clr-namespace:Mux.Markup;assembly=Mux.Markup"
    ///     xmlns:mu="clr-namespace:Mux.Markup;assembly=Mux.Markup.UI"
    ///     xmlns:mue="clr-namespace:Mux.Markup.Extras;assembly=Mux.Markup.UI"
    ///     xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">
    ///     <mu:StandaloneInputModule />
    ///     <mu:Canvas />
    ///     <mu:CanvasScaler UiScale="{mu:ConstantPhysicalSize}" />
    ///     <mu:GraphicRaycaster />
    ///     <mue:UIMesh>
    ///         <!--
    ///             You only have to wrap items with mue:UIMesh.Items
    ///             when you compile the interpreter with IL2CPP.
    ///             It is because ContentPropertyAttribute does not work with IL2CPP.
    ///         -->
    ///         <mue:UIMesh.Items>
    ///             <mue:Triangle Indices="{m:Vector3Int X=0, Y=1, Z=2}" />
    ///             <mue:Vert Value="{m:UIVertex Color={m:Color R=0, G=0, B=1}, Position={m:Vector3 X=-0.5, Y=-0.5, Z=0}}" />
    ///             <mue:Vert Value="{m:UIVertex Color={m:Color R=0, G=1, B=0}, Position={m:Vector3 X=0, Y=0.5, Z=0}}" />
    ///             <mue:Vert Value="{m:UIVertex Color={m:Color R=1, G=0, B=0}, Position={m:Vector3 X=0.5, Y=-0.5, Z=0}}" />
    ///         </mue:UIMesh.Items>
    ///     </mue:UIMesh>
    /// </m:RectTransform>
    /// ]]>
    /// </code>
    /// </example>
    [AcceptEmptyServiceProvider]
    public class Vector3Int : IMarkupExtension<UnityEngine.Vector3Int>
    {
        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.Vector3Int.x" />.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.Vector3Int.y" />.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.Vector3Int.z" />.
        /// </summary>
        public int Z { get; set; }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        public UnityEngine.Vector3Int ProvideValue(IServiceProvider serviceProvider)
        {
            return new UnityEngine.Vector3Int(X, Y, Z);
        }
    }
}
