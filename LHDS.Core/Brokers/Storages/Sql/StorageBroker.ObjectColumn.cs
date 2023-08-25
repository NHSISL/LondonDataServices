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
        public DbSet<ObjectColumn> ColumnDefinitions { get; set; }

        public async ValueTask<ObjectColumn> InsertColumnDefinitionAsync(ObjectColumn columnDefinition) =>
            await InsertAsync(columnDefinition);

        public IQueryable<ObjectColumn> SelectAllColumnDefinitions() => ReadAll<ObjectColumn>();

        public async ValueTask<ObjectColumn> SelectColumnDefinitionByIdAsync(Guid columnDefinitionId) =>
            await ReadAsync<ObjectColumn>(columnDefinitionId);

        public async ValueTask<ObjectColumn> UpdateColumnDefinitionAsync(ObjectColumn columnDefinition) =>
            await UpdateAsync(columnDefinition);

        public async ValueTask<ObjectColumn> DeleteColumnDefinitionAsync(ObjectColumn columnDefinition) =>
            await DeleteAsync(columnDefinition);
    }
}