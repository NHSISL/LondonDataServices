// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
using LHDS.Core.Models.Processings.TerminologyArtifact.Exceptions;
using LHDS.Core.Models.Processings.TerminologyArtifacts.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.TerminologyArtifacts
{
    public partial class TerminologyArtifactProcessingService
    {
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
                        message: "Failed terminology artifact processing service error occurred, contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedTerminologyArtifactProcessingServiceException);
            }
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
                    message: "Terminology artifact processing service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(terminologyArtifactProcessingServiceException);

            return terminologyArtifactProcessingServiceException;
        }
    }
}
