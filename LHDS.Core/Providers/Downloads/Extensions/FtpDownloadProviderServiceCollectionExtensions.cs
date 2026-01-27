// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LHDS.Core.Models.Configurations;
using LHDS.Core.Providers.Downloads.Builders;
using LHDS.Core.Providers.Downloads.FtpDownloads;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xeptions;

namespace LHDS.Core.Providers.Downloads.Extensions
{
    public static class FtpDownloadProviderServiceCollectionExtensions
    {
        public static IServiceCollection UseFtpDownloadProvider(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<FtpProviderRegistrationBuilder> builderAction)
        {
            IFtpDownloadProviderSettings? ftpDownloadProviderSettings =
                configuration.GetSection("ftpDownload").Get<FtpDownloadProviderSettings>();

            ValidateFtpProviderSettings(ftpDownloadProviderSettings);

            if (ftpDownloadProviderSettings != null)
            {
                FtpProviderRegistrationBuilder builder =
                    new FtpProviderRegistrationBuilder(ftpDownloadProviderSettings);

                builderAction(builder);
                services.AddTransient<IFtpDownloadProviderSettings>(_ => ftpDownloadProviderSettings);
            }

            services.AddTransient<IDownloadProvider, FtpDownloadProvider>();

            return services;
        }

        private static void ValidateFtpProviderSettings(IFtpDownloadProviderSettings? ftpDownloadProviderSettings)
        {
            if (ftpDownloadProviderSettings is null)
            {
                throw new InvalidConfigurationException("ftpDownload configuration section missing");
            }

            Validate(
                createException: () => new InvalidConfigurationException(
                    message: "Invalid ftp download provider settings."),

                (Rule: IsInvalid(ftpDownloadProviderSettings.FtpPort),
                    Parameter: "ftpDownload__ftpPort"),

                (Rule: IsInvalid(ftpDownloadProviderSettings.IncludeSubDirectories),
                    Parameter: "ftpDownload__includeSubDirectories"));
        }

        private static dynamic IsInvalid(int value) => new
        {
            Condition = value == 0,
            Message = "Configuration value does not exist"
        };

        private static dynamic IsInvalid(bool? value) => new
        {
            Condition = value == null,
            Message = "Configuration value does not exist"
        };

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Configuration value does not exist"
        };

        private static void Validate<T>(
            Func<T> createException,
            params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
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

            T invalidDataException = createException();
            invalidDataException.ThrowIfContainsErrors();
        }
    }
}
