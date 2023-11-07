// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using LHDS.Core.Models.Processings.TerminologyPolls.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.TerminologyPolls
{
    public partial class TerminologyPollProcessingService
    {
        private delegate ValueTask<TerminologyPoll> ReturningTerminologyPollFunction();

        private async ValueTask<TerminologyPoll> TryCatch(ReturningTerminologyPollFunction 
            returningTerminologyPollFunction)
        {
            try
            {
                return await returningTerminologyPollFunction();
            }
            catch (NullTerminologyPollException nullTerminologyPollException)
            {
                throw CreateAndLogValidationException(nullTerminologyPollException);
            }
        }

        private TerminologyPollProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var terminologyPollProcessingValidationException =
                new TerminologyPollProcessingValidationException(
                    message: "Terminology poll processing validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(terminologyPollProcessingValidationException);

            return terminologyPollProcessingValidationException;
        }
    }
}