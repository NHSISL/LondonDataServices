// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;

namespace LHDS.Core.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactService
    {
        private void ValidateTerminologyArtifactOnAdd(TerminologyArtifact terminologyArtifact)
        {
            ValidateTerminologyArtifactIsNotNull(terminologyArtifact);

            Validate(
                (Rule: IsInvalid(terminologyArtifact.Id), Parameter: nameof(TerminologyArtifact.Id)),
                (Rule: IsInvalid(terminologyArtifact.FullUrl), Parameter: nameof(TerminologyArtifact.FullUrl)),
                (Rule: IsInvalid(terminologyArtifact.ResourceType), Parameter: nameof(TerminologyArtifact.ResourceType)),
                (Rule: IsInvalid(terminologyArtifact.CreatedDate), Parameter: nameof(TerminologyArtifact.CreatedDate)),
                (Rule: IsInvalid(terminologyArtifact.CreatedBy), Parameter: nameof(TerminologyArtifact.CreatedBy)),
                (Rule: IsInvalid(terminologyArtifact.UpdatedDate), Parameter: nameof(TerminologyArtifact.UpdatedDate)),
                (Rule: IsInvalid(terminologyArtifact.UpdatedBy), Parameter: nameof(TerminologyArtifact.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: terminologyArtifact.UpdatedDate,
                    secondDate: terminologyArtifact.CreatedDate,
                    secondDateName: nameof(TerminologyArtifact.CreatedDate)),
                Parameter: nameof(TerminologyArtifact.UpdatedDate)),

                (Rule: IsNotSame(
                    first: terminologyArtifact.UpdatedBy,
                    second: terminologyArtifact.CreatedBy,
                    secondName: nameof(TerminologyArtifact.CreatedBy)),
                Parameter: nameof(TerminologyArtifact.UpdatedBy)),

                (Rule: IsNotRecent(terminologyArtifact.CreatedDate), Parameter: nameof(TerminologyArtifact.CreatedDate)));
        }

        private void ValidateTerminologyArtifactOnModify(TerminologyArtifact terminologyArtifact)
        {
            ValidateTerminologyArtifactIsNotNull(terminologyArtifact);

            Validate(
                (Rule: IsInvalid(terminologyArtifact.Id), Parameter: nameof(TerminologyArtifact.Id)),
                (Rule: IsInvalid(terminologyArtifact.FullUrl), Parameter: nameof(TerminologyArtifact.FullUrl)),
                (Rule: IsInvalid(terminologyArtifact.ResourceType), Parameter: nameof(TerminologyArtifact.ResourceType)),
                (Rule: IsInvalid(terminologyArtifact.CreatedDate), Parameter: nameof(TerminologyArtifact.CreatedDate)),
                (Rule: IsInvalid(terminologyArtifact.CreatedBy), Parameter: nameof(TerminologyArtifact.CreatedBy)),
                (Rule: IsInvalid(terminologyArtifact.UpdatedDate), Parameter: nameof(TerminologyArtifact.UpdatedDate)),
                (Rule: IsInvalid(terminologyArtifact.UpdatedBy), Parameter: nameof(TerminologyArtifact.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: terminologyArtifact.UpdatedDate,
                    secondDate: terminologyArtifact.CreatedDate,
                    secondDateName: nameof(TerminologyArtifact.CreatedDate)),
                Parameter: nameof(TerminologyArtifact.UpdatedDate)),

                (Rule: IsNotRecent(terminologyArtifact.UpdatedDate), Parameter: nameof(terminologyArtifact.UpdatedDate)));
        }

        public void ValidateTerminologyArtifactId(Guid terminologyArtifactId) =>
            Validate((Rule: IsInvalid(terminologyArtifactId), Parameter: nameof(TerminologyArtifact.Id)));

        private static void ValidateStorageTerminologyArtifact(
            TerminologyArtifact maybeTerminologyArtifact,
            Guid terminologyArtifactId)
        {
            if (maybeTerminologyArtifact is null)
            {
                throw new NotFoundTerminologyArtifactException(terminologyArtifactId);
            }
        }

        private static void ValidateTerminologyArtifactIsNotNull(TerminologyArtifact terminologyArtifact)
        {
            if (terminologyArtifact is null)
            {
                throw new NullTerminologyArtifactException(message: "TerminologyArtifact is null.");
            }
        }

        private static void ValidateAgainstStorageTerminologyArtifactOnModify(
            TerminologyArtifact inputTerminologyArtifact,
            TerminologyArtifact storageTerminologyArtifact)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputTerminologyArtifact.CreatedDate,
                    secondDate: storageTerminologyArtifact.CreatedDate,
                    secondDateName: nameof(TerminologyArtifact.CreatedDate)),
                Parameter: nameof(TerminologyArtifact.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputTerminologyArtifact.CreatedBy,
                    second: storageTerminologyArtifact.CreatedBy,
                    secondName: nameof(TerminologyArtifact.CreatedBy)),
                Parameter: nameof(TerminologyArtifact.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputTerminologyArtifact.UpdatedDate,
                    secondDate: storageTerminologyArtifact.UpdatedDate,
                    secondDateName: nameof(TerminologyArtifact.UpdatedDate)),
                Parameter: nameof(TerminologyArtifact.UpdatedDate)));
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string? text) => new
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
            var invalidTerminologyArtifactException =
                new InvalidTerminologyArtifactException(
                    message: "Invalid terminologyArtifact. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidTerminologyArtifactException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidTerminologyArtifactException.ThrowIfContainsErrors();
        }
    }
}