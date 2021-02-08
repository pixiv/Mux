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
    /// <summary>A <see cref="BindableObject" /> to be represented as XAML tag.</summary>
    public abstract class Node : BindableObject
    {
        /// <summary>
        /// A method to add the <see href="https://docs.unity3d.com/Manual/Components.html">component</see>
        /// to the given <see cref="T:UnityEngine.GameObject" /> and wakes it up.
        /// </summary>
        /// <remarks>
        /// This can be called only once for an instance.
        ///
        /// This adds component or child transform. It makes no other side effects to the given game object.
        /// </remarks>
        /// <param name="gameObject">The game object to contain the component.</param>
        public void AddTo(UnityEngine.GameObject gameObject)
        {
            Forms.mainThread.Send(state =>
            {
                AddToInMainThread(gameObject);
                AwakeInMainThread();
            }, null);
        }

        /// <summary>
        /// A method that actually adds the node to <see cref="T:UnityEngine.GameObject" />
        /// in the main thread.
        /// </summary>
        /// <remarks>The caller must call this method in the main thread.</remarks>
        protected internal abstract void AddToInMainThread(UnityEngine.GameObject gameObject);

        /// <summary>
        /// A methot that actually wakes the node up in the main thread.
        /// </summary>
        /// <remarks>
        /// The caller must call this method in the main thread after calling
        /// <see cref="AddToInMainThread" />.
        /// </remarks>
        protected internal abstract void AwakeInMainThread();

        /// <summary>A method to destroy Mux state in the main thread.</summary>
        /// <remarks>
        /// This method must be called in main thread because it may make operations queued for the main
        /// thread fail. For example, an application of binding will be queued for the main thread. That
        /// would fail if the binding is unapplied before the application.
        /// </remarks>
        protected internal virtual void DestroyMuxInMainThread()
        {
            UnapplyBindings();
        }
    }

    /// <summary>An interface to reload the children and XAML namescope.</summary>
    public interface IReloadable
    {
        /// <summary>A method to reload the children and XAML namescope.</summary>
        /// <remarks>
        /// This method is called for the nearest <see cref="IReloadable" /> ancestors of
        /// <see cref="RectTransform" /> whose XAML files are updated when hot reload is enabled.
        /// </remarks>
        void Reload();
    }
}
