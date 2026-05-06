// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.Mesh.Exceptions;
using NEL.MESH.Models.Clients.Mesh.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Mesh
{
    public partial class MeshService
    {
        private delegate ValueTask<bool> ReturningBoolMeshFunction();
        private delegate ValueTask<MeshMessage> ReturningMeshMessageFunction();
        private delegate ValueTask<List<string>> ReturningListofStringsMeshFunction();
        private delegate ValueTask<string> ReturningStringMeshFunction();

        private async ValueTask<bool> TryCatch(ReturningBoolMeshFunction returningMeshFunction)
        {
            try
            {
                return await returningMeshFunction();
            }
            catch (InvalidArgumentMeshException invalidArgumentMeshException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentMeshException);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception exception)
            {
                var failedMeshServiceException =
                    new FailedMeshServiceException(
                        message: "Failed mesh service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedMeshServiceException);
            }
        }

        private async ValueTask<MeshMessage> TryCatch(ReturningMeshMessageFunction returningMeshMessageFunction)
        {
            try
            {
                return await returningMeshMessageFunction();
            }
            catch (NullMeshMessageException nullMeshMessageException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullMeshMessageException);
            }
            catch (InvalidMeshMessageException invalidMeshException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidMeshException);
            }
            catch (InvalidArgumentMeshException invalidArgumentMeshException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentMeshException);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (MeshClientValidationException meshClientValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshClientValidationException);
            }
            catch (MeshClientDependencyException meshClientDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(meshClientDependencyException);
            }
            catch (MeshServiceDependencyValidationException invalidMeshException)
            {
                throw await CreateAndLogDependencyExceptionAsync(invalidMeshException);
            }
            catch (MeshClientServiceException meshClientServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(meshClientServiceException);
            }
            catch (Exception exception)
            {
                var failedMeshServiceException =
                    new FailedMeshServiceException(
                        message: "Failed mesh service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedMeshServiceException);
            }
        }

        private async ValueTask<List<string>> TryCatch(ReturningListofStringsMeshFunction returningListofStringsMeshFunction)
        {
            try
            {
                return await returningListofStringsMeshFunction();
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (MeshClientValidationException meshClientValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshClientValidationException);
            }
            catch (MeshClientDependencyException meshClientDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(meshClientDependencyException);
            }
            catch (Exception exception)
            {
                var failedMeshServiceException =
                    new FailedMeshServiceException(
                        message: "Failed mesh service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedMeshServiceException);
            }
        }

        private async ValueTask<string> TryCatch(ReturningStringMeshFunction returningStringMeshFunction)
        {
            try
            {
                return await returningStringMeshFunction();
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (InvalidArgumentMeshException invalidArgumentMeshException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentMeshException);
            }
            catch (Exception exception)
            {
                var failedMeshServiceException =
                    new FailedMeshServiceException(
                        message: "Failed mesh service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedMeshServiceException);
            }
        }

        private async ValueTask<MeshValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var meshValidationException = new MeshValidationException(
                message: "Mesh validation errors occurred, please try again.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(meshValidationException);

            return meshValidationException;
        }

        private async ValueTask<MeshServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var meshServiceException = new MeshServiceException(
                message: "Mesh service error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(meshServiceException);

            return meshServiceException;
        }

        private async ValueTask<MeshServiceDependencyException> CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var meshServiceDependencyException = new MeshServiceDependencyException(
                message: "Mesh service dependency error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(meshServiceDependencyException);

            return meshServiceDependencyException;
        }

        private async ValueTask<MeshServiceDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var meshServiceDependencyValidationException =
                new MeshServiceDependencyValidationException(
                    message: "Mesh service dependency validation occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(meshServiceDependencyValidationException);

            return meshServiceDependencyValidationException;
        }
    }
}
