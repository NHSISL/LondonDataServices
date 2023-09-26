// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Web.Tests.Acceptance.Brokers;
using Xunit;

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
            var response = await page.GotoAsync(broker.ApiBaseUrl);

            var actualTitle = await page.TitleAsync();

            // then
            actualTitle.Should().BeEquivalentTo(expectedTitle);
        }

        [Fact]
        public async Task VerifyHomePageTitle()
        {
            try
            {
                // given
                await using var context =
                    await broker.browser.NewContextAsync(new()
                    {
                        IgnoreHTTPSErrors = true
                    });

                var page = await context.NewPageAsync();
                var expectedTitle = "London Data Service Admin Portal";

                // when
                var response = await page.GotoAsync(broker.FrontendBaseUrl);
                //var response = await page.GotoAsync("https://localhost:44405/");

                var response1 = await page.WaitForResponseAsync("**/*");

                var actualTitle = await page.TitleAsync();

                // then
                actualTitle.Should().BeEquivalentTo(expectedTitle);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Fact]
        public async Task VerifyHomePageTitleTest()
        {
            var page = await broker.browser.NewPageAsync();

            await page.GotoAsync(
                broker.ApiBaseUrl,
                new Microsoft.Playwright.PageGotoOptions
                {
                    WaitUntil = Microsoft.Playwright.WaitUntilState.Load
                });

            var expectedTitle = "London Data Service Admin Portal";

            var actualTitle = await page.TitleAsync();

            // then
            actualTitle.Should().BeEquivalentTo(expectedTitle);
        }
    }
}
