// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LHDS.Core.Models.Processings.Mesh.Exceptions;

namespace LHDS.Core.Services.Processings.Mesh
{
    public partial class MeshProcessingService
    {
        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidMeshProcessingException = new InvalidMeshProcessingArgumentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidMeshProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidMeshProcessingException.ThrowIfContainsErrors();
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
