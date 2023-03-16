// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Collections;
using System.Text;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using System.Linq;

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
            Validate(
               (Rule: IsInvalid(fileName), Parameter: nameof(fileName)));
        }

        private static void ValidateDocumentProcessingOnRemove(string fileName)
        {
            Validate(
               (Rule: IsInvalid(fileName), Parameter: nameof(fileName)));
        }

        private static void ValidateGetDownloadLinkArguments(string fileName)
        {
            ValidateDocumentProcessingFileNameIsNotNull(fileName);
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

        private string GetValidationSummary(IDictionary data)
        {
            StringBuilder validationSummary = new StringBuilder();

            foreach (DictionaryEntry entry in data)
            {
                string errorSummary = ((List<string>)entry.Value)
                    .Select((string value) => value)
                    .Aggregate((string current, string next) => current + ", " + next);

                validationSummary.Append($"{entry.Key} => {errorSummary};  ");
            }

            return validationSummary.ToString();
        }
    }
}
