using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollService
    {
        private delegate ValueTask<TerminologyPoll> ReturningTerminologyPollFunction();

        private async ValueTask<TerminologyPoll> TryCatch(ReturningTerminologyPollFunction returningTerminologyPollFunction)
        {
            try
            {
                return await returningTerminologyPollFunction();
            }
            catch (NullTerminologyPollException nullTerminologyPollException)
            {
                throw CreateAndLogValidationException(nullTerminologyPollException);
            }
            catch (InvalidTerminologyPollException invalidTerminologyPollException)
            {
                throw CreateAndLogValidationException(invalidTerminologyPollException);
            }
            catch (SqlException sqlException)
            {
                var failedTerminologyPollStorageException =
                    new FailedTerminologyPollStorageException(
                        message: "Failed terminologyPoll storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedTerminologyPollStorageException);
            }
        }

        private TerminologyPollValidationException CreateAndLogValidationException(Xeption exception)
        {
            var terminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(terminologyPollValidationException);

            return terminologyPollValidationException;
        }

        private TerminologyPollDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var terminologyPollDependencyException = 
                new TerminologyPollDependencyException(
                    message: "TerminologyPoll dependency error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogCritical(terminologyPollDependencyException);

            return terminologyPollDependencyException;
        }
    }
}