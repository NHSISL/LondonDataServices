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
            // https://github.com/isaacrlevin/.NET-Conf-Demos/blob/56b6519d50124f4f3fe2e24f0fdf8231e292838b/Tests/WebServerTests.cs
            // given
            await using var context = await broker.browser.NewContextAsync(new() { IgnoreHTTPSErrors = true });
            var page = await context.NewPageAsync();
            var expectedTitle = "London Data Service Admin Portal";

            //// when
            var response = await page.GotoAsync(broker.FrontendBaseUrl);
            var actualTitle = await page.TitleAsync();

            // then
            actualTitle.Should().BeEquivalentTo(expectedTitle);
        }
    }
}
