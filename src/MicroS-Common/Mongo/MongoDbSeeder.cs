using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;

namespace MicroS_Common.Mongo
{
    public abstract class MongoDbSeeder : IMongoDbSeeder
    {
        protected readonly IMongoDatabase Database;
        protected ILogger<MongoDbSeeder> Logger { get; }
        private readonly List<Action> seeders;
        public MongoDbSeeder(IMongoDatabase database,ILogger<MongoDbSeeder> logger)
        {
            Database = database;
            Logger = logger;
            seeders = new List<Action>();
        }
        public List<Action> Seeders => seeders;
        public async Task SeedAsync()
        {
            FeedSeeder();
            await CustomSeedAsync();
        }
        protected abstract void FeedSeeder();
        protected virtual async Task CustomSeedAsync()
        {
            seeders.ForEach(seeder => seeder());
            await Task.CompletedTask;
        }
    }
}
