using Autofac;
using MicroS_Common.Types;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Reflection;
using System.Linq;
namespace MicroS_Common.Mongo
{
    public static class Extensions
    {
        public static ContainerBuilder AddMongo(this ContainerBuilder builder, Type seederType = null)
        {
            builder.Register(context =>
            {
                var configuration = context.Resolve<IConfiguration>();
                var options = configuration.GetOptions<MongoDbOptions>("mongo");

                return options;
            }).SingleInstance();

            builder.Register(context =>
            {
                var options = context.Resolve<MongoDbOptions>();

                return new MongoClient(options.ConnectionString);
            }).SingleInstance();

            builder.Register(context =>
            {
                var options = context.Resolve<MongoDbOptions>();
                var client = context.Resolve<MongoClient>();
                return client.GetDatabase(options.Database);

            }).InstancePerLifetimeScope();

            builder.RegisterType<MongoDbInitializer>()
                .As<IMongoDbInitializer>()
                .InstancePerLifetimeScope();

            if (seederType == null)
                builder.RegisterType<MongoDbSeeder>()
                    .As<IMongoDbSeeder>()
                    .InstancePerLifetimeScope();
            else
                builder.RegisterType(seederType)
               .As<IMongoDbSeeder>()
               .InstancePerLifetimeScope();
            return builder;
        }

        /// <summary>
        /// Add Mongo Repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="builder"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public static ContainerBuilder AddMongoRepository<TEntity>(this ContainerBuilder builder, string collectionName)
            where TEntity : IIdentifiable
        {
            builder.Register(ctx => new MongoRepository<TEntity>(ctx.Resolve<IMongoDatabase>(), collectionName))
                  .As<IMongoRepository<TEntity>>()
                  .InstancePerLifetimeScope();
            return builder;
        }

        /// <summary>
        /// Add Mongo Repository
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public static ContainerBuilder AddMongoRepositoryByType(this ContainerBuilder builder, Type type, string collectionName)

        {
            var m0 = typeof(MicroS_Common.Mongo.Extensions).GetMethod("AddMongoRepository");
            var m1 = m0.MakeGenericMethod(type);
            m1.Invoke(null, new object[] { builder, collectionName });

            return builder;
        }

        public static ContainerBuilder AddRepositories(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            var typesWithMyAttribute =
                from a in assemblies
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(MongoDocumentAttribute), true)
                where attributes != null && attributes.Length > 0
                select new { Type = t, Attributes = attributes.Cast<MongoDocumentAttribute>() };
            foreach (var item in typesWithMyAttribute)
            {

                builder.AddMongoRepositoryByType(item.Type, item.Attributes.First().Name);
            }
            return builder;


        }
    }
}
