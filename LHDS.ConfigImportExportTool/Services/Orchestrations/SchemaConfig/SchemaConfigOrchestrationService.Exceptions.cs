// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Orchestrations.SchemaConfigs.Exceptions;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Orchestrations.SchemaConfigs
{
    internal partial class SchemaConfigOrchestrationService
    {
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask TryCatch(
            ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentSchemaConfigOrchestrationException invalidArgumentSchemaConfigOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentSchemaConfigOrchestrationException);
            }
            catch (NullSpecificationObjectListException nullSpecificationObjectListException)
            {
                throw CreateAndLogValidationException(nullSpecificationObjectListException);
            }
            catch (Exception exception)
            {
                var failedFileServiceException =
                    new FailedSchemaConfigOrchestrationServiceException(
                        message: "Failed schema config orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedFileServiceException);
            }
        }

        private SchemaConfigValidationOrchestrationException CreateAndLogValidationException(Xeption exception)
        {
            var schemaConfigValidationOrchestrationException = new SchemaConfigValidationOrchestrationException(
                message: "Schema config orchestration validation error occurred, fix the errors and try again.",
                innerException: exception);

            this.loggingBroker.LogErrorAsync(schemaConfigValidationOrchestrationException);

            return schemaConfigValidationOrchestrationException;
        }

        private SchemaConfigOrchestrationDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var schemaConfigOrchestrationDependencyValidationException =
                new SchemaConfigOrchestrationDependencyValidationException(
                    message: "Schema config orchestration dependency validation error occurred, please contact support.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogErrorAsync(schemaConfigOrchestrationDependencyValidationException);

            return schemaConfigOrchestrationDependencyValidationException;
        }

        private SchemaConfigOrchestrationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var schemaConfigOrchestrationDependencyException = new SchemaConfigOrchestrationDependencyException(
                message: "Schema config orchestration dependency error occurred, please contact support.",
                innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogErrorAsync(schemaConfigOrchestrationDependencyException);

            return schemaConfigOrchestrationDependencyException;
        }

        private SchemaConfigOrchestrationServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var schemaConfigOrchestrationServiceException = new SchemaConfigOrchestrationServiceException(
                message: "Schema config orchestration service error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogErrorAsync(schemaConfigOrchestrationServiceException);

            return schemaConfigOrchestrationServiceException;
        }
    }
}
