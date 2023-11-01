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
                Parameter: nameof(OntologyConceptMap.UpdatedDate)),

                (Rule: IsNotSame(
                    first: ontologyConceptMap.UpdatedBy,
                    second: ontologyConceptMap.CreatedBy,
                    secondName: nameof(OntologyConceptMap.CreatedBy)),
                Parameter: nameof(OntologyConceptMap.UpdatedBy)),

                (Rule: IsNotRecent(ontologyConceptMap.CreatedDate), Parameter: nameof(OntologyConceptMap.CreatedDate)));
        }

        private void ValidateOntologyConceptMapOnModify(OntologyConceptMap ontologyConceptMap)
        {
            ValidateOntologyConceptMapIsNotNull(ontologyConceptMap);

            Validate(
                (Rule: IsInvalid(ontologyConceptMap.Id), Parameter: nameof(OntologyConceptMap.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(ontologyConceptMap.CreatedDate), Parameter: nameof(OntologyConceptMap.CreatedDate)),
                (Rule: IsInvalid(ontologyConceptMap.CreatedBy), Parameter: nameof(OntologyConceptMap.CreatedBy)),
                (Rule: IsInvalid(ontologyConceptMap.UpdatedDate), Parameter: nameof(OntologyConceptMap.UpdatedDate)),
                (Rule: IsInvalid(ontologyConceptMap.UpdatedBy), Parameter: nameof(OntologyConceptMap.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: ontologyConceptMap.UpdatedDate,
                    secondDate: ontologyConceptMap.CreatedDate,
                    secondDateName: nameof(OntologyConceptMap.CreatedDate)),
                Parameter: nameof(OntologyConceptMap.UpdatedDate)),

                (Rule: IsNotRecent(ontologyConceptMap.UpdatedDate), Parameter: nameof(ontologyConceptMap.UpdatedDate)));
        }

        public void ValidateOntologyConceptMapId(Guid ontologyConceptMapId) =>
            Validate((Rule: IsInvalid(ontologyConceptMapId), Parameter: nameof(OntologyConceptMap.Id)));

        private static void ValidateStorageOntologyConceptMap(OntologyConceptMap maybeOntologyConceptMap, Guid ontologyConceptMapId)
        {
            if (maybeOntologyConceptMap is null)
            {
                throw new NotFoundOntologyConceptMapException(ontologyConceptMapId);
            }
        }

        private static void ValidateOntologyConceptMapIsNotNull(OntologyConceptMap ontologyConceptMap)
        {
            if (ontologyConceptMap is null)
            {
                throw new NullOntologyConceptMapException(message: "OntologyConceptMap is null.");
            }
        }

        private static void ValidateAgainstStorageOntologyConceptMapOnModify(OntologyConceptMap inputOntologyConceptMap, OntologyConceptMap storageOntologyConceptMap)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputOntologyConceptMap.CreatedDate,
                    secondDate: storageOntologyConceptMap.CreatedDate,
                    secondDateName: nameof(OntologyConceptMap.CreatedDate)),
                Parameter: nameof(OntologyConceptMap.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputOntologyConceptMap.CreatedBy,
                    second: storageOntologyConceptMap.CreatedBy,
                    secondName: nameof(OntologyConceptMap.CreatedBy)),
                Parameter: nameof(OntologyConceptMap.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputOntologyConceptMap.UpdatedDate,
                    secondDate: storageOntologyConceptMap.UpdatedDate,
                    secondDateName: nameof(OntologyConceptMap.UpdatedDate)),
                Parameter: nameof(OntologyConceptMap.UpdatedDate)));
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