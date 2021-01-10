using System;
using System.Collections.Generic;
using TinYard.API.Interfaces;
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
        
        public event Action PreBundlesInstalled;
        public event Action PostBundlesInstalled;

        public event Action PreConfigsInstalled;
        public event Action PostConfigsInstalled;

        public event Action PostInitialize;

        //Properties
        public IMapper Mapper { get { return _mapper; } }
        private IMapper _mapper;

        public IInjector Injector { get { return _injector; } }
        private IInjector _injector;

        //Private variables
        private List<IExtension> _extensionsToInstall;
        private HashSet<IExtension> _extensionsInstalled;

        private List<IBundle> _bundlesToInstall;
        private HashSet<IBundle> _bundlesInstalled;

        private List<IConfig> _configsToInstall;
        private HashSet<IConfig> _configsInstalled;

        private HashSet<object> _detainedObjs;

        private bool _initialized = false;

        public Context()
        {
            _bundlesToInstall = new List<IBundle>();
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
        }

        public IContext Install(IExtension extension)
        {
            _extensionsToInstall.Add(extension);

            return this;
        }

        public IContext Install(IBundle bundle)
        {
            _bundlesToInstall.Add(bundle);
            
            return this;
        }

        public IContext Configure(IConfig config)
        {
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
                throw new ContextException("Context already initialized");

            //Install bundles first as they'll be adding to the extensions and configs list - Don't want this happening when we're enumerating those lists
            PreBundlesInstalled?.Invoke();

            InstallBundles();

            PostBundlesInstalled?.Invoke();

            //Let anything know we're about to install extensions
            PreExtensionsInstalled?.Invoke();

            InstallExtensions();

            //Invoke anything listening to when Extensions are finished installing
            PostExtensionsInstalled?.Invoke();

            PreConfigsInstalled?.Invoke();

            InstallConfigs();

            PostConfigsInstalled?.Invoke();

            _initialized = true;

            PostInitialize?.Invoke();
        }

        public void Detain(object objToDetain)
        {
            _detainedObjs.Add(objToDetain);
        }

        public void Release(object objToRelease)
        {
            if(_detainedObjs.Contains(objToRelease))
                _detainedObjs.Remove(objToRelease);
        }

        private void InstallBundles()
        {
            //Get the previous installed extensions and configs out of the way so we can place
            //the extensions after the bundles

            //REFACTOR : Is there a better way to do this?
            //Maybe we can use the `PostBundlesInstalled` hook in .Install & .Configure methods
            //So we don't need to reallocate/move these below
            IExtension[] extensionsToHold = new IExtension[_extensionsToInstall.Count];
            _extensionsToInstall.CopyTo(extensionsToHold);

            IConfig[] configsToHold = new IConfig[_configsToInstall.Count];
            _configsToInstall.CopyTo(configsToHold);

            _extensionsToInstall.Clear();
            _configsToInstall.Clear();

            _bundlesInstalled = new HashSet<IBundle>();
            foreach(IBundle bundle in _bundlesToInstall)
            {
                //This simply passes the Context into the Bundle so that it can call
                //context.install(extension).Configure(config);
                bundle.Install(this);
                bool added = _bundlesInstalled.Add(bundle);

                if(!added)
                {
                    throw new ContextException("Bundle " + bundle.ToString() + " already installed");
                }
            }

            _bundlesToInstall.Clear();

            _extensionsToInstall.AddRange(extensionsToHold);
            _configsToInstall.AddRange(configsToHold);
        }

        private void InstallExtensions()
        {
            _extensionsInstalled = new HashSet<IExtension>();
            foreach (IExtension currentExtension in _extensionsToInstall)
            {
                currentExtension.Install(this);
                bool added = _extensionsInstalled.Add(currentExtension);

                //We don't want any extensions installed multiple times
                if (!added)
                {
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
                //Inject into the config before we call configure, ensuring it has anything needed
                _injector.Inject(currentConfig);

                currentConfig.Configure();

                bool added = _configsInstalled.Add(currentConfig);

                //We don't want configs installed multiple times
                if(!added)
                {
                    throw new ContextException("Config " + currentConfig.ToString() + " already configured");
                }
            }

            _configsToInstall.Clear();
        }

        private void InjectValueMapper(IMappingObject mappingObject)
        {
            _injector.Inject(mappingObject.MappedValue);
        }
    }
}
