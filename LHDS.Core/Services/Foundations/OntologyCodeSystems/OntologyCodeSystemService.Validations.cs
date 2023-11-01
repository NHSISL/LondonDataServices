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
                    first: ontologyCodeSystem.UpdatedBy,
                    second: ontologyCodeSystem.CreatedBy,
                    secondName: nameof(OntologyCodeSystem.CreatedBy)),
                Parameter: nameof(OntologyCodeSystem.UpdatedBy)),

                (Rule: IsNotRecent(ontologyCodeSystem.CreatedDate), Parameter: nameof(OntologyCodeSystem.CreatedDate)));
        }

        private void ValidateOntologyCodeSystemOnModify(OntologyCodeSystem ontologyCodeSystem)
        {
            ValidateOntologyCodeSystemIsNotNull(ontologyCodeSystem);

            Validate(
                (Rule: IsInvalid(ontologyCodeSystem.Id), Parameter: nameof(OntologyCodeSystem.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(ontologyCodeSystem.CreatedDate), Parameter: nameof(OntologyCodeSystem.CreatedDate)),
                (Rule: IsInvalid(ontologyCodeSystem.CreatedBy), Parameter: nameof(OntologyCodeSystem.CreatedBy)),
                (Rule: IsInvalid(ontologyCodeSystem.UpdatedDate), Parameter: nameof(OntologyCodeSystem.UpdatedDate)),
                (Rule: IsInvalid(ontologyCodeSystem.UpdatedBy), Parameter: nameof(OntologyCodeSystem.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: ontologyCodeSystem.UpdatedDate,
                    secondDate: ontologyCodeSystem.CreatedDate,
                    secondDateName: nameof(OntologyCodeSystem.CreatedDate)),
                Parameter: nameof(OntologyCodeSystem.UpdatedDate)),

                (Rule: IsNotRecent(ontologyCodeSystem.UpdatedDate), Parameter: nameof(ontologyCodeSystem.UpdatedDate)));
        }

        public void ValidateOntologyCodeSystemId(Guid ontologyCodeSystemId) =>
            Validate((Rule: IsInvalid(ontologyCodeSystemId), Parameter: nameof(OntologyCodeSystem.Id)));

        private static void ValidateStorageOntologyCodeSystem(OntologyCodeSystem maybeOntologyCodeSystem, Guid ontologyCodeSystemId)
        {
            if (maybeOntologyCodeSystem is null)
            {
                throw new NotFoundOntologyCodeSystemException(ontologyCodeSystemId);
            }
        }

        private static void ValidateOntologyCodeSystemIsNotNull(OntologyCodeSystem ontologyCodeSystem)
        {
            if (ontologyCodeSystem is null)
            {
                throw new NullOntologyCodeSystemException(message: "OntologyCodeSystem is null.");
            }
        }

        private static void ValidateAgainstStorageOntologyCodeSystemOnModify(OntologyCodeSystem inputOntologyCodeSystem, OntologyCodeSystem storageOntologyCodeSystem)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputOntologyCodeSystem.CreatedDate,
                    secondDate: storageOntologyCodeSystem.CreatedDate,
                    secondDateName: nameof(OntologyCodeSystem.CreatedDate)),
                Parameter: nameof(OntologyCodeSystem.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputOntologyCodeSystem.CreatedBy,
                    second: storageOntologyCodeSystem.CreatedBy,
                    secondName: nameof(OntologyCodeSystem.CreatedBy)),
                Parameter: nameof(OntologyCodeSystem.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputOntologyCodeSystem.UpdatedDate,
                    secondDate: storageOntologyCodeSystem.UpdatedDate,
                    secondDateName: nameof(OntologyCodeSystem.UpdatedDate)),
                Parameter: nameof(OntologyCodeSystem.UpdatedDate)));
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

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
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

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTimeOffset();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

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