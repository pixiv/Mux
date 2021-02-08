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

namespace Mux.Platform
{
    internal sealed class TickerService : UnityEngine.MonoBehaviour
    {
        private event EventHandler signal;

        public event EventHandler Signal
        {
            add
            {
                var start = signal == null;

                signal += value;

                if (start)
                {
                    StartCoroutine("SendSignals");
                }
            }

            remove
            {
                signal -= value;

                if (signal == null)
                {
                    StopCoroutine("SendSignals");
                }
            }
        }

        public bool paused { get; private set; } = false;

        private System.Collections.IEnumerator SendSignals()
        {
            while (true)
            {
                yield return null;
                signal(this, new EventArgs());
            }
        }

        private void OnApplicationPause(bool paused)
        {
            this.paused = paused;
        }
    }
}
