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
using System;
using System.Threading;

namespace Mux.Tests.Platform
{
    [TestFixture]
    public static class Ticker
    {
        [Test]
        public static void SystemEnabled()
        {
            var gameObject = new UnityEngine.GameObject();

            try
            {
                var service = gameObject.AddComponent<Mux.Platform.TickerService>();
                var ticker = new Mux.Platform.Ticker(service);
                Assert.That(ticker.SystemEnabled);
            }
            finally
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
    }

    [TestFixture]
    public static class PlatformServices
    {
        [Test]
        public static void BeginInvokeOnMainThread()
        {
            SynchronizationContext context = null;

            SynchronizationContextWaiter.Execute(() =>
            {
                var services = new Mux.Platform.PlatformServices();

                services.BeginInvokeOnMainThread(
                    () => context = SynchronizationContext.Current);
            });

            Assert.AreEqual(Mux.Forms.mainThread, context);
        }

        [Test]
        public static void CreateTicker()
        {
            SynchronizationContextWaiter.Execute(() =>
            {
                var services = new Mux.Platform.PlatformServices();
                var ticker = services.CreateTicker();
                Assert.IsInstanceOf<Mux.Platform.Ticker>(ticker);
            });
        }

        [Test]
        public static void GetAssemblies()
        {
            SynchronizationContextWaiter.Execute(() =>
            {
                var services = new Mux.Platform.PlatformServices();
                var expected = System.AppDomain.CurrentDomain.GetAssemblies();
                var result = services.GetAssemblies();
                Assert.AreEqual(expected, result);
            });
        }

        [Test]
        public static void GetMD5Hash()
        {
            SynchronizationContextWaiter.Execute(() =>
            {
                var services = new Mux.Platform.PlatformServices();
                Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", services.GetMD5Hash(""));
            });
        }

        [Test]
        public static void GetNamedSize()
        {
            SynchronizationContextWaiter.Execute(() =>
            {
                var services = new Mux.Platform.PlatformServices();

                Assert.Throws<NotImplementedException>(
                    () => services.GetNamedSize(new Xamarin.Forms.NamedSize(), null, false));
            });
        }

        [Test]
        public static void GetStreamAsync()
        {
            SynchronizationContextWaiter.Execute(() =>
            {
                var services = new Mux.Platform.PlatformServices();

                Assert.Throws<NotImplementedException>(
                    () => services.GetStreamAsync(null, new CancellationToken()));
            });
        }

        [Test]
        public static void GetUserStoreForApplication()
        {
            SynchronizationContextWaiter.Execute(() =>
            {
                var services = new Mux.Platform.PlatformServices();

                Assert.Throws<NotImplementedException>(
                    () => services.GetUserStoreForApplication());
            });
        }

        [Test]
        public static void StartTimer()
        {
            SynchronizationContextWaiter.Execute(() =>
            {
                var services = new Mux.Platform.PlatformServices();
                var done = new ManualResetEvent(false);
                var thread = new Thread(() => services.StartTimer(new TimeSpan(2), () =>
                {
                    done.Set();
                    return false;
                }));

                thread.Start();
                done.WaitOne();
            });
        }

        [Test]
        public static void RuntimePlatform()
        {
            SynchronizationContextWaiter.Execute(() =>
            {
                var services = new Mux.Platform.PlatformServices();
                Assert.AreEqual("Unity", services.RuntimePlatform);
            });
        }
    }

    [TestFixture]
    public static class DeviceInfo
    {
        [Test]
        public static void DisplayRound()
        {
            var info = new Mux.Platform.DeviceInfo();
            Assert.AreEqual(0, info.DisplayRound(0));
        }
    }
}
