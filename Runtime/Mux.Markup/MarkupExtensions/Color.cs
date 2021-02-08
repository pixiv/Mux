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
    /// that represents <see cref="T:UnityEngine.Color" />.
    /// </summary>
    /// <example>
    /// <code language="xaml">
    /// <![CDATA[
    /// <m:RectTransform
    ///     xmlns="http://xamarin.com/schemas/2014/forms"
    ///     xmlns:m="clr-namespace:Mux.Markup;assembly=Mux.Markup"
    ///     xmlns:mu="clr-namespace:Mux.Markup;assembly=Mux.Markup.UI"
    ///     xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">
    ///     <mu:StandaloneInputModule />
    ///     <mu:Canvas />
    ///     <mu:CanvasScaler UiScale="{mu:ConstantPhysicalSize}" />
    ///     <mu:GraphicRaycaster />
    ///     <mu:Image Color="{m:Color R=0, G=0, B=1}" />
    /// </m:RectTransform>
    /// ]]>
    /// </code>
    /// </example>
    [AcceptEmptyServiceProvider]
    public class Color : IMarkupExtension<UnityEngine.Color>
    {
        /// <summary>A property that represents <see cref="F:UnityEngine.Color.r" />.</summary>
        public float R { get; set; }

        /// <summary>A property that represents <see cref="F:UnityEngine.Color.g" />.</summary>
        public float G { get; set; }

        /// <summary>A property that represents <see href="F:UnityEngine.Color.b" />.</summary>
        public float B { get; set; }

        /// <summary>A property that represents <see href="F:UnityEngine.Color.a" />.</summary>
        public float A { get; set; } = 1;

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        public UnityEngine.Color ProvideValue(IServiceProvider serviceProvider)
        {
            return new UnityEngine.Color(R, G, B, A);
        }
    }

    /// <summary>
    /// A <xref href="Xamarin.Forms.Xaml.IMarkupExtension`1?text=markup extension" />
    /// that represents <see cref="T:UnityEngine.Color32" />.
    /// </summary>
    /// <summary>
    /// A <xref href="Xamarin.Forms.Xaml.IMarkupExtension`1?text=markup extension" />
    /// that represents <see cref="T:UnityEngine.Color" />.
    /// </summary>
    /// <example>
    /// <code language="xaml">
    /// <![CDATA[
    /// <m:RectTransform
    ///     xmlns="http://xamarin.com/schemas/2014/forms"
    ///     xmlns:m="clr-namespace:Mux.Markup;assembly=Mux.Markup"
    ///     xmlns:mu="clr-namespace:Mux.Markup;assembly=Mux.Markup.UI"
    ///     xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">
    ///     <mu:StandaloneInputModule />
    ///     <mu:Canvas />
    ///     <mu:CanvasScaler UiScale="{mu:ConstantPhysicalSize}" />
    ///     <mu:GraphicRaycaster />
    ///     <mu:Image Color="{m:Color32 R=0, G=0, B=255}" />
    /// </m:RectTransform>
    /// ]]>
    /// </code>
    /// </example>
    [AcceptEmptyServiceProvider]
    public sealed class Color32 : IMarkupExtension<UnityEngine.Color32>
    {
        /// <summary>A property that represents <see cref="F:UnityEngine.Color32.r" />.</summary>
        public byte R { get; set; }

        /// <summary>A property that represents <see cref="F:UnityEngine.Color32.g" />.</summary>
        public byte G { get; set; }

        /// <summary>A property that represents <see cref="F:UnityEngine.Color32.b" />.</summary>
        public byte B { get; set; }

        /// <summary>A property that represents <see cref="F:UnityEngine.Color32.a" />.</summary>
        public byte A { get; set; } = 255;

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        public UnityEngine.Color32 ProvideValue(IServiceProvider serviceProvider)
        {
            return new UnityEngine.Color32(R, G, B, A);
        }
    }
}
