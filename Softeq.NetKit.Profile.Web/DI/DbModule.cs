// Developed by Softeq Development Corporation
// http://www.softeq.com

using Autofac;
using Softeq.NetKit.Profile.Domain.Infrastructure;
using Softeq.NetKit.Profile.SQLRepository;

namespace Softeq.NetKit.Profile.Web.DI
{
    public class DbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>();
        }
    }
}