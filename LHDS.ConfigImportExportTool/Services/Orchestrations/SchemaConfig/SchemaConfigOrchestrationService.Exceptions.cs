// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Orchestrations.SchemaConfigs.Exceptions;
using LHDS.ConfigImportExportTool.Models.Processings.DataSets.Exceptions;
using LHDS.ConfigImportExportTool.Models.Processings.ObjectColumns.Exceptions;
using LHDS.ConfigImportExportTool.Models.Processings.SpecificationObjects.Exceptions;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Orchestrations.SchemaConfigs
{
    internal partial class SchemaConfigOrchestrationService
    {
        private delegate ValueTask<List<SpecificationObject>> ReturningListSpecificationObjectsFunction();
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask<List<SpecificationObject>> TryCatch(
            ReturningListSpecificationObjectsFunction returningListSpecificationObjectsFunction)
        {
            try
            {
                return await returningListSpecificationObjectsFunction();
            }
            catch (InvalidArgumentSchemaConfigOrchestrationException invalidArgumentSchemaConfigOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentSchemaConfigOrchestrationException);
            }
            catch (DataSetProcessingValidationException dataSetProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetProcessingValidationException);
            }
            catch (DataSetProcessingDependencyValidationException dataSetProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetProcessingDependencyValidationException);
            }
            catch (SpecificationObjectProcessingValidationException specificationObjectProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(specificationObjectProcessingValidationException);
            }
            catch (SpecificationObjectProcessingDependencyValidationException
                specificationObjectProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    specificationObjectProcessingDependencyValidationException);
            }
        }

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
            catch (DataSetProcessingValidationException dataSetProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetProcessingValidationException);
            }
            catch (DataSetProcessingDependencyValidationException dataSetProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetProcessingDependencyValidationException);
            }
            catch (SpecificationObjectProcessingValidationException specificationObjectProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(specificationObjectProcessingValidationException);
            }
            catch (SpecificationObjectProcessingDependencyValidationException
                specificationObjectProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    specificationObjectProcessingDependencyValidationException);
            }
            catch (ObjectColumnProcessingValidationException objectColumnProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(objectColumnProcessingValidationException);
            }
            catch (ObjectColumnProcessingDependencyValidationException
                objectColumnProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(objectColumnProcessingDependencyValidationException);
            }
            catch (DataSetProcessingDependencyException dataSetProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(dataSetProcessingDependencyException);
            }
            catch (DataSetProcessingServiceException dataSetProcessingServiceException)
            {
                throw CreateAndLogDependencyException(dataSetProcessingServiceException);
            }
            catch (SpecificationObjectProcessingDependencyException specificationObjectProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(specificationObjectProcessingDependencyException);
            }
            catch (SpecificationObjectProcessingServiceException specificationObjectProcessingServiceException)
            {
                throw CreateAndLogDependencyException(specificationObjectProcessingServiceException);
            }
            catch (ObjectColumnProcessingDependencyException objectColumnProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(objectColumnProcessingDependencyException);
            }
            catch (ObjectColumnProcessingServiceException objectColumnProcessingServiceException)
            {
                throw CreateAndLogDependencyException(objectColumnProcessingServiceException);
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
