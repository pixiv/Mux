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

using Mux.Markup;
using Xamarin.Forms;

/// <summary>
/// This namespace contains reimplementations of Unity's builtin GameObjects.
/// </summary>

namespace Mux.Playground.Markup
{
    internal sealed class ButtonTransform : RectTransform
    {
        [XamlImport] private extern void InitializeComponent();

        public ButtonTransform()
        {
            InitializeComponent();
        }
    }

    internal sealed class CanvasTransform : RectTransform
    {
        [XamlImport] private extern void InitializeComponent();

        public CanvasTransform()
        {
            InitializeComponent();
        }
    }

    internal sealed class DropdownTransform : RectTransform
    {
        [XamlImport] private extern void InitializeComponent();

        public DropdownTransform()
        {
            InitializeComponent();
        }
    }

    internal class EventSystemTransform : Transform
    {
        [XamlImport] private extern void InitializeComponent();

        public EventSystemTransform()
        {
            InitializeComponent();
        }
    }

    internal sealed class ImageTransform : RectTransform

    {
        [XamlImport] private extern void InitializeComponent();

        public ImageTransform()
        {
            InitializeComponent();
        }
    }

    internal sealed class InputFieldTransform : RectTransform
    {
        [XamlImport] private extern void InitializeComponent();

        public InputFieldTransform()
        {
            InitializeComponent();
        }
    }

    internal class PanelTransform : RectTransform
    {
        [XamlImport] private extern void InitializeComponent();

        public PanelTransform()
        {
            InitializeComponent();
        }
    }

    internal sealed class RawImageTransform : RectTransform
    {
        [XamlImport] private extern void InitializeComponent();

        public RawImageTransform()
        {
            InitializeComponent();
        }
    }

    internal sealed class ScrollViewTransform : PanelTransform
    {
#pragma warning disable 0649
        private RectTransform _viewport;
        private ScrollRect _scrollRect;
#pragma warning restore 0649

        [XamlImport] private extern void InitializeComponent();

        private static void OnContentChanged(BindableObject sender, object oldValue, object newValue)
        {
            var content = new Binding("Body", BindingMode.OneWay, null, null, null, newValue);
            var transform = (ScrollViewTransform)sender;

            ((RectTransform)oldValue)?.Destroy();
            transform._viewport.Add((RectTransform)newValue);
            transform._scrollRect.SetBinding(ScrollRect.ContentProperty, content);
        }

        public ScrollViewTransform()
        {
            InitializeComponent();
            _viewport.Add(Content);
            var content = new Binding("Body", BindingMode.OneWay, null, null, null, Content);
            _scrollRect.SetBinding(ScrollRect.ContentProperty, content);
        }

        public RectTransform Content
        {
            get
            {
                return (RectTransform)GetValue(ContentProperty);
            }

            set
            {
                SetValue(ContentProperty, value);
            }
        }

        public Mux.Markup.ScrollRect.Modifier Inertia
        {
            get
            {
                return (Mux.Markup.ScrollRect.Modifier)GetValue(InertiaProperty);
            }

            set
            {
                SetValue(InertiaProperty, value);
            }
        }

        public Mux.Markup.ScrollRect.Modifier Movement
        {
            get
            {
                return (Mux.Markup.ScrollRect.Modifier)GetValue(MovementProperty);
            }

            set
            {
                SetValue(MovementProperty, value);
            }
        }

        public UnityEngine.UI.ScrollRect.ScrollbarVisibility HorizontalScrollbarVisibility
        {
            get
            {
                return (UnityEngine.UI.ScrollRect.ScrollbarVisibility)GetValue(HorizontalScrollbarVisibilityProperty);
            }

            set
            {
                SetValue(HorizontalScrollbarVisibilityProperty, value);
            }
        }

        public UnityEngine.UI.ScrollRect.ScrollbarVisibility VerticalScrollbarVisibility
        {
            get
            {
                return (UnityEngine.UI.ScrollRect.ScrollbarVisibility)GetValue(VerticalScrollbarVisibilityProperty);
            }

            set
            {
                SetValue(VerticalScrollbarVisibilityProperty, value);
            }
        }

        public static readonly BindableProperty ContentProperty = BindableProperty.Create(
            "Content",
            typeof(RectTransform),
            typeof(ScrollViewTransform),
            null,
            BindingMode.OneWayToSource,
            null,
            OnContentChanged,
            null,
            null,
            sender => new RectTransform
            {
                X = new Mux.Markup.Stretch(),
                Y = new Sized { SizeDelta = 300 }
            });

