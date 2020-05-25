# TinYard

![.NET](https://github.com/KieranBond/TinYard/workflows/.NET/badge.svg)

## Table Of Contents

* [TinYard](#TinYard)
* [Table Of Contents](#Table-Of-Contents)
* [What is TinYard?](#What-Is-TinYard)
* [Using TinYard](#Using-TinYard)
* [TinYard Internals](#TinYard-Internals)
* [How to contribute](#Contribution)
    * [Monetary Contribution](#Funding.)
    * [Coffee Contribution](#Buy-me-a-coffee)



## What is TinYard?

TinYard is a C# Framework.

At its basics, the framework is to help you create any application with an Event System at the core of your application.

The framework provides a few tools to help you do this, such as;

* Dependency Injection
* Event System
* Value Mapping

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

An object that inherits `IContext` should be the hub in which you access TinYard. This should be where you install [Extensions](#IExtension) ([configure](#IConfig) them too), as well as where you [Map](#IMapper) anything of use.

The `IContext` should also have an [IInjector](#IInjector) that it uses, but you likely won't need this very often. 

[Context](#Context) below, is the de-facto implementation of `IContext` for TinYard.

Every `IContext` should provide `event` callbacks to certain parts of its `Initialize` method. This will allow you to have more control over your `Context`.

#### Context

The standard TinYard implementation of `IContext`.

`Context` provides basic implementations of the `IContext` interface, and does nothing too fancy.

##### Construction

In `Context`'s constructor a new [ValueMapper](#ValueMapper) and [TinYard Injector](#TinYardInjector) are created, which are then accessable as `Mapper` and `Injector` respectively.

The `Context`, `Mapper`, and `Injector` are then all mapped on the `Mapper` to their interface equivalents (`IContext`, `IMapper`, `IInjector`) - Allowing them to be injected into any object needing them.

##### Initialize

The `Context` can only be 'Initalized' once - This means you can only call the `Initalize` method once, any more calls to it will raise a [Context Exception](#ContextException).

The `Initalize` method has four steps:
* Install Bundles
* Install Extensions
* Install Configs
* Post Initialize

##### Install Bundles

Install Bundles is the first of the four steps. It runs first, as it has a direct effect on the next two steps.

[Bundles](#IBundle) should typically be just installing `IExtension`'s and `IConfig`'s.  

When an `IBundle` is installed via the `Install(IBundle bundle)` method, it is added to a private `List<IBundle>` named `_bundlesToInstall`. When the `Context` goes to install the List of `IBundle`'s it simply calls the `Install` method on each and passes itself to the Bundle, which in form then usually calls `Install(IExtension extension)` and `Configure(IConfig config)` onto the `Context`, just making the installation of multiple Extensions and Configs that are together a bit tidier.

So, Bundles are simply just wrappers for installing multiple Extensions and Configs. Because of that, this is why they are installed first - So that the actual extensions and configs installation is 

##### Install Extensions



#### ContextException

### IMapper

#### ValueMapper

#### IMappingObject

#### MappingObject

### IInjector

#### TinYardInjector

#### Inject Attribute

### IExtension

### IConfig

### IBundle

---

## [Contribution](CONTRIBUTING.md)

### [Code of Conduct](CODE_OF_CONDUCT.md)
### [Funding](.github/FUNDING.yml)
### [Buy me a coffee](https://www.buymeacoffee.com/KieranB)
