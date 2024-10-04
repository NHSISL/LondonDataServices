// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using STX.EFCore.Client.Clients;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker : EFxceptionsContext, IStorageBroker
    {
        private readonly IConfiguration configuration;
        private readonly IEFCoreClient efCoreClient;

        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.Database.Migrate();
            this.efCoreClient = new EFCoreClient(this);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            AddConfigurations(modelBuilder);
        }

        private static void AddConfigurations(ModelBuilder modelBuilder)
        {
            AddAddressConfigurations(modelBuilder);
            AddDataSetConfigurations(modelBuilder);
            AddDataSetSpecificationConfigurations(modelBuilder);
            AddDataTypeConfigurations(modelBuilder);
            AddSupplierConfigurations(modelBuilder);
            AddIngestionTrackingConfigurations(modelBuilder);
            AddIngestionTrackingAuditConfigurations(modelBuilder);
            AddOptOutConfigurations(modelBuilder);
            AddObjectColumnConfigurations(modelBuilder);
            AddPdsAuditConfigurations(modelBuilder);
            AddResolvedAddressConfigurations(modelBuilder);
            AddSpecificationObjectConfigurations(modelBuilder);
            AddSubscriberAgreementConfigurations(modelBuilder);
            AddTerminologyArtifactConfigurations(modelBuilder);
            AddTerminologyPollConfigurations(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            string connectionString = this.configuration.GetConnectionString(name: "DefaultConnection") ?? string.Empty;
            optionsBuilder.UseSqlServer(connectionString);
        }

        private async ValueTask<T> InsertAsync<T>(T @object) where T : class =>
            await efCoreClient.InsertAsync(@object);

        private IQueryable<T> SelectAll<T>() where T : class => this.Set<T>();

        private async ValueTask<IQueryable<T>> SelectAllAsync<T>() where T : class =>
            await efCoreClient.SelectAllAsync<T>();

        private async ValueTask<T?> SelectAsync<T>(params object[] @objectIds) where T : class =>
            await efCoreClient.SelectAsync<T>(@objectIds);

        private async ValueTask<T> UpdateAsync<T>(T @object) where T : class =>
            await efCoreClient.UpdateAsync(@object);

        private async ValueTask<T> DeleteAsync<T>(T @object) where T : class =>
            await efCoreClient.UpdateAsync(@object);

        private async ValueTask BulkInsertAsync<T>(IEnumerable<T> objects) where T : class =>
            await efCoreClient.BulkInsertAsync<T>(objects);

        private async ValueTask BulkUpdateAsync<T>(IEnumerable<T> objects) where T : class =>
            await efCoreClient.BulkUpdateAsync<T>(objects);

        private async ValueTask BulkDeleteAsync<T>(IEnumerable<T> objects) where T : class =>
            await efCoreClient.BulkDeleteAsync<T>(objects);
    }
}
