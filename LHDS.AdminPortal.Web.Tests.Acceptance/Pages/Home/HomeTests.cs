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
        public async Task VerifyHomePageTitle()
        {
            // using var playwright = await Playwright.CreateAsync();
            //await using var browser = await playwright.Chromium.LaunchAsync();
            // var context = await browser.NewContextAsync();
            // var page = await context.NewPageAsync();
            //await page.GotoAsync("https://playwright.dev/dotnet");

            // given
            await using var context = await broker.browser.NewContextAsync(new() { IgnoreHTTPSErrors = true });
            var page = await context.NewPageAsync();
            var expectedTitle = "London Data Service Admin Portal";

            //// when
            await page.GotoAsync(broker.FrontendBaseUrl);

            var actualTitle = await page.TitleAsync();

            // then
            actualTitle.Should().BeEquivalentTo(expectedTitle);
        }
    }
}
