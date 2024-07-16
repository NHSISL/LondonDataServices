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
                throw CreateAndLogValidationException(nullBlobContainersOptOutOrchestrationException);
            }
            catch (NullConfigOptOutOrchestrationException nullConfigOptOutOrchestrationException)
            {
                throw CreateAndLogValidationException(nullConfigOptOutOrchestrationException);
            }
            catch (InvalidConfigOptOutOrchestrationException invalidConfigOptOutOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidConfigOptOutOrchestrationException);
            }
            catch (InvalidArgumentOptOutOrchestrationException invalidArgumentRetieveOptOutStatusOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentRetieveOptOutStatusOrchestrationException);
            }
            catch (OptOutOrchestrationDependencyValidationException optOutOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(optOutOrchestrationDependencyValidationException);
            }
            catch (OptOutProcessingValidationException csvMapperProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(csvMapperProcessingValidationException);
            }
            catch (OptOutProcessingDependencyValidationException csvMapperProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(csvMapperProcessingDependencyValidationException);
            }
            catch (MeshProcessingValidationException meshProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshProcessingValidationException);
            }
            catch (MeshProcessingDependencyValidationException meshProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshProcessingDependencyValidationException);
            }
            catch (DocumentProcessingValidationException meshProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshProcessingValidationException);
            }
            catch (DocumentProcessingDependencyValidationException meshProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshProcessingDependencyValidationException);
            }
            catch (CsvHelperClientValidationException csvHelperClientValidationException)
            {
                throw CreateAndLogDependencyValidationException(csvHelperClientValidationException);
            }
            catch (OptOutOrchestrationDependencyException optOutOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationDependencyException);
            }
            catch (OptOutOrchestrationServiceException optOutOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationServiceException);
            }
            catch (OptOutProcessingDependencyException optOutOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationDependencyException);
            }
            catch (OptOutProcessingServiceException optOutOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationServiceException);
            }
            catch (MeshProcessingDependencyException optOutOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationDependencyException);
            }
            catch (MeshProcessingServiceException optOutOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationServiceException);
            }
            catch (DocumentProcessingDependencyException optOutOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationDependencyException);
            }
            catch (DocumentProcessingServiceException optOutOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationServiceException);
            }
            catch (CsvHelperClientDependencyException csvHelperClientDependencyException)
            {
                throw CreateAndLogDependencyException(csvHelperClientDependencyException);
            }
            catch (CsvHelperClientServiceException csvHelperClientServiceException)
            {
                throw CreateAndLogDependencyException(csvHelperClientServiceException);
            }

            catch (InvalidMeshMessageOrchestrationException invalidMeshMessageOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidMeshMessageOrchestrationException);
            }
            catch (AggregateException aggregateException)
            {
                var failedOptOutOrchestrationServiceException =
                    new FailedOptOutOrchestrationServiceException(
                        message: "Failed opt out aggregate orchestration service error occurred, " +
                            "please contact support.",
                        innerException: aggregateException);

                throw CreateAndLogServiceException(failedOptOutOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedOptOutServiceException =
                    new FailedOptOutOrchestrationServiceException(
                        message: "Failed opt out orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedOptOutServiceException);
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
                throw CreateAndLogValidationException(nullBlobContainersOptOutOrchestrationException);
            }
            catch (NullConfigOptOutOrchestrationException nullConfigOptOutOrchestrationException)
            {
                throw CreateAndLogValidationException(nullConfigOptOutOrchestrationException);
            }
            catch (InvalidConfigOptOutOrchestrationException invalidConfigOptOutOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidConfigOptOutOrchestrationException);
            }
            catch (InvalidArgumentOptOutOrchestrationException invalidArgumentRetieveOptOutStatusOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentRetieveOptOutStatusOrchestrationException);
            }
            catch (OptOutOrchestrationDependencyValidationException optOutOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(optOutOrchestrationDependencyValidationException);
            }
            catch (OptOutProcessingValidationException csvMapperProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(csvMapperProcessingValidationException);
            }
            catch (OptOutProcessingDependencyValidationException csvMapperProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(csvMapperProcessingDependencyValidationException);
            }
            catch (MeshProcessingValidationException meshProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshProcessingValidationException);
            }
            catch (MeshProcessingDependencyValidationException meshProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshProcessingDependencyValidationException);
            }
            catch (DocumentProcessingValidationException meshProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshProcessingValidationException);
            }
            catch (DocumentProcessingDependencyValidationException meshProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshProcessingDependencyValidationException);
            }
            catch (CsvHelperClientValidationException csvHelperClientValidationException)
            {
                throw CreateAndLogDependencyValidationException(csvHelperClientValidationException);
            }
            catch (OptOutOrchestrationDependencyException optOutOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationDependencyException);
            }
            catch (OptOutOrchestrationServiceException optOutOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationServiceException);
            }
            catch (OptOutProcessingDependencyException optOutOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationDependencyException);
            }
            catch (OptOutProcessingServiceException optOutOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationServiceException);
            }
            catch (MeshProcessingDependencyException optOutOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationDependencyException);
            }
            catch (MeshProcessingServiceException optOutOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationServiceException);
            }
            catch (DocumentProcessingDependencyException optOutOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationDependencyException);
            }
            catch (DocumentProcessingServiceException optOutOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationServiceException);
            }
            catch (CsvHelperClientDependencyException csvHelperClientDependencyException)
            {
                throw CreateAndLogDependencyException(csvHelperClientDependencyException);
            }
            catch (CsvHelperClientServiceException csvHelperClientServiceException)
            {
                throw CreateAndLogDependencyException(csvHelperClientServiceException);
            }

            catch (InvalidMeshMessageOrchestrationException invalidMeshMessageOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidMeshMessageOrchestrationException);
            }
            catch (Exception exception)
            {
                var failedOptOutServiceException =
                    new FailedOptOutOrchestrationServiceException(
                        message: "Failed opt out orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedOptOutServiceException);
            }
        }

        private OptOutOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var decryptionOrchestrationValidationException =
                new OptOutOrchestrationValidationException(
                    message: "Opt Out orchestration validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(decryptionOrchestrationValidationException);

            return decryptionOrchestrationValidationException;
        }

        private OptOutOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var retrieveOptOutStatusOrchestrationDependencyValidationException =
                new OptOutOrchestrationDependencyValidationException(
                    message: "Opt Out orchestration dependency validation errors occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(retrieveOptOutStatusOrchestrationDependencyValidationException);

            return retrieveOptOutStatusOrchestrationDependencyValidationException;
        }

        private OptOutOrchestrationDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var optOutOrchestrationDependencyException =
                new OptOutOrchestrationDependencyException(
                    message: "Opt Out orchestration dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(optOutOrchestrationDependencyException);

            throw optOutOrchestrationDependencyException;
        }

        private OptOutOrchestrationServiceException
            CreateAndLogServiceException(Xeption exception)
        {
            var optOutOrchestrationServiceException =
                new OptOutOrchestrationServiceException(
                    message: "Opt Out orchestration service error occurred, please contact support.",
                    exception);

            this.loggingBroker.LogError(optOutOrchestrationServiceException);

            throw optOutOrchestrationServiceException;
        }
    }
}
