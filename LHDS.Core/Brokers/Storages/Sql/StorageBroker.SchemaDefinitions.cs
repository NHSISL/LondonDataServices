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
        public DbSet<DatasetObject> SchemaDefinitions { get; set; }

        public async ValueTask<DatasetObject> InsertSchemaDefinitionAsync(DatasetObject schemaDefinition) =>
            await InsertAsync(schemaDefinition);

        public IQueryable<DatasetObject> SelectAllSchemaDefinitions() => ReadAll<DatasetObject>();

        public async ValueTask<DatasetObject> SelectSchemaDefinitionByIdAsync(Guid schemaDefinitionId) =>
            await ReadAsync<DatasetObject>(schemaDefinitionId);

        public async ValueTask<DatasetObject> UpdateSchemaDefinitionAsync(DatasetObject schemaDefinition) =>
            await UpdateAsync(schemaDefinition);

        public async ValueTask<DatasetObject> DeleteSchemaDefinitionAsync(DatasetObject schemaDefinition) =>
            await DeleteAsync(schemaDefinition);
    }
}