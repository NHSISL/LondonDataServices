using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.DataSets
{
    public partial class DataSetService
    {
        private delegate ValueTask<DataSet> ReturningDataSetFunction();

        private async ValueTask<DataSet> TryCatch(ReturningDataSetFunction returningDataSetFunction)
        {
            try
            {
                return await returningDataSetFunction();
            }
            catch (NullDataSetException nullDataSetException)
            {
                throw CreateAndLogValidationException(nullDataSetException);
            }
            catch (InvalidDataSetException invalidDataSetException)
            {
                throw CreateAndLogValidationException(invalidDataSetException);
            }
        }

        private DataSetValidationException CreateAndLogValidationException(Xeption exception)
        {
            var dataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetValidationException);

            return dataSetValidationException;
        }
    }
}