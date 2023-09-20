// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSets;

namespace LHDS.Core.Services.Processings.DataSets
{
    public interface IDataSetProcessingService
    {
        ValueTask<DataSet> AddDataSetAsync(DataSet dataSet);
        IQueryable<DataSet> RetrieveAllDataSets();
    }
}
