namespace MicroS_Common.Mongo
{
    public class MongoDbOptions
    {
        public const string SECTION = "mongo";
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public bool Seed { get; set; }
    }
}
