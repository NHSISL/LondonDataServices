// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            efCoreClient = new EFCoreClient(this);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            AddConfigurations(modelBuilder);
        }

        private static void AddConfigurations(ModelBuilder modelBuilder)
        {
            AddAuditConfigurations(modelBuilder);
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
            string connectionString = this.configuration.GetConnectionString(name: "DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        private async ValueTask<T> InsertAsync<T>(T @object, CancellationToken cancellationToken = default)
            where T : class =>
                await efCoreClient.InsertAsync(@object, cancellationToken);

        private async ValueTask<IQueryable<T>> SelectAllAsync<T>(CancellationToken cancellationToken = default)
            where T : class =>
                await efCoreClient.SelectAllAsync<T>(cancellationToken);

        private async ValueTask<T> SelectAsync<T>(object[] @objectIds, CancellationToken cancellationToken = default)
            where T : class =>
                await efCoreClient.SelectAsync<T>(@objectIds, cancellationToken);

        private async ValueTask<T> UpdateAsync<T>(T @object, CancellationToken cancellationToken = default)
            where T : class =>
                await efCoreClient.UpdateAsync(@object, cancellationToken);

        private async ValueTask<T> DeleteAsync<T>(T @object, CancellationToken cancellationToken = default)
            where T : class =>
                await efCoreClient.DeleteAsync(@object, cancellationToken);

        private async ValueTask BulkInsertAsync<T>(
            IEnumerable<T> objects,
            bool useTransaction = true,
            CancellationToken cancellationToken = default)
                where T : class =>
                    await efCoreClient.BulkInsertAsync<T>(objects, useTransaction, cancellationToken);

        private async ValueTask<IEnumerable<T>> BulkReadAsync<T>(
            IEnumerable<T> objects,
            CancellationToken cancellationToken = default)
                where T : class =>
                    await efCoreClient.BulkReadAsync<T>(objects, cancellationToken);

        private async ValueTask BulkUpdateAsync<T>(
            IEnumerable<T> objects,
            bool useTransaction = true,
            CancellationToken cancellationToken = default)
                where T : class =>
                    await efCoreClient.BulkUpdateAsync<T>(objects, useTransaction, cancellationToken);

        private async ValueTask BulkDeleteAsync<T>(
            IEnumerable<T> objects,
            bool useTransaction = true,
            CancellationToken cancellationToken = default)
                where T : class =>
                    await efCoreClient.BulkDeleteAsync<T>(objects, useTransaction, cancellationToken);

        private async ValueTask BulkUpsertAsync<T>(
            IEnumerable<T> objects,
            bool useTransaction = true,
            CancellationToken cancellationToken = default)
                where T : class =>
                    await efCoreClient.BulkUpsertAsync<T>(objects, useTransaction, cancellationToken);

        private async ValueTask<bool> ExistsAsync<T>(
            object[] objectIds,
            CancellationToken cancellationToken = default)
                where T : class =>
                    await efCoreClient.ExistsAsync<T>(objectIds, cancellationToken);
    }
}
