// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.CsvHelpers.Exceptions;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Foundations.CsvHelpers
{
    public partial class CsvHelperService
    {
        private delegate ValueTask<List<T>> ReturningObjectListFunction();

        private async ValueTask<List<T>> TryCatch(ReturningObjectListFunction returningObjectListFunction)
        {
            try
            {
                return await returningObjectListFunction();
            }
            catch (InvalidArgumentCsvHelperException invalidArgumentCsvHelperException)
            {
                throw CreateAndLogValidationException(invalidArgumentCsvHelperException);
            }
        }

        private CsvHelperValidationException CreateAndLogValidationException(Xeption exception)
        {
            var csvHelperValidationException = new CsvHelperValidationException(
                message: "Csv helper validation error occurred, fix the errors and try again.",
                innerException: exception);

            return csvHelperValidationException;
        }

        private CsvHelperDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var csvHelperServiceDependencyValidationException =
                new CsvHelperDependencyValidationException(
                    message: "Csv helper dependency validation error occurred, please contact support.",
                    innerException: exception);

            return csvHelperServiceDependencyValidationException;
        }

        private CsvHelperDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var csvHelperServiceDependencyException = new CsvHelperDependencyException(
                message: "CsvHelper dependency error occurred, please contact support.",
                innerException: exception);

            return csvHelperServiceDependencyException;
        }

        private CsvHelperDependencyException CreateAndLogCriticalDependencyException(
            Xeption exception)
        {
            var csvHelperServiceDependencyException = new CsvHelperDependencyException(
                message: "Csv helper dependency error occurred, please contact support.",
                innerException: exception);

            return csvHelperServiceDependencyException;
        }

        private CsvHelperServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var csvHelperServiceException = new CsvHelperServiceException(
                message: "CsvHelper service error occurred, please contact support.",
                innerException: exception);

            return csvHelperServiceException;
        }
    }
}
