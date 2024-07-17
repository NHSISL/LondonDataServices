// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker : EFxceptionsContext, IStorageBroker
    {
        private readonly IConfiguration configuration;

        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.Database.Migrate();
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
            string connectionString = this.configuration.GetConnectionString(name: "DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        public override void Dispose() { }

        private async ValueTask BulkInsertAsync<T>(IEnumerable<T> objects) where T : class
        {
            objects.ToList().ForEach(@object => this.Entry(@object).State = EntityState.Added);
            this.AddRange(objects);
            await this.SaveChangesAsync();
            objects.ToList().ForEach(@object => this.Entry(@object).State = EntityState.Detached);
        }

        private async ValueTask<T> InsertAsync<T>(T @object)
        {
            this.Entry(@object).State = EntityState.Added;
            await this.SaveChangesAsync();
            this.Entry(@object).State = EntityState.Detached;

            return @object;
        }

        private IQueryable<T> ReadAll<T>() where T : class => this.Set<T>();

        private async ValueTask<T> ReadAsync<T>(params object[] @objectIds) where T : class =>
            await this.FindAsync<T>(objectIds);

        private async ValueTask<T> UpdateAsync<T>(T @object)
        {
            this.Entry(@object).State = EntityState.Modified;
            await this.SaveChangesAsync();
            this.Entry(@object).State = EntityState.Detached;

            return @object;
        }

        private async ValueTask BulkUpdateAsync<T>(IEnumerable<T> objects) where T : class
        {
            objects.ToList().ForEach(@object => this.Entry(@object).State = EntityState.Modified);
            this.AddRange(objects);
            await this.SaveChangesAsync();
            objects.ToList().ForEach(@object => this.Entry(@object).State = EntityState.Detached);
        }

        private async ValueTask<T> DeleteAsync<T>(T @object)
        {
            this.Entry(@object).State = EntityState.Deleted;
            await this.SaveChangesAsync();
            this.Entry(@object).State = EntityState.Detached;

            return @object;
        }
    }
}
