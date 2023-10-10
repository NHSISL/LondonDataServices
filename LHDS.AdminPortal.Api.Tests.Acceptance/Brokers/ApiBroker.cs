// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net.Http;
using LHDS.Core.Models.Orchestrations.Downloads;
using LHDS.Core.Providers.Cryptography;
using LHDS.Core.Providers.Cryptography.Gpg;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Downloads;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
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
        internal IIngestionTrackingAuditService ingestionTrackingAuditService;
        internal IDocumentService documentService;
        internal IIngestionTrackingService ingestionTrackingService;
        internal IDownloadService downloadService;
        internal ICryptographyProvider cryptographyProvider;
        internal IConfiguration configuration;
        internal LandingConfiguration landingConfiguration;

        public ApiBroker()
        {
            this.webApplicationFactory = new WebApplicationFactory<Startup>();

            this.ingestionTrackingAuditService = (IngestionTrackingAuditService)webApplicationFactory
                .Services.GetService<IIngestionTrackingAuditService>();

            this.documentService = (DocumentService)webApplicationFactory.Services.GetService<IDocumentService>();
            this.downloadService = (DownloadService)webApplicationFactory.Services.GetService<IDownloadService>();

            this.ingestionTrackingService =
                (IngestionTrackingService)webApplicationFactory.Services.GetService<IIngestionTrackingService>();

            this.httpClient = this.webApplicationFactory.CreateClient();
            this.apiFactoryClient = new RESTFulApiFactoryClient(this.httpClient);

            this.cryptographyProvider =
                (GpgCryptographyProvider)webApplicationFactory.Services.GetService<ICryptographyProvider>();

            this.configuration = this.webApplicationFactory.Services.GetService<IConfiguration>();
            this.landingConfiguration = this.webApplicationFactory.Services.GetService<LandingConfiguration>();
        }
    }
}