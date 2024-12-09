// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Runtime.CompilerServices;
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

        if (executionType == "Import")
        {
            await executionTypes
        }
        else { }

        var calculator = new Conf(calcOperations.ToArray());
        Console.WriteLine($"{operation}");
        Console.WriteLine(calculator.Calculate(operation, values));
        Console.ReadLine();
    }
}