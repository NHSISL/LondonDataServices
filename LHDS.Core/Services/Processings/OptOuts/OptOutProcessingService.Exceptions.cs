// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.OptOuts
{
    public partial class OptOutProcessingService
    {
        private delegate ValueTask<OptOut> ReturningOptOutFunction();

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
                    new FailedOptOutProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedOptOutProcessingServiceException);
            }
        }

        private OptOutProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            string validationSummary = GetValidationSummary(exception.Data);

            var optOutProcessingValidationException =
                new OptOutProcessingValidationException(exception, validationSummary);

            this.loggingBroker.LogError(optOutProcessingValidationException);

            return optOutProcessingValidationException;
        }

        private OptOutProcessingDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var optOutProcessingDependencyValidationException =
                new OptOutProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

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

