// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.Documents
{
    public partial class DocumentProcessingService
    {
        private static void ValidateDocumentProcessingOnAdd(Stream input, string fileName, string container)
        {
            Validate(
                createException: () => new InvalidArgumentsDocumentProcessingException(
                    message: "Invalid document processing arguments. Please correct the errors and try again."),

                (Rule: IsInvalidInputStream(input), Parameter: "Input"),
                (Rule: IsInvalid(fileName), Parameter: "FileName"),
                (Rule: IsInvalid(container), Parameter: "Container"));
        }

        private static void ValidateDocumentProcessingOnRetrieve(Stream output, string fileName, string container)
        {
            Validate(
                createException: () => new InvalidArgumentsDocumentProcessingException(
                    message: "Invalid document processing arguments. Please correct the errors and try again."),

               (Rule: IsInvalidOutputStream(output), Parameter: "Output"),
               (Rule: IsInvalid(container), Parameter: "Container"),
               (Rule: IsInvalid(fileName), Parameter: "FileName"));
        }

        private static void ValidateDocumentProcessingOnRemove(string fileName, string container)
        {
            Validate(
                createException: () => new InvalidArgumentsDocumentProcessingException(
                    message: "Invalid document processing arguments. Please correct the errors and try again."),

               (Rule: IsInvalid(container), Parameter: "Container"),
               (Rule: IsInvalid(fileName), Parameter: "FileName"));
        }

        private static void ValidateGetDownloadLinkArguments(string fileName, string container)
        {
            Validate(
                createException: () => new InvalidArgumentsDocumentProcessingException(
                    message: "Invalid document processing arguments. Please correct the errors and try again."),

               (Rule: IsInvalid(container), Parameter: "Container"),
               (Rule: IsInvalid(fileName), Parameter: "FileName"));
        }

        private static void ValidateDocumentProcessingIsNotNull(Document document)
        {
            if (document is null)
            {
                throw new NullDocumentProcessingException(
                    message: $"Document processing is Null");
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
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void ValidateDocumentProcessingFileNameIsNotNull(string fileName)
        {
            if (fileName is null)
            {
                throw new NullDocumentProcessingFileNameException(
                    message: "Null document processing file name. Please correct the errors and try again.");
            }
        }

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
