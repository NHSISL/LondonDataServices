// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Text.Json;
using Azure.Core.Extensions;
using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Storage.Blobs;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Blobs;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Clients;
using LHDS.Core.Clients.Extensions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Configurations;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.DataSetObjects;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Providers.Downloads.Extensions;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.DataSetObjects;
using LHDS.Core.Services.Foundations.DataSets;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Foundations.DataTypes;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.ObjectColumns;
using LHDS.Core.Services.Foundations.OptOuts;
using LHDS.Core.Services.Foundations.PdsAudits;
using LHDS.Core.Services.Foundations.Suppliers;
using LHDS.Core.Services.Processings.OptOuts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Azure;
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
            AddBrokers(services, this.Configuration);
            AddFoundationServices(services, this.Configuration);
            AddOrchestrationServices(services, this.Configuration);
            AddProcessingServices(services, this.Configuration);
            services.AddLandingClient(this.Configuration);
            services.UseFtpDownloadProvider(this.Configuration, builder => builder.AddFtpDownloadProvider());

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

        private static void AddBrokers(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IIdentifierBroker, IdentifierBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<IBlobStorageBrokerSettings, BlobStorageBrokerSettings>();
            services.AddTransient<IBlobStorageBroker, BlobStorageBroker>();
            services.AddTransient<IAzureBlobClient, AzureBlobClient>();
        }

        private static void AddFoundationServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IIngestionTrackingService, IngestionTrackingService>();
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddTransient<IAuditService, AuditService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IOptOutService, OptOutService>();
            services.AddTransient<IPdsAuditService, PdsAuditService>();
            services.AddTransient<IDataSetService, DataSetService>();
            services.AddTransient<IDataSetObjectService, DataSetObjectService>();
            services.AddTransient<IDataSetSpecificationService, DataSetSpecificationService>();
            services.AddTransient<IDataTypeService, DataTypeService>();
            services.AddTransient<IObjectColumnService, ObjectColumnService>();
            services.AddTransient<IDataSetService, DataSetService>();

            var blobStorageSettings = configuration.GetSection("blobStorage").Get<BlobStorageSettings>();
            ValidateBlobStorageSettings(blobStorageSettings);

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
                    Parameter: "blobStorage__azureTenantId"),

                (Rule: IsInvalid(blobStorageSettings.BlobContainerName),
                    Parameter: "blobStorage__blobContainerName"));
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
        { }

        private static void AddProcessingServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IOptOutProcessingService, OptOutProcessingService>();
        }

        private IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder =
               new ODataConventionModelBuilder();

            builder.EntitySet<Audit>("Audits");
            builder.EntitySet<DataSet>("DataSets");
            builder.EntitySet<DataSetSpecification>("DataSetSpecifications");
            builder.EntitySet<DataSetObject>("DataSetObjects");
            builder.EntitySet<DataType>("DataTypes");
            builder.EntitySet<IngestionTracking>("IngestionTrackings");
            builder.EntitySet<ObjectColumn>("ObjectColumns");
            builder.EntitySet<OptOut>("OptOuts");
            builder.EntitySet<PdsAudit>("PdsAudits");
            builder.EntitySet<Supplier>("Suppliers");
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
