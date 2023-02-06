// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Landings.Client.Providers.Downloads.Builders;
using LHDS.Landings.Client.Providers.Downloads.FtpDownloads;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LHDS.Landings.Client.Providers.Downloads.Extensions
{
    public static class FtpDownloadProviderServiceCollectionExtensions
    {
        public static IServiceCollection UseFtpDownloadProvider(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<FtpProviderRegistrationBuilder> builderAction)
        {
            IFtpDownloadProviderSettings ftpDownloadProviderSettings =
                new FtpDownloadProviderSettings(configuration);

            FtpProviderRegistrationBuilder builder =
                new FtpProviderRegistrationBuilder(ftpDownloadProviderSettings);

            builderAction(builder);

            services.AddTransient<IFtpDownloadProviderSettings>(_ => ftpDownloadProviderSettings);
            services.AddTransient<IDownloadProvider, FtpDownloadProvider>();


            return services;
        }
    }
}
