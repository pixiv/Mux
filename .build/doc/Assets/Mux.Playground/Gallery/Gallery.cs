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
using UnityEngine;

namespace Mux.Playground
{
    internal sealed class Gallery : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private Resources _resources;
#pragma warning restore 0649

        private readonly GalleryViewModel _viewModel = new GalleryViewModel();

        private void Awake()
        {
            var transform = new GalleryTransform();
            transform.BindingContext = _viewModel;
            _viewModel.Resources = _resources;
            transform.AddTo(gameObject);
        }

        private void OnValidate()
        {
            _viewModel.Resources = _resources;
        }
    }

    /// <summary>
    /// A class that implements the root node of the gallery.
    /// </summary>
    /// <remarks>
    /// This class implements <see cref="IReloadable" />. When a XAML of any
    /// child is modified, this component will be reloaded, which effectively
    /// reloads all children.
    /// </remarks>
    internal sealed class GalleryTransform : Mux.Markup.Transform, IReloadable
    {
        [XamlImport] private extern void InitializeComponent();

        public GalleryTransform()
        {
            InitializeComponent();
        }

        public void Reload()
        {
            InitializeComponent();
        }
    }

    internal sealed class GalleryViewModel : ViewModel
    {
    }
}
