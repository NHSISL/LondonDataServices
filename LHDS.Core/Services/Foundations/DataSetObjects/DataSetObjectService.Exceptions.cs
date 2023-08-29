using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSetObjects;
using LHDS.Core.Models.Foundations.DataSetObjects.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectService
    {
        private delegate ValueTask<DataSetObject> ReturningDataSetObjectFunction();

        private async ValueTask<DataSetObject> TryCatch(ReturningDataSetObjectFunction returningDataSetObjectFunction)
        {
            try
            {
                return await returningDataSetObjectFunction();
            }
            catch (NullDataSetObjectException nullDataSetObjectException)
            {
                throw CreateAndLogValidationException(nullDataSetObjectException);
            }
        }

        private DataSetObjectValidationException CreateAndLogValidationException(Xeption exception)
        {
            var dataSetObjectValidationException =
                new DataSetObjectValidationException(
                    message: "DataSetObject validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetObjectValidationException);

            return dataSetObjectValidationException;
        }
    }
}