// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.Mesh.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Mesh
{
    public partial class MeshService
    {
        private delegate ValueTask<bool> ReturningBoolMeshFunction();
        private delegate ValueTask<MeshMessage> ReturningMeshMessageFunction();
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
                    new FailedMeshServiceException(exception);

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
            catch (NullHeadersException nullHeadersException)
            {
                throw CreateAndLogValidationException(nullHeadersException);
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
