// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Security.Cryptography;
using LHDS.ConfigImportExportTool.Clients.ImportExports;
using LHDS.ConfigImportExportTool.Models.Coordinations.ImportExports.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xeptions;

internal class Program
{

    private static async Task Main(string[] args)
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadLine();
        string executionType = string.Empty;
        string dataSetName = string.Empty;
        string version = string.Empty;
        string filePath = string.Empty;

        try
        {
            var client = new ImportExportClient();
            //Try to gracefully assign if args exist
            executionType = args[0];
            dataSetName = args[1];
            version = args[2];
            filePath = args[3];

            if (args.Length == 4)
            {
                if (executionType.ToLower() != "import" && executionType.ToLower() != "export")
                {
                    Console.WriteLine("Please enter a correct execution type (import or export)");
                    return;
                }

                switch (executionType.ToLower())
                {
                    case "import":
                        await client.Import(dataSetName, version, filePath);
                        Console.WriteLine("Import into config DB is successful.");
                        break;
                    case "export":
                        await client.Export(dataSetName, version, filePath);
                        Console.WriteLine($"Export from config DB to {filePath} is successful.");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                //TODO: Add Windows Form
                //var form = new frmImportExport(client,  executionType, dataSetName, version, filePath);
                //form.Show();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during {executionType} operation: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}
