// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
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
    }
}
