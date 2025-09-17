// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Ontologies.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Ontologies
{
    public partial class OntologyService
    {
        public async ValueTask ValidateArgs(string relativeUrl)
        {
            Validate(
                createException: () => new InvalidArgumentOntologyException(
                    message: "Invalid ontology arguments. Please correct the error and try again."),

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
