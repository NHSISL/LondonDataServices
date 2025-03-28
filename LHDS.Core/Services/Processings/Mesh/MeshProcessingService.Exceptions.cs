// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
                throw await CreateAndLogValidationExceptionAsync(exception);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshValidationException);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshDependencyValidationException);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(meshDependencyException);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(meshServiceException);
            }
            catch (Exception exception)
            {
                var failedMeshProcessingServiceException =
                    new FailedMeshProcessingServiceException(
                        message: "Failed mesh processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedMeshProcessingServiceException);
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
                throw await CreateAndLogValidationExceptionAsync(exception);
            }
            catch (InvalidMeshMessageProcessingException invalidMeshMessageProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidMeshMessageProcessingException);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshValidationException);
            }
            catch (NullMeshMessageProcessingException exception)
            {
                throw await CreateAndLogValidationExceptionAsync(exception);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshDependencyValidationException);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(meshDependencyException);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(meshServiceException);
            }
            catch (Exception exception)
            {
                var failedMeshProcessingServiceException =
                    new FailedMeshProcessingServiceException(
                        message: "Failed mesh processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedMeshProcessingServiceException);
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
                throw await CreateAndLogDependencyValidationExceptionAsync(meshValidationException);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshDependencyValidationException);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(meshDependencyException);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(meshServiceException);
            }
            catch (InvalidMeshProcessingArgumentException exception)
            {
                throw await CreateAndLogValidationExceptionAsync(exception);
            }
            catch (Exception exception)
            {
                var failedMeshProcessingServiceException =
                    new FailedMeshProcessingServiceException(
                        message: "Failed mesh processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedMeshProcessingServiceException);
            }
        }

        private async ValueTask<MeshProcessingValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var meshProcessingValidationExceptionn =
                new MeshProcessingValidationException(
                    message: "Mesh processing validation errors occured, please try again",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(meshProcessingValidationExceptionn);

            return meshProcessingValidationExceptionn;
        }

        private async ValueTask<MeshProcessingDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var meshProcessingDependencyValidationException =
                new MeshProcessingDependencyValidationException(
                    message: "Mesh processing dependency validation occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(meshProcessingDependencyValidationException);

            return meshProcessingDependencyValidationException;
        }

        private async ValueTask<MeshProcessingDependencyException>
           CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var meshProcessingDependencyException =
                new MeshProcessingDependencyException(
                    message: "Mesh processing dependency error occurred, please contact support.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(meshProcessingDependencyException);

            throw meshProcessingDependencyException;
        }

        private async ValueTask<MeshProcessingServiceException> CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var meshProcessingServiceException = new
                MeshProcessingServiceException(
                message: "Mesh processing service error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(meshProcessingServiceException);

            return meshProcessingServiceException;
        }
    }

}

