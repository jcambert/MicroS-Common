using Autofac;
using AutoMapper;
using Consul;
using MicroS_Common.Applications;
using MicroS_Common.Authentication;
using MicroS_Common.Consul;
using MicroS_Common.Dispatchers;
using MicroS_Common.Jeager;
using MicroS_Common.Mongo;
using MicroS_Common.Mvc;
using MicroS_Common.RabbitMq;
using MicroS_Common.Redis;
using MicroS_Common.Repository;
using MicroS_Common.Services.Identity.Dto;
using MicroS_Common.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
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
        public ApplicationOptions ApplicationOptions { get; }
        public bool UseMongo { get; }
        protected abstract Type DomainType { get; }
        protected virtual bool UseCors => false;
        private Type MongoDatabaseSeederType => Assembly.GetEntryAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(MongoDbSeeder))).FirstOrDefault();
        public BaseStartup(IConfiguration configuration)
        {
            Configuration = configuration;
            UseMongo = (Configuration.GetOptions<MongoDbOptions>("mongo") != null);
            ApplicationOptions options;
            ApplicationOptions = configuration.TryGetOptions<ApplicationOptions>("app", out options) ? options : new ApplicationOptions() { Name = ApplicationOptions.DEFAULT_NAME };
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {

            services.AddCustomMvc();
            services.AddSwaggerDocs();
            services.AddConsul();
            services.AddJwt();
            services.AddJaeger();
            services.AddRedis();
            services.AddOpenTracing();
            if (UseMongo) services.AddInitializers(typeof(IMongoDbInitializer));
            services.AddAutoMapper(typeof(BaseStartup).Assembly, Assembly.GetEntryAssembly(), DomainType?.Assembly);
            services.AddRepositories(typeof(BaseStartup).Assembly, Assembly.GetEntryAssembly());
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

        }
        public virtual void ConfigureContainer(ContainerBuilder builder)
        {

            builder.RegisterAssemblyTypes(typeof(BaseStartup).Assembly).AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly()).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(DomainType?.Assembly).AsImplementedInterfaces();
            //Used only for Identity Services
            builder.RegisterType<PasswordHasher<User>>().As<IPasswordHasher<User>>();
            builder.AddDispatchers();
            builder.AddRabbitMq();
            if (UseMongo)
                builder.AddMongo(MongoDatabaseSeederType).AddRepositories(typeof(BaseStartup).Assembly, Assembly.GetEntryAssembly(), DomainType?.Assembly); ;
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
                app.UseDeveloperExceptionPage();
            }

            app.UseAllForwardedHeaders();
            if (UseCors)
                app.UseCors("CorsPolicy");
            app.UseSwaggerDocs();
            app.UseErrorHandler();
            app.UseServiceId();
            app.UseMvc();
            SubscribeEventAndMessageBus(app.UseRabbitMq());

            var consulServiceId = app.UseConsul();

            applicationLifetime.ApplicationStarted.Register(() =>
            {
                logger.LogInformation($"{ApplicationOptions.Name} Service is started");
            });

            applicationLifetime.ApplicationStopped.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(consulServiceId);
                logger.LogInformation($"{ApplicationOptions.Name} Service is being stopping");
            });

            startupInitializer.InitializeAsync();

           
         
        }
        protected virtual void SubscribeEventAndMessageBus(IBusSubscriber bus)
        {

        }

    }
}
