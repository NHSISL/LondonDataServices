// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Files.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Files
{
    internal partial class FileService
    {
        private delegate ValueTask<bool> ReturningBooleanFunction();
        private delegate ValueTask<byte[]> ReturningByteArrayFunction();
        private delegate ValueTask<string> ReturningStringFunction();
        private delegate ValueTask<List<string>> ReturningStringListFunction();
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask<bool> TryCatch(ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return await returningBooleanFunction();
            }
            catch (InvalidArgumentFileException invalidArgumentFileException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentFileException);
            }
            catch (ArgumentNullException argumentNullException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(
                        message: "Invalid file service dependency validation error occurred.",
                        innerException: argumentNullException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidFileDependencyException);
            }
            catch (ArgumentOutOfRangeException argumentOutOfRangeException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(
                        message: "Invalid file service dependency validation error occurred.",
                        innerException: argumentOutOfRangeException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidFileDependencyException);
            }
            catch (ArgumentException argumentException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(
                        message: "Invalid file service dependency validation error occurred.",
                        innerException: argumentException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidFileDependencyException);
            }
            catch (SerializationException serializationException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: serializationException);

                throw await CreateAndLogDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (OutOfMemoryException outOfMemoryException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: outOfMemoryException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (IOException ioException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: ioException);

                throw await CreateAndLogDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: unauthorizedAccessException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (Exception exception)
            {
                var failedFileServiceException =
                    new FailedFileServiceException(
                        message: "Failed file service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedFileServiceException);
            }
        }

        private async ValueTask<byte[]> TryCatch(ReturningByteArrayFunction returningByteArrayFunction)
        {
            try
            {
                return await returningByteArrayFunction();
            }
            catch (InvalidArgumentFileException invalidArgumentFileException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentFileException);
            }
            catch (ArgumentNullException argumentNullException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(
                        message: "Invalid file service dependency validation error occurred.",
                        innerException: argumentNullException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidFileDependencyException);
            }
            catch (ArgumentOutOfRangeException argumentOutOfRangeException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(
                        message: "Invalid file service dependency validation error occurred.",
                        innerException: argumentOutOfRangeException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidFileDependencyException);
            }
            catch (ArgumentException argumentException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(
                        message: "Invalid file service dependency validation error occurred.",
                        innerException: argumentException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidFileDependencyException);
            }
            catch (SerializationException serializationException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: serializationException);

                throw await CreateAndLogDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (OutOfMemoryException outOfMemoryException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: outOfMemoryException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (IOException ioException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: ioException);

                throw await CreateAndLogDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: unauthorizedAccessException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (Exception exception)
            {
                var failedFileServiceException =
                    new FailedFileServiceException(
                        message: "Failed file service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedFileServiceException);
            }
        }

        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (InvalidArgumentFileException invalidArgumentFileException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentFileException);
            }
            catch (ArgumentNullException argumentNullException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(
                        message: "Invalid file service dependency validation error occurred.",
                        innerException: argumentNullException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidFileDependencyException);
            }
            catch (ArgumentOutOfRangeException argumentOutOfRangeException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(
                        message: "Invalid file service dependency validation error occurred.",
                        innerException: argumentOutOfRangeException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidFileDependencyException);
            }
            catch (ArgumentException argumentException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(
                        message: "Invalid file service dependency validation error occurred.",
                        innerException: argumentException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidFileDependencyException);
            }
            catch (SerializationException serializationException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: serializationException);

                throw await CreateAndLogDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (OutOfMemoryException outOfMemoryException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: outOfMemoryException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (IOException ioException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: ioException);

                throw await CreateAndLogDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: unauthorizedAccessException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (Exception exception)
            {
                var failedFileServiceException =
                    new FailedFileServiceException(
                        message: "Failed file service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedFileServiceException);
            }
        }

        private async ValueTask<List<string>> TryCatch(ReturningStringListFunction returningStringListFunction)
        {
            try
            {
                return await returningStringListFunction();
            }
            catch (InvalidArgumentFileException invalidArgumentFileException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentFileException);
            }
            catch (ArgumentNullException argumentNullException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(
                        message: "Invalid file service dependency validation error occurred.",
                        innerException: argumentNullException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidFileDependencyException);
            }
            catch (ArgumentOutOfRangeException argumentOutOfRangeException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(
                        message: "Invalid file service dependency validation error occurred.",
                        innerException: argumentOutOfRangeException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidFileDependencyException);
            }
            catch (ArgumentException argumentException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(
                        message: "Invalid file service dependency validation error occurred.",
                        innerException: argumentException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidFileDependencyException);
            }
            catch (SerializationException serializationException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: serializationException);

                throw await CreateAndLogDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (OutOfMemoryException outOfMemoryException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: outOfMemoryException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (IOException ioException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: ioException);

                throw await CreateAndLogDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: unauthorizedAccessException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (Exception exception)
            {
                var failedFileServiceException =
                    new FailedFileServiceException(
                        message: "Failed file service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedFileServiceException);
            }
        }

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentFileException invalidArgumentFileException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentFileException);
            }
            catch (ArgumentNullException argumentNullException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(
                        message: "Invalid file service dependency validation error occurred.",
                        innerException: argumentNullException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidFileDependencyException);
            }
            catch (ArgumentOutOfRangeException argumentOutOfRangeException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(
                        message: "Invalid file service dependency validation error occurred.",
                        innerException: argumentOutOfRangeException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidFileDependencyException);
            }
            catch (ArgumentException argumentException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(
                        message: "Invalid file service dependency validation error occurred.",
                        innerException: argumentException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidFileDependencyException);
            }
            catch (SerializationException serializationException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: serializationException);

                throw await CreateAndLogDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (OutOfMemoryException outOfMemoryException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: outOfMemoryException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (IOException ioException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: ioException);

                throw await CreateAndLogDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(
                        message: "Failed file dependency error occurred, please contact support.",
                        innerException: unauthorizedAccessException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedFileDependencyException);
            }
            catch (Exception exception)
            {
                var failedFileServiceException =
                    new FailedFileServiceException(
                        message: "Failed file service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedFileServiceException);
            }
        }

        private async ValueTask<FileValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var fileValidationException = new FileValidationException(
                message: "File validation error occurred, fix the errors and try again.",
                innerException: exception);

            return fileValidationException;
        }

        private async ValueTask<FileDependencyValidationException> CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var fileServiceDependencyValidationException =
                new FileDependencyValidationException(
                    message: "File dependency validation error occurred, please contact support.",
                    innerException: exception);

            return fileServiceDependencyValidationException;
        }

        private async ValueTask<FileDependencyException> CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var fileServiceDependencyException = new FileDependencyException(
                message: "File dependency error occurred, please contact support.",
                innerException: exception);

            return fileServiceDependencyException;
        }

        private async ValueTask<FileDependencyException> CreateAndLogCriticalDependencyExceptionAsync(
            Xeption exception)
        {
            var fileServiceDependencyException = new FileDependencyException(
                message: "File dependency error occurred, please contact support.",
                innerException: exception);

            return fileServiceDependencyException;
        }

        private async ValueTask<FileServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var fileServiceException = new FileServiceException(
                message: "File service error occurred, please contact support.",
                innerException: exception);

            return fileServiceException;
        }
    }
}
