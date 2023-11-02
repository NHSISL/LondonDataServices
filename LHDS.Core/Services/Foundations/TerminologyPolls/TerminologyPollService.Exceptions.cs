using System.Threading.Tasks;
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
    }
}