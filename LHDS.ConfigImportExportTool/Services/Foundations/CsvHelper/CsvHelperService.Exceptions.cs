// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.CsvHelpers.Exceptions;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Foundations.CsvHelpers
{
    internal partial class CsvHelperService<T>
    {
        private delegate ValueTask<List<T>> ReturningObjectListFunction<T>();
        private delegate ValueTask<string> ReturningStringFunction();

        private async ValueTask<List<T>> TryCatch<T>(ReturningObjectListFunction<T> returningObjectListFunction)
        {
            try
            {
                return await returningObjectListFunction();
            }
            catch (InvalidArgumentCsvHelperException invalidArgumentCsvHelperException)
            {
                throw CreateAndLogValidationException(invalidArgumentCsvHelperException);
            }
            catch (Exception exception)
            {
                var failedCsvHelperServiceException =
                    new FailedCsvHelperServiceException(
                        message: "Failed csv helper service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedCsvHelperServiceException);
            }
        }

        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (InvalidArgumentCsvHelperException invalidArgumentCsvHelperException)
            {
                throw CreateAndLogValidationException(invalidArgumentCsvHelperException);
            }
            catch (Exception exception)
            {
                var failedCsvHelperServiceException =
                    new FailedCsvHelperServiceException(
                        message: "Failed csv helper service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedCsvHelperServiceException);
            }
        }

        private CsvHelperValidationException CreateAndLogValidationException(Xeption exception)
        {
            var csvHelperValidationException = new CsvHelperValidationException(
                message: "Csv helper validation error occurred, fix the errors and try again.",
                innerException: exception);

            this.loggingBroker.LogErrorAsync(csvHelperValidationException);

            return csvHelperValidationException;
        }

        private CsvHelperDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var csvHelperServiceDependencyValidationException =
                new CsvHelperDependencyValidationException(
                    message: "Csv helper dependency validation error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogErrorAsync(csvHelperServiceDependencyValidationException);

            return csvHelperServiceDependencyValidationException;
        }

        private CsvHelperDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var csvHelperDependencyException = new CsvHelperDependencyException(
                message: "Csv helper dependency error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogErrorAsync(csvHelperDependencyException);

            return csvHelperDependencyException;
        }

        private CsvHelperServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var csvHelperServiceException = new CsvHelperServiceException(
                message: "Csv helper service error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogErrorAsync(csvHelperServiceException);

            return csvHelperServiceException;
        }
    }
}
