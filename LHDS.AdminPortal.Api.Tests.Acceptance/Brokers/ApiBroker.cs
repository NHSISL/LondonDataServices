// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Net.Http;
using Attrify.InvisibleApi.Models;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Services.Foundations.Documents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RESTFulSense.Clients;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private readonly AcceptanceTestApplicationFactory<Startup> acceptanceTestApplicationFactory;
        private readonly HttpClient httpClient;
        private readonly IRESTFulApiFactoryClient apiFactoryClient;
        internal readonly IDocumentService documentService;
        internal IConfiguration configuration;
        internal LandingConfiguration landingConfiguration;
        internal readonly InvisibleApiKey invisibleApiKey;

        public ApiBroker()
        {
            this.acceptanceTestApplicationFactory = new AcceptanceTestApplicationFactory<Startup>();
            this.invisibleApiKey = this.acceptanceTestApplicationFactory.Services.GetService<InvisibleApiKey>();
            this.httpClient = this.acceptanceTestApplicationFactory.CreateClient();
            this.httpClient.DefaultRequestHeaders.Add(this.invisibleApiKey.Key, this.invisibleApiKey.Value);
            this.apiFactoryClient = new RESTFulApiFactoryClient(this.httpClient);
            this.configuration = this.acceptanceTestApplicationFactory.Services.GetService<IConfiguration>();
            this.landingConfiguration = this.acceptanceTestApplicationFactory.Services.GetService<LandingConfiguration>();
            this.documentService = this.acceptanceTestApplicationFactory.Services.GetService<IDocumentService>();
        }
    }
}