using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TinYard.Framework.Impl.VO;

namespace TinYard.Framework.Impl.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Constructor | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class InjectAttribute : Attribute
    {
        #region Constructors and Properties

        public string Name { get { return _name; } }
        private string _name;

        public bool AllowMultiple { get { return _allowMultiple; } }
        private bool _allowMultiple = false;

        public InjectAttribute(string name) : this(name, false)
        {

        }

        public InjectAttribute(bool allowMultiple) : this(null, allowMultiple)
        {

        }

        public InjectAttribute(string name = null, bool allowMultiple = false)
        {
            this._name = name;
            this._allowMultiple = allowMultiple;
        }

        #endregion


        #region Helper functions

        public static List<InjectableInformation> GetInjectableInformation(Type classToInjectInto)
        {
            List<InjectableInformation> injectables = new List<InjectableInformation>();
            
            foreach(PropertyInfo property in classToInjectInto.GetProperties())
            {
                var attribute = property.GetCustomAttributes<InjectAttribute>(true).FirstOrDefault();
                if(attribute != null && property.GetSetMethod(true) != null)//Get the Set method of property, even if not public
                {
                    InjectableInformation info = new InjectableInformation(attribute, property);
                    injectables.Add(info);
                }
            }

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