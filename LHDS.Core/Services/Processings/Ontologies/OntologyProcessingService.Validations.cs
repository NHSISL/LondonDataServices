// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Processings.Ontologies.Exceptions;

namespace LHDS.Core.Services.Processings.Ontologies
{
    public partial class OntologyProcessingService
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
            var invalidArgumentOntologyProcessingException = new InvalidArgumentOntologyProcessingException(
                message: "Invalid ontology processing arguments. Please correct the error and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentOntologyProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentOntologyProcessingException.ThrowIfContainsErrors();
        }
    }
}
