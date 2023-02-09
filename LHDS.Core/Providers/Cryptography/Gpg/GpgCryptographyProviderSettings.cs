// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Microsoft.Extensions.Configuration;

namespace LHDS.Core.Providers.Cryptography.Gpg
{
    public class GpgCryptographyProviderSettings : IGpgCryptographyProviderSettings
    {
        private IConfiguration Configuration { get; }

        public GpgCryptographyProviderSettings(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public string PrivateKey => this.GetSettings("cryptography:gpgPrivateKey", true);
        public string PublicKey => this.GetSettings("cryptography:gpgPublicKey", true);
        public string Passphrase => this.GetSettings("cryptography:gpgPassphrase", true);

        private string GetSettings(string configurationKey, bool mandatory = true)
        {
            var value = this.Configuration[configurationKey];

            if (string.IsNullOrEmpty(value))
            {
                if (mandatory)
                {
                    throw new Exception($"Configuration value {configurationKey} does not exist");
                }
            }

            return value;
        }
    }
}
