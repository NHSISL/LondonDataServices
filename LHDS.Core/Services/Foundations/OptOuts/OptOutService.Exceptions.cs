using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using LHDS.Core.Models.OptOuts;
using LHDS.Core.Models.OptOuts.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.OptOuts
{
    public partial class OptOutService
    {
        private delegate ValueTask<OptOut> ReturningOptOutFunction();

        private async ValueTask<OptOut> TryCatch(ReturningOptOutFunction returningOptOutFunction)
        {
            try
            {
                return await returningOptOutFunction();
            }
            catch (NullOptOutException nullOptOutException)
            {
                throw CreateAndLogValidationException(nullOptOutException);
            }
            catch (InvalidOptOutException invalidOptOutException)
            {
                throw CreateAndLogValidationException(invalidOptOutException);
            }
            catch (SqlException sqlException)
            {
                var failedOptOutStorageException =
                    new FailedOptOutStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedOptOutStorageException);
            }
        }

        private OptOutValidationException CreateAndLogValidationException(Xeption exception)
        {
            var optOutValidationException =
                new OptOutValidationException(exception);

            this.loggingBroker.LogError(optOutValidationException);

            return optOutValidationException;
        }

        private OptOutDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var optOutDependencyException = new OptOutDependencyException(exception);
            this.loggingBroker.LogCritical(optOutDependencyException);

            return optOutDependencyException;
        }
    }
}