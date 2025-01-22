// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Net.Http;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Services.Foundations.Documents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RESTFulSense.Clients;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private readonly TestWebApplicationFactory<Program> acceptanceTestApplicationFactory;
        private readonly HttpClient httpClient;
        private readonly IRESTFulApiFactoryClient apiFactoryClient;
        internal readonly IDocumentService documentService;
        internal IConfiguration configuration;
        internal LandingConfiguration landingConfiguration;

        public ApiBroker()
        {
            this.acceptanceTestApplicationFactory = new TestWebApplicationFactory<Program>();
            this.httpClient = this.acceptanceTestApplicationFactory.CreateClient();
            this.apiFactoryClient = new RESTFulApiFactoryClient(this.httpClient);
            this.configuration = this.acceptanceTestApplicationFactory.Services.GetService<IConfiguration>();
            this.landingConfiguration = this.acceptanceTestApplicationFactory.Services.GetService<LandingConfiguration>();
            this.documentService = this.acceptanceTestApplicationFactory.Services.GetService<IDocumentService>();
        }
    }
}