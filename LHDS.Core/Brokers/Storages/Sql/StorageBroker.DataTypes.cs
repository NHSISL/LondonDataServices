// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataTypes;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<DataType> DataTypes { get; set; }

        public async ValueTask<DataType> InsertDataTypeAsync(DataType dataType) =>
            await InsertAsync(dataType);

        public async ValueTask<IQueryable<DataType>> SelectAllDataTypesAsync() => await SelectAllAsync<DataType>();

        public async ValueTask<DataType> SelectDataTypeByIdAsync(Guid dataTypeId) =>
            await SelectAsync<DataType>(dataTypeId);

        public async ValueTask<DataType> UpdateDataTypeAsync(DataType dataType) =>
            await UpdateAsync(dataType);

        public async ValueTask<DataType> DeleteDataTypeAsync(DataType dataType) =>
            await DeleteAsync(dataType);
    }
}
