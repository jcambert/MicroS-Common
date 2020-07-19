using Autofac;
using AutoMapper;
using Consul;
using MicroS_Common.Authentication;
using MicroS_Common.Consul;
using MicroS_Common.Dispatchers;
using MicroS_Common.Dispatchers.Operations.Events;
using MicroS_Common.Jeager;
using MicroS_Common.Logging;
using MicroS_Common.Mongo;
using MicroS_Common.Mvc;
using MicroS_Common.RabbitMq;
using MicroS_Common.Redis;
using MicroS_Common.Repository;
using MicroS_Common.RestEase;
using MicroS_Common.Services.Identity.Dto;
using MicroS_Common.Services.SignalR;
using MicroS_Common.Services.SignalR.Hubs;
using MicroS_Common.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MicroS_Common
{
    public interface IBaseStartup
    {

    }
    public abstract class BaseStartup : IBaseStartup
    {
        private static readonly string[] Headers = new[] { "X-Operation", "X-Resource", "X-Total-Count" };
        public IConfiguration Configuration { get; }
        public AppOptions ApplicationOptions { get; }
        public SignalrOptions SignalROptions { get;  }
        public ILogger<BaseStartup> Logger { get; private set; }
        public bool UseSignalR => !string.IsNullOrEmpty(SignalROptions.Hub);
        public bool UseMongo { get; }
        protected virtual bool UseCors => false;
        protected abstract Type DomainType { get; }
        //private Type MongoDatabaseSeederType => Assembly.GetEntryAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(MongoDbSeeder))).FirstOrDefault();
        List<Assembly> Assemblies { get; }
        public BaseStartup(IConfiguration configuration)
        {
            Configuration = configuration;
            UseMongo = (Configuration.GetOptions<MongoDbOptions>(MongoDbOptions.SECTION).Database != null);
            SignalROptions = Configuration.GetOptions<SignalrOptions>(SignalrOptions.SECTION);
            ApplicationOptions = configuration.GetOptions<AppOptions>(AppOptions.SECTION);

            Assemblies = new List<Assembly> {
                typeof(BaseStartup).Assembly,
                Assembly.GetEntryAssembly(),
                //Assembly.GetExecutingAssembly(),
            };
            if (DomainType != null)
                Assemblies.Add(DomainType?.Assembly);
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            Logger = serviceProvider.GetService<ILogger<BaseStartup>>();
            services.AddCustomMvc();
            services.AddSwaggerDocs();
            services.AddConsul();
            services.AddJwt();
            services.AddJaeger();
            services.AddRedis();
            //services.AddOpenTracing();
            services.AddAutoMapper(Assemblies);
            services.AddRepositories(Assemblies);
            if (UseCors)
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", cors =>
                            cors
                                //.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials()
                                .WithExposedHeaders(Headers));
                });
            ConfigureAuthorization(services);
            services.RegisterAllServiceForwarders(Assembly.GetEntryAssembly());
            if (UseSignalR)
                services.AddSignalr();
            if (ApplicationOptions.UseBlazor)
            {
                services.AddRazorPages();
                services.AddServerSideBlazor();
            }
        }

        
        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            var bld = builder.RegisterAssemblyTypes(Assemblies.ToArray());
            if (!UseMongo)
                bld.Except<MongoDbInitializer>();
            bld.Except<StartupInitializer>().AsImplementedInterfaces().InstancePerLifetimeScope();

             //Used only for Identity Services
            builder.RegisterType<PasswordHasher<User>>().As<IPasswordHasher<User>>();
            builder.AddDispatchers();
            builder.AddRabbitMq();
            if (UseMongo)
                builder
                    .AddMongo()
                    .AddRepositories(Assemblies);
        }

        public virtual void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IHostApplicationLifetime applicationLifetime,
            IStartupInitializer startupInitializer,
            IConsulClient consulClient,
            IServiceProvider services,
           ILogger<BaseStartup> logger)
        {
            
            if (env.IsDevelopment())
            {
                app.UseMiddleware<RequestLoggingMiddleware>();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            var consulServiceId = app.UseConsul();
            applicationLifetime.ApplicationStarted.Register(() =>
            {
                startupInitializer.InitializeAsync();
                logger.LogInformation($"{ApplicationOptions.Name} Service is started");
            });

            applicationLifetime.ApplicationStopped.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(consulServiceId);
                logger.LogInformation($"{ApplicationOptions.Name} Service is being stopping");
            });
            if (UseCors)
                app.UseCors("CorsPolicy");
            app.UseAllForwardedHeaders();
            if (ApplicationOptions.UseHttps)
                app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSwaggerDocs();
            app.UseErrorHandler();
            app.UseServiceId();
            app.UseAuthentication();
            app.UseAccessTokenValidator();
            if(UseSignalR)
                app.UseSignalR(routes =>
                {
                    logger.LogInformation($"SignalR Hub Mapping on /{SignalROptions.Hub}");
                    routes.MapHub<MicroSHub>($"/{SignalROptions.Hub}");

                });
            app.UseMvc();
            SubscribeEventAndMessageBus(app.UseRabbitMq());
            if (ApplicationOptions.UseBlazor)
            {
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapBlazorHub();
                    endpoints.MapFallbackToPage("/_Host");
                });
            }
        }
        protected virtual void SubscribeEventAndMessageBus(IBusSubscriber bus)
        {
            if (UseSignalR)
            {
                bus.SubscribeEvent<OperationPending>(@namespace: "operations")
                .SubscribeEvent<OperationCompleted>(@namespace: "operations")
                .SubscribeEvent<OperationRejected>(@namespace: "operations");
            }

            if (DomainType != null)
            {
                bus.SubscribeAllMessages(true, DomainType.Assembly);
               // bus.SubscribeOnRejected(DomainType.Assembly);
            }
        }

        protected virtual void ConfigureAuthorization(IServiceCollection services)
        {

        }
    }
}
