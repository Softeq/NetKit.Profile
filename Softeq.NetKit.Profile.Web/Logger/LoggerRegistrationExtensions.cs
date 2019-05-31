// Developed by Softeq Development Corporation
// http://www.softeq.com

using Autofac;
using CorrelationId;
using Serilog;
using Softeq.NetKit.Profile.Web.Utility;

namespace Softeq.NetKit.Profile.Web.Logger
{
    public static class LoggerRegistrationExtensions
    {
        public static void AddLogger(this ContainerBuilder builder)
        {
            builder.Register((c, p) =>
                {
                    var correlationContextAccessor = c.Resolve<ICorrelationContextAccessor>();
                    return Log.Logger.ForContext(new CorrelationIdEnricher(correlationContextAccessor));
                })
                .As<ILogger>().InstancePerLifetimeScope();
        }
    }
}