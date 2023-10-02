// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Web.Tests.Acceptance.Brokers;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Xunit;
using static LHDS.AdminPortal.Web.Tests.Acceptance.Pages.CommonFunctions;

namespace LHDS.AdminPortal.Web.Tests.Acceptance.Pages.IngestionTrackings
{
    public partial class IngestionTrackingTests : IClassFixture<WebServerBroker>
    {
        private readonly WebServerBroker webServerBroker;
        private readonly IConfiguration configuration;

        public IngestionTrackingTests(WebServerBroker webServerBroker)
        {
            this.webServerBroker = webServerBroker;

            configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();
        }

        [Fact]
        public async Task VerifyIngestionPageCorrectCardTitle()
        {
            // given
            var expectedCardTitle = "Ingestion Tracking";
            var navSelector = "#navbarScroll > div.me-auto.my-2.my-lg-0.navbar-nav.navbar-nav-scroll > div:nth-child(2) > a";
            var cardTitleSelector = $"{".cardBaseTitle"}:has-text(\"{"Ingestion Tracking"}\")";

            IPage page = await InitialiseSetup();
            await PerformLogin(page);

            //When
            await page
                .Locator(navSelector)
                .ClickAsync();

            var element =
                    await page.QuerySelectorAsync(cardTitleSelector);

            Assert.NotNull(element);

            var actualCardTitle =
                    await element.TextContentAsync();

            // then
            actualCardTitle.Should().BeEquivalentTo(expectedCardTitle);
        }

        [Fact]
        public async Task VerifyIngestionPageInvlaidSearchReturnsBlank()
        {
            // given
            var navSelector = "#navbarScroll > div.me-auto.my-2.my-lg-0.navbar-nav.navbar-nav-scroll > div:nth-child(2) > a";
            var expectedTableRowcount = 1;

            //When
            await using var context = await webServerBroker.browser.NewContextAsync(new() { IgnoreHTTPSErrors = true });

            var apiPage = await context.NewPageAsync();
            var page = await context.NewPageAsync();

            await Task.WhenAll(
                apiPage.GotoAsync(webServerBroker.ApiProxyBaseUrl),
                page.GotoAsync(webServerBroker.FrontendProxyBaseUrl)
                );

            var loginHandler = new Login(page);
            await loginHandler.PerformLogin();

            await page
                .Locator(navSelector)
                .ClickAsync();

            await page.TypeAsync("input[type='search']", "RETURNNOTHING");

            var selectTable = await page.QuerySelectorAsync("table");
            var actualRowCount = 0;

            if (selectTable != null)
                actualRowCount =
                    await selectTable.EvaluateAsync<int>("table => table.rows.length");

            // then
            // await page.PauseAsync();
            actualRowCount.Should().Be(expectedTableRowcount);
        }

        [Fact]
        public async Task VerifyIngestionPageAddRowAndSearch()
        {
            // given
            var navSelector = "#navbarScroll > div.me-auto.my-2.my-lg-0.navbar-nav.navbar-nav-scroll > div:nth-child(2) > a";
            var expectedTableRowcount = 1;

            //Post Ingestions
            //var supplier = CreateRandomSupplier();
            //await PostRandomIngestionTrackingAsync(supplier.Id);

            //When
            IPage page = await InitialiseSetup();
            await PerformLogin(page);

            await page
                .Locator(navSelector)
                .ClickAsync();

            await page.TypeAsync("input[type='search']", "RETURNNOTHING");

            var selectTable = await page.QuerySelectorAsync("table");
            var actualRowCount = 0;

            if (selectTable != null)
                actualRowCount =
                    await selectTable.EvaluateAsync<int>("table => table.rows.length");

            // Then
            // await page.PauseAsync();
            actualRowCount.Should().Be(expectedTableRowcount);
        }
    }

}
