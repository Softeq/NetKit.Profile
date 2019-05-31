// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Autofac;
using CorrelationId;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Softeq.CloudStorage.Extension;
using Softeq.HttpClient.Extension.Utility;
using Softeq.NetKit.Components.EventBus;
using Softeq.NetKit.Components.EventBus.Abstract;
using Softeq.NetKit.Components.EventBus.Managers;
using Softeq.NetKit.Components.EventBus.Service;
using Softeq.NetKit.Components.EventBus.Service.Connection;
using Softeq.NetKit.Integrations.EventLog;
using Softeq.NetKit.Integrations.EventLog.Abstract;
using Softeq.NetKit.IntegrationService.EventTypes.Handlers;
using Softeq.NetKit.Profile.Domain.Models.Configuration;
using Softeq.NetKit.ProfileService.Utility.HttpClient;

namespace Softeq.NetKit.Profile.Web.DI
{
    public class StartupModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region IntegrationServices

            builder.Register(x =>
            {
                var context = x.Resolve<IComponentContext>();
                var configurationRoot = context.Resolve<IConfiguration>();

                var cfg = new CloudStorageConfiguration(
                    configurationRoot[ConfigurationSettings.AzureStorageContentStorageHost],
                    configurationRoot[ConfigurationSettings.AzureStorageUserPhotoContainerName],
                    Convert.ToInt32(configurationRoot[ConfigurationSettings.AzureStorageUserPhotoSize]),
                    configurationRoot[ConfigurationSettings.AzureStorageTempContainerName],
                    Convert.ToInt32(configurationRoot[ConfigurationSettings.AzureStorageContainerAccessTokenTimeToLive]));
                return cfg;
            });

            builder.Register(x =>
                {
                    var context = x.Resolve<IComponentContext>();
                    var config = context.Resolve<IConfiguration>();
                    var storage = new AzureCloudStorage(config[ConfigurationSettings.AzureStorageConnectionString]);
                    return storage;
                })
                .As<IContentStorage>();

            builder
                .Register(context =>
                {
                    var config = context.Resolve<IConfiguration>();

                    return new TopicClient(config[ConfigurationSettings.AzureServiceBusConnectionString],
                        config[ConfigurationSettings.AzureServiceBusTopicClientName]);
                })
                .As<ITopicClient>()
                .SingleInstance();

            builder.Register(x =>
            {
                var context = x.Resolve<IComponentContext>();
                var config = context.Resolve<IConfiguration>();

                return new ServiceBusPersisterConnectionConfiguration
                {
                    ConnectionString = config[ConfigurationSettings.AzureServiceBusConnectionString],
                    TopicConfiguration = new ServiceBusPersisterTopicConnectionConfiguration
                    {
                        TopicName = config[ConfigurationSettings.AzureServiceBusTopicClientName],
                        SubscriptionName = config[ConfigurationSettings.AzureServiceBusSubscriptionClientName]
                    }
                };

            }).AsSelf().SingleInstance();

            builder.RegisterType<ServiceBusPersisterConnection>()
                .As<IServiceBusPersisterConnection>()
                .SingleInstance();

            builder.Register(context =>
            {
                var config = context.Resolve<IConfiguration>();

                return new MessageQueueConfiguration
                {
                    TimeToLiveInMinutes = Convert.ToInt32(config[ConfigurationSettings.AzureServiceBusTimeToLive])
                };
            });

            builder.RegisterType<EventBusSubscriptionsManager>().As<IEventBusSubscriptionsManager>();
            builder.RegisterType<EventBusService>().As<IEventBusPublisher>();
            builder.RegisterType<EventBusService>().As<IEventBusSubscriber>();

            builder.Register(context =>
            {
                var config = context.Resolve<IConfiguration>();

                return new EventPublishConfiguration(config[ConfigurationSettings.AzureServiceBusEventPublisherId]);

            }).AsSelf();

            #endregion

            #region EventBusHandlers

            builder.RegisterType<AccountRegisteredEventHandler>();
            builder.RegisterType<IntegrationEventLogService>().As<IIntegrationEventLogService>();
            builder.RegisterType<ExceptionHandler>().As<IExceptionHandler>();

            #endregion

            builder.RegisterType<CorrelationContextAccessor>().As<ICorrelationContextAccessor>().SingleInstance();
            builder.RegisterType<CorrelationContextFactory>().As<ICorrelationContextFactory>().InstancePerDependency();

            builder.Register(x =>
            {
                var context = x.Resolve<IComponentContext>();
                var configurationRoot = context.Resolve<IConfiguration>();

                var cfg = new HttpClientOptions(
                    configurationRoot[ConfigurationSettings.DependenciesPaymentServiceApiUrl],
                    configurationRoot[ConfigurationSettings.DependenciesNotificationServiceApiUrl],
                    configurationRoot[ConfigurationSettings.DependenciesMessagingServiceApiUrl]);

                return cfg;
            });
        }
    }
}