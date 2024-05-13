// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.Mesh.Exceptions;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Orchestrations.Pds.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationService
    {
        private delegate ValueTask<PdsAudit> ReturningPdsAuditFunction();
        private delegate ValueTask<bool> ReturningPdsBooleanFunction();
        private delegate ValueTask<List<PdsAudit>> ReturningPdsAuditListFunciton();

        private async ValueTask<PdsAudit> TryCatch(ReturningPdsAuditFunction returningPdsAuditFunction)
        {
            try
            {
                return await returningPdsAuditFunction();
            }
            catch (NullConfigPdsOrchestrationException nullConfigPdsOrchestrationException)
            {
                throw CreateAndLogValidationException(nullConfigPdsOrchestrationException);
            }
            catch (NullBlobContainersPdsOrchestrationException nullBlobContainersPdsOrchestrationException)
            {
                throw CreateAndLogValidationException(nullBlobContainersPdsOrchestrationException);
            }
            catch (InvalidArgumentPdsException invalidArgumentPdsException)
            {
                throw CreateAndLogValidationException(invalidArgumentPdsException);
            }
            catch (PdsOrchestrationValidationException pdsOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(pdsOrchestrationValidationException);
            }
            catch (PdsOrchestrationDependencyValidationException pdsOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(pdsOrchestrationDependencyValidationException);
            }
            catch (DocumentValidationException meshValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshValidationException);
            }
            catch (DocumentDependencyValidationException meshDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshDependencyValidationException);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshValidationException);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshDependencyValidationException);
            }
            catch (PdsOrchestrationDependencyException pdsOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(pdsOrchestrationDependencyException);
            }
            catch (PdsOrchestrationServiceException pdsOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(pdsOrchestrationServiceException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw CreateAndLogDependencyException(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw CreateAndLogDependencyException(documentServiceException);
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
                var failedPdsServiceException =
                    new FailedPdsOrchestrationServiceException(
                        message: "Failed PDS orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedPdsServiceException);
            }
        }

        private async ValueTask<bool> TryCatch(ReturningPdsBooleanFunction returningPdsBooleanFunction)
        {
            try
            {
                return await returningPdsBooleanFunction();
            }
            catch (NullConfigPdsOrchestrationException nullConfigPdsOrchestrationException)
            {
                throw CreateAndLogValidationException(nullConfigPdsOrchestrationException);
            }
            catch (NullBlobContainersPdsOrchestrationException nullBlobContainersPdsOrchestrationException)
            {
                throw CreateAndLogValidationException(nullBlobContainersPdsOrchestrationException);
            }
            catch (InvalidArgumentPdsException invalidArgumentPdsException)
            {
                throw CreateAndLogValidationException(invalidArgumentPdsException);
            }
            catch (PdsOrchestrationValidationException pdsOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(pdsOrchestrationValidationException);
            }
            catch (PdsOrchestrationDependencyValidationException pdsOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(pdsOrchestrationDependencyValidationException);
            }
            catch (DocumentValidationException meshValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshValidationException);
            }
            catch (DocumentDependencyValidationException meshDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshDependencyValidationException);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshValidationException);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshDependencyValidationException);
            }
            catch (PdsOrchestrationDependencyException pdsOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(pdsOrchestrationDependencyException);
            }
            catch (PdsOrchestrationServiceException pdsOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(pdsOrchestrationServiceException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw CreateAndLogDependencyException(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw CreateAndLogDependencyException(documentServiceException);
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
                var failedPdsServiceException =
                    new FailedPdsOrchestrationServiceException(
                        message: "Failed PDS orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedPdsServiceException);
            }
        }

        private async ValueTask<List<PdsAudit>> TryCatch(ReturningPdsAuditListFunciton returningPdsAuditListFunciton)
        {
            try
            {
                return await returningPdsAuditListFunciton();
            }
            catch (PdsOrchestrationValidationException pdsOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(pdsOrchestrationValidationException);
            }
            catch (PdsOrchestrationDependencyValidationException pdsOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(pdsOrchestrationDependencyValidationException);
            }
            catch (NullBlobContainersPdsOrchestrationException nullBlobContainersPdsOrchestrationException)
            {
                throw CreateAndLogValidationException(nullBlobContainersPdsOrchestrationException);
            }
            catch (DocumentValidationException meshValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshValidationException);
            }
            catch (DocumentDependencyValidationException meshDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshDependencyValidationException);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshValidationException);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshDependencyValidationException);
            }
            catch (PdsOrchestrationDependencyException pdsOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(pdsOrchestrationDependencyException);
            }
            catch (PdsOrchestrationServiceException pdsOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(pdsOrchestrationServiceException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw CreateAndLogDependencyException(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw CreateAndLogDependencyException(documentServiceException);
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
                var failedPdsServiceException =
                    new FailedPdsOrchestrationServiceException(
                        message: "Failed PDS orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedPdsServiceException);
            }
        }

        private PdsOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var pdsValidationException =
                new PdsOrchestrationValidationException(
                    message: "PDS orchestration validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(pdsValidationException);

            return pdsValidationException;
        }

        private PdsOrchestrationDependencyValidationException
           CreateAndLogDependencyValidationException(Xeption exception)
        {
            var pdsOrchestrationDependencyValidationException =
                new PdsOrchestrationDependencyValidationException(
                    message: "PDS orchestration validation errors occurred, please try again.",
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(pdsOrchestrationDependencyValidationException);

            return pdsOrchestrationDependencyValidationException;
        }

        private PdsOrchestrationDependencyException
           CreateAndLogDependencyException(Xeption exception)
        {
            var pdsOrchestrationDependencyException =
                new PdsOrchestrationDependencyException(
                    message: "PDS orchestration dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(pdsOrchestrationDependencyException);

            throw pdsOrchestrationDependencyException;
        }

        private PdsOrchestrationServiceException
            CreateAndLogServiceException(Xeption exception)
        {
            var pdsOrchestrationServiceException =
                new PdsOrchestrationServiceException(
                    message: "PDS orchestration service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(pdsOrchestrationServiceException);

            throw pdsOrchestrationServiceException;
        }
    }
}
