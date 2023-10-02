// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Web.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Web.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Web.Tests.Acceptance.Models.Suppliers;
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
            var navSelector =
                "#navbarScroll > div.me-auto.my-2.my-lg-0.navbar-nav.navbar-nav-scroll > div:nth-child(2) > a";

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
            var navSelector =
                "#navbarScroll > div.me-auto.my-2.my-lg-0.navbar-nav.navbar-nav-scroll > div:nth-child(2) > a";

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
            actualRowCount.Should().Be(expectedTableRowcount);
        }

        [Fact]
        public async Task VerifyIngestionPageAddRowAndSearch()
        {
            // given
            var navSelector =
                "#navbarScroll > div.me-auto.my-2.my-lg-0.navbar-nav.navbar-nav-scroll > div:nth-child(2) > a";

            var expectedTableRowcount = 2;

            IPage page = await InitialiseSetup();
            await PerformLogin(page);

            Supplier randomSupplier = await PostRandomSupplierAsync();

            ValueTask<IngestionTracking> randomIngestionTrackingTask = PostRandomIngestionTrackingAsync(randomSupplier.Id);
            IngestionTracking randomIngestionTracking = await randomIngestionTrackingTask;
            string fileName = randomIngestionTracking.EncryptedFileName;

            //When
            await page
                .Locator(navSelector)
                .ClickAsync();

            await page.TypeAsync("input[type='search']", fileName);
            await page.PauseAsync();

            var actualRowCount = await page.EvalOnSelectorAsync<int>("table tbody", "el => el.rows.length");

            //Then
            actualRowCount.Should().Be(expectedTableRowcount);

            // Cleanup
            await this.webServerBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.Id);
            await this.webServerBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact]
        public async Task VerifyIngestionPageAddRowAndCheckDetailslayoutComponentCount()
        {
            // given
            var navSelector =
                "#navbarScroll > div.me-auto.my-2.my-lg-0.navbar-nav.navbar-nav-scroll > div:nth-child(2) > a";

            var detailsBtn
                = "#maincontent > div > div > div > div.undefined.cardBaseContent > table > tbody > tr:nth-child(1) > td:nth-child(5) > a > button";

            var expectedRowCount = 12;
            var expectedButtonCount = 3;

            IPage page = await InitialiseSetup();
            await PerformLogin(page);

            Supplier randomSupplier = await PostRandomSupplierAsync();

            ValueTask<IngestionTracking> randomIngestionTrackingTask = PostRandomIngestionTrackingAsync(randomSupplier.Id);
            IngestionTracking randomIngestionTracking = await randomIngestionTrackingTask;
            string fileName = randomIngestionTracking.EncryptedFileName;

            //When
            await page
                .Locator(navSelector)
                .ClickAsync();

            await page.TypeAsync("input[type='search']", fileName);
            await page.Locator(detailsBtn).ClickAsync();

            var actualRowCount = await page.EvalOnSelectorAllAsync<int>("div.summaryRow", "els => els.length");

            var actualButtonCount = await page.EvalOnSelectorAsync<int>(
                ".cardBaseContent",
                "parentDiv => parentDiv.querySelectorAll('button').length"
            );

            await page.PauseAsync();

            //Then
            actualRowCount.Should().Be(expectedRowCount);
            actualButtonCount.Should().Be(expectedButtonCount);

            // Cleanup
            await this.webServerBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.Id);
            await this.webServerBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }
    }

}
