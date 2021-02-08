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

using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace Mux.Tests
{
    internal static class SynchronizationContextWaiter
    {
        private sealed class SynchronizationContext : System.Threading.SynchronizationContext
        {
            [StructLayout(LayoutKind.Auto)]
            private readonly struct Request
            {
                private readonly System.Threading.SendOrPostCallback _callback;
                private readonly object _state;
                private readonly System.Threading.ManualResetEvent _manualReset;

                public Request(System.Threading.SendOrPostCallback callback, object state, System.Threading.ManualResetEvent manualReset)
                {
                    _callback = callback;
                    _state = state;
                    _manualReset = manualReset;
                }

                public void Execute()
                {
                    _callback(_state);
                    _manualReset?.Set();
                }
            }

            private readonly BlockingCollection<Request> _requests = new BlockingCollection<Request>(new ConcurrentQueue<Request>());
            public readonly System.Threading.SynchronizationContext mainThread;

            public SynchronizationContext(System.Threading.SynchronizationContext mainThread)
            {
                this.mainThread = mainThread;
            }

            public override void Send(System.Threading.SendOrPostCallback callback, object state)
            {
                if (SynchronizationContext.Current == mainThread)
                {
                    callback(state);
                }
                else
                {
                    using (var manualReset = new System.Threading.ManualResetEvent(false))
                    {
                        _requests.Add(new Request(callback, state, manualReset));
                        manualReset.WaitOne();
                    }
                }
            }

            public override void Post(System.Threading.SendOrPostCallback callback, object state)
            {
                _requests.Add(new Request(callback, state, null));
            }

            public void Execute()
            {
                _requests.Take().Execute();
            }
        }

        public static void Execute(System.Action act)
        {
            var execute = true;
            var intercepter = new SynchronizationContext(Mux.Forms.mainThread);

            Mux.Forms.mainThread = intercepter;

            try
            {
                var task = System.Threading.Tasks.Task.Run(() =>
                {
                    try
                    {
                        act();
                    }
                    finally
                    {
                        Mux.Forms.mainThread.Post(state =>
                        {
                            execute = false;
                        }, null);
                    }
                });

                while (execute)
                {
                    intercepter.Execute();
                }

                task.Wait();
            }
            finally
            {
                Mux.Forms.mainThread = intercepter.mainThread;
            }
        }
    }
}
