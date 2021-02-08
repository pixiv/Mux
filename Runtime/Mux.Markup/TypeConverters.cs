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
using System.Globalization;

namespace Mux.Markup
{
    /// <summary>
    /// A <xref href="Xamarin.Forms.TypeConverter?text=type converter" />
    /// that converts layer name to integer.
    /// </summary>
    /// <seealso cref="M:UnityEngine.LayerMask.NameToLayer(System.String)" />
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
    ///     <m:RectTransform Layer="UI">
    ///         <mu:Text>
    ///             <mu:Text.Content>
    /// You will see exceptions if you run this on the interpreter.
    /// That is because IL2CPP does not support TypeConversionAttribute.
    ///             </mu:Text.Content>
    ///         </mu:Text>
    ///     </m:RectTransform>
    /// </m:RectTransform>
    /// ]]>
    /// </code>
    /// </example>
    [Xamarin.Forms.Xaml.TypeConversion(typeof(int))]
    public class LayerTypeConverter : Xamarin.Forms.TypeConverter
    {
        /// <inheritdoc />
        [Obsolete("ConvertFrom is obsolete as of Xamarin.Forms version 2.2.0. Please use ConvertFromInvariantString (string) instead.")]
        public override object ConvertFrom(object source)
        {
            return ConvertFromInvariantString((string)source);
        }

        /// <inheritdoc />
        [Obsolete("ConvertFrom is obsolete as of Xamarin.Forms version 2.2.0. Please use ConvertFromInvariantString (string) instead.")]
        public override object ConvertFrom(CultureInfo culture, object source)
        {
            return ConvertFrom(source);
        }

        /// <inheritdoc />
        public override object ConvertFromInvariantString(string source)
        {
            return UnityEngine.LayerMask.NameToLayer(source);
        }
    }
}
