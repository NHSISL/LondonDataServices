// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.CsvMappers.Exceptions;
using LHDS.Core.Models.Processings.CsvMappers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.CsvMappers
{
    public partial class CsvMapperProcessingService
    {
        private delegate ValueTask<string> ReturningStringFunction();
        private delegate ValueTask<List<T>> ReturningObjectFunction<T>();

        private async ValueTask<List<T>> TryCatch<T>(ReturningObjectFunction<T> returningObjectFunction)
        {
            try
            {
                return await returningObjectFunction();
            }
            catch (InvalidCsvMapperArgumentsException invalidCsvMapperArgumentsException)
            {
                throw CreateAndLogValidationException(invalidCsvMapperArgumentsException);
            }
            catch (CsvMapperValidationException documentValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentValidationException);
            }
            catch (CsvMapperDependencyValidationException documentDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentDependencyValidationException);
            }
            catch (CsvMapperDependencyException documentDependencyException)
            {
                throw CreateAndLogDependencyException(documentDependencyException);
            }
            catch (CsvMapperServiceException documentServiceException)
            {
                throw CreateAndLogDependencyException(documentServiceException);
            }
            catch (Exception exception)
            {
                var failedCsvMapperServiceException =
                    new FailedCsvMapperServiceException(
                        message: "Failed CSV mapper service error occurred, contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedCsvMapperServiceException);
            }
        }

        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (InvalidCsvMapperArgumentsException invalidCsvMapperArgumentsException)
            {
                throw CreateAndLogValidationException(invalidCsvMapperArgumentsException);
            }
            catch (CsvMapperValidationException documentValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentValidationException);
            }
            catch (CsvMapperDependencyValidationException documentDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentDependencyValidationException);
            }
            catch (CsvMapperDependencyException documentDependencyException)
            {
                throw CreateAndLogDependencyException(documentDependencyException);
            }
            catch (CsvMapperServiceException documentServiceException)
            {
                throw CreateAndLogDependencyException(documentServiceException);
            }
            catch (Exception exception)
            {
                var failedCsvMapperServiceException =
                    new FailedCsvMapperServiceException(
                        message: "Failed CSV mapper service error occurred, contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedCsvMapperServiceException);
            }
        }

        private CsvMapperValidationException CreateAndLogValidationException(Xeption exception)
        {
            var csvMapperValidationException = new CsvMapperValidationException(
                message: "CSV mapper validation errors occurred, fix the errors and try again.",
                innerException: exception);

            this.loggingBroker.LogError(csvMapperValidationException);

            return csvMapperValidationException;
        }

        private CsvMapperProcessingDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var csvMapperProcessingDependencyValidationException =
                new CsvMapperProcessingDependencyValidationException(
                    message: "Csv Mapper processing dependency validation occurred, please try again.",
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(csvMapperProcessingDependencyValidationException);

            return csvMapperProcessingDependencyValidationException;
        }

        private CsvMapperProcessingDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var csvMapperProcessingDependencyException =
                new CsvMapperProcessingDependencyException(
                    message: "Csv Mapper processing dependency validation occurred, please try again.",
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(csvMapperProcessingDependencyException);

            throw csvMapperProcessingDependencyException;
        }

        private CsvMapperProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var csvMapperProcessingServiceException = new CsvMapperProcessingServiceException(
                message: "Csv Mapper processing service error occurred, contact support.",
                exception);

            this.loggingBroker.LogError(csvMapperProcessingServiceException);

            return csvMapperProcessingServiceException;
        }
    }
}
