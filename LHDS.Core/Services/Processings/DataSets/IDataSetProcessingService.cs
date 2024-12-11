// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSets;

namespace LHDS.Core.Services.Processings.DataSets
{
    public interface IDataSetProcessingService
    {
        ValueTask<DataSet> AddDataSetAsync(DataSet dataSet);
        IQueryable<DataSet> RetrieveAllDataSets();
        ValueTask<DataSet> RetrieveDataSetByIdAsync(Guid dataSetId);
        ValueTask<DataSet> RetrieveOrAddDataSetAsync(DataSet dataSet);
        ValueTask<DataSet> ModifyOrAddDataSetAsync(DataSet dataSet);
        ValueTask<DataSet> ModifyDataSetAsync(DataSet dataSet);
        ValueTask<DataSet> RemoveDataSetByIdAsync(Guid dataSetId);
    }
}
