// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Softeq.NetKit.Profile.Domain.Models.Configuration;

namespace Softeq.NetKit.Profile.Test.Dependency
{
    public class StartupModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var configurationRoot = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            builder.RegisterInstance(configurationRoot)
                .As<IConfigurationRoot>();

            builder.RegisterType<LoggerFactory>()
                .As<ILoggerFactory>();

            builder.Register(context =>
                {
                    var temp = context.Resolve<IComponentContext>();
                    return new AutofacServiceProvider(temp);
                })
                .As<IServiceProvider>()
                .SingleInstance();

            builder.Register(x =>
            {
                var cfg = new CloudStorageConfiguration(
                    configurationRoot[ConfigurationSettings.AzureStorageContentStorageHost],
                    configurationRoot[ConfigurationSettings.AzureStorageUserPhotoContainerName],
                    Convert.ToInt32(configurationRoot[ConfigurationSettings.AzureStorageUserPhotoSize]),
                    configurationRoot[ConfigurationSettings.AzureStorageTempContainerName],
                    Convert.ToInt32(configurationRoot[ConfigurationSettings.AzureStorageContainerAccessTokenTimeToLive]));
                return cfg;
            }).AsSelf().SingleInstance();
        }
    }
}