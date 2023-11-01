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
                (Rule: IsInvalid(ontologyCodeSystem.UpdatedBy), Parameter: nameof(OntologyCodeSystem.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: ontologyCodeSystem.UpdatedDate,
                    secondDate: ontologyCodeSystem.CreatedDate,
                    secondDateName: nameof(OntologyCodeSystem.CreatedDate)),
                Parameter: nameof(OntologyCodeSystem.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: ontologyCodeSystem.UpdatedBy,
                    secondId: ontologyCodeSystem.CreatedBy,
                    secondIdName: nameof(OntologyCodeSystem.CreatedBy)),
                Parameter: nameof(OntologyCodeSystem.UpdatedBy)));
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

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };

        private static dynamic IsNotSame(
           string first,
           string second,
           string secondName) => new
           {
               Condition = first != second,
               Message = $"Text is not the same as {secondName}"
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