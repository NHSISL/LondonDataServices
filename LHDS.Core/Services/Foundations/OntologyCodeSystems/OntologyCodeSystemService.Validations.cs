using System;
using LHDS.Core.Models.Foundations.OntologyCodeSystems;
using LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions;

namespace LHDS.Core.Services.Foundations.OntologyCodeSystems
{
    public partial class OntologyCodeSystemService
    {
        private void ValidateOntologyCodeSystemOnAdd(OntologyCodeSystem ontologyCodeSystem)
        {
            ValidateOntologyCodeSystemIsNotNull(ontologyCodeSystem);

            Validate(
                (Rule: IsInvalid(ontologyCodeSystem.Id), Parameter: nameof(OntologyCodeSystem.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(ontologyCodeSystem.CreatedDate), Parameter: nameof(OntologyCodeSystem.CreatedDate)),
                (Rule: IsInvalid(ontologyCodeSystem.CreatedBy), Parameter: nameof(OntologyCodeSystem.CreatedBy)),
                (Rule: IsInvalid(ontologyCodeSystem.UpdatedDate), Parameter: nameof(OntologyCodeSystem.UpdatedDate)),
                (Rule: IsInvalid(ontologyCodeSystem.UpdatedBy), Parameter: nameof(OntologyCodeSystem.UpdatedBy)));
        }

        private static void ValidateOntologyCodeSystemIsNotNull(OntologyCodeSystem ontologyCodeSystem)
        {
            if (ontologyCodeSystem is null)
            {
                throw new NullOntologyCodeSystemException(message: "OntologyCodeSystem is null.");
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

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidOntologyCodeSystemException = 
                new InvalidOntologyCodeSystemException(
                    message: "Invalid ontologyCodeSystem. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidOntologyCodeSystemException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidOntologyCodeSystemException.ThrowIfContainsErrors();
        }
    }
}