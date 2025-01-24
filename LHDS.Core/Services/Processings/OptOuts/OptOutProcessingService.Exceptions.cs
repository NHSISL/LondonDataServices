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
                throw CreateAndLogValidationException(nullOptOutProcessingException);
            }
            catch (InvalidArgumentOptOutProcessingException invalidArgumentOptOutProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentOptOutProcessingException);
            }
            catch (OptOutValidationException optOutValidationException)
            {
                throw CreateAndLogDependencyValidationException(optOutValidationException);
            }
            catch (OptOutDependencyValidationException optOutDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(optOutDependencyValidationException);
            }
            catch (OptOutDependencyException optOutDependencyException)
            {
                throw CreateAndLogDependencyException(optOutDependencyException);
            }
            catch (OptOutServiceException optOutServiceException)
            {
                throw CreateAndLogDependencyException(optOutServiceException);
            }
            catch (Exception exception)
            {
                var failedOptOutProcessingServiceException =
                    new FailedOptOutProcessingServiceException(
                        message: "Failed opt out processing service error occurred, please contact support.",
                        exception);

                throw CreateAndLogServiceException(failedOptOutProcessingServiceException);
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
                throw CreateAndLogValidationException(nullOptOutProcessingException);
            }
            catch (InvalidArgumentOptOutProcessingException invalidArgumentOptOutProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentOptOutProcessingException);
            }
            catch (OptOutValidationException optOutValidationException)
            {
                throw CreateAndLogDependencyValidationException(optOutValidationException);
            }
            catch (OptOutDependencyValidationException optOutDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(optOutDependencyValidationException);
            }
            catch (OptOutDependencyException optOutDependencyException)
            {
                throw CreateAndLogDependencyException(optOutDependencyException);
            }
            catch (OptOutServiceException optOutServiceException)
            {
                throw CreateAndLogDependencyException(optOutServiceException);
            }
            catch (Exception exception)
            {
                var failedOptOutProcessingServiceException =
                    new FailedOptOutProcessingServiceException(
                        message: "Failed opt out processing service error occurred, please contact support.",
                        exception);

                throw CreateAndLogServiceException(failedOptOutProcessingServiceException);
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
                throw CreateAndLogValidationException(invalidArgumentOptOutProcessingException);
            }
            catch (OptOutDependencyValidationException optOutDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(optOutDependencyValidationException);
            }
            catch (OptOutValidationException optOutValidationException)
            {
                throw CreateAndLogDependencyValidationException(optOutValidationException);
            }
            catch (OptOutDependencyException optOutDependencyException)
            {
                throw CreateAndLogDependencyException(optOutDependencyException);
            }
            catch (OptOutServiceException optOutServiceException)
            {
                throw CreateAndLogDependencyException(optOutServiceException);
            }
            catch (Exception exception)
            {
                var failedOptOutProcessingServiceException =
                    new FailedOptOutProcessingServiceException(
                        message: "Failed opt out processing service error occurred, please contact support.",
                        exception);

                throw CreateAndLogServiceException(failedOptOutProcessingServiceException);
            }
        }

        private OptOutProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var optOutProcessingValidationException =
                new OptOutProcessingValidationException(exception);

            this.loggingBroker.LogError(optOutProcessingValidationException);

            return optOutProcessingValidationException;
        }

        private OptOutProcessingDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var optOutProcessingDependencyValidationException =
                new OptOutProcessingDependencyValidationException(
                    exception?.InnerException as Xeption);

            this.loggingBroker.LogError(optOutProcessingDependencyValidationException);

            return optOutProcessingDependencyValidationException;
        }

        private OptOutProcessingDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var optOutProcessingDependencyException =
                new OptOutProcessingDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(optOutProcessingDependencyException);

            throw optOutProcessingDependencyException;
        }

        private OptOutProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var optOutProcessingServiceException = new
                OptOutProcessingServiceException(exception);

            this.loggingBroker.LogError(optOutProcessingServiceException);

            return optOutProcessingServiceException;
        }
    }
}
