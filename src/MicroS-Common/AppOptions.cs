namespace MicroS_Common
{
    public class AppOptions
    {
        public const string SECTION = "app";
        public string Name { get; set; }
        public bool UseHttps { get; set; } = false;
        public bool UseBlazor { get; set; } = false;
    }
}
