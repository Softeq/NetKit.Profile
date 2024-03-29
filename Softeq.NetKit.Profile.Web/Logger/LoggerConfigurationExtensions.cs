﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.ExtensionMethods;
using Softeq.Serilog.Extension;

namespace Softeq.NetKit.Profile.Web.Logger
{
    public static class LoggerConfigurationExtensions
    {
        public static IWebHostBuilder UseSerilog(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((context, collection) => collection.AddLogging(builder => SetupSerilog(context, collection)));
        }

        private static void SetupSerilog(WebHostBuilderContext hostingContext, IServiceCollection serviceCollection)
        {
            var applicationName = hostingContext.Configuration[ConfigurationSettings.SerilogApplicationName];
            var environment = hostingContext.HostingEnvironment.EnvironmentName;
            var applicationSlotName = $"{applicationName}:{environment}";

            var loggerConfiguration = new LoggerConfiguration().ReadFrom.Configuration(hostingContext.Configuration)
                                                               .Enrich.WithProperty(PropertyNames.Application, applicationSlotName);

            var template = GetLogTemplate();

            if (hostingContext.HostingEnvironment.IsProduction() ||
                hostingContext.HostingEnvironment.IsStaging() ||
                hostingContext.HostingEnvironment.IsDevelopment())
            {
                var instrumentationKey = hostingContext.Configuration[ConfigurationSettings.ApplicationInsightsInstrumentationKey];
                var telemetryClient = new TelemetryClient { InstrumentationKey = instrumentationKey };
                loggerConfiguration.WriteTo.ApplicationInsightsEvents(telemetryClient, logEventToTelemetryConverter: LogEventsToTelemetryConverter);

                serviceCollection.AddSingleton(telemetryClient);
            }
            else
            {
                loggerConfiguration.WriteTo.Debug(outputTemplate: template);
            }

            bool.TryParse(hostingContext.Configuration[ConfigurationSettings.SerilogEnableLocalFileSink], out var localFileSinkEnabled);
            if (localFileSinkEnabled)
            {
                loggerConfiguration.WriteTo.RollingFile("logs/log-{Date}.txt",
                                                        outputTemplate: template,
                                                        fileSizeLimitBytes: int.Parse(hostingContext.Configuration[ConfigurationSettings.SerilogFileSizeLimitMBytes]) * 1024 * 1024);
            }

            var logger = loggerConfiguration.CreateLogger();
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                logger.Event("UnhandledExceptionCaughtByAppDomainUnhandledExceptionHandler")
                    .With.Message("{ExceptionObject} {IsTerminating}", args.ExceptionObject, args.IsTerminating)
                    .AsError();
            };

            Log.Logger = logger;
        }

        private static ITelemetry LogEventsToTelemetryConverter(LogEvent serilogLogEvent, IFormatProvider formatProvider)
        {
            if (serilogLogEvent.Exception == null)
            {
                if (serilogLogEvent.Properties.ContainsKey(PropertyNames.EventId))
                {
                    var eventTelemetry = new EventTelemetry(serilogLogEvent.Properties[PropertyNames.EventId].ToString())
                    {
                        Timestamp = serilogLogEvent.Timestamp
                    };
                    serilogLogEvent.ForwardPropertiesToTelemetryProperties(eventTelemetry, formatProvider);
                    return eventTelemetry;
                }

                var exceptionTelemetry = new ExceptionTelemetry(new Exception($"Event does not contain '{PropertyNames.EventId}' property"))
                {
                    Timestamp = serilogLogEvent.Timestamp
                };
                serilogLogEvent.ForwardPropertiesToTelemetryProperties(exceptionTelemetry, formatProvider);
                return exceptionTelemetry;
            }
            return serilogLogEvent.ToDefaultExceptionTelemetry(formatProvider);
        }

        private static string GetLogTemplate()
        {
            var template = new SerilogTemplateBuilder().Timestamp()
                .Level()
                .CorrelationId()
                .EventId()
                .Message()
                .NewLine()
                .Exception()
                .Build();
            return template;
        }
    }
}