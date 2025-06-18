// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using Attrify.Extensions;
using Attrify.InvisibleApi.Models;
using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Storage.Blobs;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Brokers.Telemetries;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Configurations;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Providers.Downloads;
using LHDS.Core.Providers.Downloads.Extensions;
using LHDS.Core.Providers.Downloads.MockDownloads;
using LHDS.Core.Services.Coordinations.Decryptions;
using LHDS.Core.Services.Foundations.Addresses;
using LHDS.Core.Services.Foundations.Cryptographies;
using LHDS.Core.Services.Foundations.DataSets;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Foundations.DataTypes;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.HealthChecks;
using LHDS.Core.Services.Foundations.HealthChecks.IngestionTracking;
using LHDS.Core.Services.Foundations.HealthChecks.ResolvedAddress;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.ObjectColumns;
using LHDS.Core.Services.Foundations.OptOuts;
using LHDS.Core.Services.Foundations.PdsAudits;
using LHDS.Core.Services.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Foundations.SecureDatas;
using LHDS.Core.Services.Foundations.SpecificationObjects;
using LHDS.Core.Services.Foundations.Suppliers;
using LHDS.Core.Services.Foundations.TerminologyArtifacts;
using LHDS.Core.Services.Foundations.TerminologyPolls;
using LHDS.Core.Services.Orchestrations.HealthChecks.IngestionTrackings;
using LHDS.Core.Services.Orchestrations.HealthChecks.ResolvedAddresses;
using LHDS.Core.Services.Processings.Addresses;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Downloads;
using LHDS.Core.Services.Processings.IngestionTrackingAudits;
using LHDS.Core.Services.Processings.IngestionTrackings;
using LHDS.Core.Services.Processings.OptOuts;
using LHDS.Core.Services.Processings.ResolvedAddresses;
using LHDS.Core.Services.Processings.SecureDatas;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;

