// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using Autofac;
using AutoMapper;
using Softeq.NetKit.ProfileService.Services;

namespace Softeq.NetKit.Profile.Test.Dependency
{
    public class AutomapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Configure AutoMapper
            typeof(BaseService)?.Assembly.GetTypes().Where(x => !x.IsAbstract && x.IsAssignableTo<AutoMapper.Profile>());

            builder.RegisterAssemblyTypes(typeof(BaseService).Assembly)
                .Where(x => !x.IsAbstract && x.IsAssignableTo<AutoMapper.Profile>())
                .As<AutoMapper.Profile>()
                .SingleInstance();

            builder.RegisterAssemblyTypes(typeof(BaseService).Assembly)
                .Where(x => !x.IsAbstract
                            && x.GetInterfaces().Any(t => t.IsGenericType
                                                          && (t.GetGenericTypeDefinition() == typeof(IValueResolver<,,>)
                                                              || t.GetGenericTypeDefinition() == typeof(IMemberValueResolver<,,,>)
                                                              || t.GetGenericTypeDefinition() == typeof(ITypeConverter<,>))))
                .AsImplementedInterfaces()
                .AsSelf();

            builder.Register(context =>
            {
                var temp = context.Resolve<IComponentContext>();
                var profiles = temp.Resolve<IEnumerable<AutoMapper.Profile>>();
                var config = new MapperConfiguration(x =>
                {
                    foreach (var profile in profiles)
                    {
                        x.AddProfile(profile);
                    }
                    x.ConstructServicesUsing(temp.Resolve);
                });
                return config;
            })
                .SingleInstance()
                .AsSelf()
                .AutoActivate();

            builder.Register(context =>
            {
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper();
            })
                .As<IMapper>();
        }
    }
}