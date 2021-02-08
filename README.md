# Mux

Mux provides data binding infrastructure to Unity.

## Features

Mux is useful as a powerful UI toolkit on Unity, integrated with Mux.Markup.UI.

* Fully Open-source including dependencies (except Unity itself)
 * Hackable
* Text-based representation
 * No more clicking and dragging
 * Easy to merge
* Fully managed
 * Full stacktrace
 * High portability
 * High stablity
* Compiler-backed
 * Less overhead
* uGUI-backed
 * Easy to migrate to/from uGUI
 * Powers Unity Editor Inspector
 * Has rich documentation
* Xamarin.Forms-backed
 * Proven data template/data binding infrastructure for UI
 * Has rich documentation
 * Familiar for Xamarin.Forms/WPF developers
* Minimal
 * High maintainability
* Hot Reload
 * Fast iteration

## Installation

Unity Editor must be 2020.2 or possibly newer.

First, add this repository to the [`Project` manifest as `com.pixiv.mux`, a dependency](https://docs.unity3d.com/Manual/upm-dependencies.html).

You will need Mux.Markup.UI (`com.pixiv.mux.markup.ui`) if you would like to integrate Mux with uGUI.
Mux.Markup.Animation (`com.pixiv.mux.markup.animation`) is for animation.

The target framework must be ".NET 4.x".

## API

The following APIs are available:

### Full APIs of [System.Collections.Immutable](https://www.nuget.org/packages/System.Collections.Immutable/)

System.Collections.Immutable "provides collections that are thread safe and guaranteed to never change their contents,
also known as immutable collections."

### Xamarin.Forms XAML Runtime

Xamarin.Forms XAML compiler and markups in [MS-XAML-2009](https://docs.microsoft.com/ja-jp/dotnet/framework/xaml-services/xaml-2009-language-features)
are available. The difference from the original runtime is that it compiles XAML by default.

You can also [write your own markup extension](https://docs.microsoft.com/ja-jp/xamarin/xamarin-forms/xaml/xaml-basics/xaml-markup-extensions)
using [`Xamarin.Forms.Xaml.IMarkupExtension`](https://docs.microsoft.com/ja-jp/dotnet/api/xamarin.forms.xaml.imarkupextension?view=xamarin-forms).
It can be annoted with [`Xamarin.Forms.AcceptEmptyServiceProviderAttribute`](https://docs.microsoft.com/ja-jp/dotnet/api/xamarin.forms.xaml.acceptemptyserviceproviderattribute?view=xamarin-forms),
just like Xamarin.Forms markup extensions are.

[`DataTemplate`](https://docs.microsoft.com/ja-jp/dotnet/api/xamarin.forms.datatemplate?view=xamarin-forms) and
[`DataTemplateSelector`](https://docs.microsoft.com/ja-jp/dotnet/api/xamarin.forms.datatemplateselector?view=xamarin-forms) are available.

### Xamarin.Forms data binding infrastructure

In addition to markups in [MS-XAML-2009], a data binding infrastructure is proted from Xamarin.Forms.
Specifically, it includes:

* [Xamarin.Forms.BindableObject](https://docs.microsoft.com/ja-jp/dotnet/api/xamarin.forms.bindableobject?view=xamarin-forms)
* [Xamarin.Forms.Binding](https://docs.microsoft.com/ja-jp/dotnet/api/xamarin.forms.binding?view=xamarin-forms)
* [Xamarin.Forms.BindingBase](https://docs.microsoft.com/ja-jp/dotnet/api/xamarin.forms.bindingbase?view=xamarin-forms)

### Mux APIs

Mux APIs provide Unity-specific runtime services and markups for Unity objects. Its documentation can be built with
[DocFX](https://dotnet.github.io/docfx/).

#### The playground and caveats

The documentation has a playground and exhibits examples with the component. However, there are something to be noted:

* The playground is built on [an interpreter on IL2CPP](#the-interpreter).
* [Managed bytecode stripping](https://docs.unity3d.com/Manual/IL2CPP-BytecodeStripping.html) is disabled for `Xamarin.Forms.Xaml` and Mux assemblies.

#### Building documentation

Building documentation requires the following:

* MSBuild
* NPM
* Node.js

The following subprojects must be prepared as well:
* Mux.Markup.Animation at `.build/doc`
* Mux.Markup.UI at `.build/doc`
* Xamarin.Forms modified for Mux at `.build`

Run `Build` target of `.build/doc/doc.msbuildproj`. `PATH` environment variable must be configured so that `Unity`,
`npm`, and `npx` can be resolved.

## DocFX Template

DocFX template used for the documentation is available at `.docfx_template`. It is also used for
[VRoid SDK documentation](https://developer.vroid.com/sdk/docs/VRoidSDK.html).

You need to [merge it with the default template of DocFX](https://dotnet.github.io/docfx/tutorial/howto_create_custom_template.html#merge-template-with-default-template)
to make it functional.

## Incompatibility with IL2CPP

### The Interpreter

IL2CPP does not provide the constructor arguments of custom attributes. Due to that restriction,
important attributes such as [Xamarin.Forms.ContentPropertyAttribute](https://docs.microsoft.com/dotnet/api/xamarin.forms.contentpropertyattribute?view=xamarin-forms)
will not work with the interpreter.

A workaround is to use the compiler.

### Data binding

Normal data binding relies on reflection, and Unity linker fails to figure out dependencies on
a property. You have two options to resolve the problem:

1. [Use compiled bindings](https://docs.microsoft.com/xamarin/xamarin-forms/app-fundamentals/data-binding/compiled-bindings).

2. Configure Unity Linker accordingly to prevent properties from being stripped.

See [an article of Unity describing Unity linker](https://docs.unity3d.com/Manual/IL2CPP-BytecodeStripping.html).

## Development

### Coding Style

Mux conforms to [the .NET Core C# Coding Style](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/coding-style.md).
Use [modified CodeFormatter](https://github.com/akihikodaki/codeformatter/releases/tag/v1.0.0-alpha7-aki-r0) to format your code when developing Mux.

```
CodeFormatter /copyright:Copyright.txt CodeFormatter.rsp
```

### Documentation

Provide documentations for public APIs. Please make sure DocFX does not emit any warnings nor errors.

Provide cross references to Unity with hyperlinks. You should check if the number of unresolved
uid is increased by passing `--logLevel Verbose` when building the documentation.

### Xamarin.Forms internal dependencies

Although modifications of Xamarin.Forms made for Mux is minimal, Mux depends on a number of internal interfaces:

* Xamarin.Forms.Build.Tasks.XamlCTask (Note that Mux uses the task *without* full MSBuild.)
* [Xamarin.Forms.BindableObject.SetValueCore](https://docs.microsoft.com/dotnet/api/xamarin.forms.bindableobject.setvaluecore?view=xamarin-forms)
* [Xamarin.Forms.Device.PlatformServices](https://docs.microsoft.com/dotnet/api/xamarin.forms.device.platformservices?view=xamarin-forms)
* [Xamarin.Forms.Device.Info](https://docs.microsoft.com/dotnet/api/xamarin.forms.device.info_1?view=xamarin-forms)
* [Xamarin.Forms.Internals.DeviceInfo](https://docs.microsoft.com/dotnet/api/xamarin.forms.internals.deviceinfo?view=xamarin-forms)
* [Xamarin.Forms.Internals.IPlatformServices](https://docs.microsoft.com/dotnet/api/xamarin.forms.internals.iplatformservices?view=xamarin-forms)
* [Xamarin.Forms.Internals.IResourceDictionary](https://docs.microsoft.com/dotnet/api/xamarin.forms.internals.iresourcedictionary?view=xamarin-forms)
* [Xamarin.Forms.Internals.ISystemResourcesProvider](https://docs.microsoft.com/dotnet/api/xamarin.forms.internals.isystemresourcesprovider?view=xamarin-forms)
* [Xamarin.Forms.Internals.INameScope.FindByName](https://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.internals.inamescope.findbyname?view=xamarin-forms)
* [Xamarin.Forms.Internals.NameScope.GetNameScope](https://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.internals.namescope.getnamescope?view=xamarin-forms)
* [Xamarin.Forms.Internals.ResourceLoader.ResourceProvider2](https://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.internals.resourceloader.resourceprovider2?view=xamarin-forms)
* [Xamarin.Forms.Internals.Ticker](https://docs.microsoft.com/dotnet/api/xamarin.forms.internals.ticker?view=xamarin-forms)
* Xamarin.Forms.ListProxy
* [Xamarin.Forms.Xaml.TypeConversionAttribute](https://docs.microsoft.com/dotnet/api/xamarin.forms.xaml.typeconversionattribute?view=xamarin-forms)
* [Xamarin.Forms.Xaml.XamlResourceIdAttribute](https://docs.microsoft.com/dotnet/api/xamarin.forms.xaml.xamlresourceidattribute?view=xamarin-forms)

Changes for those interfaces must be reviewed when altering Xamarin.Forms libraries.

### External dependencies

Mux includes external dependencies. You can remove existing dependencies and recreate.

Run `Clean` target of `.build/Mux.msbuildproj` to clean all dependencies.

Run its `Build` target *on Mono* to recreate.

## Licenses

See `Licenses` directory or "Licenses" article of the documentation.
