// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
            StringBuilder validationErrors = new StringBuilder();
            validationErrors.AppendLine("Configuration error(s):");
            IDictionary errors = new Dictionary<string, List<string>>();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    validationErrors.AppendLine($"{parameter}");

                    if (errors.Contains(parameter))
                    {
                        (errors[parameter] as List<string>)?.Add(rule.Message);
                        return;
                    }

                    errors.Add(parameter, new List<string> { rule.Message });
                }
            }

            var invalidConfigurationException = new InvalidConfigurationException(
                message: validationErrors.ToString(),
                data: errors);

            invalidConfigurationException.ThrowIfContainsErrors();
        }
    }
}
