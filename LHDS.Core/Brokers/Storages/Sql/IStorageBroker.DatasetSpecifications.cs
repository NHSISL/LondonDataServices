// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSetSpecifications;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<DataSetSpecification> InsertDataSetSpecificationAsync(DataSetSpecification datasetSpecification);
        IQueryable<DataSetSpecification> SelectAllDataSetSpecifications();
        ValueTask<DataSetSpecification> SelectDataSetSpecificationByIdAsync(Guid datasetSpecificationId);
        ValueTask<DataSetSpecification> UpdateDataSetSpecificationAsync(DataSetSpecification datasetSpecification);
        ValueTask<DataSetSpecification> DeleteDataSetSpecificationAsync(DataSetSpecification datasetSpecification);
    }
}