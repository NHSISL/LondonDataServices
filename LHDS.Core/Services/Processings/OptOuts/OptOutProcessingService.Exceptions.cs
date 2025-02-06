// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.OptOuts
{
    public partial class OptOutProcessingService
    {
        private delegate ValueTask<OptOut> ReturningOptOutFunction();
        private delegate ValueTask<List<OptOut>> ReturningOptOutListFunction();
        private delegate ValueTask<IQueryable<OptOut>> ReturningOptOutListsFunction();

        private async ValueTask<OptOut> TryCatch(ReturningOptOutFunction returningOptOutFunction)
        {
            try
            {
                return await returningOptOutFunction();
            }
            catch (NullOptOutProcessingException nullOptOutProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullOptOutProcessingException);
            }
            catch (InvalidArgumentOptOutProcessingException invalidArgumentOptOutProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentOptOutProcessingException);
            }
            catch (OptOutValidationException optOutValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(optOutValidationException);
            }
            catch (OptOutDependencyValidationException optOutDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(optOutDependencyValidationException);
            }
            catch (OptOutDependencyException optOutDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutDependencyException);
            }
            catch (OptOutServiceException optOutServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutServiceException);
            }
            catch (Exception exception)
            {
                var failedOptOutProcessingServiceException =
                    new FailedOptOutProcessingServiceException(
                        message: "Failed opt out processing service error occurred, please contact support.",
                        exception);

                throw await CreateAndLogServiceExceptionAsync(failedOptOutProcessingServiceException);
            }
        }

        private async ValueTask<List<OptOut>> TryCatch(ReturningOptOutListFunction returningOptOutListFunction)
        {
            try
            {
                return await returningOptOutListFunction();
            }
            catch (NullOptOutProcessingException nullOptOutProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullOptOutProcessingException);
            }
            catch (InvalidArgumentOptOutProcessingException invalidArgumentOptOutProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentOptOutProcessingException);
            }
            catch (OptOutValidationException optOutValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(optOutValidationException);
            }
            catch (OptOutDependencyValidationException optOutDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(optOutDependencyValidationException);
            }
            catch (OptOutDependencyException optOutDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutDependencyException);
            }
            catch (OptOutServiceException optOutServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutServiceException);
            }
            catch (Exception exception)
            {
                var failedOptOutProcessingServiceException =
                    new FailedOptOutProcessingServiceException(
                        message: "Failed opt out processing service error occurred, please contact support.",
                        exception);

                throw await CreateAndLogServiceExceptionAsync(failedOptOutProcessingServiceException);
            }
        }

        private async ValueTask<IQueryable<OptOut>> TryCatch(ReturningOptOutListsFunction returningOptOutListsFunction)
        {
            try
            {
                return await returningOptOutListsFunction();
            }
            catch (InvalidArgumentOptOutProcessingException invalidArgumentOptOutProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentOptOutProcessingException);
            }
            catch (OptOutDependencyValidationException optOutDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(optOutDependencyValidationException);
            }
            catch (OptOutValidationException optOutValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(optOutValidationException);
            }
            catch (OptOutDependencyException optOutDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutDependencyException);
            }
            catch (OptOutServiceException optOutServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutServiceException);
            }
            catch (Exception exception)
            {
                var failedOptOutProcessingServiceException =
                    new FailedOptOutProcessingServiceException(
                        message: "Failed opt out processing service error occurred, please contact support.",
                        exception);

                throw await CreateAndLogServiceExceptionAsync(failedOptOutProcessingServiceException);
            }
        }

        private async ValueTask<OptOutProcessingValidationException> CreateAndLogValidationExceptionAsync(
            Xeption exception)
        {
            var optOutProcessingValidationException =
                new OptOutProcessingValidationException(
                    message: "OptOut processing validation errors occured, please try again", 
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(optOutProcessingValidationException);

            return optOutProcessingValidationException;
        }

        private async ValueTask<OptOutProcessingDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var optOutProcessingDependencyValidationException =
                new OptOutProcessingDependencyValidationException(
                    message: "Opt out processing dependency validation error occurred, please contact support.",
                    innerException: exception?.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(optOutProcessingDependencyValidationException);

            return optOutProcessingDependencyValidationException;
        }

        private async ValueTask<OptOutProcessingDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var optOutProcessingDependencyException =
                new OptOutProcessingDependencyException(
                    message: "Opt out processing dependency error occurred, please contact support.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(optOutProcessingDependencyException);

            throw optOutProcessingDependencyException;
        }

        private async ValueTask<OptOutProcessingServiceException> CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var optOutProcessingServiceException = new
                OptOutProcessingServiceException(
                message: "Opt out processing service error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(optOutProcessingServiceException);

            return optOutProcessingServiceException;
        }
    }
}
