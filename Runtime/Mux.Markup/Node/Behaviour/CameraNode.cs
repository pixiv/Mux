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
    /// <summary>A <see cref="Behaviour{T}" /> that represents <see cref="UnityEngine.Camera" /></summary>
    public class Camera : Behaviour<UnityEngine.Camera>
    {
        /// <summary>Backing store for the <see cref="AllowDynamicResolution" /> property.</summary>
        public static readonly BindableProperty AllowDynamicResolutionProperty = CreateBindableBodyProperty<bool>(
            "AllowDynamicResolution",
            typeof(Camera),
            (body, value) => body.allowDynamicResolution = value);

        /// <summary>Backing store for the <see cref="AllowHDR" /> property.</summary>
        public static readonly BindableProperty AllowHDRProperty = CreateBindableBodyProperty<bool>(
            "AllowHDR",
            typeof(Camera),
            (body, value) => body.allowHDR = value,
            true);

        /// <summary>Backing store for the <see cref="AllowMSAA" /> property.</summary>
        public static readonly BindableProperty AllowMSAAProperty = CreateBindableBodyProperty<bool>(
            "AllowMSAA",
            typeof(Camera),
            (body, value) => body.allowMSAA = value,
            true);

        /// <summary>Backing store for the <see cref="BackgroundColor" /> property.</summary>
        public static readonly BindableProperty BackgroundColorProperty = CreateBindableBodyProperty<UnityEngine.Color>(
            "BackgroundColor",
            typeof(Camera),
            (body, value) => body.backgroundColor = value,
            new UnityEngine.Color(0.192f, 0.302f, 0.475f, 0));

        /// <summary>Backing store for the <see cref="CameraType" /> property.</summary>
        public static readonly BindableProperty CameraTypeProperty = CreateBindableBodyProperty<UnityEngine.CameraType>(
            "CameraType",
            typeof(Camera),
            (body, value) => body.cameraType = value,
            UnityEngine.CameraType.Game);

        /// <summary>Backing store for the <see cref="ClearFlags" /> property.</summary>
        public static readonly BindableProperty ClearFlagsProperty = CreateBindableBodyProperty<UnityEngine.CameraClearFlags>(
            "ClearFlags",
            typeof(Camera),
            (body, value) => body.clearFlags = value,
            UnityEngine.CameraClearFlags.Skybox);

        /// <summary>Backing store for the <see cref="ClearStencilAfterLightingPass" /> property.</summary>
        public static readonly BindableProperty ClearStencilAfterLightingPassProperty = CreateBindableBodyProperty<bool>(
            "ClearStencilAfterLightingPass",
            typeof(Camera),
            (body, value) => body.clearStencilAfterLightingPass = value);

        /// <summary>Backing store for the <see cref="CullingMask" /> property.</summary>
        public static readonly BindableProperty CullingMaskProperty = CreateBindableBodyProperty<int>(
            "CullingMask",
            typeof(Camera),
            (body, value) => body.cullingMask = value,
            -1);

        /// <summary>Backing store for the <see cref="Depth" /> property.</summary>
        public static readonly BindableProperty DepthProperty = CreateBindableBodyProperty<float>(
            "Depth",
            typeof(Camera),
            (body, value) => body.depth = value);

        /// <summary>Backing store for the <see cref="DepthTextureMode" /> property.</summary>
        public static readonly BindableProperty DepthTextureModeProperty = CreateBindableBodyProperty<UnityEngine.DepthTextureMode>(
            "DepthTextureMode",
            typeof(Camera),
            (body, value) => body.depthTextureMode = value,
            UnityEngine.DepthTextureMode.None);

        /// <summary>Backing store for the <see cref="EventMask" /> property.</summary>
        public static readonly BindableProperty EventMaskProperty = CreateBindableBodyProperty<int>(
            "EventMask",
            typeof(Camera),
            (body, value) => body.eventMask = value,
            -1);

        /// <summary>Backing store for the <see cref="FarClipPlane" /> property.</summary>
        public static readonly BindableProperty FarClipPlaneProperty = CreateBindableBodyProperty<float>(
            "FarClipPlane",
            typeof(Camera),
            (body, value) => body.farClipPlane = value,
            1000f);

        /// <summary>Backing store for the <see cref="FieldOfView" /> property.</summary>
        public static readonly BindableProperty FieldOfViewProperty = CreateBindableBodyProperty<float>(
            "FieldOfView",
            typeof(Camera),
            (body, value) => body.fieldOfView = value,
            60f);

        /// <summary>Backing store for the <see cref="FocalLength" /> property.</summary>
        public static readonly BindableProperty FocalLengthProperty = CreateBindableBodyProperty<float>(
            "FocalLength",
            typeof(Camera),
            (body, value) => body.focalLength = value,
            50f);

        /// <summary>Backing store for the <see cref="ForceIntoRenderTexture" /> property.</summary>
        public static readonly BindableProperty ForceIntoRenderTextureProperty = CreateBindableBodyProperty<bool>(
            "ForceIntoRenderTexture",
            typeof(Camera),
            (body, value) => body.forceIntoRenderTexture = value);

        /// <summary>Backing store for the <see cref="GateFit" /> property.</summary>
        public static readonly BindableProperty GateFitProperty = CreateBindableBodyProperty<UnityEngine.Camera.GateFitMode>(
            "GateFit",
            typeof(Camera),
            (body, value) => body.gateFit = value,
            UnityEngine.Camera.GateFitMode.Horizontal);

        /// <summary>Backing store for the <see cref="LayerCullDistances" /> property.</summary>
        public static readonly BindableProperty LayerCullDistancesProperty = CreateBindableBodyProperty<float[]>(
            "LayerCullDistances",
            typeof(Camera),
            (body, value) => body.layerCullDistances = value,
            new float[32]);

        /// <summary>Backing store for the <see cref="LayerCullSpherical" /> property.</summary>
        public static readonly BindableProperty LayerCullSphericalProperty = CreateBindableBodyProperty<bool>(
            "LayerCullSpherical",
            typeof(Camera),
            (body, value) => body.layerCullSpherical = value);

        /// <summary>Backing store for the <see cref="LensShift" /> property.</summary>
        public static readonly BindableProperty LensShiftProperty = CreateBindableBodyProperty<UnityEngine.Vector2>(
            "LensShift",
            typeof(Camera),
            (body, value) => body.lensShift = value);

        /// <summary>Backing store for the <see cref="NearClipPlane" /> property.</summary>
        public static readonly BindableProperty NearClipPlaneProperty = CreateBindableBodyProperty<float>(
            "NearClipPlane",
            typeof(Camera),
            (body, value) => body.nearClipPlane = value,
            0.3f);

        /// <summary>Backing store for the <see cref="OpaqueSortMode" /> property.</summary>
        public static readonly BindableProperty OpaqueSortModeProperty = CreateBindableBodyProperty<UnityEngine.Rendering.OpaqueSortMode>(
            "OpaqueSortMode",
            typeof(Camera),
            (body, value) => body.opaqueSortMode = value,
            UnityEngine.Rendering.OpaqueSortMode.Default);

        /// <summary>Backing store for the <see cref="Orthographic" /> property.</summary>
        public static readonly BindableProperty OrthographicProperty = CreateBindableBodyProperty<bool>(
            "Orthographic",
            typeof(Camera),
            (body, value) => body.orthographic = value);

        /// <summary>Backing store for the <see cref="OrthographicSize" /> property.</summary>
        public static readonly BindableProperty OrthographicSizeProperty = CreateBindableBodyProperty<float>(
            "OrthographicSize",
            typeof(Camera),
            (body, value) => body.orthographicSize = value,
            5f);

        /// <summary>Backing store for the <see cref="Rect" /> property.</summary>
        public static readonly BindableProperty RectProperty = CreateBindableBodyProperty<UnityEngine.Rect>(
            "Rect",
            typeof(Camera),
            (body, value) => body.rect = value,
            new UnityEngine.Rect(0, 0, 1, 1));

        /// <summary>Backing store for the <see cref="RenderingPath" /> property.</summary>
        public static readonly BindableProperty RenderingPathProperty = CreateBindableBodyProperty<UnityEngine.RenderingPath>(
            "RenderingPath",
            typeof(Camera),
            (body, value) => body.renderingPath = value,
            UnityEngine.RenderingPath.UsePlayerSettings);

        /// <summary>Backing store for the <see cref="Scene" /> property.</summary>
        public static readonly BindableProperty SceneProperty = CreateBindableBodyProperty<UnityEngine.SceneManagement.Scene>(
            "Scene",
            typeof(Camera),
            (body, value) => body.scene = value);

        /// <summary>Backing store for the <see cref="SensorSize" /> property.</summary>
        public static readonly BindableProperty SensorSizeProperty = CreateBindableBodyProperty<UnityEngine.Vector2>(
            "SensorSize",
            typeof(Camera),
            (body, value) => body.sensorSize = value,
            new UnityEngine.Vector2(36, 24));

        /// <summary>Backing store for the <see cref="StereoConvergence" /> property.</summary>
        public static readonly BindableProperty StereoConvergenceProperty = CreateBindableBodyProperty<float>(
            "StereoConvergence",
            typeof(Camera),
            (body, value) => body.stereoConvergence = value,
            10);

        /// <summary>Backing store for the <see cref="StereoSeparation" /> property.</summary>
        public static readonly BindableProperty StereoSeparationProperty = CreateBindableBodyProperty<float>(
            "StereoSeparation",
            typeof(Camera),
            (body, value) => body.stereoSeparation = value,
            0.022f);

        /// <summary>Backing store for the <see cref="StereoTargetEye" /> property.</summary>
        public static readonly BindableProperty StereoTargetEyeProperty = CreateBindableBodyProperty<UnityEngine.StereoTargetEyeMask>(
            "StereoTargetEye",
            typeof(Camera),
            (body, value) => body.stereoTargetEye = value,
            UnityEngine.StereoTargetEyeMask.Both);

        /// <summary>Backing store for the <see cref="TargetDisplay" /> property.</summary>
        public static readonly BindableProperty TargetDisplayProperty = CreateBindableBodyProperty<int>(
            "TargetDisplay",
            typeof(Camera),
            (body, value) => body.targetDisplay = value);

        /// <summary>Backing store for the <see cref="TargetTexture" /> property.</summary>
        public static readonly BindableProperty TargetTextureProperty = CreateBindableBodyProperty<UnityEngine.RenderTexture>(
            "TargetTexture",
            typeof(Camera),
            (body, value) => body.targetTexture = value);

        /// <summary>Backing store for the <see cref="TransparencySortAxis" /> property.</summary>
        public static readonly BindableProperty TransparencySortAxisProperty = CreateBindableBodyProperty<UnityEngine.Vector3>(
            "TransparencySortAxis",
            typeof(Camera),
            (body, value) => body.transparencySortAxis = value,
            UnityEngine.Vector3.forward);

        /// <summary>Backing store for the <see cref="TransparencySortMode" /> property.</summary>
        public static readonly BindableProperty TransparencySortModeProperty = CreateBindableBodyProperty<UnityEngine.TransparencySortMode>(
            "TransparencySortMode",
            typeof(Camera),
            (body, value) => body.transparencySortMode = value,
            UnityEngine.TransparencySortMode.Default);

        /// <summary>Backing store for the <see cref="UseJitteredProjectionMatrixForTransparentRendering" /> property.</summary>
        public static readonly BindableProperty UseJitteredProjectionMatrixForTransparentRenderingProperty = CreateBindableBodyProperty<bool>(
            "UseJitteredProjectionMatrixForTransparentRendering",
            typeof(Camera),
            (body, value) => body.useJitteredProjectionMatrixForTransparentRendering = value,
            true);

        /// <summary>Backing store for the <see cref="UseOcclusionCulling" /> property.</summary>
        public static readonly BindableProperty UseOcclusionCullingProperty = CreateBindableBodyProperty<bool>(
            "UseOcclusionCulling",
            typeof(Camera),
            (body, value) => body.useOcclusionCulling = value,
            true);

        /// <summary>Backing store for the <see cref="UsePhysicalProperties" /> property.</summary>
        public static readonly BindableProperty UsePhysicalPropertiesProperty = CreateBindableBodyProperty<bool>(
            "UsePhysicalProperties",
            typeof(Camera),
            (body, value) => body.usePhysicalProperties = value);

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.allowDynamicResolution" />.</summary>
        public bool AllowDynamicResolution
        {
            get
            {
                return (bool)GetValue(AllowDynamicResolutionProperty);
            }

            set
            {
                SetValue(AllowDynamicResolutionProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.allowHDR" />.</summary>
        public bool AllowHDR
        {
            get
            {
                return (bool)GetValue(AllowHDRProperty);
            }

            set
            {
                SetValue(AllowHDRProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.allowMSAA" />.</summary>
        public bool AllowMSAA
        {
            get
            {
                return (bool)GetValue(AllowMSAAProperty);
            }

            set
            {
                SetValue(AllowMSAAProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.backgroundColor" />.</summary>
        /// <seealso cref="Color" />
        /// <seealso cref="Color32" />
        public UnityEngine.Color BackgroundColor
        {
            get
            {
                return (UnityEngine.Color)GetValue(BackgroundColorProperty);
            }

            set
            {
                SetValue(BackgroundColorProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.cameraType" />.</summary>
        public UnityEngine.CameraType CameraType
        {
            get
            {
                return (UnityEngine.CameraType)GetValue(CameraTypeProperty);
            }

            set
            {
                SetValue(CameraTypeProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.clearFlags" />.</summary>
        public UnityEngine.CameraClearFlags ClearFlags
        {
            get
            {
                return (UnityEngine.CameraClearFlags)GetValue(ClearFlagsProperty);
            }

            set
            {
                SetValue(ClearFlagsProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.clearStencilAfterLightingPass" />.</summary>
        public bool ClearStencilAfterLightingPass
        {
            get
            {
                return (bool)GetValue(ClearStencilAfterLightingPassProperty);
            }

            set
            {
                SetValue(ClearStencilAfterLightingPassProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.cullingMask" />.</summary>
        public int CullingMask
        {
            get
            {
                return (int)GetValue(CullingMaskProperty);
            }

            set
            {
                SetValue(CullingMaskProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.depth" />.</summary>
        public float Depth
        {
            get
            {
                return (float)GetValue(DepthProperty);
            }

            set
            {
                SetValue(DepthProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.depthTextureMode" />.</summary>
        public UnityEngine.DepthTextureMode DepthTextureMode
        {
            get
            {
                return (UnityEngine.DepthTextureMode)GetValue(DepthTextureModeProperty);
            }

            set
            {
                SetValue(DepthTextureModeProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.eventMask" />.</summary>
        public int EventMask
        {
            get
            {
                return (int)GetValue(EventMaskProperty);
            }

            set
            {
                SetValue(EventMaskProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.farClipPlane" />.</summary>
        public float FarClipPlane
        {
            get
            {
                return (float)GetValue(FarClipPlaneProperty);
            }

            set
            {
                SetValue(FarClipPlaneProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.fieldOfView" />.</summary>
        public float FieldOfView
        {
            get
            {
                return (float)GetValue(FieldOfViewProperty);
            }

            set
            {
                SetValue(FieldOfViewProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.focalLength" />.</summary>
        public float FocalLength
        {
            get
            {
                return (float)GetValue(FocalLengthProperty);
            }

            set
            {
                SetValue(FocalLengthProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.forceIntoRenderTexture" />.</summary>
        public bool ForceIntoRenderTexture
        {
            get
            {
                return (bool)GetValue(ForceIntoRenderTextureProperty);
            }

            set
            {
                SetValue(ForceIntoRenderTextureProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.gateFit" />.</summary>
        public UnityEngine.Camera.GateFitMode GateFit
        {
            get
            {
                return (UnityEngine.Camera.GateFitMode)GetValue(GateFitProperty);
            }

            set
            {
                SetValue(GateFitProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.layerCullDistances" />.</summary>
        public float[] LayerCullDistances
        {
            get
            {
                return (float[])GetValue(LayerCullDistancesProperty);
            }

            set
            {
                SetValue(LayerCullDistancesProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.layerCullSpherical" />.</summary>
        public bool LayerCullSpherical
        {
            get
            {
                return (bool)GetValue(LayerCullSphericalProperty);
            }

            set
            {
                SetValue(LayerCullSphericalProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.lensShift" />.</summary>
        /// <seealso cref="Vector2" />
        public UnityEngine.Vector2 LensShift
        {
            get
            {
                return (UnityEngine.Vector2)GetValue(LensShiftProperty);
            }

            set
            {
                SetValue(LensShiftProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.nearClipPlane" />.</summary>
        public float NearClipPlane
        {
            get
            {
                return (float)GetValue(NearClipPlaneProperty);
            }

            set
            {
                SetValue(NearClipPlaneProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.opaqueSortMode" />.</summary>
        public UnityEngine.Rendering.OpaqueSortMode OpaqueSortMode
        {
            get
            {
                return (UnityEngine.Rendering.OpaqueSortMode)GetValue(OpaqueSortModeProperty);
            }

            set
            {
                SetValue(OpaqueSortModeProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.orthographic" />.</summary>
        public bool Orthographic
        {
            get
            {
                return (bool)GetValue(OrthographicProperty);
            }

            set
            {
                SetValue(OrthographicProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.orthographicSize" />.</summary>
        public float OrthographicSize
        {
            get
            {
                return (float)GetValue(OrthographicSizeProperty);
            }

            set
            {
                SetValue(OrthographicSizeProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.rect" />.</summary>
        public UnityEngine.Rect Rect
        {
            get
            {
                return (UnityEngine.Rect)GetValue(RectProperty);
            }

            set
            {
                SetValue(RectProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.renderingPath" />.</summary>
        public UnityEngine.RenderingPath RenderingPath
        {
            get
            {
                return (UnityEngine.RenderingPath)GetValue(RenderingPathProperty);
            }

            set
            {
                SetValue(RenderingPathProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.scene" />.</summary>
        public UnityEngine.SceneManagement.Scene Scene
        {
            get
            {
                return (UnityEngine.SceneManagement.Scene)GetValue(SceneProperty);
            }

            set
            {
                SetValue(SceneProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.sensorSize" />.</summary>
        /// <seealso cref="Vector2" />
        public UnityEngine.Vector2 SensorSize
        {
            get
            {
                return (UnityEngine.Vector2)GetValue(SensorSizeProperty);
            }

            set
            {
                SetValue(SensorSizeProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.stereoConvergence" />.</summary>
        public float StereoConvergence
        {
            get
            {
                return (float)GetValue(StereoConvergenceProperty);
            }

            set
            {
                SetValue(StereoConvergenceProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.stereoSeparation" />.</summary>
        public float StereoSeparation
        {
            get
            {
                return (float)GetValue(StereoSeparationProperty);
            }

            set
            {
                SetValue(StereoSeparationProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.stereoTargetEye" />.</summary>
        public UnityEngine.StereoTargetEyeMask StereoTargetEye
        {
            get
            {
                return (UnityEngine.StereoTargetEyeMask)GetValue(StereoTargetEyeProperty);
            }

            set
            {
                SetValue(StereoTargetEyeProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.targetDisplay" />.</summary>
        public int TargetDisplay
        {
            get
            {
                return (int)GetValue(TargetDisplayProperty);
            }

            set
            {
                SetValue(TargetDisplayProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.targetTexture" />.</summary>
        public UnityEngine.RenderTexture TargetTexture
        {
            get
            {
                return (UnityEngine.RenderTexture)GetValue(TargetTextureProperty);
            }

            set
            {
                SetValue(TargetTextureProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.transparencySortAxis" />.</summary>
        /// <seealso cref="Vector3" />
        public UnityEngine.Vector3 TransparencySortAxis
        {
            get
            {
                return (UnityEngine.Vector3)GetValue(TransparencySortAxisProperty);
            }

            set
            {
                SetValue(TransparencySortAxisProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.transparencySortMode" />.</summary>
        public UnityEngine.TransparencySortMode TransparencySortMode
        {
            get
            {
                return (UnityEngine.TransparencySortMode)GetValue(TransparencySortModeProperty);
            }

            set
            {
                SetValue(TransparencySortModeProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.useJitteredProjectionMatrixForTransparentRendering" />.</summary>
        public bool UseJitteredProjectionMatrixForTransparentRendering
        {
            get
            {
                return (bool)GetValue(UseJitteredProjectionMatrixForTransparentRenderingProperty);
            }

            set
            {
                SetValue(UseJitteredProjectionMatrixForTransparentRenderingProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.useOcclusionCulling" />.</summary>
        public bool UseOcclusionCulling
        {
            get
            {
                return (bool)GetValue(UseOcclusionCullingProperty);
            }

            set
            {
                SetValue(UseOcclusionCullingProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Camera.usePhysicalProperties" />.</summary>
        public bool UsePhysicalProperties
        {
            get
            {
                return (bool)GetValue(UsePhysicalPropertiesProperty);
            }

            set
            {
                SetValue(UsePhysicalPropertiesProperty, value);
            }
        }

        /// <inheritdoc />
        protected internal override void AwakeInMainThread()
        {
            Body.allowDynamicResolution = AllowDynamicResolution;
            Body.allowHDR = AllowHDR;
            Body.allowMSAA = AllowMSAA;
            Body.backgroundColor = BackgroundColor;
            Body.cameraType = CameraType;
            Body.clearFlags = ClearFlags;
            Body.clearStencilAfterLightingPass = ClearStencilAfterLightingPass;
            Body.cullingMask = CullingMask;
            Body.depth = Depth;
            Body.depthTextureMode = DepthTextureMode;
            Body.eventMask = EventMask;
            Body.farClipPlane = FarClipPlane;
            Body.fieldOfView = FieldOfView;
            Body.focalLength = FocalLength;
            Body.forceIntoRenderTexture = ForceIntoRenderTexture;
            Body.gateFit = GateFit;
            Body.layerCullDistances = LayerCullDistances;
            Body.layerCullSpherical = LayerCullSpherical;
            Body.lensShift = LensShift;
            Body.nearClipPlane = NearClipPlane;
            Body.opaqueSortMode = OpaqueSortMode;
            Body.orthographic = Orthographic;
            Body.orthographicSize = OrthographicSize;
            Body.rect = Rect;
            Body.renderingPath = RenderingPath;
            Body.scene = Scene;
            Body.sensorSize = SensorSize;
            Body.stereoConvergence = StereoConvergence;
            Body.stereoSeparation = StereoSeparation;
            Body.stereoTargetEye = StereoTargetEye;
            Body.targetDisplay = TargetDisplay;
            Body.targetTexture = TargetTexture;
            Body.useJitteredProjectionMatrixForTransparentRendering = UseJitteredProjectionMatrixForTransparentRendering;
            Body.useOcclusionCulling = UseOcclusionCulling;
            Body.usePhysicalProperties = UsePhysicalProperties;

            if (IsSet(TransparencySortAxisProperty))
            {
                Body.transparencySortAxis = TransparencySortAxis;
            }

            if (IsSet(TransparencySortModeProperty))
            {
                Body.transparencySortMode = TransparencySortMode;
            }

            base.AwakeInMainThread();
        }
    }
}
