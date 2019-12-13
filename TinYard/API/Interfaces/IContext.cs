﻿using System;

namespace TinYard.API.Interfaces
{
    public interface IContext
    {
        event Action PreExtensionsInstalled;
        event Action PostExtensionsInstalled;

        IContext Install(IExtension extension);
        
        void Initialize();

        bool ContainsExtension(IExtension extension);
    }
}