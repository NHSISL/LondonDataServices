// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.Mesh.Exceptions;
using LHDS.Core.Models.Processings.Mesh.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.Mesh
{
    public partial class MeshProcessingService
    {
        private delegate ValueTask<bool> ReturningBoolMeshFunction();
        private delegate ValueTask<MeshMessage> ReturningMessageMeshFunction();
        private delegate ValueTask<List<string>> ReturningStringsMeshFunction();

        private async ValueTask<bool> TryCatch(ReturningBoolMeshFunction returningBoolMeshFunction)
        {
            try
            {
                return await returningBoolMeshFunction();
            }
            catch (InvalidMeshProcessingArgumentException exception)
            {
                throw CreateAndLogValidationException(exception);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshValidationException);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshDependencyValidationException);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw CreateAndLogDependencyException(meshDependencyException);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw CreateAndLogDependencyException(meshServiceException);
            }
            catch (Exception exception)
            {
                var failedMeshProcessingServiceException =
                    new FailedMeshProcessingServiceException(
                        message: "Failed mesh processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedMeshProcessingServiceException);
            }
        }

        private async ValueTask<MeshMessage> TryCatch(ReturningMessageMeshFunction returningMessageMeshFunction)
        {
            try
            {
                return await returningMessageMeshFunction();
            }
            catch (InvalidMeshProcessingArgumentException exception)
            {
                throw CreateAndLogValidationException(exception);
            }
            catch (InvalidMeshMessageProcessingException invalidMeshMessageProcessingException)
            {
                throw CreateAndLogValidationException(invalidMeshMessageProcessingException);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshValidationException);
            }
            catch (NullMeshMessageProcessingException exception)
            {
                throw CreateAndLogValidationException(exception);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshDependencyValidationException);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw CreateAndLogDependencyException(meshDependencyException);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw CreateAndLogDependencyException(meshServiceException);
            }
            catch (Exception exception)
            {
                var failedMeshProcessingServiceException =
                    new FailedMeshProcessingServiceException(
                        message: "Failed mesh processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedMeshProcessingServiceException);
            }
        }

        private async ValueTask<List<string>> TryCatch(ReturningStringsMeshFunction returningStringsMeshFunction)
        {
            try
            {
                return await returningStringsMeshFunction();
            }
            catch (MeshValidationException meshValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshValidationException);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshDependencyValidationException);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw CreateAndLogDependencyException(meshDependencyException);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw CreateAndLogDependencyException(meshServiceException);
            }
            catch (InvalidMeshProcessingArgumentException exception)
            {
                throw CreateAndLogValidationException(exception);
            }
            catch (Exception exception)
            {
                var failedMeshProcessingServiceException =
                    new FailedMeshProcessingServiceException(
                        message: "Failed mesh processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedMeshProcessingServiceException);
            }
        }

        private MeshProcessingValidationException
            CreateAndLogValidationException(Xeption exception)
        {
            var meshProcessingValidationExceptionn =
                new MeshProcessingValidationException(
                    message: "Mesh processing validation errors occured, please try again",
                    innerException: exception);

            this.loggingBroker.LogError(meshProcessingValidationExceptionn);

            return meshProcessingValidationExceptionn;
        }

        private MeshProcessingDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var meshProcessingDependencyValidationException =
                new MeshProcessingDependencyValidationException(
                    message: "Mesh processing dependency validation occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(meshProcessingDependencyValidationException);

            return meshProcessingDependencyValidationException;
        }

        private MeshProcessingDependencyException
           CreateAndLogDependencyException(Xeption exception)
        {
            var meshProcessingDependencyException =
                new MeshProcessingDependencyException(
                    message: "Mesh processing dependency error occurred, please contact support.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(meshProcessingDependencyException);

            throw meshProcessingDependencyException;
        }

        private MeshProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var meshProcessingServiceException = new
                MeshProcessingServiceException(
                message: "Mesh processing service error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogError(meshProcessingServiceException);

            return meshProcessingServiceException;
        }
    }

}

