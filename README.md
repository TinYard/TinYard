# TinYard
[![GitHub license](https://img.shields.io/github/license/TinYard/TinYard.svg)](https://github.com/TinYard/TinYard/blob/master/LICENSE)    
![.NET](https://github.com/KieranBond/TinYard/workflows/.NET/badge.svg) ![.NET Test Suite](https://github.com/KieranBond/TinYard/workflows/.NET%20Test%20Suite/badge.svg)     
![NuGet](https://img.shields.io/nuget/v/TinYard)

## Table Of Contents

* [TinYard](#TinYard)
* [Table Of Contents](#Table-Of-Contents)
* [What is TinYard?](#What-Is-TinYard)
* [Using TinYard](#Using-TinYard)
    * [Getting Started](#Getting-Started)
    * [Example Projects](#Example-Projects)
* [TinYard Internals](#TinYard-Internals)
* [TinYard Extensions](#TinYard-Extensions)
* [TinYard Bundles](#TinYard-Bundles)
* [How to contribute](#Contribution)
    * [Monetary Contribution](#Funding)
    * [Coffee Contribution](#Buy-me-a-coffee)



## What is TinYard?

TinYard is an IoC-container written in C# for .NET that brings more features than just a bog-standard IoC-container.

TinYard aims to improve your code by providing an easy-to-use IoC container and more through its powerful but simple extensibility.
The framework provides a few tools to help you create awesome things, such as:

* [Dependency Injection / IoC](#IInjector)
* [Event System](#Event-System-Extension)
* [Command System](#Command-System-Extension)
* [MVC Suite](#MVC-Bundle)

It's also super easy to add your own [extensions](#TinYard-Extensions), you just have to `install` them when you want to use them!


## Using TinYard

Take a look at the [internals](#TinYard-Internals) below, have a look at the [example projects](#Example-Projects), or read the [getting started guide](#Getting-Started).

### Getting Started

Using TinYard is super simple - especially when only wanting its IoC capabilities.

To get up and running with TinYard all you need is a [`Context`](#Context):

```c#
Context context = new Context();

context.Mapper.Map<IExampleInterface>().ToValue<ExampleImplementation>();
```

With the snippet above setup, you can [`Inject`](#Inject-Attribute) the `ExampleImplementation` into another class by asking for the `IExampleInterface` in the class like so:

```c#
public class InjectableExample
{
    [Inject]
    public IExampleInterface implementation;

    //..
}
```
Now, to get this `InjectableExample` provided with the value, we have two options:

1. Map `InjectableExample` on the `context.Mapper`
2. Call `Inject` on the `context.Injector`

Often, you'll find you end up doing option 1 without thinking about it as it can be handy to `Map` many objects.. but sometimes, you really will not want to do that!

This is where option 2 can come in. The `context.Injector` handles all `injections`, so simply call `context.Inject(injectableExampleInstance)` and voila!

A lot of the above is performed for you in the [`MVC Bundle`](#MVC-Bundle) extensions, making it a great tool for more complex use. To learn a bit more about this, take a look at [extensions](#TinYard-Extensions).

### Example Projects

* [Example To Do List](https://github.com/TinYard/TinYard-Basic-Example)

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
* [Factories](#Factories)
    * [MappingValueFactory](#MappingValueFactory)
    * [GuardFactory](#GuardFactory)
* [Guards](#Guards)
    * [IGuard](#IGuard)
    * [Guard](#Guard)
* [IExtension](#IExtension)
* [IConfig](#IConfig)
* [IBundle](#IBundle)

### IContext

An object that inherits [`IContext`](#IContext) should be the hub in which you access TinYard. This should be where you install [Extensions](#IExtension) ([configure](#IConfig) them too), as well as where you [Map](#IMapper) anything of use.

The [`IContext`](#IContext) should also have an [IInjector](#IInjector) that it uses, but you likely won't need this very often unless doing some more complex extensions or debugging. 

[Context](#Context) below, is the de-facto implementation of [`IContext`](#IContext) for TinYard.

Every [`IContext`](#IContext) should provide `event` callbacks to certain parts of its [`Initialize`](#Initalize) method. This will allow you to have more control over your [`Context`](#IContext).

Every [`IContext`](#IContext) should also provide `Detain` and `Release` methods that should primarily be used to avoid an object being Garbage Collected pre-maturely when its scope has been left. This has a super niche use case so make sure it's required when using, otherwise you may cause unneccessary memory buildup.

#### Context

The standard TinYard implementation of [`IContext`](#IContext).

[`Context`](#Context) provides basic implementations of the [`IContext`](#IContext) interface, and does nothing too fancy.

The [`Context`](#Context) class implements `Detain` and `Release` methods by simply tracking them in a `HashSet<object>` that keeps them in memory.

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

[`ContextException`](#ContextException) is an `exception` that should only be thrown from within the [`IContext`](#IContext). It should primarily be thrown only when the User has used an in-proper or unwanted action with the [`IContext`](#IContext).

### IMapper

`IMapper` should be implemented by an object that is going to be providing Mapping functionality.

Mapping is where we can 'map' an object, to another - They're linked.

`IMapper` should also make use of [`IMappingObject`](#IMappingObject)'s to be consistent across implementations.

An `IMapper` should also have a Factory so that it can aid `IMappingObject`'s in creating values they may need - A factory is optional, but is recommended for this.

An example of this is the [`ValueMapper`](#ValueMapper).

#### ValueMapper

`ValueMapper` is an implementation of [`IMapper`](#IMapper), and provides a 'value' to an `interface` or `base` class.

An example of how to use it:

```c-sharp
valueMapper.Map<IContext>().ToValue(context);
```

This example, means that when we request the Value of [`IContext`](#IContext) from the `valueMapper` object, we receive the `context` object - Whatever that is.

`ValueMapper` is used as the primary `IMapper` for [`Context`](#Context).

`ValueMapper` has a [`MappingValueFactory`](#MappingValueFactory). `ValueMapper` upon creating [`MappingObject`](#MappingObject)'s, provides a reference to itself to the object so that it can access this factory if needed to help it build a value.

#### IMappingObject

`IMappingObject` is used in tandem with [`IMapper`](#IMapper). 

`IMappingObject` has two components:

* `MappedType`
* `MappedValue`

`MappedType` is the `type` that this `IMappingObject` is a reference to.

So, looking at the example used in [Value Mapper](#ValueMapper):

When `Map<T>()` is called on the `ValueMapper`, it returns the `IMappingObject`.

Internally, `Map<T>()` on the `ValueMapper` calls `Map<T>()` on a newly created `IMappingObject` and then returns this newly created object. This `Map<T>()` method on IMappingObject should set the `MappedType` to the type of `T`.

`MappedValue` is then the value that is set with the `ToValue(object value)` method.

The `BuildValue<T>()` function can create the `MappedValue` object of type `T` for you. It should be doing this via a Factory it has access to.

#### MappingObject

`MappingObject` provides a super-simple implementation of [`IMappingObject`](#IMappingObject) that is used by [`ValueMapper`](#ValueMapper). 

`MappingObject` optionally has a reference to the `IMapper` that creates it, passed to it via the constructor. This is so that it can use the Factory that the `IMapper` has to build an object when `BuildValue<T>` is called on the `MappingObject`. If no `IMapper` is provided, it will simply not be able to build the value.

### IInjector

An `IInjector` should provide two easy-to-use `Inject` methods, and twi `CreateInjected` methods.

The first `Inject` method should provide an object, that has been provided as a parameter, values to any Field that has the [`Inject` attribute](#Inject-Attribute).

The second `Inject` method should have `target` and `value` objects passed as arguments. The `target` object should be injected into, specifically looking to provide it with the `value` object if possible.
 
The `CreateInjected` method has two versions:

One should be provided with a Generic `T` parameter. This method will create and return an instance of Type `T` that is has created via Constructor Injection and provided dependencies.

The other is identical except that it accepts a `Type` parameter instead of a Generic.

All [`IInjector`](#IInjector)'s should also have an internal collection of values that can be injected into any class when the first `Inject` method is called upon it. This collection should be added to / provided to the [`IInjector`](#IInjector) via the `AddInjectable` method. 

#### TinYardInjector

`TinYardInjector` is the standard implementation of [`IInjector`](#IInjector) used by the [standard `IContext` implementation](#Context).

`TinYardInjector` requires an [`IContext`](#IContext) and an [`IMapper`](#IMapper) to be passed to it when constructed.

`TinYardInjector` provides the 'injected' value of a Field by finding a [`Mapping`](#IMappingObject) of the Field via the [`IContext`](#IContext) provided in construction and the [`IMapper`](#IMapper) that it has, alongside its internal collection that can be added to via the `AddInjectable` method.

To perform Construction Injection, you need to use one of the `CreateInjected` methods. Internally these methods are identical, the Generic version calls the non-Generic version with `typeof(T)`. The `TinYardInjector` will firstly identify any constructors for the object Type that are marked with the [`Inject` attribute](#Inject-Attribute), if any are found it will attempt to create the object with these constructors as a priority - it then will attempt to create the object by finding the constructor it can provide the most parameters to.

#### Inject Attribute

The `Inject` attribute can be added to any Field or Class Constructor.

The `Inject` attribute acts as a flag to the [`IInjector`](#IInjector) in your [`IContext`](#IContext).

In the standard implementation of [`IContext`](#IContext), [`Context`](#Context) -
When a value is added to an [`IMappingObject`](#IMappingObject), the [`IMapper`](#IMapper) lets the [`IContext`](#IContext) know that the value needs injecting into, which in turn tells its [`IInjector`](#IInjector) to Inject into it.

### Factories

Below is the Factories available or in-use in TinYard.

Factories should be providing creation of specific objects, usually including injecting into them upon creation.

#### IFactory

All [`Factories`](#Factories) should be extending this interface.

This is to ensure that every `Factory` is simple and usable.

It is expected that most, if not all, [`Factories`](#Factories) will override the `Build` method with a more clear definition.

#### MappingValueFactory

The [`MappingValueFactory`](#MappingValueFactory) is used by the [`ValueMapper`](#ValueMapper), and aids in creation of [`IMappingObject`](#IMappingObject)'s values.

As all [`IMappingObject`](#IMappingObject)'s have a method that might need a value object to be created, the [`ValueMapper`](#ValueMapper) provides the [`MappingObject`](#MappingObject) with its factory to create that value from.

Internally, the `BuildValue<T>` method uses the [`IInjector`](#IInjector) that is mapped to its parent [`IMapper`](#IMapper) after determining the type required.  

#### GuardFactory

The [`GuardFactory`](#GuardFactory) is created and mapped in the [`Context`](#Context) to `IGuardFactory`. The [`GuardFactory`](#GuardFactory) is a super simple [`Factory`](#IFactory) that creates [`IGuard`](#IGuard)'s via the `Activator` class - Expecting the [`IGuard`](#IGuard) to have a parameterless constructor and be derived from the [`Guard`](#Guard) base class.

The `Build` function of the [`GuardFactory`](#GuardFactory) requires being passed the `Type` of the [`Guard`](#Guard) you want to build. For example, if you wanted the `Factory` to build a `ExampleGuard` class (that inherits from [`Guard`](#Guard)), you would call `Build` like so: `Build(typeof(ExampleGuard))`.

### Guards

Currently, [`Guards`](#Guards) can only be applied to [`Command`](#ICommand)'s, via a related [`ICommandMapping`](#ICommandMapping).

[`Guards`](#Guards) require that before the item they're 'guarding' is allowed to be used/executed, they have to be 'Satisfied'. Whilst this can't be enforced, it is expected and done anywhere that accepts [`Guards`](#Guards) in the main distribution of [`TinYard`](#TinYard).

All [`Guards`](#Guards) that you want to create and use should inherit the [`Guard`](#Guard) base class - They should also have a parameterless constructor that doesn't perform anything. All checks should be done in the `Satisfy` method.

[`Guards`](#Guards) can and should be injected into before calling the 'Satisfied' method.

#### IGuard

[`IGuard`](#IGuard) is the interface that all [`Guards`](#Guards) implement.

This interface is always required as it provides the `Satisfy` method that is the most important part of [`Guards`](#Guards).

#### Guard

[`Guard`](#Guard) is an abstract base class to all [`Guards`](#Guards). The reason we have this base class is so that we have a guaranteed parameterless constructor for the [`GuardFactory`](#GuardFactory) class to invoke.

This is the class that should be inherited for all [`Guards`](#Guards) that you want to use and create.

### IExtension

The `IExtension` interface is used to demonstrate that the class is an Extension that is capable of being installed into an [`IContext`](#IContext), providing extended capabilities.

Typically, this is mostly where [mappings](#IMapper) should occur.

### IConfig

A class implementing `IConfig` should provide configuration to an [`IExtension`](#IExtension), as well as configuration options to further customise an [`IExtension`](#IExtension).

### IBundle

An `IBundle` should be a small class that is used to 'bundle up' specific [`IConfig`](#IConfig) and [`IExtension`](#IExtension)'s together, typically as they are useful together or dependant on each other. 

An `IBundle` is simply to help ensure your [`IContext`](#IContext) installations are kept tidier and easier to maintain.

## TinYard Extensions

Extensions in TinYard are as easy as plug-and-play. You can use the ones that come with the framework, as well as build your own.

TinYard comes with some [Extensions](#IExtension) available to use, packaged within the framework.

The [Extensions](#IExtension) bundled with TinYard include:

* [Event System](#Event-System-Extension)
* [Logging](#Logging-Extension)
* [View Controller Extension](#View-Controller-Extension)
* [Mediator Map Extension](#Mediator-Map-Extension)
* [Command System Extension](#Command-System-Extension)
* [Callback Timer Extension](#Callback-Timer-Extension)

These [Extensions](#IExtension) can be installed by installing their respective [Extension](#IExtension) class into the [Context](#IContext).

To find the [Extension](#IExtension) and [Configs](#IConfig) available, look at the appropriate [Extension](#IExtension) section as linked above.

## Event System Extension

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

An [`IDispatcher`](#IDispatcher) should provide the ability to `Dispatch` an [`IEvent`](#IEvent)

### IEventDispatcher

[`IEventDispatcher`](#IEventDispatcher) is an extension of the [`IDispatcher`](#IDispatcher) interface.

It provides the ability to add and remove listeners (and callbacks for these) for [`IEvent`](#IEvent)s, as well as the capability provided by [`IDispatcher`](#IDispatcher). 

#### EventDispatcher

[`EventDispatcher`](#EventDispatcher) is an implementation of the [`IEventDispatcher`](#IEventDispatcher) interface.

The [`EventDispatcher`](#EventDispatcher) keeps track of any [`Listener`](#Listener)'s that have been added by having a `dictionary<Enum, Listener` of them. It tracks [`Listener`](#Listener)'s by the `type` of an [`IEvent`](#IEvent), and so when a [`Listener`](#Listener) is wanted for that [`IEvent`](#IEvent) `type` either a new [`Listener`](#Listener) is created or we add to the exisiting [`Listener`](#Listener) the given callback.

The `Dispatch` method that is required from inheritance of the [`IDispatcher`](#IDispatcher) interface is implemented by checking for any [`Listener`](#Listener)'s of the [`IEvent`](#IEvent) `type` (the [`IEvent`](#IEvent) being dispatched) and then invoking the delegates that have been added to the respective [`Listener`](#Listener). 

#### Listener

[`Listener`](#Listener) is a VO used in the [`IEventDispatcher`](#IEventDispatcher). 

[`Listener`](#Listener) provides an easy way to keep track of callbacks that want invoking for certain [`IEvent`](#IEvent) `type`s, which are added in the [`IEventDispatcher`](#IEventDispatcher).

A [`Listener`](#Listener) has a `type` that it tracks, and when a new callback wants to be added it adds it to a list of `delegate`s.

## Logging Extension

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
This limits how many lines are logged to the file before a new log file is created. If you pass this method a value of 0 or lower, it will not cap the number of logs in a file.

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

NB: Setting the maximum number of lines logged per file at 0 or less will indicate that there is no limit to the [`FileLogger`](#FileLogger) and thus will only ever use one file.

## View Controller Extension

### About the Extension

The [View Controller Extension](#View-Controller-Extension) provides two things:

* The base impl of [`IView`](#IView), [`View`](#View)
* The [`ViewRegister`](#ViewRegister)

#### Extension and Configurations

To install the [View Controller Extension](#View-Controller-Extension), install the [`ViewControllerExtension`](#View-Controller-Extension) class into your [Context](#IContext).

Currently, there are no configurations for the Extension.

### IView

[`IView`](#IView) is the interface that all implementations of a `View` should inherit.

### View

[`View`](#View) is the base implementation of [`IView`]. 

[`View`](#View) does no more than register itself to the [`ViewRegister`](#ViewRegister) upon creation.

This is to ensure that it can send [`event`](#IEvent)'s to other parts of the framework.

[`View`](#View) also has a public [`IEventDispatcher`](#IEventDispatcher) property. This is so that it can `Dispatch` events to anything listening.

### ViewRegister

[`ViewRegister`](#ViewRegister) is a Singleton class that provides static access through the `Instance` property.

The job of [`ViewRegister`](#ViewRegister) is to provide a place where all [`View`](#View)'s are accessible - So that they can be injected into, listened to, or anything else.

## Mediator Map Extension

### Dependencies

This Extension is dependant on:

* [Event System Extension](#Event-System-Extension)
* [View Controller Extension](#View-Controller-Extension)

### About the Extension

This extension aims to provide a place to `Map` [`Mediator`](#IMediator)'s and [`IView`](#IView)'s together via the [`MediatorMapper`](#MediatorMapper).

This `Mapper` will build the correct [`IMediator`](#IMediator) when the associated [`IView`](#IView) type has been registered to the [`ViewRegister`](#ViewRegister), and attach them together so that a [`IView`](#IView) can dispatch to everything else connected to the [`IContext`](#IContext).

The [Mediator Map Extension](#Mediator-Map-Extension) provides:

* The [`IMediator`](#IMediator) interface and base impl, [`Mediator`](#Mediator).
* The [`IMediatorMapper`](#IMediatorMapper) interface and base impl, [`MediatorMapper`](#MediatorMapper).
* The [`IMediatorFactory`](#IMediatorFactory) interface and its base impl, [`MediatorFactory`](#MediatorFactory).
* [`IMediatorMappingObject`](#IMediatorMappingObject) interface and impl, [`MediatorMappingObject`](#MediatorMappingObject).

#### Extension and Configurations

To install the [Mediator Map Extension](#Mediator-Map-Extension), install the [`MediatorMapExtension`](#Mediator-Map-Extension) class into your [Context](#IContext).

Currently, there are no configurations for the Extension.

### IMediator

A [`Mediator`](#IMediator) provides the ability for each [`View`](#View) to send and receive events from every [`IEventDispatcher`](#IEventDispatcher) linked to the [`IContext`](#IContext).

All implementations of this interface should provide a default constructor, to ensure they can be built by a [`IMediatorFactory`](#IMediatorFactory).

### Mediator

This is the base, abstract implementation of [`IMediator`](#IMediator). 

The [`Mediator`](#Mediator) has the main mapped [`IEventDispatcher`](#IEventDispatcher) injected, and also has reference to the [`IView`](#IView) that it is listening to. 

When the related [`IView`](#IView) property, known as ViewComponent, is set the [`Mediator`](#Mediator) will use reflection to fetch the [View's](#IView) [`IEventDispatcher`](#IEventDispatcher) so that it can hook into it and add listeners when required.

The base [`Mediator`](#Mediator) class provides methods to add listeners to the [`IView`](#IView), as well as to the [`IContext`](#IContext)'s mapped [`IEventDispatcher`](#IEventDispatcher).

##### Configure
When creating your own [`Mediator`](#Mediator), you will have to provide a `Configure` method implementation. This is where you should add any listeners, as you will not have a reference to your [`IView`](#IView) in the constructor but this method should be called when a [`IView`](#IView) is provided.

##### Attached View

When creating your own [`Mediator`](#Mediator), you'll want to have a specific [`IView`](#IView) referenced as a field. To get this [`IView`](#IView), simply attach the [`Inject`](#Inject) attribute.

### IMediatorMapper

An [`IMediatorMapper`](#IMediatorMapper) provides a place to `Map` a [`IMediator`](#IMediator) to a respective object. The base implementation of this is the [`MediatorMapper`](#MediatorMapper).

An [`IMediatorMapper`](#IMediatorMapper) should also have an [`IMediatorFactory`](#IMediatorFactory) associated with it that can create a [`IMediator`](#IMediator) for you.

### MediatorMapper

Every View that gets registered needs to have an [`IMediator`](#IMediator) to be heard outside of itself. The [`MediatorMapper`](#MediatorMapper) helps ensure that each [`View`](#View) has a [`Mediator`](#Mediator) attached and being its [`dispatcher`](#IDispatcher).

By 'mapping' a [`View`](#View) to a [`Mediator`](#Mediator), it guarantees that the [`Mediator`](#Mediator) will be created when the [`View`](#View) is registered - The [`MediatorMapper`](#MediatorMapper) uses a Factory to create the associated [`Mediator`](#Mediator) (but this can only happen if you 'map' one!).

Once the [`Mediator`](#Mediator) is created, it will be injected into by the [`Context`](#Context)'s [`IInjector`](#IInjector), as well as the [`View`](#View) value set.

### IMediatorMappingObject

An [`IMediatorMappingObject`](#IMediatorMappingObject) is similar to a [`IMappingObject`](#IMappingObject), but not the same.

[`IMediatorMappingObject`](#IMediatorMappingObject) is built purposefully for [`IView`](#IView)'s and [`IMediator`](#IMediator)'s.

The base implementation of [`IMediatorMappingObject`](#IMediatorMappingObject) is an [`MediatorMappingObject`](#MediatorMappingObject).

### MediatorMappingObject

The [`MediatorMappingObject`](#MediatorMappingObject) is the base implementation of [`IMediatorMappingObject`](#IMediatorMappingObject).

As there is similarity to the [`IMappingObject`](#IMappingObject) class, one is used internally to do most of the work.

[`MediatorMappingObject`](#MediatorMappingObject) simply provides different names to the [`IMappingObject`](#IMappingObject) methods, fields, and properties - As well as `Type` restrictions.

### IMediatorFactory

The [`IMediatorFactory`](#IMediatorFactory) interface extends upon the [`IFactory`](#IFactory) interface. 

[`IMediatorFactory`](#IMediatorFactory) provides building of specifically [`IMediator`](#IMediator)'s.

### MediatorFactory

[`MediatorFactory`](#MediatorFactory) is the base implementation of [`IMediatorFactory`](#IMediatorFactory) and is used by the [`MediatorMapper`](#MediatorMapper). 

This `Factory` is simple in that it calls a default constructor expected on all [`IMediator`](#IMediator) implementations.

## Command System Extension

### Dependencies

The [Command System Extension](#Command-System-Extension) is depdendant on:

* [Event System Extension](#Event-System-Extension)

### About the Extension

The [Command System Extension](#Command-System-Extension) brings [Commands](#ICommand) to TinYard. 

The idea of this is that you can execute one-of code when specific [`event`](#IEvent)'s are dispatched. The most common use-case for this is to fetch data from a `model` and then dispatch that data to somewhere else - Often the same place that the original [`event`](#IEvent) was dispatched from.

A [`Command`](#ICommand) is created when needed by the [`EventCommandMap`](#EventCommandMap), and then left to be garbage collected after it has been 'executed' - They are intended for singular uses, and if you require something that is not then perhaps look at using a `service` or `model`.

#### Extension and Configurations

To install the [Command System Extension](#Command-System-Extension), install the [`CommandSystemExtension`](#Command-System-Extension) class into your [Context](#IContext).

No configurations are available for the [Command System Extension](#Command-System-Extension).

### ICommand

The [`ICommand`](#ICommand) interface is the base of any `Command`. All this interface requires is a method called `Execute()` in your `Command`. 

The `Execute` method is invoked once the `Command` has been created and all injections completed.

### ICommandMap

An [`ICommandMap`](#ICommandMap) simply has to have two `Map` functions that return an [`ICommandMapping`](#ICommandMapping). 

### Event Command Map

The [`Event Command Map`](#Event-Command-Map) is the base impl of [`ICommandMap`](#ICommandMap) that is used link [`event`](#IEvent)'s to [`command`](#ICommand)'s. It ensures that when that specific [`event`](#IEvent) and `type` are dispatched on the mapped [`IEventDispatcher`](#IEventDispatcher), the appropriate [`command`](#ICommand) is created, injected into, and invoked (via the `Execute` method).

Internally it does this by having reference to the mapped [`IEventDispatcher`](#IEventDispatcher) and adding a listener to that, with a callback function that builds and invokes the correct [`command`](#ICommand).

Before any [`Command`](#ICommand) is executed, the [`ICommandMapping`](#ICommandMapping) is checked for [`Guards`](#Guards). If any [`Guards`](#Guards) are related to the mapping:   
   * They are built via the [`GuardFactory`](#GuardFactory).
   * They are then injected into.
   * The `Satisfies` method is then called.
       * If the method returns false, the [`ICommand`](#ICommand) is not executed and we return early.
       * If the method returns true, we move onto the next [`Guard`](#IGuard) until all have been satisifed. If all are satisfied, we execute the [`ICommand`](#ICommand).

### ICommandFactory

[`ICommandFactory`](#ICommandFactory) is an [`IFactory`](#IFactory) that is made for specifically creating [`ICommand`](#ICommand)'s.

### Command Factory

[`CommandFactory`](#Command-Factory) is the base impl of [`ICommandFactory`](#ICommandFactory) that creates [`ICommand`](#ICommand)'s and injects into them. It also injects the [`event`](#IEvent) that is causing its creation, if it can.

### ICommandMapping

[`ICommandMapping`](#ICommandMapping) is a spin on [`IMappingObject`](#IMappingObject), but changed for keeping a correlation between [`event`](#IEvent)'s and [`command`](#ICommand)'s.

[`ICommandMapping`](#ICommandMapping)'s can have [`Guards`](#Guards) that should be 'satisfied' before executing the [`ICommand`](#ICommand).

### CommandMapping

[`CommandMapping`](#CommandMapping) is the base impl of [`ICommandMapping`](#ICommandMapping) and is also a spin on [`MappingObject`](#MappingObject).

## Callback Timer Extension

### Dependencies

The [Callback Timer Extension](#Callback-Timer-Extension) is depdendant on:

* [Event System Extension](#Event-System-Extension)
* [Command System Extension](#Command-System-Extension)

### About the Extension

The [Callback Timer Extension](#Callback-Timer-Extension) provides you the ability to easily invoke any Action in any amount of time from now.

Need to dispatch an event in 5 seconds? Use the [`ICallbackTimer`](#ICallback-Timer) instead of writing some messy get around.

To use the [`ICallbackTimer`](#ICallback-Timer) you can inject it into your anything, or use the new events provided.

#### Extension and Configurations

To install the [Callback Timer Extension](#Callback-Timer-Extension), install the [`CallbackTimerExtension`](#Callback-Timer-Extension) class into your [Context](#IContext).

No configurations are available for the [Callback Timer Extension](#Callback-Timer-Extension).

### ICallback Timer

The [`ICallbackTimer`](#ICallback-Timer) interface is the bread-and-butter of this extension for the user. You should `inject` the [`ICallbackTimer`](#ICallback-Timer) when wanting to make use of the `extension`, or use the `event`s found further below.

This interface has only a few functions: 

```c#
void AddTimer(int ticks, Action callback)
void AddTimer(double seconds, Action callback)

bool RemoveTimer(Action callback)
```

### Events

In the [Callback Timer Extension](#Callback-Timer-Extension) there are two `event`s that can be utilised to save you from needing to inject the [`ICallbackTimer`](#ICallback-Timer).

These are:    
[`AddCallbackTimerEvent`](#Add-Event),    
[`RemoveCallbackTimerEvent`](#Remove-Event)

#### Add Event

The [`AddCallbackTimerEvent`](#Add-Event) has two constructors; one allows you to specify the [`Timer`](#Timer) duration in `tick`s and the other allows you to specify the duration in  seconds.

The length of a `tick` is determined by the `System.Diagnostics.Stopwatch.Frequency` property.

#### Remove Event

The [`RemoveCallbackTimerEvent`](#Remove-Event) has only one constructor:
`RemoveCallbackTimerEvent(Type type, Action callbackToRemove)`.

This seems pretty self-explanatory. Pass in the same callback as what was added, and it will remove any timers with that callback.

### Commands

There are, like the events, only two commands in the [`Callback Timer Extension`](#Callback-Timer-Extension).

[`AddCallbackTimerCommand`](#Add-Command)
[`RemoveCallbackTimerCommand`](#Remove-Command)

#### Add Command

The [`AddCallbackTimerCommand`](#Add-Command) has the [`ICallbackTimer`](#ICallback-Timer) injected, and simply calls the `AddTimer` function with the callback and duration provided from the `event`.

#### Remove Command

The [`RemoveCallbackTimerCommand`](#Remove-Command) has an even easier job than the [`AddCallbackTimerCommand`](#Add-Command) - It is setup the same, but just has to call `RemoveTimer` on the [`ICallbackTimer`](#ICallback-Timer) with the provided callback.

### Callback Timer Service

The [`CallbackTimerService`](#Callback-Timer-Service) class is the meat of the `extension`. This class inherits the [`ICallbackTimer`](#ICallback Timer) interface and implements it - it is then `mapped` onto the `context` by the `extension` under the interface.

Upon construction, the [`CallbackTimerService`] starts a `Task` that is an Update loop - A never ending loop that will each loop call `UpdateTimers`, which in turn calls `Update` on each [`Timer`](#Timer).

Each loop has a small amount of time between it, known as `delta time`. This is passed onto the [`Timer`](#Timer)'s so that they can internally update their time tracking. `delta time` is determined by using a `System.Diagnostics.Stopwatch` and recording the time between each loop.

### Timer

The [`Timer`](#Timer) VO class is a simple class that makes keeping track of callbacks easier for the [`CallbackTimerService`](#CallbackTimerService). 

The main functionality of the class is in the `Update` function that increments the `CurrentLifetime` property. Once `CurrentLifetime` is bigger or equal to the `TimerDuration` property, the `TimerCallback` action is invoked. 

---

## TinYard Bundles

The currently included bundles in TinYard are:

* [`MVCBundle`](#MVCBundle)

### MVC Bundle

The MVC Bundle contains:
 * Event System Extension
 * View Controller Extension
 * Mediator Map Extension
 * Command System Extension

## [Contribution](.github/CONTRIBUTING.md)

### [Code of Conduct](.github/CODE_OF_CONDUCT.md)
### [Funding](.github/FUNDING.yml)
### [Buy me a coffee](https://www.buymeacoffee.com/KieranB)
