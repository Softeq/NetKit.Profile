// Developed by Softeq Development Corporation
// http://www.softeq.com

using Autofac;
using Microsoft.Extensions.Configuration;
using Softeq.CloudStorage.Extension;
using Softeq.NetKit.ProfileService.Abstract;

namespace Softeq.NetKit.Profile.Test.Dependency
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x =>
                {
                    var context = x.Resolve<IComponentContext>();
                    var config = context.Resolve<IConfigurationRoot>();
                    var storage = new AzureCloudStorage(config[ConfigurationSettings.AzureStorageConnectionString]);
                    return storage;
                })
                .As<IContentStorage>();

            builder.RegisterType<ProfileService.Services.ProfileService>()
                .As<IProfileService>();

            builder.RegisterType<ProfileService.Services.AttachmentService>()
                .As<IAttachmentService>();
        }
    }
}