// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
                throw await CreateAndLogValidationException(nullConfigPdsOrchestrationException);
            }
            catch (NullBlobContainersPdsOrchestrationException nullBlobContainersPdsOrchestrationException)
            {
                throw await CreateAndLogValidationException(nullBlobContainersPdsOrchestrationException);
            }
            catch (InvalidArgumentPdsException invalidArgumentPdsException)
            {
                throw await CreateAndLogValidationException(invalidArgumentPdsException);
            }
            catch (PdsOrchestrationValidationException pdsOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationException(pdsOrchestrationValidationException);
            }
            catch (PdsOrchestrationDependencyValidationException pdsOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationException(pdsOrchestrationDependencyValidationException);
            }
            catch (DocumentValidationException meshValidationException)
            {
                throw await CreateAndLogDependencyValidationException(meshValidationException);
            }
            catch (DocumentDependencyValidationException meshDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationException(meshDependencyValidationException);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw await CreateAndLogDependencyValidationException(meshValidationException);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationException(meshDependencyValidationException);
            }
            catch (PdsOrchestrationDependencyException pdsOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyException(pdsOrchestrationDependencyException);
            }
            catch (PdsOrchestrationServiceException pdsOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyException(pdsOrchestrationServiceException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw await CreateAndLogDependencyException(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw await CreateAndLogDependencyException(documentServiceException);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw await CreateAndLogDependencyException(meshDependencyException);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw await CreateAndLogDependencyException(meshServiceException);
            }
            catch (Exception exception)
            {
                var failedPdsServiceException =
                    new FailedPdsOrchestrationServiceException(
                        message: "Failed PDS orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceException(failedPdsServiceException);
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
                throw await CreateAndLogValidationException(nullConfigPdsOrchestrationException);
            }
            catch (NullBlobContainersPdsOrchestrationException nullBlobContainersPdsOrchestrationException)
            {
                throw await CreateAndLogValidationException(nullBlobContainersPdsOrchestrationException);
            }
            catch (InvalidArgumentPdsException invalidArgumentPdsException)
            {
                throw await CreateAndLogValidationException(invalidArgumentPdsException);
            }
            catch (PdsOrchestrationValidationException pdsOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationException(pdsOrchestrationValidationException);
            }
            catch (PdsOrchestrationDependencyValidationException pdsOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationException(pdsOrchestrationDependencyValidationException);
            }
            catch (DocumentValidationException meshValidationException)
            {
                throw await CreateAndLogDependencyValidationException(meshValidationException);
            }
            catch (DocumentDependencyValidationException meshDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationException(meshDependencyValidationException);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw await CreateAndLogDependencyValidationException(meshValidationException);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationException(meshDependencyValidationException);
            }
            catch (PdsOrchestrationDependencyException pdsOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyException(pdsOrchestrationDependencyException);
            }
            catch (PdsOrchestrationServiceException pdsOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyException(pdsOrchestrationServiceException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw await CreateAndLogDependencyException(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw await CreateAndLogDependencyException(documentServiceException);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw await CreateAndLogDependencyException(meshDependencyException);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw await CreateAndLogDependencyException(meshServiceException);
            }
            catch (Exception exception)
            {
                var failedPdsServiceException =
                    new FailedPdsOrchestrationServiceException(
                        message: "Failed PDS orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceException(failedPdsServiceException);
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
                throw await CreateAndLogDependencyValidationException(pdsOrchestrationValidationException);
            }
            catch (PdsOrchestrationDependencyValidationException pdsOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationException(pdsOrchestrationDependencyValidationException);
            }
            catch (NullBlobContainersPdsOrchestrationException nullBlobContainersPdsOrchestrationException)
            {
                throw await CreateAndLogValidationException(nullBlobContainersPdsOrchestrationException);
            }
            catch (DocumentValidationException meshValidationException)
            {
                throw await CreateAndLogDependencyValidationException(meshValidationException);
            }
            catch (DocumentDependencyValidationException meshDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationException(meshDependencyValidationException);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw await CreateAndLogDependencyValidationException(meshValidationException);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationException(meshDependencyValidationException);
            }
            catch (PdsOrchestrationDependencyException pdsOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyException(pdsOrchestrationDependencyException);
            }
            catch (PdsOrchestrationServiceException pdsOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyException(pdsOrchestrationServiceException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw await CreateAndLogDependencyException(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw await CreateAndLogDependencyException(documentServiceException);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw await CreateAndLogDependencyException(meshDependencyException);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw await CreateAndLogDependencyException(meshServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedPdsOrchestrationServiceException =
                    new FailedPdsOrchestrationServiceException(
                        message: "Failed PDS aggregate orchestration service error occurred, " +
                            "please contact support.",
                        innerException: aggregateException);

                throw await CreateAndLogServiceException(failedPdsOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedPdsServiceException =
                    new FailedPdsOrchestrationServiceException(
                        message: "Failed PDS orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceException(failedPdsServiceException);
            }
        }

        private async ValueTask<PdsOrchestrationValidationException> CreateAndLogValidationException(Xeption exception)
        {
            var pdsValidationException =
                new PdsOrchestrationValidationException(
                    message: "PDS orchestration validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(pdsValidationException);

            return pdsValidationException;
        }

        private async ValueTask<PdsOrchestrationDependencyValidationException>
           CreateAndLogDependencyValidationException(Xeption exception)
        {
            var pdsOrchestrationDependencyValidationException =
                new PdsOrchestrationDependencyValidationException(
                    message: "PDS orchestration dependency validation errors occurred, fix the errors and try again.",
                    exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(pdsOrchestrationDependencyValidationException);

            return pdsOrchestrationDependencyValidationException;
        }

        private async ValueTask<PdsOrchestrationDependencyException>
           CreateAndLogDependencyException(Xeption exception)
        {
            var pdsOrchestrationDependencyException =
                new PdsOrchestrationDependencyException(
                    message: "PDS orchestration dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(pdsOrchestrationDependencyException);

            throw pdsOrchestrationDependencyException;
        }

        private async ValueTask<PdsOrchestrationServiceException>
            CreateAndLogServiceException(Xeption exception)
        {
            var pdsOrchestrationServiceException =
                new PdsOrchestrationServiceException(
                    message: "PDS orchestration service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(pdsOrchestrationServiceException);

            throw pdsOrchestrationServiceException;
        }
    }
}
