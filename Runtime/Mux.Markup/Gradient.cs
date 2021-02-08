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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;

namespace Mux.Markup
{
    /// <summary>
    /// A <see cref="BindableObject" /> that represents <see cref="T:UnityEngine.Gradient" />.
    /// </summary>
    public class Gradient : BindableObject, IEnumerable
    {
        private sealed class GradientKeysCollectionChangedHandler
        {
            private readonly WeakReference<Gradient> _gradient;
            public readonly NotifyCollectionChangedEventHandler alphaHandler;
            public readonly NotifyCollectionChangedEventHandler colorHandler;

            public GradientKeysCollectionChangedHandler(Gradient gradient)
            {
                _gradient = new WeakReference<Gradient>(gradient);

                alphaHandler = (sender, args) =>
                {
                    Gradient value;
                    var alphaKeys = (IEnumerable<UnityEngine.GradientAlphaKey>)sender;

                    if (_gradient.TryGetTarget(out value))
                    {
                        value.Body.alphaKeys = alphaKeys.ToArray();
                    }
                    else
                    {
                        ((INotifyCollectionChanged)sender).CollectionChanged -= alphaHandler;
                    }
                };

                colorHandler = (sender, args) =>
                {
                    Gradient value;
                    var colorKeys = (IEnumerable<UnityEngine.GradientColorKey>)sender;

                    if (_gradient.TryGetTarget(out value))
                    {
                        value.Body.colorKeys = colorKeys.ToArray();
                    }
                    else
                    {
                        ((INotifyCollectionChanged)sender).CollectionChanged -= colorHandler;
                    }
                };
            }
        }

        /// <summary>Backing store for <see cref="Body" /></summary>
        public static readonly BindableProperty BodyProperty = BindableProperty.CreateReadOnly(
            "Body",
            typeof(UnityEngine.Gradient),
            typeof(Gradient),
            null,
            BindingMode.OneWayToSource,
            null,
            null,
            null,
            null,
            CreateDefaultBody).BindableProperty;

        /// <summary>Backing store for <see cref="AlphaKeys" /></summary>
        public static readonly BindableProperty AlphaKeysProperty = BindableProperty.Create(
            "AlphaKeys",
            typeof(IEnumerable<UnityEngine.GradientAlphaKey>),
            typeof(Gradient),
            null,
            BindingMode.OneWay,
            null,
            OnAlphaKeysChanged,
            null,
            null,
            CreateDefaultAlphaKeys);

        /// <summary>Backing store for <see cref="ColorKeys" /></summary>
        public static readonly BindableProperty ColorKeysProperty = BindableProperty.Create(
            "ColorKeys",
            typeof(IEnumerable<UnityEngine.GradientColorKey>),
            typeof(Gradient),
            null,
            BindingMode.OneWay,
            null,
            OnColorKeysChanged,
            null,
            null,
            CreateDefaultColorKeys);

        /// <summary>Backing store for <see cref="Mode" /></summary>
        public static readonly BindableProperty ModeProperty = BindableProperty.Create(
            "Mode",
            typeof(UnityEngine.GradientMode),
            typeof(Gradient),
            null,
            BindingMode.OneWay,
            null,
            OnModeChanged);

        private static object CreateDefaultBody(BindableObject sender)
        {
            return new UnityEngine.Gradient();
        }

        private static object CreateDefaultAlphaKeys(BindableObject sender)
        {
            var keys = new ObservableCollection<UnityEngine.GradientAlphaKey>();
            keys.CollectionChanged += ((Gradient)sender)._handler.alphaHandler;
            return keys;
        }

        private static object CreateDefaultColorKeys(BindableObject sender)
        {
            var keys = new ObservableCollection<UnityEngine.GradientColorKey>();
            keys.CollectionChanged += ((Gradient)sender)._handler.colorHandler;
            return keys;
        }

