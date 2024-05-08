// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
                throw CreateAndLogValidationException(invalidArgumentMeshException);
            }
            catch (Exception exception)
            {
                var failedMeshServiceException =
                    new FailedMeshServiceException(
                        message: "Failed mesh service error occurred, please contact support.", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedMeshServiceException);
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
                throw CreateAndLogValidationException(nullMeshMessageException);
            }
            catch (InvalidMeshMessageException invalidMeshException)
            {
                throw CreateAndLogValidationException(invalidMeshException);
            }
            catch (InvalidArgumentMeshException invalidArgumentMeshException)
            {
                throw CreateAndLogValidationException(invalidArgumentMeshException);
            }
            catch (MeshClientValidationException meshClientValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshClientValidationException);
            }
            catch (MeshClientDependencyException meshClientDependencyException)
            {
                throw CreateAndLogDependencyException(meshClientDependencyException);
            }
            catch (MeshServiceDependencyValidationException invalidMeshException)
            {
                throw CreateAndLogDependencyException(invalidMeshException);
            }
            catch (MeshClientServiceException meshClientServiceException)
            {
                throw CreateAndLogDependencyException(meshClientServiceException);
            }
            catch (Exception exception)
            {
                var failedMeshServiceException =
                    new FailedMeshServiceException(
                        message: "Failed mesh service error occurred, please contact support.", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedMeshServiceException);
            }
        }

        private async ValueTask<List<string>> TryCatch(ReturningListofStringsMeshFunction returningListofStringsMeshFunction)
        {
            try
            {
                return await returningListofStringsMeshFunction();
            }
            catch (MeshClientValidationException meshClientValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshClientValidationException);
            }
            catch (MeshClientDependencyException meshClientDependencyException)
            {
                throw CreateAndLogDependencyException(meshClientDependencyException);
            }
            catch (Exception exception)
            {
                var failedMeshServiceException =
                    new FailedMeshServiceException(
                        message: "Failed mesh service error occurred, please contact support.", 
                        innerException: exception);

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
                    new FailedMeshServiceException(
                        message: "Failed mesh service error occurred, please contact support.", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedMeshServiceException);
            }
        }

        private MeshValidationException CreateAndLogValidationException(Xeption exception)
        {
            var meshValidationException = new MeshValidationException(
                message: "Mesh validation errors occurred, please try again.",
                innerException: exception);
            this.loggingBroker.LogError(meshValidationException);

            return meshValidationException;
        }

        private MeshServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var meshServiceException = new MeshServiceException(
                message: "Mesh service error occurred, please contact support.", 
                innerException: exception);

            this.loggingBroker.LogError(meshServiceException);

            return meshServiceException;
        }

        private MeshServiceDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var meshServiceDependencyException = new MeshServiceDependencyException(
                message: "Mesh service dependency error occurred, please contact support.", 
                innerException: exception);

            this.loggingBroker.LogError(meshServiceDependencyException);

            return meshServiceDependencyException;
        }

        private MeshServiceDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var meshServiceDependencyValidationException =
                new MeshServiceDependencyValidationException(
                    message: "Mesh service dependency validation occurred, please try again.", 
                    innerException: exception);

            this.loggingBroker.LogError(meshServiceDependencyValidationException);

            return meshServiceDependencyValidationException;
        }
    }
}
