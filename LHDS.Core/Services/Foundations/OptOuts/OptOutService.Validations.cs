// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;

namespace LHDS.Core.Services.Foundations.OptOuts
{
    public partial class OptOutService
    {
        private void ValidateOptOutOnAdd(OptOut optOut)
        {
            ValidateOptOutIsNotNull(optOut);

            Validate(
                (Rule: IsInvalid(optOut.Id), Parameter: nameof(OptOut.Id)),
                (Rule: IsInvalid(optOut.NhsNumber), Parameter: nameof(OptOut.NhsNumber)),
                (Rule: IsInvalid(optOut.OptOutStatus), Parameter: nameof(OptOut.OptOutStatus)),
                (Rule: IsInvalid(optOut.CreatedDate), Parameter: nameof(OptOut.CreatedDate)),
                (Rule: IsInvalid(optOut.CreatedBy), Parameter: nameof(OptOut.CreatedBy)),
                (Rule: IsInvalid(optOut.UpdatedDate), Parameter: nameof(OptOut.UpdatedDate)),
                (Rule: IsInvalid(optOut.UpdatedBy), Parameter: nameof(OptOut.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: optOut.UpdatedDate,
                    secondDate: optOut.CreatedDate,
                    secondDateName: nameof(OptOut.CreatedDate)),
                Parameter: nameof(OptOut.UpdatedDate)),

                (Rule: IsNotSame(
                    first: optOut.UpdatedBy,
                    second: optOut.CreatedBy,
                    secondName: nameof(OptOut.CreatedBy)),
                Parameter: nameof(OptOut.UpdatedBy)),

                (Rule: IsNotRecent(optOut.CreatedDate), Parameter: nameof(OptOut.CreatedDate)));
        }

        private void ValidateOptOutOnModify(OptOut optOut)
        {
            ValidateOptOutIsNotNull(optOut);

            Validate(
                (Rule: IsInvalid(optOut.Id), Parameter: nameof(OptOut.Id)),
                (Rule: IsInvalid(optOut.NhsNumber), Parameter: nameof(OptOut.NhsNumber)),
                (Rule: IsInvalid(optOut.OptOutStatus), Parameter: nameof(OptOut.OptOutStatus)),
                (Rule: IsInvalid(optOut.CreatedDate), Parameter: nameof(OptOut.CreatedDate)),
                (Rule: IsInvalid(optOut.CreatedBy), Parameter: nameof(OptOut.CreatedBy)),
                (Rule: IsInvalid(optOut.UpdatedDate), Parameter: nameof(OptOut.UpdatedDate)),
                (Rule: IsInvalid(optOut.UpdatedBy), Parameter: nameof(OptOut.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: optOut.UpdatedDate,
                    secondDate: optOut.CreatedDate,
                    secondDateName: nameof(OptOut.CreatedDate)),
                Parameter: nameof(OptOut.UpdatedDate)),

                (Rule: IsNotRecent(optOut.UpdatedDate), Parameter: nameof(optOut.UpdatedDate)));
        }

        public void ValidateOptOutId(Guid optOutId) =>
            Validate((Rule: IsInvalid(optOutId), Parameter: nameof(OptOut.Id)));

        private static void ValidateStorageOptOut(OptOut maybeOptOut, Guid optOutId)
        {
            if (maybeOptOut is null)
            {
                throw new NotFoundOptOutException(optOutId);
            }
        }

        private static void ValidateOptOutIsNotNull(OptOut optOut)
        {
            if (optOut is null)
            {
                throw new NullOptOutException();
            }
        }

        private static void ValidateAgainstStorageOptOutOnModify(OptOut inputOptOut, OptOut storageOptOut)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputOptOut.CreatedDate,
                    secondDate: storageOptOut.CreatedDate,
                    secondDateName: nameof(OptOut.CreatedDate)),
                Parameter: nameof(OptOut.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputOptOut.CreatedBy,
                    second: storageOptOut.CreatedBy,
                    secondName: nameof(OptOut.CreatedBy)),
                Parameter: nameof(OptOut.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputOptOut.UpdatedDate,
                    secondDate: storageOptOut.UpdatedDate,
                    secondDateName: nameof(OptOut.UpdatedDate)),
                Parameter: nameof(OptOut.UpdatedDate)));
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
           string first,
           string second,
           string secondName) => new
           {
               Condition = first != second,
               Message = $"Text is not the same as {secondName}"
           };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
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
            var invalidOptOutException = new InvalidOptOutException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidOptOutException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidOptOutException.ThrowIfContainsErrors();
        }
    }
}