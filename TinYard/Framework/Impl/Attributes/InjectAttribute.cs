using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TinYard.Framework.Impl.VO;

namespace TinYard.Framework.Impl.Attributes
{
    //TODO: See if we can allow injection into Properties.. Might be an issue due to private sets?
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Constructor /*| AttributeTargets.Property*/, AllowMultiple = false, Inherited = true)]
    public class InjectAttribute : Attribute
    {
        #region Constructors and Properties

        public string Name { get { return _name; } }
        private string _name;

        public InjectAttribute(string name = null)
        {
            this._name = name;
        }

        #endregion


        #region Helper functions

        public static List<InjectableInformation> GetInjectableInformation(Type classToInjectInto)
        {
            List<InjectableInformation> injectables = new List<InjectableInformation>();
            
            foreach(FieldInfo field in classToInjectInto.GetFields())
            {
                var attribute = field.GetCustomAttributes<InjectAttribute>(true).FirstOrDefault();
                if (attribute != null)
                {
                    InjectableInformation info = new InjectableInformation(attribute, field);
                    injectables.Add(info);
                }
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

        #endregion
    }
}