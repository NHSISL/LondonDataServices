using System.IO;
using System.Reflection;
using LHDS.Core.Providers.Downloads;
using LHDS.Core.Providers.Downloads.DiskDownloads;
using LHDS.Core.Providers.Downloads.FtpDownloads;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

public class AcceptanceTestApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        string dropfolder = "landing";
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            services.Remove(new ServiceDescriptor(typeof(IDownloadProvider), typeof(FtpDownloadProvider)));

            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string defaultFolderPath = Path.Combine(assemblyPath, "temp", dropfolder);

            services.AddTransient<IDownloadProvider>(_ =>
                new DiskDownloadProvider(new DiskDownloadProviderSettings
                {
                    IncludeSubDirectories = true,
                    LocalRootFolder = defaultFolderPath
                }));
        });
    }
}