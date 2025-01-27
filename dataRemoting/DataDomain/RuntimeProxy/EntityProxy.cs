using System;
using System.Reflection;

namespace DataDomain
{
    public class EntityProxy : DispatchProxy, IEntityProxy, IEntity
    {
        public IEntityType EntityType { get; set; }
        public DataEntity DataObject { get; set; }
        public IServiceProvider ActivationServiceProvider { get; set; }
        public void TryActivate()
        {
            Instance = EntityType.ComponentActivationExpression.Compile()(ActivationServiceProvider, this);
        }

        public object Instance;
        
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            if (targetMethod.Name.StartsWith("get_"))
            {
                return targetMethod.Invoke(DataObject.GetSegment(targetMethod.DeclaringType), args);
            }

            if (targetMethod.Name.StartsWith("set_"))
            {
                return targetMethod.Invoke(DataObject.GetSegment(targetMethod.DeclaringType), args);
            }

            if (Instance != null)
            {
                return targetMethod.Invoke(Instance, args);
            }
            
            throw new NotImplementedException();
        }

        public string Path
        {
            get => DataObject.Path;
            set => DataObject.Path = value;
        }

        public bool HasSegment<TSegment>()
        {
            return DataObject.HasSegment<TSegment>();
        }

        public TSegment GetSegment<TSegment>()
        {
            return DataObject.GetSegment<TSegment>();
        }

        public object GetSegment(Type segment)
        {
            return DataObject.GetSegment(segment);
        }

        public object GetSegment(IClrType segment)
        {
            return DataObject.GetSegment(segment);
        }

        public void SetSegment(IClrType segment, object model)
        {
            DataObject.SetSegment(segment, model);
        }
    }
}