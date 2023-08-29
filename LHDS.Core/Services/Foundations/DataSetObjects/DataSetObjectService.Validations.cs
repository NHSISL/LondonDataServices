using LHDS.Core.Models.Foundations.DataSetObjects;
using LHDS.Core.Models.Foundations.DataSetObjects.Exceptions;

namespace LHDS.Core.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectService
    {
        private void ValidateDataSetObjectOnAdd(DataSetObject dataSetObject)
        {
            ValidateDataSetObjectIsNotNull(dataSetObject);
        }

        private static void ValidateDataSetObjectIsNotNull(DataSetObject dataSetObject)
        {
            if (dataSetObject is null)
            {
                throw new NullDataSetObjectException(message: "DataSetObject is null.");
            }
        }
    }
}