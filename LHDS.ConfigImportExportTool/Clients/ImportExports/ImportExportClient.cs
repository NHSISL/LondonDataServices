// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Brokers.CsvHelpers;
using LHDS.ConfigImportExportTool.Brokers.DateTimes;
using LHDS.ConfigImportExportTool.Brokers.Files;
using LHDS.ConfigImportExportTool.Brokers.Identifiers;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Brokers.Storages.Sql;
using LHDS.ConfigImportExportTool.Models.Clients.ImportExports.Exceptions;
using LHDS.ConfigImportExportTool.Models.Coordinations.ImportExports.Exceptions;
using LHDS.ConfigImportExportTool.Models.Foundations.Configurations.Retries;
using LHDS.ConfigImportExportTool.Services.Coordinations.ImportExports;
using LHDS.ConfigImportExportTool.Services.Foundations.CsvHelpers;
using LHDS.ConfigImportExportTool.Services.Foundations.DataSets;
using LHDS.ConfigImportExportTool.Services.Foundations.Files;
using LHDS.ConfigImportExportTool.Services.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Services.Orchestrations.ReadSchema;
using LHDS.ConfigImportExportTool.Services.Orchestrations.SchemaConfigs;
using LHDS.ConfigImportExportTool.Services.Processings.DataSets;
using LHDS.ConfigImportExportTool.Services.Processings.ObjectColumns;
using LHDS.ConfigImportExportTool.Services.Processings.SpecificationObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Clients.ImportExports
{
    public class ImportExportClient : IImportExportClient
    {
        private readonly IImportExportCoordinationService importExportCoordinationService;
        private readonly IConfiguration configuration;
        internal readonly IStorageBroker StorageBroker;

        public ImportExportClient()
        {
            string aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var args = Environment.GetCommandLineArgs();
            var environmentArg = args.FirstOrDefault(arg => arg.StartsWith("--environment="));

            var environmentName = !string.IsNullOrEmpty(aspNetCoreEnvironment)
                ? aspNetCoreEnvironment
                : !string.IsNullOrEmpty(environmentArg)
                    ? environmentArg
                    : "Development";

            var configurationBuilder = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables();

            this.configuration = configurationBuilder.Build();

            IHost host = RegisterServices(this.configuration);
            importExportCoordinationService = host.Services.GetRequiredService<IImportExportCoordinationService>();
            StorageBroker = host.Services.GetRequiredService<IStorageBroker>();
        }

        internal ImportExportClient(IImportExportCoordinationService importExportCoordinationService)
        {
            this.importExportCoordinationService = importExportCoordinationService;
        }

        private static IHost RegisterServices(IConfiguration configuration)
        {
            IHostBuilder builder = Host.CreateDefaultBuilder();

            builder.ConfigureServices(services =>
            {
                services.AddSingleton(configuration);
                services.AddTransient<ICsvHelperBroker, CsvHelperBroker>();
                services.AddTransient<IDateTimeBroker, DateTimeBroker>();
                services.AddTransient<IStorageBroker, StorageBroker>();
                services.AddTransient<IIdentifierBroker, IdentifierBroker>();
                services.AddTransient<ILoggingBroker, LoggingBroker>();
                services.AddTransient<IFileBroker, FileBroker>();
                services.AddTransient<ICsvHelperService, CsvHelperService>();
                services.AddTransient<IDataSetService, DataSetService>();
                services.AddTransient<IFileService, FileService>();

                int maxRetryAttempts =
                    configuration.GetSection("RetryConfig:MaxRetryAttempts").Get<int>();

                int pauseBetweenFailuresInSeconds =
                    configuration.GetSection("RetryConfig:PauseBetweenFailuresInSeconds").Get<int>();

                var retrySettings =
                    new RetryConfig(maxRetryAttempts, TimeSpan.FromSeconds(pauseBetweenFailuresInSeconds));

                services.AddSingleton<IRetryConfig>(retrySettings);
                services.AddTransient<IObjectColumnService, ObjectColumnService>();
                services.AddTransient<ISpecificationObjectService, SpecificationObjectService>();
                services.AddTransient<IDataSetProcessingService, DataSetProcessingService>();
                services.AddTransient<ISpecificationObjectProcessingService, SpecificationObjectProcessingService>();
                services.AddTransient<IObjectColumnProcessingService, ObjectColumnProcessingService>();
                services.AddTransient<IReadSchemaOrchestrationService, ReadSchemaOrchestrationService>();
                services.AddTransient<ISchemaConfigOrchestrationService, SchemaConfigOrchestrationService>();
                services.AddTransient<IImportExportCoordinationService, ImportExportCoordinationService>();
            });

            return builder.Build();
        }

        public async ValueTask Import(string dataSetName, string version, string filePath)
        {
            try
            {
                await importExportCoordinationService.Import(dataSetName, version, filePath);
            }
            catch (ImportExportCoordinationValidationException ImportExportCoordinationValidationException)
            {
                throw new ImportExportClientValidationException(
                    message: "Import export client validation error occurred, fix errors and try again.",
                    innerException: ImportExportCoordinationValidationException.InnerException as Xeption);
            }
            catch (ImportExportCoordinationDependencyValidationException
                ImportExportCoordinationDependencyValidationException)
            {
                throw new ImportExportClientValidationException(
                    message: "Import export client validation error occurred, fix errors and try again.",
                    innerException: ImportExportCoordinationDependencyValidationException.InnerException as Xeption);
            }
            catch (ImportExportCoordinationDependencyException
                ImportExportCoordinationDependencyException)
            {
                throw new ImportExportClientDependencyException(
                    message: "Import export client dependency error occurred, please contact support.",
                    innerException: ImportExportCoordinationDependencyException.InnerException as Xeption);
            }
            catch (ImportExportCoordinationServiceException
                ImportExportCoordinationServiceException)
            {
                throw new ImportExportClientServiceException(
                    message: "Import export client service error occurred, fix errors and try again.",
                    ImportExportCoordinationServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask Export(string dataSetName, string version, string filePath)
        {
            try
            {
                await this.importExportCoordinationService.Export(dataSetName, version, filePath);
            }
            catch (ImportExportCoordinationValidationException ImportExportCoordinationValidationException)
            {
                throw new ImportExportClientValidationException(
                    message: "Import export client validation error occurred, please contact support.",
                    innerException: ImportExportCoordinationValidationException.InnerException as Xeption);
            }
            catch (ImportExportCoordinationDependencyValidationException
                ImportExportCoordinationDependencyValidationException)
            {
                throw new ImportExportClientValidationException(
                    message: "Import export client validation error occurred, please contact support.",
                    innerException: ImportExportCoordinationDependencyValidationException.InnerException as Xeption);
            }
            catch (ImportExportCoordinationDependencyException
                ImportExportCoordinationDependencyException)
            {
                throw new ImportExportClientDependencyException(
                    message: "Import export client dependency error occurred, please contact support.",
                    innerException: ImportExportCoordinationDependencyException.InnerException as Xeption);
            }
            catch (ImportExportCoordinationServiceException
                ImportExportCoordinationServiceException)
            {
                throw new ImportExportClientServiceException(
                    message: "Import export client service error occurred, fix errors and try again.",
                    ImportExportCoordinationServiceException.InnerException as Xeption);
            }
        }
    }
}
