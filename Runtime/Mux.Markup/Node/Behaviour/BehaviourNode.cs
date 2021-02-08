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
    /// <summary>A <see cref="Component" /> that represents <see cref="T:UnityEngine.Behaviour" />.</summary>
    public class Behaviour<T> : Component<T> where T : UnityEngine.Behaviour
    {
        /// <summary>Backing store for the <see cref="Enabled" /> property.</summary>
        public static readonly BindableProperty EnabledProperty = CreateBindableBodyProperty<bool>(
            "Enabled",
            typeof(Behaviour<T>),
            (body, value) => body.enabled = value,
            true);

        /// <summary>A property that represents <see cref="P:UnityEngine.Behaviour.enabled" />.</summary>
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

        /// <inheritdoc />
        /// <remarks>
        /// This sets <see cref="P:UnityEngine.Behaviour.enabled" /> which may prevents from setting other
        /// properties if the value is <c>false</c>. Therefore, this should be called after setting those
        /// properties in an overriding method.
        /// </remarks>
        protected internal override void AwakeInMainThread()
        {
            base.AwakeInMainThread();
            Body.enabled = Enabled;
        }
    }
}
