using System;
using System.Collections;
using System.Collections.Generic;

namespace TinYard.ExtensionMethods
{
    //Internal so that this doesn't get included in the build
    internal static class GenericConstructionExtensions
    {
        public static IList CreateList(Type listType)
        {
            Type genericListType = typeof(List<>).MakeGenericType(listType);
            return (IList)Activator.CreateInstance(genericListType);
        }
    }
}
