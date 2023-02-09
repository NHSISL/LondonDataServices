// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Providers.Cryptography.Gpg;
using LHDS.Core.Providers.Downloads.Builders;
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
                new GpgCryptographyProviderSettings(configuration);

            GpgProviderRegistrationBuilder builder =
                new GpgProviderRegistrationBuilder(gpgCryptographyProviderSettings);

            builderAction(builder);

            services.AddTransient<IGpgCryptographyProviderSettings>(_ => gpgCryptographyProviderSettings);
            services.AddTransient<ICryptographyProvider, GpgCryptographyProvider>();

            return services;
        }
    }
}
