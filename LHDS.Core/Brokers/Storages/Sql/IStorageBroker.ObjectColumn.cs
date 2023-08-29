// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ColumnDefinitions;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<ObjectColumn> InsertColumnDefinitionAsync(ObjectColumn columnDefinition);
        IQueryable<ObjectColumn> SelectAllColumnDefinitions();
        ValueTask<ObjectColumn> SelectColumnDefinitionByIdAsync(Guid columnDefinitionId);
        ValueTask<ObjectColumn> UpdateColumnDefinitionAsync(ObjectColumn columnDefinition);
        ValueTask<ObjectColumn> DeleteColumnDefinitionAsync(ObjectColumn columnDefinition);
    }
}