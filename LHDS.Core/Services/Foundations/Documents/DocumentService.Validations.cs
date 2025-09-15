// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Documents
{
    public partial class DocumentService
    {
        private static void ValidateDocumentOnAdd(Stream input, string fileName, string container)
        {
            Validate(
                createException: () => new InvalidDocumentException(
                    message: "Invalid document. Please correct the errors and try again."),

                (Rule: IsInvalidInputStream(input), Parameter: "Input"),
                (Rule: IsInvalid(fileName), Parameter: "FileName"),
                (Rule: IsInvalid(container), Parameter: "Container"));
        }

        private static void ValidateArgumentsOnRetrieve(Stream output, string fileName, string container)
        {
            Validate(
                createException: () => new InvalidDocumentException(
                    message: "Invalid document. Please correct the errors and try again."),

                (Rule: IsInvalidOutputStream(output), Parameter: "Output"),
                (Rule: IsInvalid(container), Parameter: "Container"),
                (Rule: IsInvalid(fileName), Parameter: "FileName"));
        }

        private void ValidateDeleteArguments(string fileName, string container)
        {
            Validate(
                createException: () => new InvalidDocumentException(
                    message: "Invalid document. Please correct the errors and try again."),

               (Rule: IsInvalid(container), Parameter: "Container"),
               (Rule: IsInvalid(fileName), Parameter: "FileName"));
        }

        private void ValidateGetDownloadLinkArguments(string fileName, string container)
        {
            Validate(
                createException: () => new InvalidDocumentException(
                    message: "Invalid document. Please correct the errors and try again."),

               (Rule: IsInvalid(container), Parameter: "Container"),
               (Rule: IsInvalid(fileName), Parameter: "FileName"));
        }

        private static void ValidateStorageDocument(
            Stream data,
            string fileName)
        {
            if (data is null || data.Length == 0)
            {
                throw new NotFoundDocumentException(message: $"Couldn't find documents with fileName: {fileName}.");
            }
        }

        private static dynamic IsInvalidInputStream(Stream? stream) => new
        {
            Condition = stream is null || stream.Length == 0,
            Message = "Stream is required"
        };

        private static dynamic IsInvalidOutputStream(Stream? stream) => new
        {
            Condition = stream is null || stream.Length > 0,
            Message = "Stream is required"
        };

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate<T>(
            Func<T> createException,
            params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            T invalidDataException = createException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataException.ThrowIfContainsErrors();
        }
    }
}
