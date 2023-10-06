// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Playwright;
using RESTFulSense.Clients;
using Xunit;

namespace LHDS.AdminPortal.Web.Tests.Acceptance.Brokers
{
    public partial class WebServerBroker : IAsyncLifetime, IDisposable
    {
        private readonly IHost apiHost;
        private readonly IHost frontendHost;
        private IPlaywright playwright { get; set; }
        public IBrowser browser { get; private set; }
        private Process reactAppProcess;
        public string ApiBaseUrl { get; } = $"https://localhost:{GetRandomUnusedPort()}";
        public string FrontendBaseUrl { get; } = $"https://localhost:{GetRandomUnusedPort()}";
        public string FrontendProxyBaseUrl { get; } = $"https://localhost:44405/";
        public string ApiProxyBaseUrl { get; } = $"https://localhost:7052";

        private readonly HttpClient httpClient;
        private readonly IRESTFulApiFactoryClient apiFactoryClient;

        public WebServerBroker()
        {
            apiHost = Api.Program
                .CreateHostBuilder(new string[0])
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.CaptureStartupErrors(true);
                    webBuilder.UseUrls(ApiProxyBaseUrl);
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

            this.httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ApiProxyBaseUrl);
            this.apiFactoryClient = new RESTFulApiFactoryClient(this.httpClient);
        }

        public async Task InitializeAsync()
        {
            Task reactAppTask = StartReactAppAsync();
            // Sleep for a moment to ensure the React app is started
            Thread.Sleep(10000);

            playwright = await Playwright.CreateAsync();

            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 500
            });

            await Task.WhenAll(apiHost.StartAsync(), frontendHost.StartAsync());
            await reactAppTask;
        }

        public async Task StartReactAppAsync()
        {
            string solutionDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..");
            solutionDirectory = Path.GetFullPath(solutionDirectory);
            string clientAppPath = Path.Combine(solutionDirectory, "LHDS.AdminPortal.Web", "ClientApp");

            using (reactAppProcess = new Process())
            {
                reactAppProcess.StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/C start npm run start",
                    UseShellExecute = true,
                    CreateNoWindow = true,
                    WorkingDirectory = clientAppPath,
                };

                await Task.Run(() =>
                {
                    reactAppProcess.Start();
                    reactAppProcess.WaitForExit(); // Wait for the process to exit
                });
            }
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
