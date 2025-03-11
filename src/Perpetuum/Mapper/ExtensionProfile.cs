using Perpetuum.ExportedTypes;
using Perpetuum.Services.ExtensionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perpetuum.Mapper
{
    internal class ExtensionProfile : AutoMapper.Profile
    {
        public ExtensionProfile()
        {
            //id = record.GetValue<int>("extensionid");
            //name = record.GetValue<string>("extensionname");
            //category = record.GetValue<int>("category");
            //rank = record.GetValue<int>("rank");
            //learningAttributePrimary = record.GetValue<string>("learningattributeprimary");
            //learningAttributeSecondary = record.GetValue<string>("learningattributesecondary");
            //bonus = record.GetValue<double>("bonus");
            //price = record.GetValue<int>("price");
            //_description = record.GetValue<string>("description");
            //aggregateField = (AggregateField)(record.GetValue<int?>("targetpropertyID") ?? 0);
            //hidden = record.GetValue<bool>("hidden");
            //freezeLimit = record.GetValue<int?>("freezelimit");

            CreateMap<DataContext.Entities.Extension, ExtensionInfo>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Extensionid))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Extensionname))
                .ForMember(dest => dest.aggregateField, opt => opt.MapFrom(src => (AggregateField)(src.TargetpropertyId ?? 0)))
            ;
        }
    }
}
