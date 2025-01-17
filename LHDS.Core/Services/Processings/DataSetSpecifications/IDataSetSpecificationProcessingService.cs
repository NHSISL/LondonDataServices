// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSetSpecifications;

namespace LHDS.Core.Services.Processings.DataSetSpecifications
{
    public interface IDataSetSpecificationProcessingService
    {
        ValueTask<DataSetSpecification> AddDataSetSpecificationAsync(DataSetSpecification dataSetSpecification);
        ValueTask<IQueryable<DataSetSpecification>> RetrieveAllDataSetSpecificationsAsync();
        ValueTask<DataSetSpecification> RetrieveDataSetSpecificationByIdAsync(Guid dataSetSpecificationId);
        ValueTask<DataSetSpecification> RetrieveOrAddDataSetSpecificationAsync(DataSetSpecification dataSetSpecification);
        ValueTask<DataSetSpecification> ModifyOrAddDataSetSpecificationAsync(DataSetSpecification dataSetSpecification);
        ValueTask<DataSetSpecification> ModifyDataSetSpecificationAsync(DataSetSpecification dataSetSpecification);
        ValueTask<DataSetSpecification> RemoveDataSetSpecificationByIdAsync(Guid dataSetSpecificationId);
        ValueTask<DataSetSpecification?> GetActiveDataSetSpecification(Guid supplierId);
    }
}
