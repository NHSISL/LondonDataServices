// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using Azure.Core.Extensions;
using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Storage.Blobs;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
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
using LHDS.Core.Services.Foundations.Audits;
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
using LHDS.Core.Services.Orchestrations.Downloads;
using LHDS.Core.Services.Orchestrations.EmisLandings;
using LHDS.Core.Services.Orchestrations.Ingress;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Downloads;
using LHDS.Core.Services.Processings.IngestionTrackingAudits;
using LHDS.Core.Services.Processings.IngestionTrackings;
using LHDS.Core.Services.Processings.OptOuts;
using LHDS.Core.Services.Processings.SecureDatas;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;

namespace LHDS.AdminPortal.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
           Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddLogging();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowFrontendOrigin", policy =>
                {
                    policy.WithOrigins(Configuration.GetSection("CorsOrigins").Get<List<string>>().ToArray());
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.WithExposedHeaders("Content-Disposition", "Content-Length", "Access-Control-Allow-Origin");
                });
            });

            JsonNamingPolicy jsonNamingPolicy = JsonNamingPolicy.CamelCase;

            services.AddODataQueryFilter();
            services.AddControllers()
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

            services.AddSingleton(new JsonSerializerOptions
            {
                PropertyNamingPolicy = jsonNamingPolicy,
                DictionaryKeyPolicy = jsonNamingPolicy,
            });

            services.AddDbContext<StorageBroker>();
            AddClients(services, this.Configuration);
            AddProviders(services, this.Configuration);
            AddBrokers(services, this.Configuration);
            AddFoundationServices(services, this.Configuration);
            AddOrchestrationServices(services, this.Configuration);
            AddProcessingServices(services, this.Configuration);
            AddCoordinationServices(services, this.Configuration);

            services.AddSwaggerGen(options =>
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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            services.AddApplicationInsightsTelemetry();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
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

            app.UseHttpsRedirection();
            app.UseCors("AllowFrontendOrigin");
            app.UseRouting();
            app.UseAuthentication();
            app.UseForwardedHeaders();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
        private static void AddClients(
           IServiceCollection services,
           IConfiguration configuration)
        {
            services.AddTransient<IAuditClient, AuditClient>();
            services.AddEmisLandingClient(configuration);
            services.AddDecryptionClient(configuration);
            services.UseFtpDownloadProvider(configuration, builder => builder.AddFtpDownloadProvider());
        }


        private static void AddProviders(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDownloadAbstractionProvider, DownloadAbstractionProvider>();
            services.AddTransient<IDownloadProvider, MockDownloadProvider>();
        }

        private static void AddBrokers(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IIdentifierBroker, IdentifierBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<IBlobStorageBroker, BlobStorageBroker>();
            services.AddTransient<IHashBroker, HashBroker>();
            services.AddTransient<IAzureBlobClient, AzureBlobClient>();
            services.AddTransient<ISecureDataService, SecureDataService>();
            services.AddTransient<IAuditBroker, AuditBroker>();
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
            services.AddTransient<IAddressService, AddressService>();
            services.AddTransient<IAuditService, AuditService>();
            services.AddTransient<IResolvedAddressService, ResolvedAddressService>();

            var blobStorageSettings = configuration.GetSection("blobStorage").Get<BlobStorageSettings>();
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

        private static dynamic IsInvalid(string? text) => new
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
            services.AddTransient<IEmisLandingOrchestrationService, EmisLandingOrchestrationService>();
            services.AddTransient<IEmisLandingOrchestrationService, EmisLandingOrchestrationService>();
            services.AddTransient<IIngressOrchestrationService, IngressOrchestrationService>();
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
        }

        private static void AddCoordinationServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDecryptionCoordinationService, DecryptionCoordinationService>();
        }

        private IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder =
               new ODataConventionModelBuilder();

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
            builder.EntitySet<TerminologyArtifact>("TerminologyArtifacts");
            builder.EntitySet<SubscriberCredential>("SubscriberCredentials");
            builder.EntitySet<SubscriberAgreement>("SubscriberAgreements");
            builder.EntitySet<Address>("Addresses");
            builder.EntitySet<ResolvedAddress>("ResolvedAddresses");
            builder.EnableLowerCamelCase();

            return builder.GetEdmModel();
        }
    }

    internal static class StartupExtensions
    {
        public static IAzureClientBuilder<BlobServiceClient, BlobClientOptions> AddBlobServiceClient(
            this AzureClientFactoryBuilder builder,
            string serviceUriOrConnectionString,
            bool preferMsi)
        {
            if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri))
            {
                return builder.AddBlobServiceClient(serviceUri);
            }
            else
            {
                return builder.AddBlobServiceClient(serviceUriOrConnectionString);
            }
        }
    }
}

