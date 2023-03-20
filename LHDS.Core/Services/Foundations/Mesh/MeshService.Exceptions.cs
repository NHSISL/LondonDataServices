// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Mesh
{
    public partial class MeshService
    {
        private delegate ValueTask<bool> ReturningBoolMeshFunction();
        private delegate ValueTask<string> ReturningStringMeshFunction();
        private delegate ValueTask<List<string>> ReturningStringsMeshFunction();

        private async ValueTask<bool> TryCatch(ReturningBoolMeshFunction returningMeshFunction)
        {
            try
            {
                return await returningMeshFunction();
            }
            catch (InvalidArgumentMeshException invalidArgumentMeshException)
            {
                throw CreateAndLogValidationException(invalidArgumentMeshException);
            }
            catch (Exception exception)
            {
                var failedMeshServiceException =
                    new FailedMeshServiceException(exception);

                throw CreateAndLogServiceException(failedMeshServiceException);
            }
        }

        private async ValueTask<string> TryCatch(ReturningStringMeshFunction returningStringMeshFunction)
        {
            try
            {
                return await returningStringMeshFunction();
            }
            catch (InvalidArgumentMeshException invalidArgumentMeshException)
            {
                throw CreateAndLogValidationException(invalidArgumentMeshException);
            }
            catch (Exception exception)
            {
                var failedMeshServiceException =
                    new FailedMeshServiceException(exception);

                throw CreateAndLogServiceException(failedMeshServiceException);
            }
        }

        private async ValueTask<List<string>> TryCatch(ReturningStringsMeshFunction returningStringsMeshFunction)
        {
            try
            {
                return await returningStringsMeshFunction();
            }
            catch (InvalidArgumentMeshException invalidArgumentMeshException)
            {
                throw CreateAndLogValidationException(invalidArgumentMeshException);
            }
            catch (Exception exception)
            {
                var failedMeshServiceException =
                    new FailedMeshServiceException(exception);

                throw CreateAndLogServiceException(failedMeshServiceException);
            }
        }

        private MeshValidationException CreateAndLogValidationException(Xeption exception)
        {
            string validationSummary = GetValidationSummary(exception.Data);

            var meshValidationException =
                new MeshValidationException(exception, validationSummary);

            this.loggingBroker.LogError(meshValidationException);

            return meshValidationException;
        }

        private MeshServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var meshServiceException = new MeshServiceException(exception);
            this.loggingBroker.LogError(meshServiceException);

            return meshServiceException;
        }
    }
}
