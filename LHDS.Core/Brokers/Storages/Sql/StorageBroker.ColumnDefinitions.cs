// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ColumnDefinitions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<ColumnDefinition> ColumnDefinitions { get; set; }

        public async ValueTask<ColumnDefinition> InsertColumnDefinitionAsync(ColumnDefinition columnDefinition)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<ColumnDefinition> columnDefinitionEntityEntry =
                await broker.ColumnDefinitions.AddAsync(columnDefinition);

            await broker.SaveChangesAsync();

            return columnDefinitionEntityEntry.Entity;
        }

        public IQueryable<ColumnDefinition> SelectAllColumnDefinitions()
        {
            using var broker =
                new StorageBroker(this.configuration);

            return broker.ColumnDefinitions;
        }

        public async ValueTask<ColumnDefinition> SelectColumnDefinitionByIdAsync(Guid columnDefinitionId)
        {
            using var broker =
                new StorageBroker(this.configuration);

            return await broker.ColumnDefinitions.FindAsync(columnDefinitionId);
        }

        public async ValueTask<ColumnDefinition> UpdateColumnDefinitionAsync(ColumnDefinition columnDefinition)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<ColumnDefinition> columnDefinitionEntityEntry =
                broker.ColumnDefinitions.Update(columnDefinition);

            await broker.SaveChangesAsync();

            return columnDefinitionEntityEntry.Entity;
        }

        public async ValueTask<ColumnDefinition> DeleteColumnDefinitionAsync(ColumnDefinition columnDefinition)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<ColumnDefinition> columnDefinitionEntityEntry =
                broker.ColumnDefinitions.Remove(columnDefinition);

            await broker.SaveChangesAsync();

            return columnDefinitionEntityEntry.Entity;
        }
    }
}