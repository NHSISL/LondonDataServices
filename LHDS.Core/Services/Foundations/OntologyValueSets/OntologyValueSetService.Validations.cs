using System;
using LHDS.Core.Models.Foundations.OntologyValueSets;
using LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions;

namespace LHDS.Core.Services.Foundations.OntologyValueSets
{
    public partial class OntologyValueSetService
    {
        private void ValidateOntologyValueSetOnAdd(OntologyValueSet ontologyValueSet)
        {
            ValidateOntologyValueSetIsNotNull(ontologyValueSet);

            Validate(
                (Rule: IsInvalid(ontologyValueSet.Id), Parameter: nameof(OntologyValueSet.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(ontologyValueSet.CreatedDate), Parameter: nameof(OntologyValueSet.CreatedDate)),
                (Rule: IsInvalid(ontologyValueSet.CreatedBy), Parameter: nameof(OntologyValueSet.CreatedBy)),
                (Rule: IsInvalid(ontologyValueSet.UpdatedDate), Parameter: nameof(OntologyValueSet.UpdatedDate)),
                (Rule: IsInvalid(ontologyValueSet.UpdatedBy), Parameter: nameof(OntologyValueSet.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: ontologyValueSet.UpdatedDate,
                    secondDate: ontologyValueSet.CreatedDate,
                    secondDateName: nameof(OntologyValueSet.CreatedDate)),
                Parameter: nameof(OntologyValueSet.UpdatedDate)));
        }

        private static void ValidateOntologyValueSetIsNotNull(OntologyValueSet ontologyValueSet)
        {
            if (ontologyValueSet is null)
            {
                throw new NullOntologyValueSetException(message: "OntologyValueSet is null.");
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
            var invalidOntologyValueSetException = 
                new InvalidOntologyValueSetException(
                    message: "Invalid ontologyValueSet. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidOntologyValueSetException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidOntologyValueSetException.ThrowIfContainsErrors();
        }
    }
}