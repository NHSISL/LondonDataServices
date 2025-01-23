// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Linq;
using System.Reflection;
using Attrify.InvisibleApi.Models;
using LHDS.Core.Providers.Downloads;
using LHDS.Core.Providers.Downloads.DiskDownloads;
using LHDS.Core.Providers.Downloads.FtpDownloads;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace LHDS.AdminPortal.Api.Tests.Acceptance
{
    public class AcceptanceTestApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            string dropfolder = "landing";
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                var descriptorsToRemove = services
                    .Where(descriptor =>
                        descriptor.ServiceType == typeof(IDownloadProvider)
                        && descriptor.ImplementationType == typeof(FtpDownloadProvider)).ToList();

                foreach (var descriptor in descriptorsToRemove)
                {
                    services.Remove(descriptor);
                }

                string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string defaultFolderPath = Path.Combine(assemblyPath, "temp", dropfolder);

                services.AddTransient<IDownloadProvider>(_ =>
                    new DiskDownloadProvider(new DiskDownloadProviderSettings
                    {
                        IncludeSubDirectories = true,
                        LocalRootFolder = defaultFolderPath
                    }));

                OverrideSecurityForTesting(services);
            });
        }

        private static void OverrideSecurityForTesting(IServiceCollection services)
        {
            var invisibleApiKeyDescriptor = services
                .FirstOrDefault(d => d.ServiceType == typeof(InvisibleApiKey));

            InvisibleApiKey invisibleApiKey = null;

            if (invisibleApiKeyDescriptor != null)
            {
                using (var serviceProvider = services.BuildServiceProvider())
                {
                    invisibleApiKey = serviceProvider.GetService<InvisibleApiKey>();
                }
            }

            var authenticationDescriptor = services
                .FirstOrDefault(d => d.ServiceType == typeof(IAuthenticationSchemeProvider));

            if (authenticationDescriptor != null)
            {
                services.Remove(authenticationDescriptor);
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "TestScheme";
                options.DefaultChallengeScheme = "TestScheme";
            })
            .AddScheme<CustomAuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options =>
            {
                options.InvisibleApiKey = invisibleApiKey;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("TestPolicy", policy => policy.RequireAssertion(_ => true));
            });
        }
    }
}