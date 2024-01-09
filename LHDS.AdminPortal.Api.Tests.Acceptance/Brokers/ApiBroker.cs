// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net.Http;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Providers.Cryptography;
using LHDS.Core.Providers.Cryptography.Gpg;
using LHDS.Core.Services.Foundations.Downloads;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RESTFulSense.Clients;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private readonly WebApplicationFactory<Startup> webApplicationFactory;
        private readonly HttpClient httpClient;
        private readonly IRESTFulApiFactoryClient apiFactoryClient;
        internal IDownloadService downloadService;
        internal ICryptographyProvider cryptographyProvider;
        internal IConfiguration configuration;
        internal LandingConfiguration landingConfiguration;

        public ApiBroker()
        {
            this.webApplicationFactory = new WebApplicationFactory<Startup>();
            this.downloadService = (DownloadService)webApplicationFactory.Services.GetService<IDownloadService>();
            this.httpClient = this.webApplicationFactory.CreateClient();
            this.apiFactoryClient = new RESTFulApiFactoryClient(this.httpClient);

            this.cryptographyProvider =
                (GpgCryptographyProvider)webApplicationFactory.Services.GetService<ICryptographyProvider>();

            this.configuration = this.webApplicationFactory.Services.GetService<IConfiguration>();
            this.landingConfiguration = this.webApplicationFactory.Services.GetService<LandingConfiguration>();
        }
    }
}