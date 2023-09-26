// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Configuration;
using System.Net.Http;
using LHDS.Core.Providers.Cryptography;
using LHDS.Core.Providers.Cryptography.Gpg;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Downloads;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.Documents;
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
        internal IAuditService auditService;
        internal IDocumentService documentService;
        internal IIngestionTrackingService ingestionTrackingService;
        internal IDownloadService downloadService;
        internal ICryptographyProvider cryptographyProvider;
        internal IConfiguration configuration;

        public ApiBroker()
        {
            this.webApplicationFactory = new WebApplicationFactory<Startup>();
            this.auditService = (AuditService)webApplicationFactory.Services.GetService<IAuditService>();
            this.documentService = (DocumentService)webApplicationFactory.Services.GetService<IDocumentService>();
            this.downloadService = (DownloadService)webApplicationFactory.Services.GetService<IDownloadService>();

            this.ingestionTrackingService =
                (IngestionTrackingService)webApplicationFactory.Services.GetService<IIngestionTrackingService>();

            this.httpClient = this.webApplicationFactory.CreateClient();
            this.apiFactoryClient = new RESTFulApiFactoryClient(this.httpClient);

            this.cryptographyProvider =
                (GpgCryptographyProvider)webApplicationFactory.Services.GetService<ICryptographyProvider>();

            this.configuration = this.webApplicationFactory.Services.GetService<IConfiguration>();
        }
    }
}