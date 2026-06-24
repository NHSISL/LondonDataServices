// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
            GpgProviderRegistrationBuilder builder =
                new GpgProviderRegistrationBuilder();

            builderAction(builder);
            services.AddTransient<ICryptographyProvider, GpgCryptographyProvider>();

            return services;
        }
    }
}
