using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using TinYard.API.Interfaces;
using TinYard.ExtensionMethods;
using TinYard.Framework.API.Interfaces;
using TinYard.Framework.Impl.Factories;
using TinYard.Framework.Impl.Injectors;
using TinYard.Impl.Exceptions;
using TinYard.Impl.Mappers;
using TinYard.Impl.VO;

namespace TinYard
{
    public class Context : IContext
    {
        //Timeline Properties
        public event Action PreExtensionsInstalled;
        public event Action PostExtensionsInstalled;
        
        public event Action PreConfigsInstalled;
        public event Action PostConfigsInstalled;

        public event Action PostInitialize;

        //Properties
        public IMapper Mapper { get { return _mapper; } }
        private IMapper _mapper;

        public IInjector Injector { get { return _injector; } }
        private IInjector _injector;

        public object Environment { get { return _environment; } set { SetEnvironment(value); } }
        private object _environment;

        //Private variables
        private ILogger<Context> _logger;

        private List<IExtension> _extensionsToInstall;
        private HashSet<IExtension> _extensionsInstalled;

        private HashSet<IBundle> _bundlesInstalled;

        private List<IConfig> _configsToInstall;
        private HashSet<IConfig> _configsInstalled;

        private HashSet<object> _detainedObjs;

        private bool _initialized = false;

        /// <summary>
        /// Use this Ctor only when wanting no logging
        /// </summary>
        public Context() : this( NullLogger<Context>.Instance ) { }

        public Context( ILogger<Context> logger )
        {
            _logger = logger;
            _logger.Debug("TinYard Context spinning up");

            _bundlesInstalled = new HashSet<IBundle>();
            _extensionsToInstall = new List<IExtension>();
            _configsToInstall = new List<IConfig>();

            _detainedObjs = new HashSet<object>();

            //Create our mapper, then add a hook so that we can inject into anything that gets mapped
            _mapper = new ValueMapper();
            _mapper.OnValueMapped += InjectValueMapper;

            _injector = new TinYardInjector(this, _mapper);

            //Ensure the context, mapper and injector are mapped for injection needs
            _mapper.Map<IContext>().ToValue(this);
            _mapper.Map<IMapper>().ToValue(_mapper);
            _mapper.Map<IInjector>().ToValue(_injector);


            _mapper.Map<IGuardFactory>().ToValue(new GuardFactory());

            _logger.Debug("TinYard Context spun up successfully");
        }

        public IContext Install(IExtension extension)
        {
            _logger.Debug("Adding {extension} to list of extensions to install", extension);

            _extensionsToInstall.Add(extension);

            return this;
        }

        public IContext Install(IBundle bundle)
        {
            //Unpack the bundle now so order integrity is kept
            bundle.Install(this);

            bool added = _bundlesInstalled.Add(bundle);

            if (!added)
            {
                _logger.Error("Attempted to install already installed {bundle}", bundle);
                throw new ContextException("Bundle " + bundle.ToString() + " already installed");
            }

            return this;
        }

        public IContext Configure(IConfig config)
        {
            _logger.Debug("Adding {config} to list of Configs to install", config);

            _configsToInstall.Add(config);

            return this;
        }

        public bool ContainsExtension(IExtension extension)
        {
            return _extensionsInstalled != null ? _extensionsInstalled.Contains(extension) : false;
        }

        public bool ContainsExtension<T>() where T : IExtension
        {
            foreach(IExtension extension in _extensionsInstalled)
            {
                if (extension.GetType() == typeof(T))
                    return true;
            }

            return false;
        }

        public void Initialize()
        {
            if (_initialized)
            {
                _logger.Error("TinYard {Context} already installed", this);
                throw new ContextException("Context already initialized");
            }

            //Let anything know we're about to install extensions
            _logger.Debug("Invoking {PreExtensionsInstalled} in Timeline", PreExtensionsInstalled);
            PreExtensionsInstalled?.Invoke();

            _logger.Debug("Installing {Extensions}", _extensionsToInstall);
            InstallExtensions();

            //Invoke anything listening to when Extensions are finished installing
            _logger.Debug("Invoking {PostExtensionsInstalled} in Timeline", PostExtensionsInstalled);
            PostExtensionsInstalled?.Invoke();

            _logger.Debug("Invoking {PreConfigsInstalled} in Timeline", PreConfigsInstalled);
            PreConfigsInstalled?.Invoke();

            _logger.Debug("Installing {Configs}", _configsToInstall);
            InstallConfigs();

            _logger.Debug("Invoking {PostConfigsInstalled} in Timeline", PostConfigsInstalled);
            PostConfigsInstalled?.Invoke();

            _initialized = true;

            _logger.Debug("Invoking {PostInitialize} in Timeline", PostInitialize);
            PostInitialize?.Invoke();
        }

        public void Detain(object objToDetain)
        {
            _logger.Debug("Detaining {object}", objToDetain);
            _detainedObjs.Add(objToDetain);
        }

        public void Release(object objToRelease)
        {
            if(_detainedObjs.Contains(objToRelease))
            {
                _logger.Debug("Releasing {object}", objToRelease);
                _detainedObjs.Remove(objToRelease);
            }
        }

        private void InstallExtensions()
        {
            _extensionsInstalled = new HashSet<IExtension>();
            foreach (IExtension currentExtension in _extensionsToInstall)
            {
                //Skip the extension if it's in a different environment
                if (currentExtension.Environment != Environment)
                    break;

                _logger.Debug("Attempting to install {extension}", currentExtension);
                currentExtension.Install(this);
                bool added = _extensionsInstalled.Add(currentExtension);

                //We don't want any extensions installed multiple times
                if (!added)
                {
                    _logger.Error("Failed to install {extension} - Already installed", currentExtension);
                    throw new ContextException("Extension " + currentExtension.ToString() + " already installed");
                }
            }

            _extensionsToInstall.Clear();
        }

        private void InstallConfigs()
        {
            _configsInstalled = new HashSet<IConfig>();

            foreach(IConfig currentConfig in _configsToInstall)
            {
                //Skip the config if it's in a different environment
                if (currentConfig.Environment != Environment)
                    break;

                _logger.Debug("Injecting {config}", currentConfig);
                //Inject into the config before we call configure, ensuring it has anything needed
                _injector.Inject(currentConfig);

                _logger.Debug("Attempting to configure {config}", currentConfig);
                currentConfig.Configure();

                bool added = _configsInstalled.Add(currentConfig);

                //We don't want configs installed multiple times
                if(!added)
                {
                    _logger.Error("Failed to configure {config} - Already configured", currentConfig);
                    throw new ContextException("Config " + currentConfig.ToString() + " already configured");
                }
            }

            _configsToInstall.Clear();
        }

        private void SetEnvironment(object newEnvironment)
        {
            _logger.Debug("Changing from {OldEnvironment} to {NewEnvironment}", _environment, newEnvironment);
            
            _environment = newEnvironment;

            _injector.Environment = newEnvironment;
            _mapper.Environment = newEnvironment;
        }

        private void InjectValueMapper(IMappingObject mappingObject)
        {
            _injector.Inject(mappingObject.MappedValue);
        }
    }
}
