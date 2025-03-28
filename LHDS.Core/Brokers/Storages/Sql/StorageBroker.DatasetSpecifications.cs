// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<DataSetSpecification> DataSetSpecifications { get; set; }

        public async ValueTask<DataSetSpecification> InsertDataSetSpecificationAsync(
            DataSetSpecification dataSetSpecification) => await InsertAsync(dataSetSpecification);

        public async ValueTask<IQueryable<DataSetSpecification>> SelectAllDataSetSpecificationsAsync() => 
            await SelectAllAsync<DataSetSpecification>();

        public async ValueTask<DataSetSpecification> SelectDataSetSpecificationByIdAsync(
            Guid dataSetSpecificationId) => await SelectAsync<DataSetSpecification>(dataSetSpecificationId);

        public async ValueTask<DataSetSpecification> UpdateDataSetSpecificationAsync(
            DataSetSpecification dataSetSpecification) => await UpdateAsync(dataSetSpecification);

        public async ValueTask<DataSetSpecification> DeleteDataSetSpecificationAsync(
            DataSetSpecification dataSetSpecification) => await DeleteAsync(dataSetSpecification);
    }
}