// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataTypes;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<DataType> InsertDataTypeAsync(DataType dataType, CancellationToken cancellationToken = default);
        ValueTask<IQueryable<DataType>> SelectAllDataTypesAsync(CancellationToken cancellationToken = default);
        ValueTask<DataType> SelectDataTypeByIdAsync(Guid dataTypeId, CancellationToken cancellationToken = default);
        ValueTask<DataType> UpdateDataTypeAsync(DataType dataType, CancellationToken cancellationToken = default);
        ValueTask<DataType> DeleteDataTypeAsync(DataType dataType, CancellationToken cancellationToken = default);
    }
}
