// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Documents.Exceptions;

namespace LHDS.Core.Services.Foundations.Documents
{
    public partial class DocumentService
    {
        private static void ValidateDocumentOnAdd(Document document)
        {
            ValidateDocumentIsNotNull(document);

            Validate(
                    (Rule: IsInvalid(document.DocumentData), Parameter: nameof(Document.DocumentData)),
                    (Rule: IsInvalid(document.FileName), Parameter: nameof(Document.FileName)));
        }

        private static void ValidateDocumentOnRetrieve(string fileName)
        {
            Validate(
                    (Rule: IsInvalid(fileName), Parameter: nameof(fileName)));
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

        private void ValidateDeleteArguments(string fileName)
        {
            Validate(
               (Rule: IsInvalid(fileName), Parameter: nameof(fileName)));
        }

        private void ValidateGetDownloadLinkArguments(string fileName)
        {
            Validate(
               (Rule: IsInvalid(fileName), Parameter: nameof(fileName)));
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
