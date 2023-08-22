// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SchemaDefinitions;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<SchemaDefinition> SchemaDefinitions { get; set; }

        public async ValueTask<SchemaDefinition> InsertSchemaDefinitionAsync(SchemaDefinition schemaDefinition) =>
            await InsertAsync(schemaDefinition);

        public IQueryable<SchemaDefinition> SelectAllSchemaDefinitions() => ReadAll<SchemaDefinition>();

        public async ValueTask<SchemaDefinition> SelectSchemaDefinitionByIdAsync(Guid schemaDefinitionId) =>
            await ReadAsync<SchemaDefinition>(schemaDefinitionId);

        public async ValueTask<SchemaDefinition> UpdateSchemaDefinitionAsync(SchemaDefinition schemaDefinition) =>
            await UpdateAsync(schemaDefinition);

        public async ValueTask<SchemaDefinition> DeleteSchemaDefinitionAsync(SchemaDefinition schemaDefinition) =>
            await DeleteAsync(schemaDefinition);
    }
}