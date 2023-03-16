// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.MeshItems.Exceptions;
using LHDS.Core.Models.Processings.Mesh.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.Mesh
{
    public partial class MeshProcessingService
    {
        private delegate ValueTask<bool> ReturningBoolMeshFunction();

        private async ValueTask<bool> TryCatch(ReturningBoolMeshFunction returningMeshFunction)
        {
            try
            {
                return await returningMeshFunction();
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
                    new FailedMeshProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedMeshProcessingServiceException);
            }
        }

        private MeshProcessingDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var meshProcessingDependencyValidationException =
                new MeshProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(meshProcessingDependencyValidationException);

            return meshProcessingDependencyValidationException;
        }

        private MeshProcessingDependencyException
           CreateAndLogDependencyException(Xeption exception)
        {
            var meshProcessingDependencyException =
                new MeshProcessingDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(meshProcessingDependencyException);

            throw meshProcessingDependencyException;
        }

        private MeshProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var meshProcessingServiceException = new
                MeshProcessingServiceException(exception);

            this.loggingBroker.LogError(meshProcessingServiceException);

            return meshProcessingServiceException;
        }
    }

}

