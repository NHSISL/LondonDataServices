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
        public DbSet<DataSetSpecification> DatasetSpecifications { get; set; }

        public async ValueTask<DataSetSpecification> InsertDatasetSpecificationAsync(
            DataSetSpecification datasetSpecification) => await InsertAsync(datasetSpecification);

        public IQueryable<DataSetSpecification> SelectAllDatasetSpecifications() => ReadAll<DataSetSpecification>();

        public async ValueTask<DataSetSpecification> SelectDatasetSpecificationByIdAsync(
            Guid datasetSpecificationId) => await ReadAsync<DataSetSpecification>(datasetSpecificationId);

        public async ValueTask<DataSetSpecification> UpdateDatasetSpecificationAsync(
            DataSetSpecification datasetSpecification) => await UpdateAsync(datasetSpecification);

        public async ValueTask<DataSetSpecification> DeleteDatasetSpecificationAsync(
            DataSetSpecification datasetSpecification) => await DeleteAsync(datasetSpecification);
    }
}