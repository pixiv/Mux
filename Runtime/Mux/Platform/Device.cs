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

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Xamarin.Forms;

namespace Mux.Platform
{
    internal sealed class Ticker : Xamarin.Forms.Internals.Ticker
    {
        private TickerService _service;
        public override bool SystemEnabled => !_service.paused;

        public Ticker(TickerService service)
        {
            _service = service;
        }

        private void OnSignal(object sender, EventArgs args)
        {
            SendSignals();
        }

        protected override void DisableTimer()
        {
            _service.Signal -= OnSignal;
        }

        protected override void EnableTimer()
        {
            _service.Signal += OnSignal;
        }
    }

    internal class PlatformServices : Xamarin.Forms.Internals.IPlatformServices
    {
        private TickerService _tickerService;
        public bool IsInvokeRequired => false;

        public PlatformServices()
        {
            Forms.mainThread.Post(state =>
            {
                var gameObject = new GameObject { hideFlags = HideFlags.HideAndDontSave };
                _tickerService = gameObject.AddComponent<TickerService>();
            }, null);
        }

        public void BeginInvokeOnMainThread(Action action)
        {
            Forms.mainThread.Send(_ => action(), null);
        }

        public Xamarin.Forms.Internals.Ticker CreateTicker()
        {
            return new Ticker(_tickerService);
        }

        public System.Reflection.Assembly[] GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        public string GetHash(string input)
        {
            throw new NotImplementedException();
        }

        public string GetMD5Hash(string input)
        {
            var builder = new StringBuilder();

            foreach (var element in MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input)))
            {
                builder.Append(element.ToString("x2"));
            }

            return builder.ToString();
        }
        public Xamarin.Forms.Color GetNamedColor(string name)
        {
            return Xamarin.Forms.Color.Default;
        }

        public double GetNamedSize(NamedSize size, Type targetElementType, bool useOldSizes)
        {
            throw new NotImplementedException();
        }

        public SizeRequest GetNativeSize(VisualElement view, double widthConstraint, double heightConstraint)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Xamarin.Forms.Internals.IIsolatedStorageFile GetUserStoreForApplication()
        {
            throw new NotImplementedException();
        }

        public void OpenUriAction(Uri uri)
        {
            Forms.mainThread.Post(state => UnityEngine.Application.OpenURL(uri.AbsoluteUri), null);
        }

        public OSAppTheme RequestedTheme => OSAppTheme.Unspecified;

        public void StartTimer(TimeSpan interval, Func<bool> callback)
        {
            Timer timer = null;

            timer = new Timer((state) =>
            {
                if (!callback())
                    timer.Dispose();
            }, null, new TimeSpan(0), interval);
        }

        public string RuntimePlatform => "Unity";

        public void QuitApplication()
        {
            Forms.mainThread.Send(state => UnityEngine.Application.Quit(), null);
        }
    }

    internal sealed class DeviceInfo : Xamarin.Forms.Internals.DeviceInfo
    {
        public override double DisplayRound(double value) => value;
        public override Xamarin.Forms.Size PixelScreenSize => new Xamarin.Forms.Size(Screen.width, Screen.height);
        public override Xamarin.Forms.Size ScaledScreenSize => new Xamarin.Forms.Size(Screen.width / ScalingFactor, Screen.height / ScalingFactor);
        public override double ScalingFactor => (double)Screen.dpi / 160;
    }
}
