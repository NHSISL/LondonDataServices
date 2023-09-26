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
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
