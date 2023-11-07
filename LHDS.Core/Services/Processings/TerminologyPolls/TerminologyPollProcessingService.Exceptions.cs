// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ObjectColumns.Exceptions;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using LHDS.Core.Models.Processings.ObjectColumns.Exceptions;
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
            catch (InvalidTerminologyPollException invalidTerminologyPollException)
            {
                throw CreateAndLogValidationException(invalidTerminologyPollException);
            }
            catch (TerminologyPollValidationException terminologyPollValidationException)
            {
                throw CreateAndLogDependencyValidationException(terminologyPollValidationException);
            }
            catch (TerminologyPollDependencyValidationException terminologyPollDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(terminologyPollDependencyValidationException);
            }
            catch (TerminologyPollDependencyException terminologyPollDependencyException)
            {
                throw CreateAndLogDependencyException(terminologyPollDependencyException);
            }
            catch (TerminologyPollServiceException terminologyPollServiceException)
            {
                throw CreateAndLogDependencyException(terminologyPollServiceException);
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

        private TerminologyPollProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var terminologyPollProcessingDependencyValidationException =
                new TerminologyPollProcessingDependencyValidationException(
                    message: "Terminology poll processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(terminologyPollProcessingDependencyValidationException);

            return terminologyPollProcessingDependencyValidationException;
        }

        private TerminologyPollProcessingDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var terminologyPollProcessingDependencyException =
                new TerminologyPollProcessingDependencyException(
                    message: "Terminology poll processing dependency error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(terminologyPollProcessingDependencyException);

            return terminologyPollProcessingDependencyException;
        }
    }
}