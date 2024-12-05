// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Xml.Serialization;
using LHDS.ConfigImportExportTool.Clients.ImportExports;
using Microsoft.Extensions.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {
        string aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        //var args = Environment.GetCommandLineArgs();
        var environmentArg = args.FirstOrDefault(arg => arg.StartsWith("--environment="));

        // Check if you have the arguments to support building the config
        // Otherwise fallback to the appsettings

        //var environmentName = !string.IsNullOrEmpty(aspNetCoreEnvironment)
        //    ? aspNetCoreEnvironment
        //    : !string.IsNullOrEmpty(environmentArg)
        //        ? environmentArg
        //        : "Development";

        //var configurationBuilder = new ConfigurationBuilder()
        //     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //     .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
        //     .AddEnvironmentVariables();

        //this.configuration = configurationBuilder.Build();

        //ImportExportConfig config = new ImportExportConfig { };

        //var client = new ImportExportClient(config);

        //if (config.Import == true)
        //{
        //    client.Import(dataSetName: config.DataSetName, version: config.Version, filePath: config.FilePath);
        //}
        //else
        //{
        //    client.Export(dataSetName: config.DataSetName, version: config.Version, filePath: config.FilePath);
        //}

    }
}