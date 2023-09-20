// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Processings.DataSets.Exceptions;

namespace LHDS.Core.Services.Processings.DataSets
{
    public partial class DataSetProcessingService : IDataSetProcessingService
    {
        private void ValidateDataSetOnAdd(DataSet dataSet)
        {
            ValidateDataSetIsNotNull(dataSet);
        }

        private static void ValidateDataSetIsNotNull(DataSet dataSet)
        {
            if (dataSet is null)
            {
                throw new NullDataSetProcessingException(message: "DataSet is null.");
            }
        }
    }
}
