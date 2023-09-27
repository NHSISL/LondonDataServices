// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Web.Tests.Acceptance.Brokers;
using Xunit;
using static LHDS.AdminPortal.Web.Tests.Acceptance.Pages.CommonFunctions;

namespace LHDS.AdminPortal.Web.Tests.Acceptance.Pages.Home
{
    public class HomeTests : IClassFixture<WebServerBroker>
    {
        private readonly WebServerBroker broker;

        public HomeTests(WebServerBroker broker)
        {
            this.broker = broker;
        }

        [Fact]
        public async Task VerifyApiPageTitle()
        {
            // given
            await using var context =
                await broker.browser.NewContextAsync(new()
                {
                    IgnoreHTTPSErrors = true
                });

            var page = await context.NewPageAsync();
            var expectedTitle = "LDS Management API";

            //// when
            var response = await page.GotoAsync(broker.ApiProxyBaseUrl);

            var actualTitle = await page.TitleAsync();

            // then
            actualTitle.Should().BeEquivalentTo(expectedTitle);
        }

        [Fact]
        public async Task VerifyHomePageTitle()
        {
            // given
            await using var context =
                await broker.browser.NewContextAsync(new()
                {
                    IgnoreHTTPSErrors = true
                });

            var page = await context.NewPageAsync();
            var expectedTitle = "London Data Service Admin Portal";

            var response = await page.GotoAsync(broker.FrontendProxyBaseUrl,
            new Microsoft.Playwright.PageGotoOptions
            {
                Timeout = 0,
                WaitUntil = Microsoft.Playwright.WaitUntilState.DOMContentLoaded
            });

            var actualTitle = await page.TitleAsync();

            // then
            actualTitle.Should().BeEquivalentTo(expectedTitle);
        }

        [Fact]
        public async Task LoginAndCheckForPageHeader()
        {
            // given
            await using var context =
                await broker.browser.NewContextAsync(new() { IgnoreHTTPSErrors = true });

            //When
            var apiPage = await context.NewPageAsync();
            var frontendPage = await context.NewPageAsync();

            await Task.WhenAll(
                apiPage.GotoAsync(broker.ApiProxyBaseUrl),
                frontendPage.GotoAsync(broker.FrontendProxyBaseUrl)
                );

            var loginHandler = new Login(frontendPage);


            await loginHandler.PerformLogin(
                "david.hayes17@nelcsu.nhs.uk",
                "20q^0K5vl#jjLEsP"
            );

            var expectedElement = await frontendPage.IsVisibleAsync("text='London Health Data Services'");

            //await frontendPage.PauseAsync();

            // then
            expectedElement.Should().BeTrue();
        }
    }
}
