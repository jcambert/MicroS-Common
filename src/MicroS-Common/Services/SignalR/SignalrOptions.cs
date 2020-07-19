using Microsoft.AspNetCore.SignalR;

namespace MicroS_Common.Services.SignalR
{
    public class SignalrOptions
    {
        public const string SECTION = "signalr";
        public bool Enabled { get; set; }
        public string Backplane { get; set; }
        public string Hub { get; set; }
        public string Url { get; set; } = string.Empty;
        public HubOptions Options { get; set; }

    }
}
