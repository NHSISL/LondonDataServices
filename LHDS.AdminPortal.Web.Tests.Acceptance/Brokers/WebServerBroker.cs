// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Playwright;
using Xunit;

namespace LHDS.AdminPortal.Web.Tests.Acceptance.Brokers
{
    public class WebServerBroker : IAsyncLifetime, IDisposable
    {
        private readonly IHost apiHost;
        private readonly IHost frontendHost;
        private IPlaywright playwright { get; set; }
        public IBrowser browser { get; private set; }
        public string ApiBaseUrl { get; } = $"https://localhost:{GetRandomUnusedPort()}";
        public string FrontendBaseUrl { get; } = $"https://localhost:{GetRandomUnusedPort()}";
        public string FrontendProxyBaseUrl { get; } = $"https://localhost:44405/";


        public WebServerBroker()
        {
            apiHost = Api.Program
                .CreateHostBuilder(new string[0])
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.CaptureStartupErrors(true);
                    webBuilder.UseUrls(ApiBaseUrl);
                })
                .Build();

            frontendHost = Web.Program
                .CreateHostBuilder(new string[0])
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.CaptureStartupErrors(true);
                    webBuilder.UseUrls(FrontendBaseUrl);
                })
                .Build();
        }

        public async Task InitializeAsync()
        {
            playwright = await Playwright.CreateAsync();
            browser = await playwright.Chromium.LaunchAsync(
                new()
                {
                    Headless = false,
                    SlowMo = 500
                });

            await Task.WhenAll(apiHost.StartAsync(), frontendHost.StartAsync());
        }

        public async Task DisposeAsync()
        {
            await Task.WhenAll(apiHost.StopAsync(), frontendHost.StopAsync());
            apiHost?.Dispose();
            frontendHost?.Dispose();
            playwright?.Dispose();
        }

        public void Dispose()
        {
            apiHost?.Dispose();
            frontendHost?.Dispose();
            playwright?.Dispose();
        }

        private static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Any, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}
