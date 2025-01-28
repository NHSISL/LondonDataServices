// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Orchestrations.OptOuts.Exceptions;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using LHDS.Core.Models.Processings.Mesh.Exceptions;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using NHSISL.CsvHelperClient.Models.Clients.CsvHelpers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationService
    {
        private delegate ValueTask ReturningNothingFunction();
        private delegate ValueTask<T> ReturningFunction<T>();

        private async ValueTask<T> TryCatch<T>(ReturningFunction<T> returningFunction)
        {
            try
            {
                return await returningFunction();
            }
            catch (NullBlobContainersOptOutOrchestrationException nullBlobContainersOptOutOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullBlobContainersOptOutOrchestrationException);
            }
            catch (NullConfigOptOutOrchestrationException nullConfigOptOutOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullConfigOptOutOrchestrationException);
            }
            catch (InvalidConfigOptOutOrchestrationException invalidConfigOptOutOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidConfigOptOutOrchestrationException);
            }
            catch (InvalidArgumentOptOutOrchestrationException invalidArgumentRetieveOptOutStatusOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentRetieveOptOutStatusOrchestrationException);
            }
            catch (OptOutOrchestrationDependencyValidationException optOutOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(optOutOrchestrationDependencyValidationException);
            }
            catch (OptOutProcessingValidationException csvMapperProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(csvMapperProcessingValidationException);
            }
            catch (OptOutProcessingDependencyValidationException csvMapperProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(csvMapperProcessingDependencyValidationException);
            }
            catch (MeshProcessingValidationException meshProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshProcessingValidationException);
            }
            catch (MeshProcessingDependencyValidationException meshProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshProcessingDependencyValidationException);
            }
            catch (DocumentProcessingValidationException meshProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshProcessingValidationException);
            }
            catch (DocumentProcessingDependencyValidationException meshProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshProcessingDependencyValidationException);
            }
            catch (CsvHelperClientValidationException csvHelperClientValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(csvHelperClientValidationException);
            }
            catch (OptOutOrchestrationDependencyException optOutOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutOrchestrationDependencyException);
            }
            catch (OptOutOrchestrationServiceException optOutOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutOrchestrationServiceException);
            }
            catch (OptOutProcessingDependencyException optOutOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutOrchestrationDependencyException);
            }
            catch (OptOutProcessingServiceException optOutOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutOrchestrationServiceException);
            }
            catch (MeshProcessingDependencyException optOutOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutOrchestrationDependencyException);
            }
            catch (MeshProcessingServiceException optOutOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutOrchestrationServiceException);
            }
            catch (DocumentProcessingDependencyException optOutOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutOrchestrationDependencyException);
            }
            catch (DocumentProcessingServiceException optOutOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutOrchestrationServiceException);
            }
            catch (CsvHelperClientDependencyException csvHelperClientDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(csvHelperClientDependencyException);
            }
            catch (CsvHelperClientServiceException csvHelperClientServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(csvHelperClientServiceException);
            }

            catch (InvalidMeshMessageOrchestrationException invalidMeshMessageOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidMeshMessageOrchestrationException);
            }
            catch (AggregateException aggregateException)
            {
                var failedOptOutOrchestrationServiceException =
                    new FailedOptOutOrchestrationServiceException(
                        message: "Failed opt out aggregate orchestration service error occurred, " +
                            "please contact support.",
                        innerException: aggregateException);

                throw await CreateAndLogServiceExceptionAsync(failedOptOutOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedOptOutServiceException =
                    new FailedOptOutOrchestrationServiceException(
                        message: "Failed opt out orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedOptOutServiceException);
            }
        }

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (NullBlobContainersOptOutOrchestrationException nullBlobContainersOptOutOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullBlobContainersOptOutOrchestrationException);
            }
            catch (NullConfigOptOutOrchestrationException nullConfigOptOutOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullConfigOptOutOrchestrationException);
            }
            catch (InvalidConfigOptOutOrchestrationException invalidConfigOptOutOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidConfigOptOutOrchestrationException);
            }
            catch (InvalidArgumentOptOutOrchestrationException invalidArgumentRetieveOptOutStatusOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentRetieveOptOutStatusOrchestrationException);
            }
            catch (OptOutOrchestrationDependencyValidationException optOutOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(optOutOrchestrationDependencyValidationException);
            }
            catch (OptOutProcessingValidationException csvMapperProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(csvMapperProcessingValidationException);
            }
            catch (OptOutProcessingDependencyValidationException csvMapperProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(csvMapperProcessingDependencyValidationException);
            }
            catch (MeshProcessingValidationException meshProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshProcessingValidationException);
            }
            catch (MeshProcessingDependencyValidationException meshProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshProcessingDependencyValidationException);
            }
            catch (DocumentProcessingValidationException meshProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshProcessingValidationException);
            }
            catch (DocumentProcessingDependencyValidationException meshProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(meshProcessingDependencyValidationException);
            }
            catch (CsvHelperClientValidationException csvHelperClientValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(csvHelperClientValidationException);
            }
            catch (OptOutOrchestrationDependencyException optOutOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutOrchestrationDependencyException);
            }
            catch (OptOutOrchestrationServiceException optOutOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutOrchestrationServiceException);
            }
            catch (OptOutProcessingDependencyException optOutOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutOrchestrationDependencyException);
            }
            catch (OptOutProcessingServiceException optOutOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutOrchestrationServiceException);
            }
            catch (MeshProcessingDependencyException optOutOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutOrchestrationDependencyException);
            }
            catch (MeshProcessingServiceException optOutOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutOrchestrationServiceException);
            }
            catch (DocumentProcessingDependencyException optOutOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutOrchestrationDependencyException);
            }
            catch (DocumentProcessingServiceException optOutOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(optOutOrchestrationServiceException);
            }
            catch (CsvHelperClientDependencyException csvHelperClientDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(csvHelperClientDependencyException);
            }
            catch (CsvHelperClientServiceException csvHelperClientServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(csvHelperClientServiceException);
            }

            catch (InvalidMeshMessageOrchestrationException invalidMeshMessageOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidMeshMessageOrchestrationException);
            }
            catch (Exception exception)
            {
                var failedOptOutServiceException =
                    new FailedOptOutOrchestrationServiceException(
                        message: "Failed opt out orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedOptOutServiceException);
            }
        }

        private async ValueTask<OptOutOrchestrationValidationException> 
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var decryptionOrchestrationValidationException =
                new OptOutOrchestrationValidationException(
                    message: "Opt Out orchestration validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(decryptionOrchestrationValidationException);

            return decryptionOrchestrationValidationException;
        }

        private async ValueTask<OptOutOrchestrationDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var retrieveOptOutStatusOrchestrationDependencyValidationException =
                new OptOutOrchestrationDependencyValidationException(
                    message: "Opt Out orchestration dependency validation errors occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(retrieveOptOutStatusOrchestrationDependencyValidationException);

            return retrieveOptOutStatusOrchestrationDependencyValidationException;
        }

        private async ValueTask<OptOutOrchestrationDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var optOutOrchestrationDependencyException =
                new OptOutOrchestrationDependencyException(
                    message: "Opt Out orchestration dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(optOutOrchestrationDependencyException);

            throw optOutOrchestrationDependencyException;
        }

        private async ValueTask<OptOutOrchestrationServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var optOutOrchestrationServiceException =
                new OptOutOrchestrationServiceException(
                    message: "Opt Out orchestration service error occurred, please contact support.",
                    exception);

            await this.loggingBroker.LogErrorAsync(optOutOrchestrationServiceException);

            throw optOutOrchestrationServiceException;
        }
    }
}
