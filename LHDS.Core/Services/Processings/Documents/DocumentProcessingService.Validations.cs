// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Processings.Documents.Exceptions;

namespace LHDS.Core.Services.Processings.Documents
{
    public partial class DocumentProcessingService
    {
        private static void ValidateDocumentProcessingOnAdd(Document document)
        {
            ValidateDocumentProcessingIsNotNull(document);
        }

        private static void ValidateDocumentProcessingOnRetrieve(string fileName)
        {
            ValidateDocumentProcessingFileNameIsNotNull(fileName);
        }

        private static void ValidateDocumentProcessingOnRemove(string fileName)
        {
            Validate(
               (Rule: IsInvalid(fileName), Parameter: nameof(fileName)));
        }

        private static void ValidateDocumentProcessingIsNotNull(Document document)
        {
            if (document is null)
            {
                throw new NullDocumentProcessingException();
            }
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void ValidateDocumentProcessingFileNameIsNotNull(string fileName)
        {
            if (fileName is null)
            {
                throw new NullDocumentProcessingFileNameException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidDocumentProcessingException = new InvalidDocumentProcessingFileNameException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDocumentProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDocumentProcessingException.ThrowIfContainsErrors();
        }
    }
}
