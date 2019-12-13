using System;
using System.Collections.Generic;
using TinYard.API.Interfaces;

namespace TinYard
{
    public class Context : IContext
    {
        private List<IExtension> _extensionsToInstall;
        private List<IExtension> _extensionsInstalled;

        private bool _initialized = false;

        public Context()
        {
            _extensionsToInstall = new List<IExtension>();
        }

        public IContext Install(IExtension extension)
        {
            //TODO : Check if we've already got this extension in the list

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

            _extensionsInstalled = new List<IExtension>();
            foreach(IExtension currentExtension in _extensionsToInstall)
            {
                currentExtension.Install(this);
                _extensionsInstalled.Add(currentExtension);
            }

            _extensionsToInstall.Clear();
        }
    }
}
