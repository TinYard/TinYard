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
    }
}
