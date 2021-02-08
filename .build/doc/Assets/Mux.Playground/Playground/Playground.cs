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

using Mux.Markup;
using Xamarin.Forms.Xaml;

namespace Mux.Playground
{
    internal sealed class Playground : UnityEngine.MonoBehaviour
    {
#pragma warning disable 0649
        [UnityEngine.SerializeField] private Resources _resources;
#pragma warning restore 0649

#if UNITY_WEBGL
        private RectTransform _rect = new RectTransform();
        private readonly PlaygroundViewModel _viewModel = new PlaygroundViewModel();

        private void Awake()
        {
            UnityEngine.WebGLInput.captureAllKeyboardInput = false;
            _viewModel.Resources = _resources;
        }

        private void SetXaml(string xaml)
        {
            _rect.Destroy();
            _rect = new RectTransform();
            _rect.BindingContext = _viewModel;
            _rect.AddTo(gameObject);
            _rect.LoadFromXaml(xaml);
        }

        private void OnValidate()
        {
            _viewModel.Resources = _resources;
        }
#endif
    }

    internal sealed class PlaygroundViewModel : ViewModel
    {
    }
}
