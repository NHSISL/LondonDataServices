using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;

namespace LHDS.Core.Services.Foundations.DataSets
{
    public partial class DataSetService
    {
        private void ValidateDataSetOnAdd(DataSet dataSet)
        {
            ValidateDataSetIsNotNull(dataSet);
        }

        private static void ValidateDataSetIsNotNull(DataSet dataSet)
        {
            if (dataSet is null)
            {
                throw new NullDataSetException(message: "DataSet is null.");
            }
        }
    }
}