// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Coordinations.ImportExports.Exceptions;
using LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema.Exceptions;
using LHDS.ConfigImportExportTool.Models.Orchestrations.SchemaConfigs.Exceptions;
using LHDS.ConfigImportExportTool.Services.Orchestrations.ReadSchema;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Coordinations.ImportExports
{
    internal partial class ImportExportCoordinationService
    {
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask TryCatch(
            ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentImportExportCoordinationException invalidArgumentImportExportCoordinationException)
            {
                throw CreateAndLogValidationException(invalidArgumentImportExportCoordinationException);
            }
            catch (ReadSchemaValidationOrchestrationException readSchemaValidationOrchestrationException)
            {
                throw CreateAndLogDependencyValidationException(readSchemaValidationOrchestrationException);
            }
            catch (ReadSchemaOrchestrationDependencyValidationException readSchemaOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(readSchemaOrchestrationDependencyValidationException);
            }
            catch (SchemaConfigValidationOrchestrationException schemaConfigValidationOrchestrationException)
            {
                throw CreateAndLogDependencyValidationException(schemaConfigValidationOrchestrationException);
            }
            catch (SchemaConfigOrchestrationDependencyValidationException
                schemaConfigOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    schemaConfigOrchestrationDependencyValidationException);
            }
            catch (ReadSchemaOrchestrationDependencyException readSchemaOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(readSchemaOrchestrationDependencyException);
            }
            catch (ReadSchemaOrchestrationServiceException readSchemaOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(readSchemaOrchestrationServiceException);
            }
            catch (SchemaConfigOrchestrationDependencyException schemaConfigOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(schemaConfigOrchestrationDependencyException);
            }
            catch (SchemaConfigOrchestrationServiceException schemaConfigOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(schemaConfigOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedFileServiceException =
                    new FailedImportExportCoordinationServiceException(
                        message: "Failed import export coordination service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedFileServiceException);
            }
        }

        private ImportExportValidationCoordinationException CreateAndLogValidationException(Xeption exception)
        {
            var importExportValidationCoordinationException = new ImportExportValidationCoordinationException(
                message: "Import export coordination validation error occurred, fix the errors and try again.",
                innerException: exception);

            this.loggingBroker.LogErrorAsync(importExportValidationCoordinationException);

            return importExportValidationCoordinationException;
        }

        private ImportExportCoordinationDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var importExportCoordinationDependencyValidationException =
                new ImportExportCoordinationDependencyValidationException(
                    message: "Import export coordination dependency validation error occurred, please contact support.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogErrorAsync(importExportCoordinationDependencyValidationException);

            return importExportCoordinationDependencyValidationException;
        }

        private ImportExportCoordinationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var importExportCoordinationDependencyException = new ImportExportCoordinationDependencyException(
                message: "Import export coordination dependency error occurred, please contact support.",
                innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogErrorAsync(importExportCoordinationDependencyException);

            return importExportCoordinationDependencyException;
        }

        private ImportExportCoordinationServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var importExportCoordinationServiceException = new ImportExportCoordinationServiceException(
                message: "Import export coordination service error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogErrorAsync(importExportCoordinationServiceException);

            return importExportCoordinationServiceException;
        }
    }
}
