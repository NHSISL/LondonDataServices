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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;


namespace LHDS.AdminPortal.Configurations.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var invisibleApiKey = new InvisibleApiKey();
            ConfigureServices(builder, builder.Configuration, invisibleApiKey);
            var app = builder.Build();
            ConfigurePipeline(app, invisibleApiKey);
            app.Run();
        }

        public static void ConfigureServices(
           WebApplicationBuilder builder,
           IConfiguration configuration,
           InvisibleApiKey invisibleApiKey)
        {
            // Load settings from launchSettings.json (for testing)
            var projectDir = Directory.GetCurrentDirectory();
            var launchSettingsPath = Path.Combine(projectDir, "Properties", "launchSettings.json");

            if (File.Exists(launchSettingsPath))
            {
                builder.Configuration.AddJsonFile(launchSettingsPath);
            }

            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            // Add services to the container.
            var azureAdOptions = builder.Configuration.GetSection("AzureAd");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(azureAdOptions);


            var instance = builder.Configuration["AzureAd:Instance"];
            var tenantId = builder.Configuration["AzureAd:TenantId"];
            var scopes = builder.Configuration["AzureAd:Scopes"];

            if (string.IsNullOrEmpty(instance) || string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(scopes))
            {
                throw new InvalidOperationException("AzureAd configuration is incomplete. Please check appsettings.json.");
            }

            builder.Services.AddSwaggerGen(configuration =>
            {
                configuration.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{instance}{tenantId}/oauth2/v2.0/authorize"),
                            TokenUrl = new Uri($"{instance}{tenantId}/oauth2/v2.0/token"),
                            Scopes = scopes.Split(' ').ToDictionary(scope => scope, scope => "Access API as user")
                        }
                    }
                });

                configuration.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            }
                        },
                        scopes.Split(' ')
                    }
                });
            });

            builder.Services.AddSingleton(invisibleApiKey);
            builder.Services.AddAuthorization();
            builder.Services.AddHttpContextAccessor();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();

            AddProviders(builder.Services, builder.Configuration);
            AddBrokers(builder.Services, builder.Configuration);
            AddFoundationServices(builder.Services, builder.Configuration);
            AddOrchestrationServices(builder.Services, builder.Configuration);
            AddProcessingServices(builder.Services, builder.Configuration);
            AddCoordinationServices(builder.Services, builder.Configuration);
           
            builder.Services.AddEmisLandingClient(builder.Configuration);
            builder.Services.AddDecryptionClient(builder.Configuration);
            builder.Services.UseFtpDownloadProvider(builder.Configuration, builder => builder.AddFtpDownloadProvider());

            // Register IConfiguration to be available for dependency injection
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
            JsonNamingPolicy jsonNamingPolicy = JsonNamingPolicy.CamelCase;

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

            builder.Services.AddApplicationInsightsTelemetry();
        }

        public static void ConfigurePipeline(WebApplication app, InvisibleApiKey invisibleApiKey)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(configuration =>
                {
                    configuration.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

                    // Configure OAuth2 for Swagger UI
                    configuration.OAuthClientId(app.Configuration["AzureAd:ClientId"]); // Use the application ClientId
                    configuration.OAuthClientSecret(""); // For PKCE, client secret can be empty
                    configuration.OAuthUsePkce(); // Enable PKCE (Proof Key for Code Exchange)
                    configuration.OAuthScopes(app.Configuration["AzureAd:Scopes"]); // Add required scopes
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseInvisibleApiMiddleware(invisibleApiKey);
            app.MapControllers().WithOpenApi();
            app.MapFallbackToFile("/index.html");
        }

        private static void AddProviders(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDownloadAbstractionProvider, DownloadAbstractionProvider>();
            services.AddTransient<IDownloadProvider, MockDownloadProvider>();
        }

        private static void AddBrokers(IServiceCollection services, IConfiguration configuration)
        {
            ValidateAppInsightsConfiguration(configuration);
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

            ValidateBlobStorageSettings(blobStorageSettings!);
            ValidateBlobContainers(blobStorageSettings!.BlobContainers);
            services.AddSingleton<BlobContainers>(blobStorageSettings!.BlobContainers);

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

        private static void ValidateAppInsightsConfiguration(IConfiguration configuration)
        {
            string connectionString = configuration["ApplicationInsights:ConnectionString"] ?? string.Empty;
            Validate((Rule: IsInvalid(connectionString), Parameter: "applicationInsights__connectionString"));
        }

        private static void ValidateBlobStorageSettings(BlobStorageSettings blobStorageSettings)
        {
            Validate(
                (Rule: IsInvalid(blobStorageSettings.AzureBlobServiceUri),
                    Parameter: "blobStorage__azureBlobServiceUri"),

                (Rule: IsInvalid(blobStorageSettings.AzureTenantId),
                    Parameter: "blobStorage__azureTenantId"));
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