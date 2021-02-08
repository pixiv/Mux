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
    /// that represents <see cref="T:UnityEngine.GradientAlphaKey" />.
    /// </summary>
    [AcceptEmptyServiceProvider]
    public class GradientAlphaKey : IMarkupExtension<UnityEngine.GradientAlphaKey>
    {
        /// <summary>
        /// A property that represents <see cref="P:UnityEngine.GradientAlphaKey.alpha" />
        /// </summary>
        public float Alpha { get; set; }

        /// <summary>
        /// A property that represents <see cref="P:UnityEngine.GradientAlphaKey.time" />
        /// </summary>
        public float Time { get; set; }

        public UnityEngine.GradientAlphaKey ProvideValue(IServiceProvider provider)
        {
            return new UnityEngine.GradientAlphaKey(Alpha, Time);
        }

        object IMarkupExtension.ProvideValue(IServiceProvider provider)
        {
            return ProvideValue(provider);
        }
    }

    /// <summary>
    /// A <xref href="Xamarin.Forms.Xaml.IMarkupExtension`1?text=markup extension" />
    /// that represents <see cref="T:UnityEngine.GradientColorKey" />.
    /// </summary>
    [AcceptEmptyServiceProvider]
    public class GradientColorKey : IMarkupExtension<UnityEngine.GradientColorKey>
    {
        /// <summary>
        /// A property that represents <see cref="P:UnityEngine.GradientColorKey.color" />
        /// </summary>
        public UnityEngine.Color Color { get; set; }

        /// <summary>
        /// A property that represents <see cref="P:UnityEngine.GradientColorKey.time" />
        /// </summary>
        public float Time { get; set; }

        public UnityEngine.GradientColorKey ProvideValue(IServiceProvider provider)
        {
            return new UnityEngine.GradientColorKey(Color, Time);
        }

        object IMarkupExtension.ProvideValue(IServiceProvider provider)
        {
            return ProvideValue(provider);
        }
    }
}
