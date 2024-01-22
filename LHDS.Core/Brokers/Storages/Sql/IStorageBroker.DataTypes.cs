// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataTypes;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<DataType> InsertDataTypeAsync(DataType audit);
        IQueryable<DataType> SelectAllDataTypes();
        ValueTask<DataType> SelectDataTypeByIdAsync(Guid auditId);
        ValueTask<DataType> UpdateDataTypeAsync(DataType audit);
        ValueTask<DataType> DeleteDataTypeAsync(DataType audit);
    }
}
