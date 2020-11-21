using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TinYard.Framework.Impl.Attributes
{
    //TODO: See if we can allow injection into Properties.. Might be an issue due to private sets?
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Constructor /*| AttributeTargets.Property*/, AllowMultiple = false, Inherited = true)]
    public class InjectAttribute : Attribute
    {
        public static List<FieldInfo> GetInjectableFields(Type classToInjectInto)
        {
            List<FieldInfo> injectables = new List<FieldInfo>();
            
            foreach(FieldInfo field in classToInjectInto.GetFields())
            {
                if (field.GetCustomAttributes<InjectAttribute>(true).Count() > 0)
                    injectables.Add(field);
            }

            return injectables;
        }

        public static List<ConstructorInfo> GetInjectableConstructors(Type classToInjectInto)
        {
            List<ConstructorInfo> injectableConstructors = new List<ConstructorInfo>();

            foreach(ConstructorInfo constructor in classToInjectInto.GetConstructors())
            {
                if (constructor.GetCustomAttributes<InjectAttribute>(true).Count() > 0)
                    injectableConstructors.Add(constructor);
            }

            return injectableConstructors;
        }
    }
}
