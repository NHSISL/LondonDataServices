// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
                (Rule: IsInvalid(ontologyValueSet.FullUrl), Parameter: nameof(OntologyValueSet.FullUrl)),
                (Rule: IsInvalid(ontologyValueSet.ResourceType), Parameter: nameof(OntologyValueSet.ResourceType)),
                (Rule: IsInvalid(ontologyValueSet.CreatedDate), Parameter: nameof(OntologyValueSet.CreatedDate)),
                (Rule: IsInvalid(ontologyValueSet.CreatedBy), Parameter: nameof(OntologyValueSet.CreatedBy)),
                (Rule: IsInvalid(ontologyValueSet.UpdatedDate), Parameter: nameof(OntologyValueSet.UpdatedDate)),
                (Rule: IsInvalid(ontologyValueSet.UpdatedBy), Parameter: nameof(OntologyValueSet.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: ontologyValueSet.UpdatedDate,
                    secondDate: ontologyValueSet.CreatedDate,
                    secondDateName: nameof(OntologyValueSet.CreatedDate)),
                Parameter: nameof(OntologyValueSet.UpdatedDate)),

                (Rule: IsNotSame(
                    first: ontologyValueSet.UpdatedBy,
                    second: ontologyValueSet.CreatedBy,
                    secondName: nameof(OntologyValueSet.CreatedBy)),
                Parameter: nameof(OntologyValueSet.UpdatedBy)),

                (Rule: IsNotRecent(ontologyValueSet.CreatedDate), Parameter: nameof(OntologyValueSet.CreatedDate)));
        }

        private void ValidateOntologyValueSetOnModify(OntologyValueSet ontologyValueSet)
        {
            ValidateOntologyValueSetIsNotNull(ontologyValueSet);

            Validate(
                (Rule: IsInvalid(ontologyValueSet.Id), Parameter: nameof(OntologyValueSet.Id)),
                (Rule: IsInvalid(ontologyValueSet.FullUrl), Parameter: nameof(OntologyValueSet.FullUrl)),
                (Rule: IsInvalid(ontologyValueSet.ResourceType), Parameter: nameof(OntologyValueSet.ResourceType)),
                (Rule: IsInvalid(ontologyValueSet.CreatedDate), Parameter: nameof(OntologyValueSet.CreatedDate)),
                (Rule: IsInvalid(ontologyValueSet.CreatedBy), Parameter: nameof(OntologyValueSet.CreatedBy)),
                (Rule: IsInvalid(ontologyValueSet.UpdatedDate), Parameter: nameof(OntologyValueSet.UpdatedDate)),
                (Rule: IsInvalid(ontologyValueSet.UpdatedBy), Parameter: nameof(OntologyValueSet.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: ontologyValueSet.UpdatedDate,
                    secondDate: ontologyValueSet.CreatedDate,
                    secondDateName: nameof(OntologyValueSet.CreatedDate)),
                Parameter: nameof(OntologyValueSet.UpdatedDate)),

                (Rule: IsNotRecent(ontologyValueSet.UpdatedDate), Parameter: nameof(ontologyValueSet.UpdatedDate)));
        }

        public void ValidateOntologyValueSetId(Guid ontologyValueSetId) =>
            Validate((Rule: IsInvalid(ontologyValueSetId), Parameter: nameof(OntologyValueSet.Id)));

        private static void ValidateStorageOntologyValueSet(OntologyValueSet maybeOntologyValueSet, Guid ontologyValueSetId)
        {
            if (maybeOntologyValueSet is null)
            {
                throw new NotFoundOntologyValueSetException(ontologyValueSetId);
            }
        }

        private static void ValidateOntologyValueSetIsNotNull(OntologyValueSet ontologyValueSet)
        {
            if (ontologyValueSet is null)
            {
                throw new NullOntologyValueSetException(message: "OntologyValueSet is null.");
            }
        }

        private static void ValidateAgainstStorageOntologyValueSetOnModify(OntologyValueSet inputOntologyValueSet, OntologyValueSet storageOntologyValueSet)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputOntologyValueSet.CreatedDate,
                    secondDate: storageOntologyValueSet.CreatedDate,
                    secondDateName: nameof(OntologyValueSet.CreatedDate)),
                Parameter: nameof(OntologyValueSet.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputOntologyValueSet.CreatedBy,
                    second: storageOntologyValueSet.CreatedBy,
                    secondName: nameof(OntologyValueSet.CreatedBy)),
                Parameter: nameof(OntologyValueSet.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputOntologyValueSet.UpdatedDate,
                    secondDate: storageOntologyValueSet.UpdatedDate,
                    secondDateName: nameof(OntologyValueSet.UpdatedDate)),
                Parameter: nameof(OntologyValueSet.UpdatedDate)));
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