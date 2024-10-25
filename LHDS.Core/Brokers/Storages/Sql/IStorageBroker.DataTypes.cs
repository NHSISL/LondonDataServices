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
        ValueTask<DataType> InsertDataTypeAsync(DataType dataType);
        IQueryable<DataType> SelectAllDataTypes();
        ValueTask<DataType> SelectDataTypeByIdAsync(Guid dataTypeId);
        ValueTask<DataType> UpdateDataTypeAsync(DataType dataType);
        ValueTask<DataType> DeleteDataTypeAsync(DataType dataType);
    }
}
