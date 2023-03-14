// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.MeshItems.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Mesh
{
    public partial class MeshService
    {
        private delegate ValueTask<bool> ReturningMeshFunction();

        private async ValueTask<bool> TryCatch(ReturningMeshFunction returningMeshFunction)
        {
            try
            {
                return await returningMeshFunction();
            }
            catch (Exception exception)
            {
                var failedMeshServiceException =
                    new FailedMeshServiceException(exception);

                throw CreateAndLogServiceException(failedMeshServiceException);
            }
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
