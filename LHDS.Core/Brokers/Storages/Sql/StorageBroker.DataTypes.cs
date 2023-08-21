// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<DataType> DataTypes { get; set; }

        public async ValueTask<DataType> InsertDataTypeAsync(DataType dataType)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<DataType> dataTypeEntityEntry =
                await broker.DataTypes.AddAsync(dataType);

            await broker.SaveChangesAsync();

            return dataTypeEntityEntry.Entity;
        }

        public IQueryable<DataType> SelectAllDataTypes()
        {
            using var broker =
                new StorageBroker(this.configuration);

            return broker.DataTypes;
        }

        public async ValueTask<DataType> SelectDataTypeByIdAsync(Guid dataTypeId)
        {
            using var broker =
                new StorageBroker(this.configuration);

            return await broker.DataTypes.FindAsync(dataTypeId);
        }

        public async ValueTask<DataType> UpdateDataTypeAsync(DataType dataType)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<DataType> dataTypeEntityEntry =
                broker.DataTypes.Update(dataType);

            await broker.SaveChangesAsync();

            return dataTypeEntityEntry.Entity;
        }

        public async ValueTask<DataType> DeleteDataTypeAsync(DataType dataType)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<DataType> dataTypeEntityEntry =
                broker.DataTypes.Remove(dataType);

            await broker.SaveChangesAsync();

            return dataTypeEntityEntry.Entity;
        }
    }
}
