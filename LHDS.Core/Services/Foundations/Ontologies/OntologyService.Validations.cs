// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.Ontologies.Exceptions;

namespace LHDS.Core.Services.Foundations.Ontologies
{
    public partial class OntologyService
    {
        public void ValidateArgs(string relativeUrl) =>
            Validate((Rule: IsInvalid(relativeUrl), Parameter: "relativeUrl"));

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidDownloadException = new InvalidArgumentOntologyException(
                message: "Invalid ontology arguments. Please correct the error and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDownloadException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDownloadException.ThrowIfContainsErrors();
        }
    }
}
