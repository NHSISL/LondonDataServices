// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
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
        private delegate IQueryable<TerminologyPoll> ReturningTerminologyPollsFunction();

        private async ValueTask<TerminologyPoll> TryCatch(ReturningTerminologyPollFunction
            returningTerminologyPollFunction)
        {
            try
            {
                return await returningTerminologyPollFunction();
            }
            catch (NullTerminologyPollProcessingException nullTerminologyPollProcessingException)
            {
                throw CreateAndLogValidationException(nullTerminologyPollProcessingException);
            }
            catch (InvalidArgumentTerminologyPollsProcessingException
                InvalidArgumentTerminologyPollsProcessingException)
            {
                throw CreateAndLogValidationException(InvalidArgumentTerminologyPollsProcessingException);
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
            catch (Exception exception)
            {
                var failedTerminologyPollProcessingServiceException =
                    new FailedTerminologyPollProcessingServiceException(
                        message: "Failed terminology poll processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedTerminologyPollProcessingServiceException);
            }
        }

        private IQueryable<TerminologyPoll> TryCatch(ReturningTerminologyPollsFunction returningTerminologyPollsFunction)
        {
            try
            {
                return returningTerminologyPollsFunction();
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
            catch (Exception exception)
            {
                var failedTerminologyPollProcessingServiceException =
                    new FailedTerminologyPollProcessingServiceException(
                        message: "Failed terminology poll processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedTerminologyPollProcessingServiceException);
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

        private TerminologyPollProcessingServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var terminologyPollProcessingServiceException =
                new TerminologyPollProcessingServiceException(
                    message: "Terminology poll processing service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(terminologyPollProcessingServiceException);

            return terminologyPollProcessingServiceException;
        }
    }
}