        private static void OnAlphaKeysChanged(BindableObject sender, object oldValue, object newValue)
        {
            var gradient = (Gradient)sender;
            var notifyingOld = oldValue as INotifyCollectionChanged;

            if (notifyingOld != null)
            {
                notifyingOld.CollectionChanged -= gradient._handler.alphaHandler;
            }

            if (newValue != null)
            {
                var enumerableNew = (IEnumerable<UnityEngine.GradientAlphaKey>)newValue;
                var notifyingNew = newValue as INotifyCollectionChanged;

                if (notifyingNew != null)
                {
                    notifyingNew.CollectionChanged += gradient._handler.alphaHandler;
                }

                gradient.Body.alphaKeys = enumerableNew.ToArray();
            }
        }

        private static void OnColorKeysChanged(BindableObject sender, object oldValue, object newValue)
        {
            var gradient = (Gradient)sender;
            var notifyingOld = oldValue as INotifyCollectionChanged;

            if (notifyingOld != null)
            {
                notifyingOld.CollectionChanged -= gradient._handler.colorHandler;
            }

            if (newValue != null)
            {
                var enumerableNew = (IEnumerable<UnityEngine.GradientColorKey>)newValue;
                var notifyingNew = newValue as INotifyCollectionChanged;

                if (notifyingNew != null)
                {
                    notifyingNew.CollectionChanged += gradient._handler.colorHandler;
                }

                gradient.Body.colorKeys = enumerableNew.ToArray();
            }
        }

        private static void OnModeChanged(BindableObject sender, object oldValue, object newValue)
        {
            ((Gradient)sender).Body.mode = (UnityEngine.GradientMode)newValue;
        }

        private readonly GradientKeysCollectionChangedHandler _handler;

        /// <summary>A property to get or set the underlaying <see cref="T:UnityEngine.Gradient" />.</summary>
        /// <remarks>
        /// <see cref="P:UnityEngine.Gradient.alphaKeys" /> and <see cref="P:UnityEngine.Gradient.colorKeys" />
        /// must not be changed.
        /// </remarks>
        public UnityEngine.Gradient Body => (UnityEngine.Gradient)GetValue(BodyProperty);

        /// <summary>A property that represents <see cref="P:UnityEngine.Gradient.alphaKeys" /></summary>
        /// <seealso cref="GradientAlphaKey" />
        public IEnumerable<UnityEngine.GradientAlphaKey> AlphaKeys
        {
            get
            {
                return (IEnumerable<UnityEngine.GradientAlphaKey>)GetValue(AlphaKeysProperty);
            }

            set
            {
                SetValue(AlphaKeysProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Gradient.colorKeys" /></summary>
        /// <seealso cref="GradientColorKey" />
        public IEnumerable<UnityEngine.GradientColorKey> ColorKeys
        {
            get
            {
                return (IEnumerable<UnityEngine.GradientColorKey>)GetValue(ColorKeysProperty);
            }

            set
            {
                SetValue(ColorKeysProperty, value);
            }
        }

        /// <summary>A property that represents <see cref="P:UnityEngine.Gradient.mode" /></summary>
        public UnityEngine.GradientMode Mode
        {
            get
            {
                return (UnityEngine.GradientMode)GetValue(ModeProperty);
            }

            set
            {
                SetValue(ModeProperty, value);
            }
        }

        public Gradient()
        {
            _handler = new GradientKeysCollectionChangedHandler(this);
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var key in AlphaKeys)
            {
                yield return key;
            }

            foreach (var key in ColorKeys)
            {
                yield return key;
            }
        }

        /// <summary>
        /// A method to add <see cref="T:UnityEngine.GradientAlphaKey" /> or
        /// <see cref="T:UnityEngine.GradientColorKey" />.
        /// </summary>
        /// <remarks>
        /// You can add <see cref="Node" /> as child elements in XAML thanks to this method.
        /// </remarks>
        /// <seealso cref="GradientAlphaKey" />
        /// <seealso cref="GradientColorKey" />
        public void Add(object key)
        {
            if (key is UnityEngine.GradientAlphaKey)
            {
                ((IList)AlphaKeys).Add(key);
                return;
            }

            if (key is UnityEngine.GradientColorKey)
            {
                ((IList)ColorKeys).Add(key);
                return;
            }

            throw new ArgumentException("key");
        }
    }
}
