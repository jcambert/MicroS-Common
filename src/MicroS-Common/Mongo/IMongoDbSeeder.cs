using System.Threading.Tasks;

namespace MicroS_Common.Mongo
{
    public interface IMongoDbSeeder
    {
        Task SeedAsync();
    }
}