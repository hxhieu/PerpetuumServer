using Autofac;
using AutoMapper;
using Perpetuum.Mapper;

namespace Perpetuum.Bootstrapper.Modules
{
    internal class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    // Scan for profiles
                    cfg.AddMaps(typeof(_Profiles).Assembly);
                });
                return config.CreateMapper();
            }).SingleInstance();
        }
    }
}
