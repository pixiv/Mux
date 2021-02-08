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
    /// An <see cref="Renderer{T}" /> that represents <see cref="T:UnityEngine.LineRenderer" />.
    /// </summary>
    public class LineRenderer : Renderer<UnityEngine.LineRenderer>
    {
        /// <summary>Backing store for the <see cref="Alignment" /> property.</summary>
        public static readonly BindableProperty AlignmentProperty = CreateBindableBodyProperty<UnityEngine.LineAlignment>(
            "Alignment",
            typeof(LineRenderer),
            (body, value) => body.alignment = value,
            UnityEngine.LineAlignment.View);

        /// <summary>Backing store for the <see cref="ColorGradient" /> property.</summary>
        public static readonly BindableProperty ColorGradientProperty = CreateBindableBodyProperty<UnityEngine.Gradient>(
            "ColorGradient",
            typeof(LineRenderer),
            (body, value) => body.colorGradient = value,
            new UnityEngine.Gradient());

        /// <summary>Backing store for the <see cref="EndColor" /> property.</summary>
        public static readonly BindableProperty EndColorProperty = CreateBindableBodyProperty<UnityEngine.Color>(
            "EndColor",
            typeof(LineRenderer),
            (body, value) => body.endColor = value,
            UnityEngine.Color.white);

        /// <summary>Backing store for the <see cref="EndWidth" /> property.</summary>
        public static readonly BindableProperty EndWidthProperty = CreateBindableBodyProperty<float>(
            "EndWidth",
            typeof(LineRenderer),
            (body, value) => body.endWidth = value,
            1f);

        /// <summary>Backing store for the <see cref="GenerateLightingData" /> property.</summary>
        public static readonly BindableProperty GenerateLightingDataProperty = CreateBindableBodyProperty<bool>(
            "GenerateLightingData",
            typeof(LineRenderer),
            (body, value) => body.generateLightingData = value,
            false);

        /// <summary>Backing store for the <see cref="Loop" /> property.</summary>
        public static readonly BindableProperty LoopProperty = CreateBindableBodyProperty<bool>(
            "Loop",
            typeof(LineRenderer),
            (body, value) => body.loop = value,
            false);

        /// <summary>Backing store for the <see cref="NumCapVertices" /> property.</summary>
        public static readonly BindableProperty NumCapVerticesProperty = CreateBindableBodyProperty<int>(
            "NumCapVertices",
            typeof(LineRenderer),
            (body, value) => body.numCapVertices = value,
            0);

        /// <summary>Backing store for the <see cref="NumCornerVertices" /> property.</summary>
        public static readonly BindableProperty NumCornerVerticesProperty = CreateBindableBodyProperty<int>(
            "NumCornerVertices",
            typeof(LineRenderer),
            (body, value) => body.numCornerVertices = value,
            0);

        /// <summary>Backing store for the <see cref="PositionCount" /> property.</summary>
        public static readonly BindableProperty PositionCountProperty = CreateBindableBodyProperty<int>(
            "PositionCount",
            typeof(LineRenderer),
            (body, value) => body.positionCount = value,
            2);

        /// <summary>Backing store for the <see cref="ShadowBias" /> property.</summary>
        public static readonly BindableProperty ShadowBiasProperty = CreateBindableBodyProperty<float>(
            "ShadowBias",
            typeof(LineRenderer),
            (body, value) => body.shadowBias = value,
            0.5f);

        /// <summary>Backing store for the <see cref="StartColor" /> property.</summary>
        public static readonly BindableProperty StartColorProperty = CreateBindableBodyProperty<UnityEngine.Color>(
            "StartColor",
            typeof(LineRenderer),
            (body, value) => body.startColor = value,
            UnityEngine.Color.white);

        /// <summary>Backing store for the <see cref="StartWidth" /> property.</summary>
        public static readonly BindableProperty StartWidthProperty = CreateBindableBodyProperty<float>(
            "StartWidth",
            typeof(LineRenderer),
            (body, value) => body.startWidth = value,
            1f);

        /// <summary>Backing store for the <see cref="TextureMode" /> property.</summary>
        public static readonly BindableProperty TextureModeProperty = CreateBindableBodyProperty<UnityEngine.LineTextureMode>(
            "TextureMode",
            typeof(LineRenderer),
            (body, value) => body.textureMode = value,
            UnityEngine.LineTextureMode.Stretch);

        /// <summary>Backing store for the <see cref="UseWorldSpace" /> property.</summary>
        public static readonly BindableProperty UseWorldSpaceProperty = CreateBindableBodyProperty<bool>(
            "UseWorldSpace",
            typeof(LineRenderer),
            (body, value) => body.useWorldSpace = value,
            true);

        /// <summary>Backing store for the <see cref="WidthCurve" /> property.</summary>
        public static readonly BindableProperty WidthCurveProperty = CreateBindableBodyProperty<UnityEngine.AnimationCurve>(
            "WidthCurve",
            typeof(LineRenderer),
            (body, value) => body.widthCurve = value,
            new UnityEngine.AnimationCurve(new UnityEngine.Keyframe(0, 1)));

        /// <summary>Backing store for the <see cref="WidthMultiplier" /> property.</summary>
        public static readonly BindableProperty WidthMultiplierProperty = CreateBindableBodyProperty<float>(
            "WidthMultiplier",
            typeof(LineRenderer),
            (body, value) => body.widthMultiplier = value,
            1f);

        /// <summary>A property that represents <see cref="P:UnityEngine.LineRenderer.alignment" />.</summary>
        public UnityEngine.LineAlignment Alignment
        {
            get
            {
                return (UnityEngine.LineAlignment)GetValue(AlignmentProperty);
            }

            set
            {
                SetValue(AlignmentProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.LineRenderer.colorGradient" />.</summary>
        /// <seealso cref="Gradient" />
        public UnityEngine.Gradient ColorGradient
        {
            get
            {
                return (UnityEngine.Gradient)GetValue(ColorGradientProperty);
            }

            set
            {
                SetValue(ColorGradientProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.LineRenderer.endColor" />.</summary>
        /// <seealso cref="Color" />
        public UnityEngine.Color EndColor
        {
            get
            {
                return (UnityEngine.Color)GetValue(EndColorProperty);
            }

            set
            {
                SetValue(EndColorProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.LineRenderer.endWidth" />.</summary>
        public float EndWidth
        {
            get
            {
                return (float)GetValue(EndWidthProperty);
            }

            set
            {
                SetValue(EndWidthProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.LineRenderer.generateLightingData" />.</summary>
        public bool GenerateLightingData
        {
            get
            {
                return (bool)GetValue(GenerateLightingDataProperty);
            }

            set
            {
                SetValue(GenerateLightingDataProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.LineRenderer.loop" />.</summary>
        public bool Loop
        {
            get
            {
                return (bool)GetValue(LoopProperty);
            }

            set
            {
                SetValue(LoopProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.LineRenderer.numCapVertices" />.</summary>
        public int NumCapVertices
        {
            get
            {
                return (int)GetValue(NumCapVerticesProperty);
            }

            set
            {
                SetValue(NumCapVerticesProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.LineRenderer.numCornerVertices" />.</summary>
        public int NumCornerVertices
        {
            get
            {
                return (int)GetValue(NumCornerVerticesProperty);
            }

            set
            {
                SetValue(NumCornerVerticesProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.LineRenderer.positionCount" />.</summary>
        public int PositionCount
        {
            get
            {
                return (int)GetValue(PositionCountProperty);
            }

            set
            {
                SetValue(PositionCountProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.LineRenderer.shadowBias" />.</summary>
        public float ShadowBias
        {
            get
            {
                return (float)GetValue(ShadowBiasProperty);
            }

            set
            {
                SetValue(ShadowBiasProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.LineRenderer.startColor" />.</summary>
        /// <seealso cref="Color" />
        public UnityEngine.Color StartColor
        {
            get
            {
                return (UnityEngine.Color)GetValue(StartColorProperty);
            }

            set
            {
                SetValue(StartColorProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.LineRenderer.startWidth" />.</summary>
        public float StartWidth
        {
            get
            {
                return (float)GetValue(StartWidthProperty);
            }

            set
            {
                SetValue(StartWidthProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.LineRenderer.textureMode" />.</summary>
        public UnityEngine.LineTextureMode TextureMode
        {
            get
            {
                return (UnityEngine.LineTextureMode)GetValue(TextureModeProperty);
            }

            set
            {
                SetValue(TextureModeProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.LineRenderer.useWorldSpace" />.</summary>
        public bool UseWorldSpace
        {
            get
            {
                return (bool)GetValue(UseWorldSpaceProperty);
            }

            set
            {
                SetValue(UseWorldSpaceProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.LineRenderer.widthCurve" />.</summary>
        public UnityEngine.AnimationCurve WidthCurve
        {
            get
            {
                return (UnityEngine.AnimationCurve)GetValue(WidthCurveProperty);
            }

            set
            {
                SetValue(WidthCurveProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.LineRenderer.widthMultiplier" />.</summary>
        public float WidthMultiplier
        {
            get
            {
                return (float)GetValue(WidthMultiplierProperty);
            }

            set
            {
                SetValue(WidthMultiplierProperty, value);
            }
        }

        /// <inheritdoc />
        protected internal override void AwakeInMainThread()
        {
            base.AwakeInMainThread();

            Body.alignment = Alignment;
            Body.colorGradient = ColorGradient;
            Body.endColor = EndColor;
            Body.endWidth = EndWidth;
            Body.generateLightingData = GenerateLightingData;
            Body.loop = Loop;
            Body.numCapVertices = NumCapVertices;
            Body.numCornerVertices = NumCornerVertices;
            Body.positionCount = PositionCount;
            Body.shadowBias = ShadowBias;
            Body.startColor = StartColor;
            Body.startWidth = StartWidth;
            Body.textureMode = TextureMode;
            Body.useWorldSpace = UseWorldSpace;
            Body.widthCurve = WidthCurve;
            Body.widthMultiplier = WidthMultiplier;
        }
    }
}
