// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSetSpecifications;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<DataSetSpecification> InsertDataSetSpecificationAsync(DataSetSpecification dataSetSpecification);
        ValueTask<IQueryable<DataSetSpecification>> SelectAllDataSetSpecificationsAsync();
        ValueTask<DataSetSpecification> SelectDataSetSpecificationByIdAsync(Guid dataSetSpecificationId);
        ValueTask<DataSetSpecification> UpdateDataSetSpecificationAsync(DataSetSpecification dataSetSpecification);
        ValueTask<DataSetSpecification> DeleteDataSetSpecificationAsync(DataSetSpecification dataSetSpecification);
    }
}