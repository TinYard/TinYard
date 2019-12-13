using System;
using System.Collections.Generic;
using TinYard.API.Interfaces;

namespace TinYard
{
    public class Context : IContext
    {
        //Properties
        public event Action PostExtensionsInstalled;
        public event Action PreExtensionsInstalled;

        //Private variables
        private List<IExtension> _extensionsToInstall;
        private HashSet<IExtension> _extensionsInstalled;

        private bool _initialized = false;

        public Context()
        {
            _extensionsToInstall = new List<IExtension>();
        }

        public IContext Install(IExtension extension)
        {
            _extensionsToInstall.Add(extension);

            return this;
        }

        public bool ContainsExtension(IExtension extension)
        {
            return _extensionsInstalled != null ? _extensionsInstalled.Contains(extension) : false;
        }

        public void Initialize()
        {
            if (_initialized)
                throw new ApplicationException("Context already initialized");

            _initialized = true;

            //Let anything know we're about to install extensions
            PreExtensionsInstalled?.Invoke();

            InstallExtensions();
            
            //Invoke anything listening to when Extensions are finished installing
            PostExtensionsInstalled?.Invoke();
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
                    throw new ApplicationException("Extension " + currentExtension.ToString() + " already installed");
                }
            }            
            
            _extensionsToInstall.Clear();
        }
    }
}
