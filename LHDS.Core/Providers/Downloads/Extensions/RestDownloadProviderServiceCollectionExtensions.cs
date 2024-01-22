// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Providers.Downloads.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace LHDS.Core.Providers.Downloads.Extensions
{
    public static class RestDownloadProviderServiceCollectionExtensions
    {
        public static IServiceCollection UseRestDownloadProvider(
            this IServiceCollection services,
            Action<RestProviderRegistrationBuilder> builderAction)
        {
            RestProviderRegistrationBuilder builder = new();
            builderAction(builder);

            return services;
        }
    }
}
