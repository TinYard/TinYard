﻿namespace TinYard.API.Interfaces
{
    public interface IMapper
    {
        IMapping Map<T>();
        IMapping ToValue(object value);

        T GetValue<T>();
    }
}
