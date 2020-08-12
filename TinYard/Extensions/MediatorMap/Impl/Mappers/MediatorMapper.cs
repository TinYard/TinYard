using System;
using TinYard.API.Interfaces;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.MediatorMap.Impl.VO;
using TinYard.Framework.API.Interfaces;
using TinYard.Impl.Mappers;

namespace TinYard.Extensions.MediatorMap.Impl.Mappers
{
    public class MediatorMapper : IMapper<IMediatorMappingObject>, IMediatorMapper
    {
        public MediatorMapper()
        {
            //_mappingFactory = new MediatorFactory(this);
        }

        public IMappingFactory MappingFactory => throw new NotImplementedException();

        public event Action<IMediatorMappingObject> OnValueMapped;

        public System.Collections.Generic.IReadOnlyList<IMediatorMappingObject> GetAllMappings()
        {
            throw new NotImplementedException();
        }

        public IMediatorMappingObject GetMapping<T2>()
        {
            throw new NotImplementedException();
        }

        public IMediatorMappingObject GetMapping(Type type)
        {
            throw new NotImplementedException();
        }

        public object GetMappingValue<T2>()
        {
            throw new NotImplementedException();
        }

        public object GetMappingValue(Type type)
        {
            throw new NotImplementedException();
        }

        public IMediatorMappingObject Map<T2>(bool autoInitializeValue = false)
        {
            throw new NotImplementedException();
        }
    }
}