namespace LHDS.AdminPortal.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .AddJsonFile("appsettings.json")
                .AddJsonFile(path: $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .AddJsonFile(path: "local.appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var invisibleApiKey = new InvisibleApiKey();
            ConfigureServices(builder, invisibleApiKey);
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var storageBroker = scope.ServiceProvider.GetRequiredService<StorageBroker>();
                storageBroker.Database.Migrate();
            }

            ConfigurePipeline(app, invisibleApiKey);
            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder, InvisibleApiKey invisibleApiKey)
        {
            builder.Services.AddRazorPages();
            builder.Services.AddLogging();
            builder.Services.AddHttpContextAccessor();

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowFrontendOrigin", policy =>
                {
                    policy.WithOrigins(builder.Configuration.GetSection("CorsOrigins").Get<List<string>>().ToArray());
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.WithExposedHeaders("Content-Disposition", "Content-Length", "Access-Control-Allow-Origin");
                });
            });

            JsonNamingPolicy jsonNamingPolicy = JsonNamingPolicy.CamelCase;
            builder.Services.AddODataQueryFilter();

            builder.Services.AddControllers()
               .AddOData(options =>
               {
                   options.AddRouteComponents("odata", GetEdmModel());
                   options.Select().Filter().Expand().OrderBy().Count().SetMaxTop(100);
               })
               .AddJsonOptions(options =>
               {
                   options.JsonSerializerOptions.PropertyNamingPolicy = jsonNamingPolicy;
                   options.JsonSerializerOptions.DictionaryKeyPolicy = jsonNamingPolicy;
                   options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                   options.JsonSerializerOptions.WriteIndented = true;
               });

            builder.Services.AddSingleton(new JsonSerializerOptions
            {
                PropertyNamingPolicy = jsonNamingPolicy,
                DictionaryKeyPolicy = jsonNamingPolicy,
            });

            builder.Services.AddSingleton(invisibleApiKey);
            builder.Services.AddDbContext<StorageBroker>();
            AddHealthApi(builder.Services, builder.Configuration);
            AddProviders(builder.Services, builder.Configuration);
            AddBrokers(builder.Services, builder.Configuration);
            AddFoundationServices(builder.Services, builder.Configuration);
            AddOrchestrationServices(builder.Services, builder.Configuration);
            AddProcessingServices(builder.Services, builder.Configuration);
            AddCoordinationServices(builder.Services, builder.Configuration);
            builder.Services.AddEmisLandingClient(builder.Configuration);
            builder.Services.AddDecryptionClient(builder.Configuration);
            builder.Services.UseFtpDownloadProvider(builder.Configuration, builder => builder.AddFtpDownloadProvider());

            builder.Services.AddSwaggerGen(options =>
            {
                var openApiInfo = new OpenApiInfo
                {
                    Title = "LHDS.AdminPortal.Api",
                    Version = "v1"
                };

                options.SwaggerDoc(
                name: "v1",
                    info: openApiInfo);
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

            builder.Services.AddApplicationInsightsTelemetry();
        }

        private static void ConfigurePipeline(WebApplication app, InvisibleApiKey invisibleApiKey)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(
                        url: "/swagger/v1/swagger.json",
                        name: "LHDS.AdminPortal.Api v1");
                });

                IdentityModelEventSource.ShowPII = true;
                app.UseODataRouteDebug();
            }

            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = HealthCheckResponseWriter.WriteResponse
            });

            app.UseHttpsRedirection();
            app.UseCors("AllowFrontendOrigin");
            app.UseRouting();
            app.UseAuthentication();
            app.UseForwardedHeaders();
            app.UseAuthorization();
            app.UseInvisibleApiMiddleware(invisibleApiKey);
            app.MapControllers().WithOpenApi();
        }

        private static void AddHealthApi(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton
                <IIngestionTrackingHealthItemService, IngestionTrackingDecryptionHealthCheckService>();

            services.AddSingleton
                <IIngestionTrackingHealthItemService, IngestionTrackingProcessingHealthCheckService>();

            services.AddSingleton
                <IIngestionTrackingHealthItemService, IngestionTrackingFailedToProcessHealthCheckService>();

            services.AddSingleton
                <IIngestionTrackingHealthItemService, IngestionTrackingFilesReceivedHealthCheckService>();

            services.AddSingleton
                <IIngestionTrackingHealthItemService, IngestionTrackingIncompleteBatchHealthCheckService>();

            services.AddSingleton
                <IResolvedAddressHealthItemService, ResolvedAddressProcessingHealthCheckService>();

            services.AddSingleton
                <IResolvedAddressHealthItemService, ResolvedAddressFailedToProcessHealthCheckService>();

            services.AddSingleton
                <IResolvedAddressHealthItemService, ResolvedAddressFailedToExportHealthCheckService>();

            services.AddHealthChecks()
                .AddCheck<IngestionTrackingHealthCheckOrchestrationService>("ingestionTrackingHealthChecks");

            services.AddHealthChecks()
                .AddCheck<ResolvedAddressHealthCheckOrchestrationService>("resolvedAddressHealthChecks");

            services.AddSingleton<IHealthCheckPublisher, HealthCheckPublisherCoordinationService>();

            int startupDelaySeconds = configuration.GetValue<int>(
                "HealthChecks:StartupDelaySeconds", 10);

            int publishIntervalSeconds = configuration.GetValue<int>(
                "HealthChecks:PublishIntervalSeconds", 60);

            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(startupDelaySeconds);
                options.Period = TimeSpan.FromSeconds(publishIntervalSeconds);
            });
        }

        private static void AddProviders(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDownloadAbstractionProvider, DownloadAbstractionProvider>();
            services.AddTransient<IDownloadProvider, MockDownloadProvider>();
        }

        private static void AddBrokers(IServiceCollection services, IConfiguration configuration)
        {
            ValidateAppInsightsCinfiguration(configuration);
            services.AddSingleton<IConfiguration>(_ => configuration);
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IIdentifierBroker, IdentifierBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddSingleton<IStorageBroker, StorageBroker>();
            services.AddTransient<IBlobStorageBroker, BlobStorageBroker>();
            services.AddTransient<IHashBroker, HashBroker>();
            services.AddTransient<IAzureBlobClient, AzureBlobClient>();
            services.AddTransient<ISecurityBroker, SecurityBroker>();
            services.AddTransient<ITelemetryBroker, TelemetryBroker>();
        }

        private static void AddFoundationServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICryptographyService, CryptographyService>();
            services.AddTransient<IIngestionTrackingService, IngestionTrackingService>();
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddTransient<IIngestionTrackingAuditService, IngestionTrackingAuditService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IOptOutService, OptOutService>();
            services.AddTransient<IPdsAuditService, PdsAuditService>();
            services.AddTransient<IDataSetService, DataSetService>();
            services.AddTransient<ISpecificationObjectService, SpecificationObjectService>();
            services.AddTransient<IDataSetSpecificationService, DataSetSpecificationService>();
            services.AddTransient<IDataTypeService, DataTypeService>();
            services.AddTransient<IObjectColumnService, ObjectColumnService>();
            services.AddTransient<IDataSetService, DataSetService>();
            services.AddTransient<ITerminologyArtifactService, TerminologyArtifactService>();
            services.AddTransient<ITerminologyPollService, TerminologyPollService>();
            services.AddTransient<ISecureDataService, SecureDataService>();
            services.AddTransient<IAddressService, AddressService>();
            services.AddTransient<IResolvedAddressService, ResolvedAddressService>();

            var blobStorageSettings = configuration.GetSection("blobStorage")
                .Get<BlobStorageSettings>();

            ValidateBlobStorageSettings(blobStorageSettings);
            ValidateBlobContainers(blobStorageSettings.BlobContainers);
            services.AddSingleton<BlobContainers>(blobStorageSettings.BlobContainers);

            var blobServiceClientOptions = new BlobClientOptions()
            {
                Transport = new HttpClientTransport(new HttpClient { Timeout = new TimeSpan(1, 0, 0) }),
                Retry = { NetworkTimeout = new TimeSpan(1, 0, 0) },
                EnableTenantDiscovery = true
            };

            services.AddSingleton(
                new BlobServiceClient(
                    serviceUri: new Uri(blobStorageSettings.AzureBlobServiceUri),
                    credential: new DefaultAzureCredential(
                        new DefaultAzureCredentialOptions
                        {
                            VisualStudioTenantId = blobStorageSettings.AzureTenantId,
                        }),
                    options: blobServiceClientOptions));
        }

        private static void ValidateBlobStorageSettings(BlobStorageSettings blobStorageSettings)
        {
            Validate(
                (Rule: IsInvalid(blobStorageSettings.AzureBlobServiceUri),
                    Parameter: "blobStorage__azureBlobServiceUri"),

                (Rule: IsInvalid(blobStorageSettings.AzureTenantId),
                    Parameter: "blobStorage__azureTenantId"));
        }

        private static void ValidateAppInsightsCinfiguration(IConfiguration configuration)
        {
            string connectionString = configuration["ApplicationInsights:ConnectionString"] ?? string.Empty;
            Validate((Rule: IsInvalid(connectionString), Parameter: "applicationInsights__connectionString"));
        }

        private static void ValidateBlobContainers(BlobContainers blobContainers)
        {
            Validate(
                (Rule: IsInvalid(blobContainers.EmisLanding),
                    Parameter: "blobContainers__emisLanding"),

                (Rule: IsInvalid(blobContainers.Versioner),
                    Parameter: "blobContainers__versioner"),

                (Rule: IsInvalid(blobContainers.OptOut),
                    Parameter: "blobContainers__optOut"),

                (Rule: IsInvalid(blobContainers.Pds),
                    Parameter: "blobContainers__pds"));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Configuration value does not exist"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidConfigurationException = new InvalidConfigurationException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidConfigurationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidConfigurationException.ThrowIfContainsErrors();
        }

        private static void AddOrchestrationServices(IServiceCollection services, IConfiguration configuration)
        {
        }

        private static void AddProcessingServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IOptOutProcessingService, OptOutProcessingService>();
            services.AddTransient<IDataSetSpecificationProcessingService, DataSetSpecificationProcessingService>();
            services.AddTransient<IDocumentProcessingService, DocumentProcessingService>();
            services.AddTransient<IDownloadProcessingService, DownloadProcessingService>();
            services.AddTransient<IIngestionTrackingProcessingService, IngestionTrackingProcessingService>();
            services.AddTransient<IIngestionTrackingAuditProcessingService, IngestionTrackingAuditProcessingService>();
            services.AddTransient<ISecureDataProcessingService, SecureDataProcessingService>();
            services.AddTransient<IAddressProcessingService, AddressProcessingService>();
            services.AddTransient<IResolvedAddressProcessingService, ResolvedAddressProcessingService>();
        }

        private static void AddCoordinationServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDecryptionCoordinationService, DecryptionCoordinationService>();
        }

        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder =
               new ODataConventionModelBuilder();

            builder.EntitySet<Address>("Addresses");
            builder.EntitySet<IngestionTrackingAudit>("IngestionTrackingAudits");
            builder.EntitySet<DataSet>("DataSets");
            builder.EntitySet<DataSetSpecification>("DataSetSpecifications");
            builder.EntitySet<SpecificationObject>("SpecificationObjects");
            builder.EntitySet<DataType>("DataTypes");
            builder.EntitySet<IngestionTracking>("IngestionTrackings");
            builder.EntitySet<ObjectColumn>("ObjectColumns");
            builder.EntitySet<OptOut>("OptOuts");
            builder.EntitySet<PdsAudit>("PdsAudits");
            builder.EntitySet<Supplier>("Suppliers");
            builder.EntitySet<ResolvedAddress>("ResolvedAddresses");
            builder.EntitySet<TerminologyArtifact>("TerminologyArtifacts");
            builder.EntitySet<SubscriberCredential>("SubscriberCredentials");
            builder.EntitySet<SubscriberAgreement>("SubscriberAgreements");
            builder.EnableLowerCamelCase();

            return builder.GetEdmModel();
        }
    }
}
