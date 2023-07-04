// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Configurations;
using LHDS.Core.Providers.Cryptography.Builders;
using LHDS.Core.Providers.Cryptography.Gpg;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LHDS.Core.Providers.Cryptography.Extensions
{
    public static class GpgCryptographyProviderServiceCollectionExtensions
    {
        public static IServiceCollection UseGpgCryptographyProvider(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<GpgProviderRegistrationBuilder> builderAction)
        {
            IGpgCryptographyProviderSettings gpgCryptographyProviderSettings =
                configuration.GetSection("cryptography").Get<GpgCryptographyProviderSettings>();

            ValidateCryptographyProviderSettings(gpgCryptographyProviderSettings);

            GpgProviderRegistrationBuilder builder =
                new GpgProviderRegistrationBuilder(gpgCryptographyProviderSettings);

            builderAction(builder);

            services.AddTransient<IGpgCryptographyProviderSettings>(_ => gpgCryptographyProviderSettings);
            services.AddTransient<ICryptographyProvider, GpgCryptographyProvider>();

            return services;
        }

        private static void ValidateCryptographyProviderSettings(
            IGpgCryptographyProviderSettings cryptographyProviderSettings)
        {
            Validate(
                (Rule: IsInvalid(cryptographyProviderSettings.PrivateKey),
                    Parameter: "cryptography__privateKey"),

                (Rule: IsInvalid(cryptographyProviderSettings.PublicKey),
                    Parameter: "cryptography__publicKey"),

                (Rule: IsInvalid(cryptographyProviderSettings.Passphrase),
                    Parameter: "cryptography__passphrase"));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Configuration value does not exist"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidConfigurationException = new InvalidConfigurationException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidConfigurationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidConfigurationException.ThrowIfContainsErrors();
        }
    }
}
