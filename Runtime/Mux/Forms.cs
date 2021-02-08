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

using Mux.Platform;
using System.Threading;
using UnityEngine;

namespace Mux
{
    /// <summary>A static class to initialize Xamarin.Forms for Unity.</summary>
    public static class Forms
    {
        /// <summary>A <see cref="SynchronizationContext" /> that represents the main thread.</summary>
        /// <remarks>
        /// This field is not thread-safe.
        ///
        /// This must be set before calling any methods.
        /// </remarks>
        public static SynchronizationContext mainThread;

        /// <summary>A static method to initialize Xamarin.Forms for Unity.</summary>
        /// <remarks>
        /// This method is not thread-safe.
        ///
        /// This must be called before calling any other methods.
        /// </remarks>
        public static void Init()
        {
            Xamarin.Forms.Internals.Log.Listeners.Add(new LogListener());
            Xamarin.Forms.DependencyService.Register<Xamarin.Forms.Internals.ISystemResourcesProvider, ResourcesProvider>();
            Xamarin.Forms.Device.PlatformServices = new PlatformServices();
            Xamarin.Forms.Device.Info = new DeviceInfo();
        }
    }
}
