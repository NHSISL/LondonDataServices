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
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (NullConfigPdsOrchestrationException nullConfigPdsOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullConfigPdsOrchestrationException);
            }
            catch (NullBlobContainersPdsOrchestrationException nullBlobContainersPdsOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullBlobContainersPdsOrchestrationException);
            }
            catch (InvalidArgumentPdsException invalidArgumentPdsException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentPdsException);
            }
            catch (PdsOrchestrationValidationException pdsOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(pdsOrchestrationValidationException);
            }
            catch (PdsOrchestrationDependencyValidationException pdsOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(pdsOrchestrationDependencyValidationException);
            }
            catch (DocumentValidationException meshValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshValidationException);
            }
            catch (DocumentDependencyValidationException meshDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshDependencyValidationException);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshValidationException);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshDependencyValidationException);
            }
            catch (PdsOrchestrationDependencyException pdsOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(pdsOrchestrationDependencyException);
            }
            catch (PdsOrchestrationServiceException pdsOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(pdsOrchestrationServiceException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentServiceException);
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
                var failedPdsServiceException =
                    new FailedPdsOrchestrationServiceException(
                        message: "Failed PDS orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedPdsServiceException);
            }
        }

        private async ValueTask<bool> TryCatch(ReturningPdsBooleanFunction returningPdsBooleanFunction)
        {
            try
            {
                return await returningPdsBooleanFunction();
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (NullConfigPdsOrchestrationException nullConfigPdsOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullConfigPdsOrchestrationException);
            }
            catch (NullBlobContainersPdsOrchestrationException nullBlobContainersPdsOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullBlobContainersPdsOrchestrationException);
            }
            catch (InvalidArgumentPdsException invalidArgumentPdsException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentPdsException);
            }
            catch (PdsOrchestrationValidationException pdsOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(pdsOrchestrationValidationException);
            }
            catch (PdsOrchestrationDependencyValidationException pdsOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    pdsOrchestrationDependencyValidationException);
            }
            catch (DocumentValidationException meshValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshValidationException);
            }
            catch (DocumentDependencyValidationException meshDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshDependencyValidationException);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshValidationException);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshDependencyValidationException);
            }
            catch (PdsOrchestrationDependencyException pdsOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(pdsOrchestrationDependencyException);
            }
            catch (PdsOrchestrationServiceException pdsOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(pdsOrchestrationServiceException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentServiceException);
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
                var failedPdsServiceException =
                    new FailedPdsOrchestrationServiceException(
                        message: "Failed PDS orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedPdsServiceException);
            }
        }

        private async ValueTask<List<PdsAudit>> TryCatch(ReturningPdsAuditListFunciton returningPdsAuditListFunciton)
        {
            try
            {
                return await returningPdsAuditListFunciton();
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (PdsOrchestrationValidationException pdsOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(pdsOrchestrationValidationException);
            }
            catch (PdsOrchestrationDependencyValidationException pdsOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(pdsOrchestrationDependencyValidationException);
            }
            catch (NullBlobContainersPdsOrchestrationException nullBlobContainersPdsOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullBlobContainersPdsOrchestrationException);
            }
            catch (DocumentValidationException meshValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshValidationException);
            }
            catch (DocumentDependencyValidationException meshDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshDependencyValidationException);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshValidationException);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshDependencyValidationException);
            }
            catch (PdsOrchestrationDependencyException pdsOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(pdsOrchestrationDependencyException);
            }
            catch (PdsOrchestrationServiceException pdsOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(pdsOrchestrationServiceException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentServiceException);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(meshDependencyException);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(meshServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedPdsOrchestrationServiceException =
                    new FailedPdsOrchestrationServiceException(
                        message: "Failed PDS aggregate orchestration service error occurred, " +
                            "please contact support.",
                        innerException: aggregateException);

                throw await CreateAndLogServiceExceptionAsync(failedPdsOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedPdsServiceException =
                    new FailedPdsOrchestrationServiceException(
                        message: "Failed PDS orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedPdsServiceException);
            }
        }

        private async ValueTask<PdsOrchestrationValidationException> 
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var pdsValidationException =
                new PdsOrchestrationValidationException(
                    message: "PDS orchestration validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(pdsValidationException);

            return pdsValidationException;
        }

        private async ValueTask<PdsOrchestrationDependencyValidationException>
           CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var pdsOrchestrationDependencyValidationException =
                new PdsOrchestrationDependencyValidationException(
                    message: "PDS orchestration dependency validation errors occurred, fix the errors and try again.",
                    exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(pdsOrchestrationDependencyValidationException);

            return pdsOrchestrationDependencyValidationException;
        }

        private async ValueTask<PdsOrchestrationDependencyException>
           CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var pdsOrchestrationDependencyException =
                new PdsOrchestrationDependencyException(
                    message: "PDS orchestration dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(pdsOrchestrationDependencyException);

            throw pdsOrchestrationDependencyException;
        }

        private async ValueTask<PdsOrchestrationServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
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
