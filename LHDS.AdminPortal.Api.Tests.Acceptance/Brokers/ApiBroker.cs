// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Configuration;
using System.Net.Http;
using LHDS.Core.Services.Foundations.Audits;
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
        internal IConfiguration configuration;

        public ApiBroker()
        {
            this.webApplicationFactory = new WebApplicationFactory<Startup>();
            this.auditService = (AuditService)webApplicationFactory.Services.GetService<IAuditService>();
            this.httpClient = this.webApplicationFactory.CreateClient();
            this.apiFactoryClient = new RESTFulApiFactoryClient(this.httpClient);
            this.configuration = this.webApplicationFactory.Services.GetService<IConfiguration>();
        }
    }
}