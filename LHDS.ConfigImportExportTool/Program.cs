// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Clients.ImportExports;

internal class Program
{

    private static async Task Main(string[] args)
    {
        string executionType = string.Empty;
        string dataSetName = string.Empty;
        string version = string.Empty;
        string filePath = string.Empty;

        try
        {
            var client = new ImportExportClient();
            executionType = args.Length > 0 ? args[0] : string.Empty;
            dataSetName = args.Length > 1 ? args[1] : string.Empty;
            version = args.Length > 2 ? args[2] : string.Empty;
            filePath = args.Length > 3 ? args[3] : string.Empty;

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
            Console.WriteLine($"An error occurred during '{executionType}' operation: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}
