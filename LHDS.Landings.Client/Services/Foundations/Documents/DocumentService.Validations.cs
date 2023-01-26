// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Models.Foundations.Documents.Exceptions;
using NEL.Premises.Api.Models.Documents.Exceptions;

namespace LHDS.Landings.Client.Services.Foundations.Downloads
{
    public partial class DocumentService
    {
        private static void ValidateDocumentOnAdd(Document document, string blobContainerName)
        {
            ValidateDocumentIsNotNull(document);

            Validate(
                    (Rule: IsInvalid(document.DocumentData), Parameter: nameof(Document.DocumentData)),
                    (Rule: IsInvalid(document.FileName), Parameter: nameof(Document.FileName)),
                    (Rule: IsInvalid(blobContainerName), Parameter: nameof(blobContainerName)));
        }

        private static void ValidateDocumentOnRetrieve(string fileName, string blobContainerName)
        {
            Validate(
                    (Rule: IsInvalid(fileName), Parameter: nameof(fileName)),
                    (Rule: IsInvalid(blobContainerName), Parameter: nameof(blobContainerName)));
        }

        private static void ValidateDocumentIsNotNull(Document document)
        {
            if (document is null)
            {
                throw new NullDocumentException();
            }
        }

        private static dynamic IsInvalid(byte[] data) => new
        {
            Condition = (data == null || data.Length == 0),
            Message = "Data is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private void ValidateDeleteArguments(string fileName, string container)
        {
            Validate(
               (Rule: IsInvalid(fileName), Parameter: nameof(fileName)),
               (Rule: IsInvalid(container), Parameter: nameof(container)));
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidDocumentException = new InvalidDocumentException();

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
