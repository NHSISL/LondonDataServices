// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Clients.ImportExports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static void Main(string[] args)
    {
        string executionType = args[0];
        string dataSetName = args[1];
        string version = args[2];
        string filePath = args[3];
        ConfigImportExport(executionType, dataSetName, version, filePath);
    }

    private static IHost RegisterServices()
    {
        IHostBuilder builder = Host.CreateDefaultBuilder();

        builder.ConfigureServices(services =>
        {
            services.AddTransient<IImportExportClient, ImportExportClient>();
        });

        return builder.Build();
    }

    private static async void ConfigImportExport(string executionType, string dataSetName, string version, string filePath)
    {
        IHost host = RegisterServices();
        var executionTypes = host.Services.GetServices<IImportExportClient>();
        var execution = new ImportExportClient();

        if (executionType == "import")
        {
            await execution.Import(dataSetName, version, filePath);
        }
        else if (executionType == "export")
        {
            await execution.Export(dataSetName, version, filePath);
        }
        else
        {
            Console.WriteLine("Please enter a correct execution type (import or export)");
        }
    }
}