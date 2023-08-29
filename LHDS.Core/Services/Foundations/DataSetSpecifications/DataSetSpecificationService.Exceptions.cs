using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationService
    {
        private delegate ValueTask<DataSetSpecification> ReturningDataSetSpecificationFunction();

        private async ValueTask<DataSetSpecification> TryCatch(ReturningDataSetSpecificationFunction returningDataSetSpecificationFunction)
        {
            try
            {
                return await returningDataSetSpecificationFunction();
            }
            catch (NullDataSetSpecificationException nullDataSetSpecificationException)
            {
                throw CreateAndLogValidationException(nullDataSetSpecificationException);
            }
        }

        private DataSetSpecificationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var dataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetSpecificationValidationException);

            return dataSetSpecificationValidationException;
        }
    }
}