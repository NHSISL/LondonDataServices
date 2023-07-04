// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Configurations;
using LHDS.Core.Providers.Downloads.Builders;
using LHDS.Core.Providers.Downloads.FtpDownloads;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LHDS.Core.Providers.Downloads.Extensions
{
    public static class FtpDownloadProviderServiceCollectionExtensions
    {
        public static IServiceCollection UseFtpDownloadProvider(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<FtpProviderRegistrationBuilder> builderAction)
        {
            IFtpDownloadProviderSettings ftpDownloadProviderSettings =
                configuration.GetSection("cryptography").Get<FtpDownloadProviderSettings>();

            ValidateFtpProviderSettings(ftpDownloadProviderSettings);

            FtpProviderRegistrationBuilder builder =
                new FtpProviderRegistrationBuilder(ftpDownloadProviderSettings);

            builderAction(builder);

            services.AddTransient<IFtpDownloadProviderSettings>(_ => ftpDownloadProviderSettings);
            services.AddTransient<IDownloadProvider, FtpDownloadProvider>();

            return services;
        }

        private static void ValidateFtpProviderSettings(IFtpDownloadProviderSettings ftpDownloadProviderSettings)
        {
            Validate(
                (Rule: IsInvalid(ftpDownloadProviderSettings.FtpPort),
                    Parameter: "ftpDownload__ftpPort"),

                (Rule: IsInvalid(ftpDownloadProviderSettings.FtpUserName),
                    Parameter: "ftpDownload__ftpUserName"),

                (Rule: IsInvalid(ftpDownloadProviderSettings.TempFolder),
                    Parameter: "ftpDownload__tempFolder"),

                (Rule: IsInvalid(ftpDownloadProviderSettings.IncludeSubDirectories),
                    Parameter: "ftpDownload__includeSubDirectories"));
        }

        private static dynamic IsInvalid(int value) => new
        {
            Condition = value == 0,
            Message = "Configuration value does not exist"
        };

        private static dynamic IsInvalid(bool value) => new
        {
            Condition = value == null,
            Message = "Configuration value does not exist"
        };

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
