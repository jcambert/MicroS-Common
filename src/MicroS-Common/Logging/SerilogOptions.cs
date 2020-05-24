namespace MicroS_Common.Logging
{
    public class SerilogOptions
    {
        public const string SECTION = "serilog";
        public bool ConsoleEnabled { get; set; }
        public string Level { get; set; }
    }
}
