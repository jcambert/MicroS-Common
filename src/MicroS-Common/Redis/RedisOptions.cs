namespace MicroS_Common.Redis
{
    public class RedisOptions
    {
        public string ConnectionString { get; set; }
        public string Instance { get; set; }

        public bool Enabled { get; set; } = true;
        public bool UseCache { get; set; } = true;
        public int? TimeLive { get; set; }
    }
}
