using System;
using System.Collections;
using System.Reflection;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Framework.Impl.VO
{
    public class InjectableInformation
    {
        public InjectAttribute Attribute { get; }
        public FieldInfo Field { get; }
        public PropertyInfo Property { get; }

        public InjectableInformation(InjectAttribute attribute, PropertyInfo property)
        {
            Attribute = attribute;
            Property = property;
        }

        public InjectableInformation(InjectAttribute attribute, FieldInfo field)
        {
            Attribute = attribute;
            Field = field;
        }

        public Type GetFieldValueType()
        {
            if(Attribute.AllowMultiple)
            {
                //Return the generic used in an IEnumerable
                return Field.FieldType.GetGenericArguments()[0];
            }
            else
            {
                return Field.FieldType;
            }
        }
    }
}
