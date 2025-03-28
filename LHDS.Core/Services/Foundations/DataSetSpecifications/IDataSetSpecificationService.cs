// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSetSpecifications;

namespace LHDS.Core.Services.Foundations.DataSetSpecifications
{
    public interface IDataSetSpecificationService
    {
        ValueTask<DataSetSpecification> AddDataSetSpecificationAsync(DataSetSpecification dataSetSpecification);
        ValueTask<IQueryable<DataSetSpecification>> RetrieveAllDataSetSpecificationsAsync();
        ValueTask<DataSetSpecification> RetrieveDataSetSpecificationByIdAsync(Guid dataSetSpecificationId);
        ValueTask<DataSetSpecification> ModifyDataSetSpecificationAsync(DataSetSpecification dataSetSpecification);
        ValueTask<DataSetSpecification> RemoveDataSetSpecificationByIdAsync(Guid dataSetSpecificationId);
    }
}