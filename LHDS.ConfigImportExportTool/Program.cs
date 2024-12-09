// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Clients.ImportExports;
using LHDS.ConfigImportExportTool.Models.Coordinations.ImportExports.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xeptions;

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
            try
            {
                await execution.Import(dataSetName, version, filePath);
                Console.WriteLine($"Import into config db is successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        else if (executionType == "export")
        {
            try
            {
                await execution.Export(dataSetName, version, filePath);
                Console.WriteLine($"Export from config db to {filePath} successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        else
        {
            Console.WriteLine("Please enter a correct execution type (import or export)");
            throw new System.NotImplementedException();
        }
    }

    private void ValidateImportFileArguments(
        string executionType,
        string dataSetName,
        string version,
        string filePath)
    {
        Validate<InvalidArgumentImportExportCoordinationException>(
            message: "Invalid import export coordination argument(s), please correct the errors and try again.",
            (Rule: IsInvalid(executionType), Parameter: nameof(executionType)),
            (Rule: IsInvalid(dataSetName), Parameter: nameof(dataSetName)),
            (Rule: IsInvalid(version), Parameter: nameof(version)),
            (Rule: IsInvalid(filePath), Parameter: nameof(filePath)));
    }

    private static dynamic IsInvalid(string? text) => new
    {
        Condition = String.IsNullOrWhiteSpace(text),
        Message = "Text is required"
    };

    private static void Validate<T>(string message, params (dynamic Rule, string Parameter)[] validations)
        where T : Xeption
    {
        var invalidDataException = (T?)Activator.CreateInstance(typeof(T), message);

        foreach ((dynamic rule, string parameter) in validations)
        {
            if (rule.Condition)
            {
                invalidDataException?.UpsertDataList(
                    key: parameter,
                    value: rule.Message);
            }
        }

        invalidDataException?.ThrowIfContainsErrors();
    }
}
