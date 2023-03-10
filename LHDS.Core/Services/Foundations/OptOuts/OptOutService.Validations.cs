using System;
using LHDS.Core.Models.OptOuts;
using LHDS.Core.Models.OptOuts.Exceptions;

namespace LHDS.Core.Services.Foundations.OptOuts
{
    public partial class OptOutService
    {
        private void ValidateOptOutOnAdd(OptOut optOut)
        {
            ValidateOptOutIsNotNull(optOut);

            Validate(
                (Rule: IsInvalid(optOut.Id), Parameter: nameof(OptOut.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(optOut.CreatedDate), Parameter: nameof(OptOut.CreatedDate)),
                (Rule: IsInvalid(optOut.CreatedByUserId), Parameter: nameof(OptOut.CreatedByUserId)),
                (Rule: IsInvalid(optOut.UpdatedDate), Parameter: nameof(OptOut.UpdatedDate)),
                (Rule: IsInvalid(optOut.UpdatedByUserId), Parameter: nameof(OptOut.UpdatedByUserId)),

                (Rule: IsNotSame(
                    firstDate: optOut.UpdatedDate,
                    secondDate: optOut.CreatedDate,
                    secondDateName: nameof(OptOut.CreatedDate)),
                Parameter: nameof(OptOut.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: optOut.UpdatedByUserId,
                    secondId: optOut.CreatedByUserId,
                    secondIdName: nameof(OptOut.CreatedByUserId)),
                Parameter: nameof(OptOut.UpdatedByUserId)),

                (Rule: IsNotRecent(optOut.CreatedDate), Parameter: nameof(OptOut.CreatedDate)));
        }

        public void ValidateOptOutId(Guid optOutId) =>
            Validate((Rule: IsInvalid(optOutId), Parameter: nameof(OptOut.Id)));

        private static void ValidateOptOutIsNotNull(OptOut optOut)
        {
            if (optOut is null)
            {
                throw new NullOptOutException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
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