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
    /// An <see cref="Component{T}" /> that represents <see cref="T:UnityEngine.Renderer" />.
    /// </summary>
    public abstract class Renderer<T> : Component<T> where T : UnityEngine.Renderer
    {
        private static readonly BindablePropertyKey s_materialPropertyKey = BindableProperty.CreateReadOnly(
            "Material",
            typeof(UnityEngine.Material),
            typeof(Renderer<T>),
            null,
            BindingMode.OneWayToSource,
            null,
            null,
            null,
            null,
            sender => ((Renderer<T>)sender).Body.material);

        /// <summary>Backing store for the <see cref="AllowOcclusionWhenDynamic" /> property.</summary>
        public static readonly BindableProperty AllowOcclusionWhenDynamicProperty = CreateBindableBodyProperty<bool>(
            "AllowOcclusionWhenDynamic",
            typeof(Renderer<T>),
            (body, value) => body.allowOcclusionWhenDynamic = value,
            true);

        /// <summary>Backing store for the <see cref="Enabled" /> property.</summary>
        public static readonly BindableProperty EnabledProperty = CreateBindableBodyProperty<bool>(
            "Enabled",
            typeof(Renderer<T>),
            (body, value) => body.enabled = value,
            true);

        /// <summary>Backing store for the <see cref="LightmapIndex" /> property.</summary>
        public static readonly BindableProperty LightmapIndexProperty = CreateBindableBodyProperty<int>(
            "LightmapIndex",
            typeof(Renderer<T>),
            (body, value) => body.lightmapIndex = value,
            -1);

        /// <summary>Backing store for the <see cref="LightmapScaleOffset" /> property.</summary>
        public static readonly BindableProperty LightmapScaleOffsetProperty = CreateBindableBodyProperty<UnityEngine.Vector4>(
            "LightmapScaleOffset",
            typeof(Renderer<T>),
            (body, value) => body.lightmapScaleOffset = value,
            new UnityEngine.Vector4(1, 1, 0, 0));

        /// <summary>Backing store for the <see cref="LightProbeProxyVolumeOverride" /> property.</summary>
        public static readonly BindableProperty LightProbeProxyVolumeOverrideProperty = CreateBindableBodyProperty<UnityEngine.GameObject>(
            "LightProbeProxyVolumeOverride",
            typeof(Renderer<T>),
            (body, value) => body.lightProbeProxyVolumeOverride = value,
            null);

        /// <summary>Backing store for the <see cref="LightProbeUsage" /> property.</summary>
        public static readonly BindableProperty LightProbeUsageProperty = CreateBindableBodyProperty<UnityEngine.Rendering.LightProbeUsage>(
            "LightProbeUsage",
            typeof(Renderer<T>),
            (body, value) => body.lightProbeUsage = value,
            UnityEngine.Rendering.LightProbeUsage.Off);

        /// <summary>Backing store for the <see cref="Material" /> property.</summary>
        public static readonly BindableProperty MaterialProperty = s_materialPropertyKey.BindableProperty;

        /// <summary>Backing store for the <see cref="MotionVectorGenerationMode" /> property.</summary>
        public static readonly BindableProperty MotionVectorGenerationModeProperty = CreateBindableBodyProperty<UnityEngine.MotionVectorGenerationMode>(
            "MotionVectorGenerationMode",
            typeof(Renderer<T>),
            (body, value) => body.motionVectorGenerationMode = value,
            UnityEngine.MotionVectorGenerationMode.Camera);

        /// <summary>Backing store for the <see cref="ProbeAnchor" /> property.</summary>
        public static readonly BindableProperty ProbeAnchorProperty = CreateBindableBodyProperty<UnityEngine.Transform>(
            "ProbeAnchor",
            typeof(Renderer<T>),
            (body, value) => body.probeAnchor = value,
            null);

        /// <summary>Backing store for the <see cref="RealtimeLightmapIndex" /> property.</summary>
        public static readonly BindableProperty RealtimeLightmapIndexProperty = CreateBindableBodyProperty<int>(
            "RealtimeLightmapIndex",
            typeof(Renderer<T>),
            (body, value) => body.realtimeLightmapIndex = value,
            -1);

        /// <summary>Backing store for the <see cref="RealtimeLightmapScaleOffset" /> property.</summary>
        public static readonly BindableProperty RealtimeLightmapScaleOffsetProperty = CreateBindableBodyProperty<UnityEngine.Vector4>(
            "RealtimeLightmapScaleOffset",
            typeof(Renderer<T>),
            (body, value) => body.realtimeLightmapScaleOffset = value,
            new UnityEngine.Vector4(1, 1, 0, 0));

        /// <summary>Backing store for the <see cref="ReceiveShadows" /> property.</summary>
        public static readonly BindableProperty ReceiveShadowsProperty = CreateBindableBodyProperty<bool>(
            "ReceiveShadows",
            typeof(Renderer<T>),
            (body, value) => body.receiveShadows = value,
            true);

        /// <summary>Backing store for the <see cref="ReflectionProbeUsage" /> property.</summary>
        public static readonly BindableProperty ReflectionProbeUsageProperty = CreateBindableBodyProperty<UnityEngine.Rendering.ReflectionProbeUsage>(
            "ReflectionProbeUsage",
            typeof(Renderer<T>),
            (body, value) => body.reflectionProbeUsage = value,
            UnityEngine.Rendering.ReflectionProbeUsage.Off);

        /// <summary>Backing store for the <see cref="RendererPriority" /> property.</summary>
        public static readonly BindableProperty RendererPriorityProperty = CreateBindableBodyProperty<int>(
            "RendererPriority",
            typeof(Renderer<T>),
            (body, value) => body.rendererPriority = value,
            0);

        /// <summary>Backing store for the <see cref="RenderingLayerMask" /> property.</summary>
        public static readonly BindableProperty RenderingLayerMaskProperty = CreateBindableBodyProperty<uint>(
            "RenderingLayerMask",
            typeof(Renderer<T>),
            (body, value) => body.renderingLayerMask = value,
            1);

        /// <summary>Backing store for the <see cref="ShadowCastingMode" /> property.</summary>
        public static readonly BindableProperty ShadowCastingModeProperty = CreateBindableBodyProperty<UnityEngine.Rendering.ShadowCastingMode>(
            "ShadowCastingMode",
            typeof(Renderer<T>),
            (body, value) => body.shadowCastingMode = value,
            UnityEngine.Rendering.ShadowCastingMode.On);

        /// <summary>Backing store for the <see cref="SharedMaterial" /> property.</summary>
        public static readonly BindableProperty SharedMaterialProperty = BindableProperty.Create(
            "SharedMaterial",
            typeof(UnityEngine.Material),
            typeof(Renderer<T>),
            null,
            BindingMode.OneWay,
            null,
            OnSharedMaterialChanged);

        /// <summary>Backing store for the <see cref="SortingLayer" /> property.</summary>
        public static readonly BindableProperty SortingLayerProperty = CreateBindableBodyProperty<int>(
            "SortingLayer",
            typeof(Renderer<T>),
            (body, value) => body.sortingLayerID = value,
            0);

        /// <summary>Backing store for the <see cref="SortingOrder" /> property.</summary>
        public static readonly BindableProperty SortingOrderProperty = CreateBindableBodyProperty<int>(
            "SortingOrder",
            typeof(Renderer<T>),
            (body, value) => body.sortingOrder = value,
            0);

        private static void OnSharedMaterialChanged(BindableObject sender, object oldValue, object newValue)
        {
            var body = ((Renderer<T>)sender).Body;

            if (body != null)
            {
                body.sharedMaterial = (UnityEngine.Material)newValue;

                if (sender.IsSet(MaterialProperty))
                {
                    sender.ClearValue(s_materialPropertyKey);
                }
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Renderer.material" />.</summary>
        public UnityEngine.Material Material => (UnityEngine.Material)GetValue(MaterialProperty);

        /// <summary>A property that represents <see cref="P:UnityEngine.Renderer.allowOcclusionWhenDynamic" />.</summary>
        public bool AllowOcclusionWhenDynamic
        {
            get
            {
                return (bool)GetValue(AllowOcclusionWhenDynamicProperty);
            }

            set
            {
                SetValue(AllowOcclusionWhenDynamicProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Renderer.enabled" />.</summary>
        public bool Enabled
        {
            get
            {
                return (bool)GetValue(EnabledProperty);
            }

            set
            {
                SetValue(EnabledProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Renderer.lightmapIndex" />.</summary>
        public int LightmapIndex
        {
            get
            {
                return (int)GetValue(LightmapIndexProperty);
            }

            set
            {
                SetValue(LightmapIndexProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Renderer.lightmapScaleOffset" />.</summary>
        public UnityEngine.Vector4 LightmapScaleOffset
        {
            get
            {
                return (UnityEngine.Vector4)GetValue(LightmapScaleOffsetProperty);
            }

            set
            {
                SetValue(LightmapScaleOffsetProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Renderer.lightProbeProxyVolumeOverride" />.</summary>
        public UnityEngine.GameObject LightProbeProxyVolumeOverride
        {
            get
            {
                return (UnityEngine.GameObject)GetValue(LightProbeProxyVolumeOverrideProperty);
            }

            set
            {
                SetValue(LightProbeProxyVolumeOverrideProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Renderer.lightProbeUsage" />.</summary>
        public UnityEngine.Rendering.LightProbeUsage LightProbeUsage
        {
            get
            {
                return (UnityEngine.Rendering.LightProbeUsage)GetValue(LightProbeUsageProperty);
            }

            set
            {
                SetValue(LightProbeUsageProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Renderer.motionVectorGenerationMode" />.</summary>
        public UnityEngine.MotionVectorGenerationMode MotionVectorGenerationMode
        {
            get
            {
                return (UnityEngine.MotionVectorGenerationMode)GetValue(MotionVectorGenerationModeProperty);
            }

            set
            {
                SetValue(MotionVectorGenerationModeProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Renderer.probeAnchor" />.</summary>
        public UnityEngine.Transform ProbeAnchor
        {
            get
            {
                return (UnityEngine.Transform)GetValue(ProbeAnchorProperty);
            }

            set
            {
                SetValue(ProbeAnchorProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Renderer.realtimeLightmapIndex" />.</summary>
        public int RealtimeLightmapIndex
        {
            get
            {
                return (int)GetValue(RealtimeLightmapIndexProperty);
            }

            set
            {
                SetValue(RealtimeLightmapIndexProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Renderer.realtimeLightmapScaleOffset" />.</summary>
        public UnityEngine.Vector4 RealtimeLightmapScaleOffset
        {
            get
            {
                return (UnityEngine.Vector4)GetValue(RealtimeLightmapScaleOffsetProperty);
            }

            set
            {
                SetValue(RealtimeLightmapScaleOffsetProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Renderer.receiveShadows" />.</summary>
        public bool ReceiveShadows
        {
            get
            {
                return (bool)GetValue(ReceiveShadowsProperty);
            }

            set
            {
                SetValue(ReceiveShadowsProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Renderer.reflectionProbeUsage" />.</summary>
        public UnityEngine.Rendering.ReflectionProbeUsage ReflectionProbeUsage
        {
            get
            {
                return (UnityEngine.Rendering.ReflectionProbeUsage)GetValue(ReflectionProbeUsageProperty);
            }

            set
            {
                SetValue(ReflectionProbeUsageProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Renderer.rendererPriority" />.</summary>
        public int RendererPriority
        {
            get
            {
                return (int)GetValue(RendererPriorityProperty);
            }

            set
            {
                SetValue(RendererPriorityProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Renderer.renderingLayerMask" />.</summary>
        public uint RenderingLayerMask
        {
            get
            {
                return (uint)GetValue(RenderingLayerMaskProperty);
            }

            set
            {
                SetValue(RenderingLayerMaskProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Renderer.shadowCastingMode" />.</summary>
        public UnityEngine.Rendering.ShadowCastingMode ShadowCastingMode
        {
            get
            {
                return (UnityEngine.Rendering.ShadowCastingMode)GetValue(ShadowCastingModeProperty);
            }

            set
            {
                SetValue(ShadowCastingModeProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Renderer.sharedMaterial" />.</summary>
        public UnityEngine.Material SharedMaterial
        {
            get
            {
                return (UnityEngine.Material)GetValue(SharedMaterialProperty);
            }

            set
            {
                SetValue(SharedMaterialProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Renderer.sortingLayerID" />.</summary>
        public int SortingLayer
        {
            get
            {
                return (int)GetValue(SortingLayerProperty);
            }

            set
            {
                SetValue(SortingLayerProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Renderer.sortingOrder" />.</summary>
        public int SortingOrder
        {
            get
            {
                return (int)GetValue(SortingOrderProperty);
            }

            set
            {
                SetValue(SortingOrderProperty, value);
            }
        }

        /// <inheritdoc />
        protected internal override void AwakeInMainThread()
        {
            base.AwakeInMainThread();

            Body.allowOcclusionWhenDynamic = AllowOcclusionWhenDynamic;
            Body.enabled = Enabled;
            Body.lightmapIndex = LightmapIndex;
            Body.lightmapScaleOffset = LightmapScaleOffset;
            Body.lightProbeProxyVolumeOverride = LightProbeProxyVolumeOverride;
            Body.lightProbeUsage = LightProbeUsage;
            Body.motionVectorGenerationMode = MotionVectorGenerationMode;
            Body.probeAnchor = ProbeAnchor;
            Body.realtimeLightmapIndex = RealtimeLightmapIndex;
            Body.realtimeLightmapScaleOffset = RealtimeLightmapScaleOffset;
            Body.receiveShadows = ReceiveShadows;
            Body.reflectionProbeUsage = ReflectionProbeUsage;
            Body.rendererPriority = RendererPriority;
            Body.renderingLayerMask = RenderingLayerMask;
            Body.shadowCastingMode = ShadowCastingMode;
            Body.sharedMaterial = SharedMaterial;
            Body.sortingLayerID = SortingLayer;
            Body.sortingOrder = SortingOrder;
        }
    }
}
