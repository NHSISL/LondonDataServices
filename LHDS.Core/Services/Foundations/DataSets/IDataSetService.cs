// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSets;

namespace LHDS.Core.Services.Foundations.DataSets
{
    public interface IDataSetService
    {
        ValueTask<DataSet> AddDataSetAsync(DataSet dataSet);
        IQueryable<DataSet> RetrieveAllDataSets();
        ValueTask<DataSet> RetrieveDataSetByIdAsync(Guid dataSetId);
        ValueTask<DataSet> ModifyDataSetAsync(DataSet dataSet);
        ValueTask<DataSet> RemoveDataSetByIdAsync(Guid dataSetId);
    }
}