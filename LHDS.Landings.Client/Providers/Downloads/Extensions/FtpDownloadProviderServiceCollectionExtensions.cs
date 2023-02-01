// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Landings.Client.Providers.Downloads.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace LHDS.Landings.Client.Providers.Downloads.Extensions
{
    public static class FtpDownloadProviderServiceCollectionExtensions
    {
        public static IServiceCollection UseFtpDownloadProvider(
            this IServiceCollection services,
            Action<FtpProviderRegistrationBuilder> builderAction)
        {
            FtpProviderRegistrationBuilder builder = new();
            builderAction(builder);

            return services;
        }
    }
}
