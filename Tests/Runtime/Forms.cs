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

using NUnit.Framework;
using System.Linq;

namespace Mux.Tests
{
    [TestFixture]
    public static class Forms
    {
        [Test]
        public static void Init()
        {
            Mux.Forms.Init();

            Assert.That(Xamarin.Forms.Internals.Log.Listeners.Any(
                listener => listener.GetType() == typeof(Mux.Platform.LogListener)));

            Assert.IsInstanceOf<Mux.Platform.ResourcesProvider>(
                Xamarin.Forms.DependencyService.Get<Xamarin.Forms.Internals.ISystemResourcesProvider>());

            Assert.IsInstanceOf<Mux.Platform.PlatformServices>(
                Xamarin.Forms.Device.PlatformServices);

            Assert.IsInstanceOf<Mux.Platform.DeviceInfo>(
                Xamarin.Forms.Device.Info);
        }
    }
}
