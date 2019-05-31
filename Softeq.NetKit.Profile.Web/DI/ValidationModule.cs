// Developed by Softeq Development Corporation
// http://www.softeq.com

using Autofac;
using FluentValidation;
using FluentValidation.Validators;
using Softeq.NetKit.ProfileService.Services;

namespace Softeq.NetKit.Profile.Web.DI
{
    public class ValidationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(BaseService).Assembly)
                .Where(x => !x.IsAbstract && x.IsAssignableTo<IValidator>())
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();

            builder.RegisterAssemblyTypes(typeof(BaseService).Assembly)
                .Where(x => !x.IsAbstract && x.IsAssignableTo<IPropertyValidator>())
                .As<IPropertyValidator>()
                .AsSelf();
        }
    }
}