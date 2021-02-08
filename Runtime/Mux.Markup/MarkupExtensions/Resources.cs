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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mux.Markup
{
    /// <summary>A class that represents <see cref="T:UnityEngine.ResourceRequest" />.</summary>
    public sealed class ResourceRequest<T> : BindableObject
    {
        private static readonly BindablePropertyKey s_assetPropertyKey = BindableProperty.CreateReadOnly(
            "Asset",
            typeof(T),
            typeof(ResourceRequest<T>),
            null);

        /// <summary>Backing store for the <see cref="Asset" /> property.</summary>
        public static readonly BindableProperty AssetProperty = s_assetPropertyKey.BindableProperty;

        private static readonly BindablePropertyKey s_isDonePropertyKey = BindableProperty.CreateReadOnly(
            "IsDone",
            typeof(bool),
            typeof(ResourceRequest<T>),
            false);

        /// <summary>Backing store for the <see cref="IsDone" /> property.</summary>
        public static readonly BindableProperty IsDoneProperty = s_isDonePropertyKey.BindableProperty;

        private static readonly BindablePropertyKey s_requestPropertyKey = BindableProperty.CreateReadOnly(
            "Request",
            typeof(UnityEngine.ResourceRequest),
            typeof(ResourceRequest<T>),
            null);

        /// <summary>Backing store for the <see cref="Request" /> property.</summary>
        public static readonly BindableProperty RequestProperty = s_requestPropertyKey.BindableProperty;

        /// <summary>A property that represents <see cref="P:UnityEngine.ResourceRequest.asset" /></summary>
        /// <remarks>
        /// This returns <see langword="null" /> instead of stalling as like
        /// <see cref="P:UnityEngine.ResourceRequest.asset" />.
        /// </remarks>
        public T Asset => (T)GetValue(AssetProperty);

        /// <summary>A property that represents <see cref="P:UnityEngine.AsyncOperation.isDone" /></summary>
        public bool IsDone => (bool)GetValue(IsDoneProperty);

        /// <summary>A property that represents <see cref="T:UnityEngine.ResourceRequest" /></summary>
        public UnityEngine.ResourceRequest Request => (UnityEngine.ResourceRequest)GetValue(RequestProperty);

        internal ResourceRequest(BaseResourceProvider provider)
        {
            Forms.mainThread.Send(state =>
            {
                var request = provider.ProvideRequest();
                SetValue(s_assetPropertyKey, request.isDone ? request.asset : null);
                SetValue(s_isDonePropertyKey, request.isDone);
                SetValue(s_requestPropertyKey, request);
                request.completed += OnCompleted;
            }, null);
        }

        private void OnCompleted(UnityEngine.AsyncOperation operation)
        {
            SetValue(s_assetPropertyKey, ((UnityEngine.ResourceRequest)operation).asset);
            SetValue(s_isDonePropertyKey, operation.isDone);
        }
    }

    /// <summary>An abstract class that represents <see cref="T:UnityEngine.ResourceRequest" />.</summary>
    public abstract class BaseResourceProvider
    {
        /// <summary>
        /// A property that represents <c>path</c> parameter of
        /// <see cref="M:UnityEngine.Resources.LoadAsync(System.String)" />.
        /// </summary>
        public string Path { get; set; }

        /// <summary>A property that represents <see cref="P:UnityEngine.ResourceRequest.asset" />.</summary>
        public BindingBase Asset { get; set; } = null;

        /// <summary>A property that represents <see cref="P:UnityEngine.AsyncOperation.isDone" />.</summary>
        public BindingBase IsDone { get; set; } = null;

        /// <summary>A property to get the underlaying <see cref="T:UnityEngine.ResourceRequest" />.</summary>
        public BindingBase Request { get; set; } = null;

        internal abstract UnityEngine.ResourceRequest ProvideRequest();
    }

    /// <summary>An abstract class that represents <see cref="T:UnityEngine.ResourceRequest" />.</summary>
    public abstract class BaseResourceProvider<T> : BaseResourceProvider, IMarkupExtension<ResourceRequest<T>>
    {
        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        public ResourceRequest<T> ProvideValue(IServiceProvider serviceProvider)
        {
            var request = new ResourceRequest<T>(this);

            if (Asset != null)
            {
                request.SetBinding(ResourceRequest<T>.AssetProperty, Asset);
            }

            if (IsDone != null)
            {
                request.SetBinding(ResourceRequest<T>.IsDoneProperty, IsDone);
            }

            if (Request != null)
            {
                request.SetBinding(ResourceRequest<T>.RequestProperty, Request);
            }

            return request;
        }
    }

    /// <summary>
    /// A <xref href="Xamarin.Forms.Xaml.IMarkupExtension`1?text=markup extension" /> that represents
    /// <see cref="T:UnityEngine.ResourceRequest" />.
    /// </summary>
    [AcceptEmptyServiceProvider]
    [ContentProperty("Path")]
    public class ResourceProvider : BaseResourceProvider<UnityEngine.Object>
    {
        internal sealed override UnityEngine.ResourceRequest ProvideRequest()
        {
            return UnityEngine.Resources.LoadAsync(Path);
        }
    }

    /// <summary>
    /// A <xref href="Xamarin.Forms.Xaml.IMarkupExtension`1?text=markup extension" /> that represents
    /// <see cref="T:UnityEngine.ResourceRequest" />.
    /// </summary>
    [AcceptEmptyServiceProvider]
    [ContentProperty("Path")]
    public class ResourceProvider<T> : BaseResourceProvider<T>
    {
        internal sealed override UnityEngine.ResourceRequest ProvideRequest()
        {
            return UnityEngine.Resources.LoadAsync(Path, typeof(T));
        }
    }
}
