using Perpetuum.DataContext.Entities;
using Perpetuum.EntityFramework;
using Perpetuum.ExportedTypes;
using Perpetuum.GenXY;

namespace Perpetuum.Mapper
{
    internal class EntityDefaultProfile : AutoMapper.Profile
    {
        public EntityDefaultProfile()
        {
            //Volume = record.GetValue<double>("volume"),
            //_descriptionToken = record.GetValue<string>("descriptionToken"),
            //_hidden = record.GetValue<bool>("hidden"),
            //Definition = definition,
            //Name = record.GetValue<string>("definitionName"),
            //Quantity = record.GetValue<int>("quantity"),
            //AttributeFlags = new EntityAttributeFlags((ulong)record.GetValue<long>("attributeflags")),
            //CategoryFlags = (CategoryFlags)record.GetValue<long>("categoryflags"),
            //Mass = record.GetValue<double>("mass"),
            //Health = record.GetValue<double>("health"),
            //Purchasable = record.GetValue<bool>("purchasable"),
            //Options = new EntityDefaultOptions(((GenxyString)record.GetValue<string>("options")).ToDictionary()),
            //EnablerExtensions = GetEnablerAndRequiredExtensions(definition),
            //Config = definitionConfigs.GetOrDefault(definition, DefinitionConfig.None),
            //Tier = new TierInfo(tierType, tierLevel)

            CreateMap<Entitydefault, EntityDefault>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Definitionname))
                .ForMember(dest => dest.AttributeFlags, opt => opt.MapFrom(src => new EntityAttributeFlags((ulong)src.Attributeflags)))
                .ForMember(dest => dest.CategoryFlags, opt => opt.MapFrom(src => (CategoryFlags)src.Categoryflags))
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => new EntityDefaultOptions(((GenxyString)src.Options).ToDictionary())))
                .ForMember(dest => dest.Tier, opt => opt.MapFrom(src => new TierInfo((TierType)(src.Tiertype ?? 0), src.Tierlevel ?? 0)))
            ;
        }
    }
}
