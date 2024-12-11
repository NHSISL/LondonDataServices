// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Models.Foundations.Files.Exceptions;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema.Exceptions;
using LHDS.ConfigImportExportTool.Models.Orchestrations.SchemaConfigs.Exceptions;
using NHSISL.CsvHelperClient.Models.Clients.CsvHelpers.Exceptions;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Orchestrations.ReadSchema
{
    internal partial class ReadSchemaOrchestrationService
    {
        private delegate ValueTask<List<SpecificationObject>> ReturningObjectColumnListFunction();
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask<List<SpecificationObject>> TryCatch(
            ReturningObjectColumnListFunction returningObjectColumnListFunction)
        {
            try
            {
                return await returningObjectColumnListFunction();
            }
            catch (InvalidArgumentReadSchemaOrchestrationException invalidArgumentReadSchemaOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentReadSchemaOrchestrationException);
            }
            catch (FileValidationException fileValidationException)
            {
                throw CreateAndLogDependencyValidationException(fileValidationException);
            }
            catch (FileDependencyValidationException fileDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(fileDependencyValidationException);
            }
            catch (CsvHelperClientValidationException csvHelperClientValidationException)
            {
                throw CreateAndLogDependencyValidationException(csvHelperClientValidationException);
            }
            catch (FileDependencyException fileDependencyException)
            {
                throw CreateAndLogDependencyException(fileDependencyException);
            }
            catch (FileServiceException fileServiceException)
            {
                throw CreateAndLogDependencyException(fileServiceException);
            }
            catch (CsvHelperClientDependencyException csvHelperClientDependencyException)
            {
                throw CreateAndLogDependencyException(csvHelperClientDependencyException);
            }
            catch (CsvHelperClientServiceException csvHelperClientServiceException)
            {
                throw CreateAndLogDependencyException(csvHelperClientServiceException);
            }
            catch (Exception exception)
            {
                var failedFileServiceException =
                    new FailedReadSchemaOrchestrationServiceException(
                        message: "Failed read schema orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedFileServiceException);
            }
        }

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentReadSchemaOrchestrationException invalidArgumentReadSchemaOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentReadSchemaOrchestrationException);
            }
            catch (NullSpecificationObjectListException nullSpecificationObjectListException)
            {
                throw CreateAndLogValidationException(nullSpecificationObjectListException);
            }
            catch (FileValidationException fileValidationException)
            {
                throw CreateAndLogDependencyValidationException(fileValidationException);
            }
            catch (FileDependencyValidationException fileDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(fileDependencyValidationException);
            }
            catch (CsvHelperClientValidationException csvHelperClientValidationException)
            {
                throw CreateAndLogDependencyValidationException(csvHelperClientValidationException);
            }
            catch (FileDependencyException fileDependencyException)
            {
                throw CreateAndLogDependencyException(fileDependencyException);
            }
            catch (FileServiceException fileServiceException)
            {
                throw CreateAndLogDependencyException(fileServiceException);
            }
            catch (CsvHelperClientDependencyException csvHelperClientDependencyException)
            {
                throw CreateAndLogDependencyException(csvHelperClientDependencyException);
            }
            catch (CsvHelperClientServiceException csvHelperClientServiceException)
            {
                throw CreateAndLogDependencyException(csvHelperClientServiceException);
            }
            catch (Exception exception)
            {
                var failedFileServiceException =
                    new FailedReadSchemaOrchestrationServiceException(
                        message: "Failed read schema orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedFileServiceException);
            }
        }

        private ReadSchemaValidationOrchestrationException CreateAndLogValidationException(Xeption exception)
        {
            var readSchemaValidationOrchestrationException = new ReadSchemaValidationOrchestrationException(
                message: "Read schema orchestration validation error occurred, fix the errors and try again.",
                innerException: exception);

            this.loggingBroker.LogErrorAsync(readSchemaValidationOrchestrationException);

            return readSchemaValidationOrchestrationException;
        }

        private ReadSchemaOrchestrationDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var readSchemaOrchestrationDependencyValidationException =
                new ReadSchemaOrchestrationDependencyValidationException(
                    message: "Read schema orchestration dependency validation error occurred, please contact support.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogErrorAsync(readSchemaOrchestrationDependencyValidationException);

            return readSchemaOrchestrationDependencyValidationException;
        }

        private ReadSchemaOrchestrationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var readSchemaOrchestrationDependencyException = new ReadSchemaOrchestrationDependencyException(
                message: "Read schema orchestration dependency error occurred, please contact support.",
                innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogErrorAsync(readSchemaOrchestrationDependencyException);

            return readSchemaOrchestrationDependencyException;
        }

        private ReadSchemaOrchestrationServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var readSchemaOrchestrationServiceException = new ReadSchemaOrchestrationServiceException(
                message: "Read schema orchestration service error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogErrorAsync(readSchemaOrchestrationServiceException);

            return readSchemaOrchestrationServiceException;
        }
    }
}
