// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
using LHDS.Core.Models.Processings.TerminologyArtifacts.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.TerminologyArtifacts
{
    public partial class TerminologyArtifactProcessingService
    {
        private delegate ValueTask<T> ReturningTerminologyArtifactProcessingFunction<T>();
        private delegate IQueryable<TerminologyArtifact> ReturningTerminologyArtifactsFunction();

        private IQueryable<TerminologyArtifact> TryCatch(
            ReturningTerminologyArtifactsFunction returningTerminologyArtifactsFunction)
        {
            try
            {
                return returningTerminologyArtifactsFunction();
            }
            catch (TerminologyArtifactValidationException terminologyArtifactValidationException)
            {
                throw CreateAndLogDependencyValidationException(terminologyArtifactValidationException);
            }
            catch (TerminologyArtifactDependencyValidationException terminologyArtifactDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(terminologyArtifactDependencyValidationException);
            }
            catch (TerminologyArtifactDependencyException terminologyArtifactDependencyException)
            {
                throw CreateAndLogDependencyException(terminologyArtifactDependencyException);
            }
            catch (TerminologyArtifactServiceException terminologyArtifactServiceException)
            {
                throw CreateAndLogDependencyException(terminologyArtifactServiceException);
            }
            catch (Exception exception)
            {
                var failedTerminologyArtifactProcessingServiceException =
                    new FailedTerminologyArtifactProcessingServiceException(
                        message: "Failed terminology artifact processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedTerminologyArtifactProcessingServiceException);
            }
        }

        private async ValueTask<T> TryCatch<T>(
            ReturningTerminologyArtifactProcessingFunction<T> returningTerminologyArtifactProcessingFunction)
        {
            try
            {
                return await returningTerminologyArtifactProcessingFunction();
            }
            catch (NullTerminologyArtifactProcessingException nullTerminologyArtifactException)
            {
                throw CreateAndLogValidationException(nullTerminologyArtifactException);
            }
            catch (InvalidArgumentTerminologyArtifactProcessingException
                invalidArgumentTerminologyArtifactProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentTerminologyArtifactProcessingException);
            }
            catch (TerminologyArtifactValidationException terminologyArtifactValidationException)
            {
                throw CreateAndLogDependencyValidationException(terminologyArtifactValidationException);
            }
            catch (TerminologyArtifactDependencyValidationException terminologyArtifactDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(terminologyArtifactDependencyValidationException);
            }
            catch (TerminologyArtifactDependencyException terminologyArtifactDependencyException)
            {
                throw CreateAndLogDependencyException(terminologyArtifactDependencyException);
            }
            catch (TerminologyArtifactServiceException terminologyArtifactServiceException)
            {
                throw CreateAndLogDependencyException(terminologyArtifactServiceException);
            }
            catch (Exception exception)
            {
                var failedTerminologyArtifactProcessingServiceException =
                    new FailedTerminologyArtifactProcessingServiceException(
                        message: "Failed terminology artifact processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedTerminologyArtifactProcessingServiceException);
            }
        }

        private TerminologyArtifactProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var terminologyArtifactProcessingValidationExceptionn =
                new TerminologyArtifactProcessingValidationException(
                    message: "Terminology artifact processing validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(terminologyArtifactProcessingValidationExceptionn);

            return terminologyArtifactProcessingValidationExceptionn;
        }

        private TerminologyArtifactProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var terminologyArtifactProcessingDependencyValidationException =
                new TerminologyArtifactProcessingDependencyValidationException(
                    message: "Terminology artifact processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(terminologyArtifactProcessingDependencyValidationException);

            return terminologyArtifactProcessingDependencyValidationException;
        }

        private TerminologyArtifactProcessingDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var terminologyArtifactProcessingDependencyException =
                new TerminologyArtifactProcessingDependencyException(
                    message: "Terminology artifact processing dependency error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(terminologyArtifactProcessingDependencyException);

            return terminologyArtifactProcessingDependencyException;
        }

        private TerminologyArtifactProcessingServiceException CreateAndLogServiceException(
           Xeption exception)
        {
            var terminologyArtifactProcessingServiceException =
                new TerminologyArtifactProcessingServiceException(
                    message: "Terminology artifact processing service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(terminologyArtifactProcessingServiceException);

            return terminologyArtifactProcessingServiceException;
        }
    }
}
