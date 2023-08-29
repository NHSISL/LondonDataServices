using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.ObjectColumns.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnService
    {
        private delegate ValueTask<ObjectColumn> ReturningObjectColumnFunction();

        private async ValueTask<ObjectColumn> TryCatch(ReturningObjectColumnFunction returningObjectColumnFunction)
        {
            try
            {
                return await returningObjectColumnFunction();
            }
            catch (NullObjectColumnException nullObjectColumnException)
            {
                throw CreateAndLogValidationException(nullObjectColumnException);
            }
            catch (InvalidObjectColumnException invalidObjectColumnException)
            {
                throw CreateAndLogValidationException(invalidObjectColumnException);
            }
        }

        private ObjectColumnValidationException CreateAndLogValidationException(Xeption exception)
        {
            var objectColumnValidationException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(objectColumnValidationException);

            return objectColumnValidationException;
        }
    }
}