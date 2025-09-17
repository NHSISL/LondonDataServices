// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Processings.Ontologies.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.Ontologies
{
    public partial class OntologyProcessingService
    {
        public void ValidateArgs(string relativeUrl)
        {
            Validate(
                createException: () => new InvalidArgumentOntologyProcessingException(
                    message: "Invalid ontology processing arguments. Please correct the errors and try again."),

                (Rule: IsInvalid(relativeUrl), Parameter: "relativeUrl"));
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
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
