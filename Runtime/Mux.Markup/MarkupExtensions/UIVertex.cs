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
    /// that represents <see cref="T:UnityEngine.UIVertex" />.
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
    ///             <mue:UIVertexTriangleStream>
    ///                 <!--
    ///                     Wrapping with mue:UIVertexTriangleStream.Verts
    ///                     for the same reason
    ///                 -->
    ///                 <mue:UIVertexTriangleStream.Verts>
    ///                     <m:UIVertex Position="{m:Vector3 X=-0.5, Y=-0.5, Z=0}" />
    ///                     <m:UIVertex Position="{m:Vector3 X=0, Y=0.5, Z=0}" />
    ///                     <m:UIVertex Position="{m:Vector3 X=0.5, Y=-0.5, Z=0}" />
    ///                 </mue:UIVertexTriangleStream.Verts>
    ///             </mue:UIVertexTriangleStream>
    ///         </mue:UIMesh.Items>
    ///     </mue:UIMesh>
    /// </m:RectTransform>
    /// ]]>
    /// </code>
    /// </example>
    [AcceptEmptyServiceProvider]
    public class UIVertex : IMarkupExtension<UnityEngine.UIVertex>
    {
        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.UIVertex.position" />.
        /// </summary>
        public UnityEngine.Vector3 Position { get; set; } = UnityEngine.UIVertex.simpleVert.position;

        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.UIVertex.normal" />.
        /// </summary>
        public UnityEngine.Vector3 Normal { get; set; } = UnityEngine.UIVertex.simpleVert.normal;

        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.UIVertex.tangent" />.
        /// </summary>
        public UnityEngine.Vector4 Tangent { get; set; } = UnityEngine.UIVertex.simpleVert.tangent;

        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.UIVertex.color" />.
        /// </summary>
        public UnityEngine.Color32 Color { get; set; } = UnityEngine.UIVertex.simpleVert.color;

        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.UIVertex.uv0" />.
        /// </summary>
        public UnityEngine.Vector2 Uv0 { get; set; } = UnityEngine.UIVertex.simpleVert.uv0;

        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.UIVertex.uv1" />.
        /// </summary>
        public UnityEngine.Vector2 Uv1 { get; set; } = UnityEngine.UIVertex.simpleVert.uv1;

        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.UIVertex.uv2" />.
        /// </summary>
        public UnityEngine.Vector2 Uv2 { get; set; } = UnityEngine.UIVertex.simpleVert.uv2;

        /// <summary>
        /// A property that represents <see cref="F:UnityEngine.UIVertex.uv3" />.
        /// </summary>
        public UnityEngine.Vector2 Uv3 { get; set; } = UnityEngine.UIVertex.simpleVert.uv3;

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        public UnityEngine.UIVertex ProvideValue(IServiceProvider serviceProvider)
        {
            return new UnityEngine.UIVertex
            {
                position = Position,
                normal = Normal,
                tangent = Tangent,
                color = Color,
                uv0 = Uv0,
                uv1 = Uv1,
                uv2 = Uv2,
                uv3 = Uv3
            };
        }
    }
}
