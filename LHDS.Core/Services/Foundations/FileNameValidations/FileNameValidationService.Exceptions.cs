// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.FileNameValidation.Exceptions;
using LHDS.Core.Models.Foundations.FileNameValidations.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.FileNameValidations
{
    public partial class FileNameValidationService
    {
        private delegate ValueTask<bool> ReturningBoolFunction();

        private async ValueTask<bool> TryCatch(ReturningBoolFunction returningBoolFunction)
        {
            try
            {
                return await returningBoolFunction();
            }
            catch (InvalidArgumentFileNameException invalidArgumentFileNameException)
            {
                throw CreateAndLogValidationException(invalidArgumentFileNameException);
            }
            catch (RegexParseException regexParseException)
            {
                var failedFileNameValidationServiceException =
                    new FailedFileNameValidationServiceException(
                        message: "Failed file name validation service error occurred, please contact support.",
                        innerException: regexParseException);

                throw CreateAndLogServiceException(failedFileNameValidationServiceException);
            }
            catch (Exception exception)
            {
                var failedFileNameValidationServiceException =
                    new FailedFileNameValidationServiceException(
                        message: "Failed file name validation service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedFileNameValidationServiceException);
            }
        }

        private FileNameValidationException CreateAndLogValidationException(Xeption exception)
        {
            var fileNameValidationException =
                new FileNameValidationException(
                    message: "File name validation error occurred, please try again.",
                    innerException: exception);

            return fileNameValidationException;
        }

        private FileNameValidationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var fileNameValidationServiceException =
                new FileNameValidationServiceException(
                    message: "File name validation service error occurred, please contact support.",
                    innerException: exception);

            return fileNameValidationServiceException;
        }
    }
}