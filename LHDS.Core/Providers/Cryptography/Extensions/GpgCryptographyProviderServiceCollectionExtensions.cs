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
using Xeptions;

namespace LHDS.Core.Providers.Cryptography.Extensions
{
    public static class GpgCryptographyProviderServiceCollectionExtensions
    {
        public static IServiceCollection UseGpgCryptographyProvider(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<GpgProviderRegistrationBuilder> builderAction)
        {
            GpgProviderRegistrationBuilder builder =
                new GpgProviderRegistrationBuilder();

            builderAction(builder);

            services.AddTransient<ICryptographyProvider, GpgCryptographyProvider>();

            return services;
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Configuration value does not exist"
        };

        private static void Validate<T>(
            Func<string, IDictionary, T> createException,
            params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            StringBuilder validationErrors = new StringBuilder();
            validationErrors.AppendLine("Configuration error(s):");

            // Non-generic IDictionary to match InvalidConfigurationException
            IDictionary errors = new Dictionary<string, List<string>>();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    validationErrors.AppendLine($"{parameter}");

                    if (errors.Contains(parameter))
                    {
                        (errors[parameter] as List<string>)?.Add(rule.Message);
                    }
                    else
                    {
                        errors.Add(parameter, new List<string> { rule.Message });
                    }
                }
            }

            T invalidDataException = createException(
                validationErrors.ToString(),
                errors);

            invalidDataException.ThrowIfContainsErrors();
        }
    }
}
