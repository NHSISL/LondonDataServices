// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using LHDS.Core.Models.Foundations.Documents.Exceptions;

namespace LHDS.Core.Services.Foundations.Documents
{
    public partial class DocumentService
    {
        private static void ValidateDocumentOnAdd(Stream input, string fileName, string container)
        {
            Validate(
                (Rule: IsInvalid(input), Parameter: "Document"),
                (Rule: IsInvalid(fileName), Parameter: "FileName"),
                (Rule: IsInvalid(container), Parameter: "Container"));
        }

        private static void ValidateDocumentOnRetrieve(string fileName, string container)
        {
            Validate(
                (Rule: IsInvalid(container), Parameter: "Container"),
                (Rule: IsInvalid(fileName), Parameter: "FileName"));
        }

        private void ValidateDeleteArguments(string fileName, string container)
        {
            Validate(
               (Rule: IsInvalid(container), Parameter: "Container"),
               (Rule: IsInvalid(fileName), Parameter: "FileName"));
        }

        private void ValidateGetDownloadLinkArguments(string fileName, string container)
        {
            Validate(
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

        private static dynamic IsInvalid(Stream input) => new
        {
            Condition = (input == null || input.Length == 0),
            Message = "Data is required"
        };

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidDocumentException = new InvalidDocumentException(
                message: "Invalid document. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDocumentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDocumentException.ThrowIfContainsErrors();
        }
    }
}
