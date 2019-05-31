// Developed by Softeq Development Corporation
// http://www.softeq.com

using Autofac;
using Softeq.NetKit.ProfileService.Abstract;

namespace Softeq.NetKit.Profile.Web.DI
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProfileService.Services.ProfileService>()
                .As<IProfileService>();

            builder.RegisterType<ProfileService.Services.AttachmentService>()
                .As<IAttachmentService>();
        }
    }
}