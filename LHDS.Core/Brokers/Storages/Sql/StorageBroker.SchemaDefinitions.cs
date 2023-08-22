// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SchemaDefinitions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<SchemaDefinition> SchemaDefinitions { get; set; }

        public async ValueTask<SchemaDefinition> InsertSchemaDefinitionAsync(SchemaDefinition schemaDefinition)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<SchemaDefinition> schemaDefinitionEntityEntry =
                await broker.SchemaDefinitions.AddAsync(schemaDefinition);

            await broker.SaveChangesAsync();

            return schemaDefinitionEntityEntry.Entity;
        }

        public IQueryable<SchemaDefinition> SelectAllSchemaDefinitions()
        {
            using var broker =
                new StorageBroker(this.configuration);

            return broker.SchemaDefinitions;
        }

        public async ValueTask<SchemaDefinition> SelectSchemaDefinitionByIdAsync(Guid schemaDefinitionId)
        {
            using var broker =
                new StorageBroker(this.configuration);

            return await broker.SchemaDefinitions.FindAsync(schemaDefinitionId);
        }

        public async ValueTask<SchemaDefinition> UpdateSchemaDefinitionAsync(SchemaDefinition schemaDefinition)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<SchemaDefinition> schemaDefinitionEntityEntry =
                broker.SchemaDefinitions.Update(schemaDefinition);

            await broker.SaveChangesAsync();

            return schemaDefinitionEntityEntry.Entity;
        }

        public async ValueTask<SchemaDefinition> DeleteSchemaDefinitionAsync(SchemaDefinition schemaDefinition)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<SchemaDefinition> schemaDefinitionEntityEntry =
                broker.SchemaDefinitions.Remove(schemaDefinition);

            await broker.SaveChangesAsync();

            return schemaDefinitionEntityEntry.Entity;
        }
    }
}