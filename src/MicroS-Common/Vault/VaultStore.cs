using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods.UserPass;

namespace MicroS_Common.Vault
{
    public class VaultStore : IVaultStore
    {
        private readonly VaultOptions _options;
        public const string VAULT_URL="VAULT_URL";
        public const string VAULT_KEY="VAULT_KEY";
        public const string VAULT_AUTH_TYPE="VAULT_AUTH_TYPE";
        public const string VAULT_TOKEN="VAULT_TOKEN";
        public const string VAULT_USERNAME="VAULT_USERNAME";
        public const string VAULT_PASSWORD="VAULT_PASSWORD";
        public VaultStore(VaultOptions options)
        {
            _options = options;
            LoadEnvironmentVariables();
        }

        public async Task<T> GetDefaultAsync<T>()
            => await GetAsync<T>(_options.Key);

        public async Task<IDictionary<string, object>> GetDefaultAsync()
            => await GetAsync(_options.Key);

        public async Task<T> GetAsync<T>(string key)
            => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject((await GetAsync(key))));

        public async Task<IDictionary<string, object>> GetAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new VaultException("Vault secret key can not be empty.");
            }
            try
            {
                var settings = new VaultClientSettings(_options.Url, GetAuthMethod());
                var client = new VaultClient(settings);
                var secret = await client.V1.Secrets.KeyValue.V2.ReadSecretAsync(key);

                return secret.Data.Data;
            }
            catch (Exception exception)
            {
                throw new VaultException($"Getting Vault secret for key: '{key}' caused an error. " +
                    $"{exception.Message}", exception, key);
            }
        }

        private IAuthMethodInfo GetAuthMethod()
        {
            switch (_options.AuthType?.ToLowerInvariant())
            {
                case "token": return new TokenAuthMethodInfo(_options.Token);
                case "userpass": return new UserPassAuthMethodInfo(_options.Username, _options.Password);
            }

            throw new VaultAuthTypeNotSupportedException($"Vault auth type: '{_options.AuthType}' is not supported.",
                _options.AuthType);
        }

        private void LoadEnvironmentVariables()
        {
            _options.Url = VAULT_URL.GetEnvironmentVariableValue() ?? _options.Url;
            _options.Key = VAULT_KEY.GetEnvironmentVariableValue() ?? _options.Key;
            _options.AuthType = VAULT_AUTH_TYPE.GetEnvironmentVariableValue() ?? _options.AuthType;
            _options.Token = VAULT_TOKEN.GetEnvironmentVariableValue() ?? _options.Token;
            _options.Username = VAULT_USERNAME.GetEnvironmentVariableValue() ?? _options.Username;
            _options.Password = VAULT_PASSWORD.GetEnvironmentVariableValue() ?? _options.Password;
        }

       
    }
}
