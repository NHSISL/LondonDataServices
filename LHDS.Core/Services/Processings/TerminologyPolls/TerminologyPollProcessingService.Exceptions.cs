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
        private delegate ValueTask<IQueryable<TerminologyPoll>> ReturningTerminologyPollsFunction();

        private async ValueTask<TerminologyPoll> TryCatch(ReturningTerminologyPollFunction
            returningTerminologyPollFunction)
        {
            try
            {
                return await returningTerminologyPollFunction();
            }
            catch (NullTerminologyPollProcessingException nullTerminologyPollProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullTerminologyPollProcessingException);
            }
            catch (InvalidArgumentTerminologyPollsProcessingException
                InvalidArgumentTerminologyPollsProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(InvalidArgumentTerminologyPollsProcessingException);
            }
            catch (TerminologyPollValidationException terminologyPollValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(terminologyPollValidationException);
            }
            catch (TerminologyPollDependencyValidationException terminologyPollDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    terminologyPollDependencyValidationException);
            }
            catch (TerminologyPollDependencyException terminologyPollDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(terminologyPollDependencyException);
            }
            catch (TerminologyPollServiceException terminologyPollServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(terminologyPollServiceException);
            }
            catch (Exception exception)
            {
                var failedTerminologyPollProcessingServiceException =
                    new FailedTerminologyPollProcessingServiceException(
                        message: "Failed terminology poll processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedTerminologyPollProcessingServiceException);
            }
        }

        private async ValueTask<IQueryable<TerminologyPoll>>
            TryCatch(ReturningTerminologyPollsFunction returningTerminologyPollsFunction)
        {
            try
            {
                return await returningTerminologyPollsFunction();
            }
            catch (TerminologyPollValidationException terminologyPollValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(terminologyPollValidationException);
            }
            catch (TerminologyPollDependencyValidationException terminologyPollDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    terminologyPollDependencyValidationException);
            }
            catch (TerminologyPollDependencyException terminologyPollDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(terminologyPollDependencyException);
            }
            catch (TerminologyPollServiceException terminologyPollServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(terminologyPollServiceException);
            }
            catch (Exception exception)
            {
                var failedTerminologyPollProcessingServiceException =
                    new FailedTerminologyPollProcessingServiceException(
                        message: "Failed terminology poll processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedTerminologyPollProcessingServiceException);
            }
        }

        private async ValueTask<TerminologyPollProcessingValidationException> CreateAndLogValidationExceptionAsync(
            Xeption exception)
        {
            var terminologyPollProcessingValidationException =
                new TerminologyPollProcessingValidationException(
                    message: "Terminology poll processing validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(terminologyPollProcessingValidationException);

            return terminologyPollProcessingValidationException;
        }

        private async ValueTask<TerminologyPollProcessingDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var terminologyPollProcessingDependencyValidationException =
                new TerminologyPollProcessingDependencyValidationException(
                    message: "Terminology poll processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(terminologyPollProcessingDependencyValidationException);

            return terminologyPollProcessingDependencyValidationException;
        }

        private async ValueTask<TerminologyPollProcessingDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var terminologyPollProcessingDependencyException =
                new TerminologyPollProcessingDependencyException(
                    message: "Terminology poll processing dependency error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(terminologyPollProcessingDependencyException);

            return terminologyPollProcessingDependencyException;
        }

        private async ValueTask<TerminologyPollProcessingServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var terminologyPollProcessingServiceException =
                new TerminologyPollProcessingServiceException(
                    message: "Terminology poll processing service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(terminologyPollProcessingServiceException);

            return terminologyPollProcessingServiceException;
        }
    }
}