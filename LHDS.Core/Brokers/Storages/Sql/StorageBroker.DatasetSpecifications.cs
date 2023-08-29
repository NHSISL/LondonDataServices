// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSets;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<DataSetSpecification> DataSetSpecifications { get; set; }

        public async ValueTask<DataSetSpecification> InsertDataSetSpecificationAsync(
            DataSetSpecification datasetSpecification) => await InsertAsync(datasetSpecification);

        public IQueryable<DataSetSpecification> SelectAllDataSetSpecifications() => ReadAll<DataSetSpecification>();

        public async ValueTask<DataSetSpecification> SelectDataSetSpecificationByIdAsync(
            Guid datasetSpecificationId) => await ReadAsync<DataSetSpecification>(datasetSpecificationId);

        public async ValueTask<DataSetSpecification> UpdateDataSetSpecificationAsync(
            DataSetSpecification datasetSpecification) => await UpdateAsync(datasetSpecification);

        public async ValueTask<DataSetSpecification> DeleteDataSetSpecificationAsync(
            DataSetSpecification datasetSpecification) => await DeleteAsync(datasetSpecification);
    }
}