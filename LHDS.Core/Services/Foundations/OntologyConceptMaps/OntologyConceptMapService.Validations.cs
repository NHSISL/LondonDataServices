using System;
using LHDS.Core.Models.Foundations.OntologyConceptMaps;
using LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions;

namespace LHDS.Core.Services.Foundations.OntologyConceptMaps
{
    public partial class OntologyConceptMapService
    {
        private void ValidateOntologyConceptMapOnAdd(OntologyConceptMap ontologyConceptMap)
        {
            ValidateOntologyConceptMapIsNotNull(ontologyConceptMap);

            Validate(
                (Rule: IsInvalid(ontologyConceptMap.Id), Parameter: nameof(OntologyConceptMap.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(ontologyConceptMap.CreatedDate), Parameter: nameof(OntologyConceptMap.CreatedDate)),
                (Rule: IsInvalid(ontologyConceptMap.CreatedBy), Parameter: nameof(OntologyConceptMap.CreatedBy)),
                (Rule: IsInvalid(ontologyConceptMap.UpdatedDate), Parameter: nameof(OntologyConceptMap.UpdatedDate)),
                (Rule: IsInvalid(ontologyConceptMap.UpdatedBy), Parameter: nameof(OntologyConceptMap.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: ontologyConceptMap.UpdatedDate,
                    secondDate: ontologyConceptMap.CreatedDate,
                    secondDateName: nameof(OntologyConceptMap.CreatedDate)),
                Parameter: nameof(OntologyConceptMap.UpdatedDate)));
        }

        private static void ValidateOntologyConceptMapIsNotNull(OntologyConceptMap ontologyConceptMap)
        {
            if (ontologyConceptMap is null)
            {
                throw new NullOntologyConceptMapException(message: "OntologyConceptMap is null.");
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidOntologyConceptMapException = 
                new InvalidOntologyConceptMapException(
                    message: "Invalid ontologyConceptMap. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidOntologyConceptMapException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidOntologyConceptMapException.ThrowIfContainsErrors();
        }
    }
}