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

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mux.Playground
{
    /// <summary>
    /// The base class of all view models.
    /// </summary>
    /// <remarks>
    /// Any <see cref="Xamarin.Forms.BindableObject" /> derivatives in
    /// <see cref="Mux.Playground" /> must have a "view model" as
    /// <see cref="Xamarin.Forms.BindableObject.BindingContext" />.
    /// A view model should derive from this class, and its name should be
    /// suffixed with <c>ViewModel</c>. In that way, any view can refer to common
    /// states such as <see cref="Resources" /> without building a singleton.
    /// </remarks>
    internal abstract class ViewModel : INotifyPropertyChanged
    {
        private Resources _resources;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Resources Resources
        {
            get
            {
                return _resources;
            }

            set
            {
                _resources = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
