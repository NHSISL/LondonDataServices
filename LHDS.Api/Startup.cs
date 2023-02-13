// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Text.Json;
using LHDS.Core.Brokers.Storages.Sql;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Logging;
using Microsoft.OData.Edm;
using Microsoft.OpenApi.Models;

namespace LHDS.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
           Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddControllers();
            services.AddLogging();

            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: "AllowFrontendOrigin", policy =>
            //    {
            //        policy.WithOrigins(Configuration.GetSection("CorsOrigins").Get<List<string>>().ToArray());
            //        policy.AllowAnyHeader();
            //        policy.AllowAnyMethod();
            //        policy.WithExposedHeaders("Content-Disposition", "Content-Length", "Access-Control-Allow-Origin");
            //    });
            //});

            JsonNamingPolicy jsonNamingPolicy = JsonNamingPolicy.CamelCase;

            services.AddControllers()
               .AddOData(options =>
               {
                   //options.AddRouteComponents("odata", GetEdmModel());
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


            services.AddSwaggerGen(options =>
            {
                var openApiInfo = new OpenApiInfo
                {
                    Title = "LHDS.Api",
                    Version = "v1"
                };

                options.SwaggerDoc(
                    name: "v1",
                    info: openApiInfo);
            });

            AddBrokers(services);
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
                        name: "NEL.Premises.Api v1");
                });

                IdentityModelEventSource.ShowPII = true;
                app.UseODataRouteDebug();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowFrontendOrigin");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }

        private static void AddBrokers(IServiceCollection services)
        {
            services.AddTransient<IStorageBroker, StorageBroker>();

        }

        private static void AddServices(IServiceCollection services)
        {
        }
        private static void AddOrchestrations(IServiceCollection services)
        {

        }

        private IEdmModel GetEdmModel()
        {
        }

        internal static class StartupExtensions
        {
        }
    }
}
