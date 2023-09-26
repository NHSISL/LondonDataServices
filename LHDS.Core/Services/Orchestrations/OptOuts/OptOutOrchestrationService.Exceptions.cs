// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Orchestrations.OptOuts.Exceptions;
using LHDS.Core.Models.Processings.CsvMappers.Exceptions;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using LHDS.Core.Models.Processings.Mesh.Exceptions;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationService
    {
        private delegate ValueTask ReturningNothingFunction();
        private delegate ValueTask<string> ReturningFileNameFunction();
        private delegate ValueTask<MeshMessage> ReturningMeshMessageFunction();
        private delegate ValueTask<List<MeshMessage>> ReturningMeshMessageListFunction();

        private async ValueTask<string> TryCatch(ReturningFileNameFunction returningFileNameFunction)
        {
            try
            {
                return await returningFileNameFunction();
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
            catch (CsvMapperProcessingValidationException csvMapperProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(csvMapperProcessingValidationException);
            }
            catch (CsvMapperProcessingDependencyValidationException csvMapperProcessingDependencyValidationException)
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
            catch (CsvMapperProcessingDependencyException optOutOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationDependencyException);
            }
            catch (CsvMapperProcessingServiceException optOutOrchestrationServiceException)
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
            catch (Exception exception)
            {
                var failedOptOutServiceException =
                    new FailedOptOutOrchestrationServiceException(
                        message: "Failed opt out orchestration service occurred, please contact support",
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
            catch (CsvMapperProcessingValidationException csvMapperProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(csvMapperProcessingValidationException);
            }
            catch (CsvMapperProcessingDependencyValidationException csvMapperProcessingDependencyValidationException)
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
            catch (CsvMapperProcessingDependencyException optOutOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationDependencyException);
            }
            catch (CsvMapperProcessingServiceException optOutOrchestrationServiceException)
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
            catch (Exception exception)
            {
                var failedOptOutServiceException =
                    new FailedOptOutOrchestrationServiceException(
                        message: "Failed opt out orchestration service occurred, please contact support",
                        innerException: exception);

                throw CreateAndLogServiceException(failedOptOutServiceException);
            }
        }

        private async ValueTask<MeshMessage> TryCatch(ReturningMeshMessageFunction returningMeshMessageFunction)
        {
            try
            {
                return await returningMeshMessageFunction();
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
            catch (CsvMapperProcessingValidationException csvMapperProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(csvMapperProcessingValidationException);
            }
            catch (CsvMapperProcessingDependencyValidationException csvMapperProcessingDependencyValidationException)
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
            catch (CsvMapperProcessingDependencyException optOutOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationDependencyException);
            }
            catch (CsvMapperProcessingServiceException optOutOrchestrationServiceException)
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
            catch (Exception exception)
            {
                var failedOptOutServiceException =
                    new FailedOptOutOrchestrationServiceException(
                        message: "Failed opt out orchestration service occurred, please contact support",
                        innerException: exception);

                throw CreateAndLogServiceException(failedOptOutServiceException);
            }
        }

        private async ValueTask<List<MeshMessage>> TryCatch(
            ReturningMeshMessageListFunction returningMeshMessageListFunction)
        {
            try
            {
                return await returningMeshMessageListFunction();
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
            catch (InvalidMeshMessageOrchestrationException invalidMeshMessageOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidMeshMessageOrchestrationException);
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
            catch (CsvMapperProcessingValidationException csvMapperProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(csvMapperProcessingValidationException);
            }
            catch (CsvMapperProcessingDependencyValidationException csvMapperProcessingDependencyValidationException)
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
            catch (CsvMapperProcessingDependencyException optOutOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(optOutOrchestrationDependencyException);
            }
            catch (CsvMapperProcessingServiceException optOutOrchestrationServiceException)
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
            catch (Exception exception)
            {
                var failedOptOutServiceException =
                    new FailedOptOutOrchestrationServiceException(
                        message: "Failed opt out orchestration service occurred, please contact support",
                        innerException: exception);

                throw CreateAndLogServiceException(failedOptOutServiceException);
            }
        }

        private OptOutOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var decryptionOrchestrationValidationException =
                new OptOutOrchestrationValidationException(exception);

            this.loggingBroker.LogError(decryptionOrchestrationValidationException);

            return decryptionOrchestrationValidationException;
        }

        private OptOutOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var retrieveOptOutStatusOrchestrationDependencyValidationException =
                new OptOutOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(retrieveOptOutStatusOrchestrationDependencyValidationException);

            return retrieveOptOutStatusOrchestrationDependencyValidationException;
        }

        private OptOutOrchestrationDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var optOutOrchestrationDependencyException =
                new OptOutOrchestrationDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(optOutOrchestrationDependencyException);

            throw optOutOrchestrationDependencyException;
        }

        private OptOutOrchestrationServiceException
            CreateAndLogServiceException(Xeption exception)
        {
            var optOutOrchestrationServiceException =
                new OptOutOrchestrationServiceException(exception);

            this.loggingBroker.LogError(optOutOrchestrationServiceException);

            throw optOutOrchestrationServiceException;
        }
    }
}
