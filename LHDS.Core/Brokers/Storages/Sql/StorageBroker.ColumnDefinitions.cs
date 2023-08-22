// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ColumnDefinitions;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<ColumnDefinition> ColumnDefinitions { get; set; }

        public async ValueTask<ColumnDefinition> InsertColumnDefinitionAsync(ColumnDefinition columnDefinition) =>
            await InsertAsync(columnDefinition);

        public IQueryable<ColumnDefinition> SelectAllColumnDefinitions() => ReadAll<ColumnDefinition>();

        public async ValueTask<ColumnDefinition> SelectColumnDefinitionByIdAsync(Guid columnDefinitionId) =>
            await ReadAsync<ColumnDefinition>(columnDefinitionId);

        public async ValueTask<ColumnDefinition> UpdateColumnDefinitionAsync(ColumnDefinition columnDefinition) =>
            await UpdateAsync(columnDefinition);

        public async ValueTask<ColumnDefinition> DeleteColumnDefinitionAsync(ColumnDefinition columnDefinition) =>
            await DeleteAsync(columnDefinition);
    }
}