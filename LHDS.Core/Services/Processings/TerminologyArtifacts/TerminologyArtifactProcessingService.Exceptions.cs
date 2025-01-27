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
        private delegate ValueTask<TerminologyArtifact> ReturningTerminologyArtifactProcessingFunction();
        private delegate ValueTask<IQueryable<TerminologyArtifact>> ReturningTerminologyArtifactsProcessingFunction();

        private async ValueTask<IQueryable<TerminologyArtifact>> TryCatch(
            ReturningTerminologyArtifactsProcessingFunction returningTerminologyArtifactsProcessingFunction)
        {
            try
            {
                return await returningTerminologyArtifactsProcessingFunction();
            }
            catch (TerminologyArtifactValidationException terminologyArtifactValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(terminologyArtifactValidationException);
            }
            catch (TerminologyArtifactDependencyValidationException terminologyArtifactDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    terminologyArtifactDependencyValidationException);
            }
            catch (TerminologyArtifactDependencyException terminologyArtifactDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(terminologyArtifactDependencyException);
            }
            catch (TerminologyArtifactServiceException terminologyArtifactServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(terminologyArtifactServiceException);
            }
            catch (Exception exception)
            {
                var failedTerminologyArtifactProcessingServiceException =
                    new FailedTerminologyArtifactProcessingServiceException(
                        message: "Failed terminology artifact processing service error occurred, " +
                            "please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedTerminologyArtifactProcessingServiceException);
            }
        }

        private async ValueTask<TerminologyArtifact> TryCatch(
            ReturningTerminologyArtifactProcessingFunction returningTerminologyArtifactProcessingFunction)
        {
            try
            {
                return await returningTerminologyArtifactProcessingFunction();
            }
            catch (NullTerminologyArtifactProcessingException nullTerminologyArtifactException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullTerminologyArtifactException);
            }
            catch (InvalidArgumentTerminologyArtifactProcessingException
                invalidArgumentTerminologyArtifactProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(
                    invalidArgumentTerminologyArtifactProcessingException);
            }
            catch (TerminologyArtifactValidationException terminologyArtifactValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(terminologyArtifactValidationException);
            }
            catch (TerminologyArtifactDependencyValidationException terminologyArtifactDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    terminologyArtifactDependencyValidationException);
            }
            catch (TerminologyArtifactDependencyException terminologyArtifactDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(terminologyArtifactDependencyException);
            }
            catch (TerminologyArtifactServiceException terminologyArtifactServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(terminologyArtifactServiceException);
            }
            catch (Exception exception)
            {
                var failedTerminologyArtifactProcessingServiceException =
                    new FailedTerminologyArtifactProcessingServiceException(
                        message: "Failed terminology artifact processing service error occurred, " +
                            "please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedTerminologyArtifactProcessingServiceException);
            }
        }

        private async ValueTask<TerminologyArtifactProcessingValidationException> CreateAndLogValidationExceptionAsync(
            Xeption exception)
        {
            var terminologyArtifactProcessingValidationExceptionn =
                new TerminologyArtifactProcessingValidationException(
                    message: "Terminology artifact processing validation error occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(terminologyArtifactProcessingValidationExceptionn);

            return terminologyArtifactProcessingValidationExceptionn;
        }

        private async ValueTask<TerminologyArtifactProcessingDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var terminologyArtifactProcessingDependencyValidationException =
                new TerminologyArtifactProcessingDependencyValidationException(
                    message: "Terminology artifact processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(terminologyArtifactProcessingDependencyValidationException);

            return terminologyArtifactProcessingDependencyValidationException;
        }

        private async ValueTask<TerminologyArtifactProcessingDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var terminologyArtifactProcessingDependencyException =
                new TerminologyArtifactProcessingDependencyException(
                    message: "Terminology artifact processing dependency error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(terminologyArtifactProcessingDependencyException);

            return terminologyArtifactProcessingDependencyException;
        }

        private async ValueTask<TerminologyArtifactProcessingServiceException> CreateAndLogServiceExceptionAsync(
           Xeption exception)
        {
            var terminologyArtifactProcessingServiceException =
                new TerminologyArtifactProcessingServiceException(
                    message: "Terminology artifact processing service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(terminologyArtifactProcessingServiceException);

            return terminologyArtifactProcessingServiceException;
        }
    }
}
