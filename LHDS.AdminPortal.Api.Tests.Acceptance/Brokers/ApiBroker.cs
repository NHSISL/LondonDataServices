// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Net.Http;
using LHDS.Core.Brokers.Downloads;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Downloads;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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
        internal IDownloadService downloadService;

        public ApiBroker()
        {
            this.webApplicationFactory = new WebApplicationFactory<Startup>();
            this.auditService = (AuditService)webApplicationFactory.Services.GetService<IAuditService>();
            this.documentService = (DocumentService)webApplicationFactory.Services.GetService<IDocumentService>();
            this.downloadService = (DownloadService)webApplicationFactory.Services.GetService<IDownloadService>();
            this.httpClient = this.webApplicationFactory.CreateClient();
            this.apiFactoryClient = new RESTFulApiFactoryClient(this.httpClient);
        }
    }
}