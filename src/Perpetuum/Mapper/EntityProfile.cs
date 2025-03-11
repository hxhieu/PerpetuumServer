using Perpetuum.EntityFramework;

namespace Perpetuum.Mapper
{
    internal class EntityProfile : AutoMapper.Profile
    {
        public EntityProfile()
        {
            //var definition = record.GetValue<int>("definition");
            //var entity = _factory.CreateWithRandomEID(definition);

            //entity.Eid = record.GetValue<long>("eid");
            //entity.Owner = record.GetValue<long?>("owner") ?? 0L;
            //entity.Parent = record.GetValue<long?>("parent") ?? 0L;
            //entity.Health = record.GetValue<double>("health");
            //entity.Name = record.GetValue<string>("ename");
            //entity.Quantity = record.GetValue<int>("quantity");
            //entity.IsRepackaged = record.GetValue<bool>("repackaged");
            //entity.DynamicProperties.Items = new GenxyString(record.GetValue<string>("dynprop")).ToDictionary().ToImmutableDictionary();

            //entity.dbState = EntityDbState.Unchanged;

            CreateMap<DataContext.Entities.Entity, Entity>()
                 .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner ?? 0L))
                 .ForMember(dest => dest.Parent, opt => opt.MapFrom(src => src.Parent ?? 0L))
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Ename))
                 .ForMember(dest => dest.IsRepackaged, opt => opt.MapFrom(src => src.Repackaged))
                 .ForMember(dest => dest.dbState, opt => opt.MapFrom(src => EntityDbState.Unchanged))
            ;
        }
    }
}
