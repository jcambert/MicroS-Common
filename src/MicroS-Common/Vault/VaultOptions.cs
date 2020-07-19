using System;
using System.Collections.Generic;
using System.Text;

namespace MicroS_Common.Vault
{
    public sealed class VaultOptions
    {
        public const string SECTION = "vault";
        public bool Enabled { get; set; }
        public string Url { get; set; }
        public string Key { get; set; }
        public string AuthType { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
