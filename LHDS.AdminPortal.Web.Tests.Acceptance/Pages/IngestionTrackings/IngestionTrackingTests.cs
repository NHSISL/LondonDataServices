// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Web.Tests.Acceptance.Brokers;
using Microsoft.Extensions.Configuration;
using Xunit;
using static LHDS.AdminPortal.Web.Tests.Acceptance.Pages.CommonFunctions;

namespace LHDS.AdminPortal.Web.Tests.Acceptance.Pages.IngestionTrackings
{
    public class IngestionTrackingTests : IClassFixture<WebServerBroker>
    {
        private readonly WebServerBroker broker;
        private readonly IConfiguration configuration;

        public IngestionTrackingTests(WebServerBroker broker)
        {
            this.broker = broker;

            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        [Fact]
        public async Task VerifyIngestionPageCardTitle()
        {
            // given
            var expectedCardTitle = "Ingestion Tracking";

            //When
            await using var context = await broker.browser.NewContextAsync(new() { IgnoreHTTPSErrors = true });

            var apiPage = await context.NewPageAsync();
            var page = await context.NewPageAsync();

            await Task.WhenAll(
                apiPage.GotoAsync(broker.ApiProxyBaseUrl),
                page.GotoAsync(broker.FrontendProxyBaseUrl)
                );

            var loginHandler = new Login(page);

            await loginHandler.PerformLogin(
                configuration["Login:Email"],
                configuration["Login:Password"]
                );

            await page
                .Locator("#navbarScroll > div.me-auto.my-2.my-lg-0.navbar-nav.navbar-nav-scroll > div:nth-child(2) > a")
                .ClickAsync();

            const string className = ".cardBaseTitle";
            const string searchText = "Ingestion Tracking";

            var selector = $"{className}:has-text(\"{searchText}\")";
            var element = await page.QuerySelectorAsync(selector);

            Assert.NotNull(element);

            var actualCardTitle = await element.TextContentAsync();

            // then
            actualCardTitle.Should().BeEquivalentTo(expectedCardTitle);
        }
    }
}
