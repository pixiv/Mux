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

using UnityEngine;

namespace Mux
{
    /// <summary>
    /// A <see cref="System.Diagnostics.TraceListener" /> to log to
    /// <a href="https://docs.unity3d.com/2018.1/Documentation/Manual/Console.html">Unity console</a>.
    /// </summary>
    /// <remarks>
    /// Xamarin.Forms writes a few logs to the trace listeners in <see cref="System.Diagnostics.Debug.Listeners" />.
    /// Adding this listener to the collection would help inestigate Xamarin.Forms related issues if you do not have any other listeners.
    /// </remarks>
    public sealed class TraceListener : System.Diagnostics.TraceListener
    {
        /// <inheritdoc />
        public override void Write(string message)
        {
            Forms.mainThread.Send(Debug.Log, message);
        }

        /// <inheritdoc />
        public override void WriteLine(string message)
        {
            Forms.mainThread.Send(Debug.Log, message);
        }
    }
}
