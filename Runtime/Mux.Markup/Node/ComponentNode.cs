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

namespace Mux.Markup
{
    /// <summary>A <see cref="Node" /> that represents <see cref="T:UnityEngine.Component" />.</summary>
    public class Component<T> : Node where T : UnityEngine.Component
    {
        /// <summary>A class to modify <see cref="T:UnityEngine.Component" />.</summary>
        /// <remarks>
        /// This is used for complex markups that represents multiple properties of the underlaying
        /// <see cref="T:UnityEngine.Component" />.
        /// </remarks>
        public abstract class Modifier : BindableObject
        {
            /// <summary>Backing store for the <see cref="Body" /> property.</summary>
            public static readonly BindableProperty BodyProperty = BindableProperty.Create(
                "Body",
                typeof(T),
                typeof(Modifier),
                null,
                BindingMode.OneWay,
                null,
                OnBoxedBodyChanged);

            private static void OnBoxedBodyChanged(BindableObject sender, object oldValue, object newValue)
            {
                Forms.mainThread.Send(state =>
                {
                    var modifier = (Modifier)state;

                    if (modifier.Body != null)
                    {
                        modifier.InitializeBodyInMainThread();
                    }
                }, sender);
            }

            /// <summary>A property that represents <see cref="T:UnityEngine.Component" /> to be modified.</summary>
            public T Body
            {
                get
                {
                    return (T)GetValue(BodyProperty);
                }

                set
                {
                    SetValue(BodyProperty, value);
                }
            }

            /// <summary>A method to initialize component.</summary>
            /// <remarks>
            /// The caller must call this method in the main thread.
            ///
            /// The caller must ensure <see cref="Body" /> property is not <see langword="null" />.
            /// </remarks>
            protected abstract void InitializeBodyInMainThread();

            /// <summary>A method to destroys Mux state.</summary>
            public void DestroyMux()
            {
                UnapplyBindings();
                Body = null;
            }
        }

        private static readonly BindablePropertyKey s_bodyPropertyKey =
            BindableProperty.CreateReadOnly("Body", typeof(T), typeof(Component<T>), null);

        /// <summary>Backing store for the <see cref="Body" /> property.</summary>
        public static readonly BindableProperty BodyProperty = s_bodyPropertyKey.BindableProperty;

#if !DOCFX
        /// <summary>
        /// A convenient method to create a new instance of the <see cref="BindableProperty" />
        /// class that represents a field or property of the <see cref="Body" />.
        /// </summary>
        /// <param name="property">
        /// The name of the BindableProperty.
        /// </param>
        /// <param name="declarer">
        /// The type of the declaring object.
        /// </param>
        /// <param name="update">
        /// A method to update the component with the given value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value for the property.
        /// </param>
        /// <param name="defaultBindingMode">
        /// The BindingMode to use on SetBinding() if no BindingMode is given. This parameter is optional. Default is BindingMode.OneWay.
        /// </param>
        /// <param name="defaultValueCreator">
        /// A Func used to initialize default value for reference types.
        /// </param>
        protected static BindableProperty CreateBindableBodyProperty<U>(string property, Type declarer, Action<T, U> update, U defaultValue = default(U), BindingMode defaultBindingMode = BindingMode.OneWay, BindableProperty.CreateDefaultValueDelegate defaultValueCreator = null)
        {
            return BindableProperty.Create(property, typeof(U), declarer, defaultValue, defaultBindingMode, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var body = ((Component<T>)bindable).Body;

                if (body != null)
                {
                    Forms.mainThread.Send(state => update(body, (U)state), newValue);
                }
            }, defaultValueCreator: defaultValueCreator);
        }
#endif

        private static BindableProperty.BindingPropertyChangedDelegate s_onBindingModifierPropertyChanged = (sender, oldValue, newValue) =>
        {
            ((Modifier)oldValue)?.DestroyMux();

            if (newValue != null)
            {
                var modifier = (Modifier)newValue;
                modifier.Body = ((Component<T>)sender).Body;
                SetInheritedBindingContext(modifier, sender.BindingContext);
            }
        };

        /// <summary>
        /// A convenient method to create a new instance of the <see cref="BindableProperty" />
        /// class that represents <see cref="Modifier" />.
        /// </summary>
        /// <remarks>
        /// This sets <see cref="Modifier.Body" /> property and lets <see cref="Modifier" /> inherit <see cref="BindableObject.BindingContext" />.
        /// Those states must be maintained by the caller.
        /// </remarks>
        /// <param name="property">
        /// The name of the BindableProperty.
        /// </param>
        /// <param name="declarer">
        /// The type of the declaring object.
        /// </param>
        /// <param name="defaultValueCreator">
        /// A Func used to initialize default value for reference types.
        /// </param>
        protected static BindableProperty CreateBindableModifierProperty(string property, Type declarer, BindableProperty.CreateDefaultValueDelegate defaultValueCreator)
        {
            return BindableProperty.Create(
                property,
                typeof(Modifier),
                declarer,
                null,
                BindingMode.OneWay,
                null,
                s_onBindingModifierPropertyChanged,
                null,
                null,
                defaultValueCreator);
        }

        /// <summary>A property to get or set the underlaying <see cref="T:UnityEngine.Component" />.</summary>
        /// <remarks>
        /// It is expected to be used to bind the <see cref="T:UnityEngine.Component" /> to properties of other <see cref="Node" />.
        /// It causes unpredictable results to using this property for other purposes.
        /// </remarks>
        public T Body
        {
            get
            {
                return (T)GetValue(BodyProperty);
            }

            protected set
            {
                SetValue(s_bodyPropertyKey, value);
            }
        }

        /// <inheritdoc />
        protected internal override void AddToInMainThread(UnityEngine.GameObject gameObject)
        {
            Body = gameObject.AddComponent<T>();
        }

        /// <inheritdoc />
        protected internal override void AwakeInMainThread()
        {
        }
    }
}
