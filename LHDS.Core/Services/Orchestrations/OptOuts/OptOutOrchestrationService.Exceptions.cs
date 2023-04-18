// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Orchestrations.OptOuts.Exceptions;
using LHDS.Core.Models.Processings.CsvMapper.Exceptions;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using LHDS.Core.Models.Processings.Mesh.Exceptions;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationService
    {
        private delegate ValueTask ReturningRetrieveOptOutStatusFunction();

        private async ValueTask TryCatch(ReturningRetrieveOptOutStatusFunction returningRetrieveOptOutFunction)
        {
            try
            {
                await returningRetrieveOptOutFunction();
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

        }

        private OptOutOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            string validationSummary = GetValidationSummary(exception.Data);

            var decryptionOrchestrationValidationException =
                new OptOutOrchestrationValidationException(exception, validationSummary);

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
    }
}
