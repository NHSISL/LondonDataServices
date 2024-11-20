// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Brokers.CsvHelpers;
using LHDS.ConfigImportExportTool.Brokers.DateTimes;
using LHDS.ConfigImportExportTool.Brokers.Files;
using LHDS.ConfigImportExportTool.Brokers.Identifiers;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Brokers.Storages.Sql;
using LHDS.ConfigImportExportTool.Models.Clients.Exceptions;
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
        internal readonly IStorageBroker storageBroker;

        public ImportExportClient()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            this.configuration = configurationBuilder.Build();
            IHost host = RegisterServices(this.configuration);
            importExportCoordinationService = host.Services.GetRequiredService<IImportExportCoordinationService>();
            storageBroker = host.Services.GetRequiredService<IStorageBroker>();
        }

        private static IHost RegisterServices(IConfiguration configuration)
        {
            IHostBuilder builder = Host.CreateDefaultBuilder();
            int maxRetryAttempts = configuration.GetSection("RetryConfig:MaxRetryAttempts").Get<int>();

            int pauseBetweenFailuresInSeconds = 
                configuration.GetSection("RetryConfig:PauseBetweenFailuresInSeconds").Get<int>();

            var retrySettings = new RetryConfig(maxRetryAttempts, TimeSpan.FromSeconds(pauseBetweenFailuresInSeconds));

            builder.ConfigureServices(configuration =>
            {
                configuration.AddTransient<ICsvHelperBroker, CsvHelperBroker>();
                configuration.AddTransient<IDateTimeBroker, DateTimeBroker>();
                configuration.AddTransient<IStorageBroker, StorageBroker>();
                configuration.AddTransient<IIdentifierBroker, IdentifierBroker>();
                configuration.AddTransient<ILoggingBroker, LoggingBroker>();
                configuration.AddTransient<IFileBroker, FileBroker>();
                configuration.AddTransient<ICsvHelperService, CsvHelperService>();
                configuration.AddTransient<IDataSetService, DataSetService>();
                configuration.AddTransient<IFileService, FileService>();
                configuration.AddSingleton<IRetryConfig>(retrySettings);
                configuration.AddTransient<IObjectColumnService, ObjectColumnService>();
                configuration.AddTransient<ISpecificationObjectService, SpecificationObjectService>();
                configuration.AddTransient<IDataSetProcessingService, DataSetProcessingService>();
                configuration.AddTransient<ISpecificationObjectProcessingService, SpecificationObjectProcessingService>();
                configuration.AddTransient<IObjectColumnProcessingService, ObjectColumnProcessingService>();
                configuration.AddTransient<IReadSchemaOrchestrationService, ReadSchemaOrchestrationService>();
                configuration.AddTransient<ISchemaConfigOrchestrationService, SchemaConfigOrchestrationService>();
                configuration.AddTransient<IImportExportCoordinationService, ImportExportCoordinationService>();
            });

            IHost host = builder.Build();

            return host;
        }

        public async ValueTask Import(string dataSetName, string version, string filePath)
        {
            try
            {
                await importExportCoordinationService.Import(dataSetName, version, filePath);
            }
            catch (ImportExportValidationCoordinationException ImportExportCoordinationValidationException)
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
    }
}
