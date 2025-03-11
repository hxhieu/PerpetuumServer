using Perpetuum.Services.Channels;

namespace Perpetuum.Mapper
{
    internal class ChannelProfile : AutoMapper.Profile
    {
        public ChannelProfile()
        {
            CreateMap<DataContext.Entities.Channel, Channel>()
                 .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (ChannelType)src.Type))
            ;
        }
    }
}
