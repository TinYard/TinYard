# TinYard

![.NET](https://github.com/KieranBond/TinYard/workflows/.NET/badge.svg)

## Table Of Contents

* [TinYard](#TinYard)
* [Table Of Contents](#Table-Of-Contents)
* [What is TinYard?](#What-Is-TinYard)
* [Using TinYard](#Using-TinYard)
* [TinYard Internals](#TinYard-Internals)
* [TinYard Extensions](#TinYard-Extensions)
* [How to contribute](#Contribution)
    * [Monetary Contribution](#Funding.)
    * [Coffee Contribution](#Buy-me-a-coffee)



## What is TinYard?

TinYard is a C# Framework.

At its basics, the framework is to help you create any application with an Event System at the core of your application.

The framework provides a few tools to help you do this, such as;

* [Dependency Injection](#IInjector)
* [Event System](#Event-System-Extension)
* [Value Mapping](#ValueMapper)

## Using TinYard

## TinYard Internals

* [IContext](#IContext)
    * [Context](#Context)
    * [Context Exception](#ContextException)
* [IMapper](#IMapper)
    * [ValueMapper](#ValueMapper)
    * [IMappingObject](#IMappingObject)
    * [MappingObject](#MappingObject)
* [IInjector](#IInjector)
    * [TinYard Injector](#TinYardInjector)
    * [Inject Attribute](#Inject-Attribute)
* [IExtension](#IExtension)
* [IConfig](#IConfig)
* [IBundle](#IBundle)

### IContext

An object that inherits [`IContext`](#IContext) should be the hub in which you access TinYard. This should be where you install [Extensions](#IExtension) ([configure](#IConfig) them too), as well as where you [Map](#IMapper) anything of use.

The [`IContext`](#IContext) should also have an [IInjector](#IInjector) that it uses, but you likely won't need this very often unless doing some more complex extensions or debugging. 

[Context](#Context) below, is the de-facto implementation of [`IContext`](#IContext) for TinYard.

Every [`IContext`](#IContext) should provide `event` callbacks to certain parts of its [`Initialize`](#Initalize) method. This will allow you to have more control over your [`Context`](#IContext).

#### Context

The standard TinYard implementation of [`IContext`](#IContext).

[`Context`](#Context) provides basic implementations of the [`IContext`](#IContext) interface, and does nothing too fancy.

##### Construction

In [`Context`](#Context)'s constructor a new [ValueMapper](#ValueMapper) and [TinYard Injector](#TinYardInjector) are created, which are then accessable as [`Mapper`](#IMapper) and [`Injector`](#IInjector) respectively.

The [`Context`](#Context), [`Mapper`](#IMapper), and [`Injector`](#IInjector) are then all mapped on the [`Mapper`](#IMapper) to their interface equivalents ([`IContext`](#IContext), [`IMapper`](#IMapper), [`IInjector`](#IInjector)) - Allowing them to be injected into any object needing them.

##### Initialize

The [`Context`](#Context) can only be 'Initalized' once - This means you can only call the [`Initalize`](#Initialize) method once, any more calls to it will raise a [Context Exception](#ContextException).

The `Initalize` method has four steps:
* [Install Bundles](#Install-Bundles)
* [Install Extensions](#Install-Extensions)
* [Install Configs](#Install-Configs)
* [Post Initialize](#Post-Initialize)

Each step has two `event` hook that can be subscribed to, one is invoked before the step and the other is invoked afterwards - Except for [Post Initialize](#Post-Initialize).

Hook order:
1. `PreBundlesInstalled`
2. `PostBundlesInstalled`
3. `PreExtensionsInstalled`
4. `PostExtensionsInstalled`
5. `PreConfigsInstalled`
6. `PostConfigsInstalled`
7. `PostInitialize`

##### Install Bundles

Install Bundles is the first of the four steps. It runs first, as it has a direct effect on the next two steps.

[Bundles](#IBundle) should typically be just installing [`IExtension`](#IExtension)'s and [`IConfig`](#IConfig)'s.  

When an [`IBundle`](#IBundle) is installed via the `Install(IBundle bundle)` method, it is added to a `private List<IBundle> _bundlesToInstall`. When the [`Context`](#Context) goes to install the List of `IBundle`'s it simply calls the `Install` method on each and passes itself to the Bundle, which in form then usually calls `Install(IExtension extension)` and `Configure(IConfig config)` onto the [`Context`](#Context), just making the installation of multiple Extensions and Configs that are together a bit tidier.

So, Bundles are simply just wrappers for installing multiple Extensions and Configs. Because of that, this is why they are installed first - So that the actual extensions and configs installation has the extensions and configs from these bundles in their lists.

##### Install Extensions

Similar to the installation of Bundles, there is `private List<IExtension> _extensionsToInstall` which contains all of the [`IExtension`](#IExtension)'s that were added to the [`Context`](#Context) via the `Install(IExtension extension)` method. 

This is why [Installing of bundles](####Install-Bundles) happens first, those extensions installed in the bundle are added to this list - We don't want to have to run our Install Extensions method again just for the ones in [`IBundles`](#IBundle).

When we get to the stage of installing all of the [`IExtension`](#IExtension)'s in our list, we iterate over the list and call the `Install` method on the [`IExtension`](#IExtension), passing the Context into the [`IExtension`](#IExtension) via the `Install` method.

Each [`IExtension`](#IExtension) that this method calls upon, then gets removed from the `_extensionsToInstall` list and added to a `private HashSet<IExtension> _extensionsInstalled` - This is so that we can check if it's installed later, using `ContainsExtension(IExtension)` and this also allows us to ensure it hasn't been installed previously.

If an [`IExtension`](#IExtension) is attempted to be installed twice, a [`ContextException`](#ContextException) is thrown.

##### Install Configs

The Install Configs step is almost identical to the [Install Extensions](#Install-Extensions) step, the only difference being that the List and Hashset are containers of [`IConfigs`](#IConfig) and instead of calling `Install` on the Config, the `Configure` method is instead called.

You may also notice that the [`IConfig`](#IConfig) is [Injected](#Inject-Attribute) into, before the `Configure` method is called.

##### Post Initialize

Currently, all that happens here is that the Post Initalize Hook is invoked.

#### ContextException

`ContextException` is an `exception` that should only be thrown from within the [`IContext`](#IContext). It should primarily be thrown only when the User has used an in-proper or unwanted action with the [`IContext`](#IContext).

### IMapper

`IMapper` should be implemented by an object that is going to be providing Mapping functionality.

Mapping is where we can 'map' an object, to another - They're linked.

`IMapper` should also make use of [`IMappingObject`](#IMappingObject)'s to be consistent across implementations.

An example of this is the [`ValueMapper`](#ValueMapper).

#### ValueMapper

`ValueMapper` is an implementation of [`IMapper`](#IMapper), and provides a 'value' to an `interface` or `base` class.

An example of how to use it:

```c-sharp
valueMapper.Map<IContext>().ToValue(context);
```

This example, means that when we request the Value of [`IContext`](#IContext) from the `valueMapper` object, we receive the `context` object - Whatever that is.

`ValueMapper` is used as the primary `IMapper` for [`Context`](#Context).

#### IMappingObject

`IMappingObject` is used in tandem with [`IMapper`](#IMapper). 

`IMappingObject` has two components:

* `MappedType`
* `MappedValue`

`MappedType` is the `type` that this `IMappingObject` is a reference to.

So, looking at the example used in [Value Mapper](#ValueMapper):

When `Map<T>()` is called on the `ValueMapper`, it returns the `IMappingObject`.

Internally, `Map<T>()` calls `Map<T>()` on a newly created `IMappingObject` and then returns this newly created object. This `Map<T>()` method on IMappingObject should set the `MappedType` to the type of `T`.

`MappedValue` is then the value that is set with the `ToValue<T>()` or `ToValue(object value)` methods.

#### MappingObject

`MappingObject` provides a super-simple implementation of [`IMappingObject`](#IMappingObject) that is used by [`ValueMapper`](#ValueMapper). 

### IInjector

An `IInjector` should provide an easy-to-use `Inject` method.

This `Inject` method should provide the object, that has been provided as a parameter, values to any Field that has the [`Inject` attribute](#Inject-Attribute).

How it does so and how it gets the correct value is up to the implementation.

#### TinYardInjector

`TinYardInjector` is the standard implementation of [`IInjector`](#IInjector) used by the [standard `IContext` implementation](#Context).

`TinYardInjector` requires an [`IContext`](#IContext) object to be passed to it when constructed.

`TinYardInjector` provides the 'injected' value of a Field by finding a [`Mapping`](#IMappingObject) of the Field via the [`IContext`](#IContext) provided in construction and the [`IMapper`](#IMapper) that it has.

#### Inject Attribute

The `Inject` attribute can be added to any Field.

The `Inject` attribute acts as a flag to the [`IMapper`](#IMapper) in your [`IContext`](#IContext).

In the standard implementation of [`IContext`](#IContext), [`Context`](#Context) -
When a value is added to an [`IMappingObject`](#IMappingObject), the [`IMapper`](#IMapper) lets the [`IContext`](#IContext) know that the value needs injecting into, which in turn tells its [`IInjector`](#IInjector) to Inject into it.

### IExtension

The `IExtension` interface is used to demonstrate that the class is an Extension that is capable of being installed into an [`IContext`](#IContext), providing extended capabilities.

Typically, this is mostly where [mappings](#IMapper) should occur.

### IConfig

A class implementing `IConfig` should provide configuration to an [`IExtension`](#IExtension), as well as configuration options to further customise an [`IExtension`](#IExtension).

### IBundle

An `IBundle` should be a small class that is used to 'bundle up' specific [`IConfig`](#IConfig) and [`IExtension`](#IExtension)'s together, typically as they are useful together or dependant on each other. 

An `IBundle` is simply to help ensure your [`IContext`](#IContext) installations are kept tidier and easier to maintain.

## TinYard Extensions

TinYard comes with some [Extensions](#IExtension) available to use, packaged within the framework.

The [Extensions](#IExtension) bundled with TinYard include:

* [Event System](#Event-System-Extension)
* [Logging](#Logging-Extension)

These [Extensions](#IExtension) can be installed by installing their respective [Extension](#IExtension) class into the [Context](#IContext).

To find the [Extension](#IExtension) and [Configs](#IConfig) available, look at the appropriate [Extension](#IExtension) section as linked above.

### Event System Extension

### About the Extension

#### Extension and Configurations

To install the [Event System Extension](#Event-System-Extension), install the `EventSystemExtension` class into your [Context](#IContext).

No [Configurations](#IConfig) are available for this [Extension](#IExtension).

### IEvent

An `IEvent` has a `type`, similar to the `type` keyword of C# - But defined via an `Enum`.

This `type` should be used to help determine what the `IEvent` represents.

#### Event

[`Event`](#Event) is an implementation of [`IEvent`](#IEvent).

This implementation simply offers a constructor and the `type` variable - These both can be overloaded.

### IDispatcher

An [`IDispatcher`][(#IDispatcher) should provide the ability to `Dispatch` an [`IEvent`](#IEvent)

### IEventDispatcher

[`IEventDispatcher`](#IEventDispatcher) is an extension of the [`IDispatcher`](#IDispatcher) interface.

It provides the ability to add and remove listeners (and callbacks for these) for [`IEvent`](#IEvent)s, as well as the capability provided by [`IDispatcher`](#IDispatcher). 

#### EventDispatcher

[`EventDispatcher`](#EventDispatcher) is an implementation of the [`IEventDispatcher`](#IEventDispatcher) interface.

#### Listener

[`Listener`](#Listener) is a VO used in the [`IEventDispatcher`](#IEventDispatcher). 

[`Listener`](#Listener) provides an easy way to keep track of callbacks that want invoking for certain [`IEvent`](#IEvent) `type`s, which are added in the [`IEventDispatcher`](#IEventDispatcher).

### Logging Extension

### About the Extension

The [Logging Extension](#Logging-Extension) provides a simple solution to 'logging' whilst using TinYard - Whether that is errors, warnings, or any form of output.

The [Extension](#Logging-Extension) aims to provide you with an [`ILogger`](#ILogger) via the [`Context`](#IContext)'s [`IMapper`](#IMapper). You can access this [`ILogger`](#ILogger) by [`injecting`](#Inject-Attribute) it into your class, or by requesting it from the [`IMapper`](#IMapper) as an [`ILogger`](#ILogger).

#### Extension and Configurations

To install the [Logging Extension](#Logging-Extension), install the `LoggingExtension` class into your [Context](#IContext).

The [Configurations](#IConfig) available for this [Extension](#IExtension) are:

* [`FileLoggingConfig`](#FileLoggingConfig)

##### FileLoggingConfig

The [`FileLoggingConfig`](#FileLoggingConfig) creates and maps a [`FileLogger`](#FileLogger) object to [`ILogger`](#ILogger). 

The [`Config`](#FileLoggingConfig) also provides a few customisation options to the created [`FileLogger`](#FileLogger) such as:

`WithFileDestination(string destination)`
This sets the directory that the log files will be saved into.

If this directory does not currently exist, it will be created.

`WithFileNamePrefix(string prefix)`
This sets a prefix that will be added to log file names.

`WithMaxLogPerFile(int maxLogs)`
This limits how many lines are logged to the file before a new log file is created.

### ILogger

[`ILogger`](#ILogger) is an interface that any logger wanting to be used by the [`LoggingExtension`](#Logging-Extension) has to implement.

It provides access to simple Logging functions.

##### FileLogger

[`FileLogger`](#FileLogger) is an implementation of [`ILogger`](#ILogger).

When used, it adds the logs to files.

On construction, you can set:
* The directory path that is used for log files 
* The maximum number of lines logged per file
* A prefix for the log files name

---

## [Contribution](CONTRIBUTING.md)

### [Code of Conduct](CODE_OF_CONDUCT.md)
### [Funding](.github/FUNDING.yml)
### [Buy me a coffee](https://www.buymeacoffee.com/KieranB)
