// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using CorrelationId;
using IdentityServer4.AccessTokenValidation;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Softeq.NetKit.Components.EventBus.Abstract;
using Softeq.NetKit.IntegrationService.EventTypes.Events;
using Softeq.NetKit.IntegrationService.EventTypes.Handlers;
using Softeq.NetKit.Profile.SQLRepository;
using Softeq.NetKit.Profile.Web.ExceptionHandling;
using Softeq.NetKit.Profile.Web.Logger;
using Softeq.NetKit.ProfileService.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace Softeq.NetKit.Profile.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(ConfigurationSettings.SQLConnectionStringName)));

            services.AddMvcCore(o =>
                {
                    o.Filters.Add(typeof(GlobalExceptionFilter));
                })
                .AddApiExplorer()
                .AddAuthorization()
                .AddJsonFormatters();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.ApiSecret = Configuration[ConfigurationSettings.IdentityApiSecret];
                    options.Authority = Configuration[ConfigurationSettings.IdentityAuthority];
                    options.RequireHttpsMetadata = false;
                    options.SupportedTokens = SupportedTokens.Both;

                    options.ApiName = "api";
                });

            services.AddHttpClient("ProfileHttpClient")
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddAutoMapper(typeof(BaseService).Assembly);

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddCors();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });

                c.SwaggerDoc("v1", new Info { Title = "API doc", Version = "v1" });
            });

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            containerBuilder.RegisterAssemblyModules(typeof(Startup).Assembly);
            containerBuilder.AddLogger();

            ApplicationContainer = containerBuilder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory, 
            IApplicationLifetime applicationLifetime,
            ApplicationDbContext context)
        {
            applicationLifetime.ApplicationStopping.Register(OnShutdown, app);

            app.UseCors(x => x.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseExceptionHandler(options =>
            {
                options.Run(async c => await ExceptionHandler.Handle(c, loggerFactory));
            });

            app.UseCorrelationId();

            app.UseAuthentication();

            #region Swagger

            if (!env.IsProduction())
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Versioned Api v1.0");
                });
            }

            #endregion

            app.UseMvc();

            //Todo: Remove after release
            try
            {
                context.Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            // TODO: Try get rid of GetAwaiter().GetResult()
            ConfigureEventBus(app).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private async Task ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBusSubscriber = app.ApplicationServices.GetService<IEventBusSubscriber>();

            await eventBusSubscriber.RegisterSubscriptionListenerAsync();
            await eventBusSubscriber.SubscribeAsync<AccountRegisteredEvent, AccountRegisteredEventHandler>();
        }

        private void OnShutdown(object builder)
        {
            if (builder is IApplicationBuilder applicationBuilder)
            {
                var telemetryClient = applicationBuilder.ApplicationServices.GetService<TelemetryClient>();
                if (telemetryClient != null)
                {
                    telemetryClient.Flush();
                    //Wait while the data is flushed
                    System.Threading.Thread.Sleep(1000);
                }
                Log.CloseAndFlush();
            }
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}