        public static readonly BindableProperty HorizontalScrollbarVisibilityProperty = BindableProperty.Create(
            "HorizontalScrollbarVisibility",
            typeof(UnityEngine.UI.ScrollRect.ScrollbarVisibility),
            typeof(ScrollViewTransform),
            UnityEngine.UI.ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport);

        public static readonly BindableProperty InertiaProperty = BindableProperty.Create(
            "Intertia",
            typeof(Mux.Markup.ScrollRect.Modifier),
            typeof(ScrollViewTransform),
            null,
            BindingMode.OneWay,
            null,
            null,
            null,
            null,
            sender => new Inertia());

        public static readonly BindableProperty MovementProperty = BindableProperty.Create(
            "Movement",
            typeof(Mux.Markup.ScrollRect.Modifier),
            typeof(ScrollViewTransform),
            null,
            BindingMode.OneWay,
            null,
            null,
            null,
            null,
            sender => new Elastic());

        public static readonly BindableProperty VerticalScrollbarVisibilityProperty = BindableProperty.Create(
            "VerticalScrollbarVisibility",
            typeof(UnityEngine.UI.ScrollRect.ScrollbarVisibility),
            typeof(ScrollViewTransform),
            UnityEngine.UI.ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport);
    }

    internal sealed class ScrollbarTransform : RectTransform
    {
        [XamlImport] private extern void InitializeComponent();

        public ScrollbarTransform()
        {
            InitializeComponent();
        }

        public UnityEngine.UI.Scrollbar Scrollbar
        {
            get
            {
                return (UnityEngine.UI.Scrollbar)GetValue(ScrollbarProperty);
            }

            set
            {
                SetValue(ScrollbarProperty, value);
            }
        }

        public UnityEngine.UI.Scrollbar.Direction Direction
        {
            get
            {
                return (UnityEngine.UI.Scrollbar.Direction)GetValue(DirectionProperty);
            }

            set
            {
                SetValue(DirectionProperty, value);
            }
        }

        public static readonly BindableProperty ScrollbarProperty = BindableProperty.Create(
            "Scrollbar",
            typeof(UnityEngine.UI.Scrollbar),
            typeof(ScrollbarTransform),
            null,
            BindingMode.OneWayToSource);

        public static readonly BindableProperty DirectionProperty = BindableProperty.Create(
            "Direction",
            typeof(UnityEngine.UI.Scrollbar.Direction),
            typeof(ScrollbarTransform));
    }

    internal sealed class SliderTransform : RectTransform
    {
        [XamlImport] private extern void InitializeComponent();

        public SliderTransform()
        {
            InitializeComponent();
        }
    }

    internal sealed class TextTransform : RectTransform
    {
        [XamlImport] private extern void InitializeComponent();

        public TextTransform()
        {
            InitializeComponent();
        }

        public UnityEngine.TextAnchor Alignment
        {
            get
            {
                return (UnityEngine.TextAnchor)GetValue(AlignmentProperty);
            }

            set
            {
                SetValue(AlignmentProperty, value);
            }
        }

        public UnityEngine.UI.Text TextComponent
        {
            get
            {
                return (UnityEngine.UI.Text)GetValue(TextComponentProperty);
            }

            set
            {
                SetValue(TextComponentProperty, value);
            }
        }

        public bool SupportRichText
        {
            get
            {
                return (bool)GetValue(SupportRichTextProperty);
            }

            set
            {
                SetValue(SupportRichTextProperty, value);
            }
        }

        public static readonly BindableProperty AlignmentProperty = BindableProperty.Create(
            "Alignment",
            typeof(UnityEngine.TextAnchor),
            typeof(TextTransform));

        public static readonly BindableProperty SupportRichTextProperty = BindableProperty.Create(
            "SupportRichText",
            typeof(bool),
            typeof(TextTransform),
            true);

        public static readonly BindableProperty TextComponentProperty = BindableProperty.Create(
            "TextComponent",
            typeof(UnityEngine.UI.Text),
            typeof(TextTransform),
            null,
            BindingMode.OneWayToSource);

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            "Text",
            typeof(string),
            typeof(TextTransform),
            "New Text");

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }

            set
            {
                SetValue(TextProperty, value);
            }
        }
    }

    internal sealed class ToggleTransform : RectTransform
    {
        [XamlImport] private extern void InitializeComponent();

        public ToggleTransform()
        {
            InitializeComponent();
        }

        public UnityEngine.UI.ToggleGroup Group
        {
            get
            {
                return (UnityEngine.UI.ToggleGroup)GetValue(GroupProperty);
            }

            set
            {
                SetValue(GroupProperty, value);
            }
        }

        public static readonly BindableProperty GroupProperty = BindableProperty.Create(
            "Group",
            typeof(UnityEngine.UI.ToggleGroup),
            typeof(ToggleTransform));
    }
}
