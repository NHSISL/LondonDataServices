// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Services.Foundations.DataSets;

namespace LHDS.Core.Services.Processings.DataSets
{
    public partial class DataSetProcessingService : IDataSetProcessingService
    {
        private readonly IDataSetService dataSetService;

        public DataSetProcessingService(IDataSetService dataSetService)
        {
            this.dataSetService = dataSetService;
        }

        public ValueTask<DataSet> AddDataSetAsync(DataSet dataSet) =>
            throw new System.NotImplementedException();
    }
}
