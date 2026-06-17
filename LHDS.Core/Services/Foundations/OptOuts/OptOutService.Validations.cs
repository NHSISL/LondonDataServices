// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.OptOuts
{
    public partial class OptOutService
    {
        private async ValueTask ValidateOptOutOnAddAsync(OptOut optOut)
        {
            ValidateOptOutIsNotNull(optOut);
            string currentUserId = await this.securityAuditBroker.GetUserIdAsync();

            Validate(
                (Rule: IsInvalid(optOut.Id), Parameter: nameof(OptOut.Id)),
                (Rule: IsInvalid(optOut.NhsNumber), Parameter: nameof(OptOut.NhsNumber)),
                (Rule: IsInvalid(optOut.Status), Parameter: nameof(OptOut.Status)),
                (Rule: IsInvalid(optOut.CreatedDate), Parameter: nameof(OptOut.CreatedDate)),
                (Rule: IsInvalid(optOut.CreatedBy), Parameter: nameof(OptOut.CreatedBy)),
                (Rule: IsInvalid(optOut.UpdatedDate), Parameter: nameof(OptOut.UpdatedDate)),
                (Rule: IsInvalid(optOut.UpdatedBy), Parameter: nameof(OptOut.UpdatedBy)),
                (Rule: IsInvalidLength(optOut.NhsNumber, 10), Parameter: nameof(OptOut.NhsNumber)),
                (Rule: IsInvalidLength(optOut.Status, 50), Parameter: nameof(OptOut.Status)),
                (Rule: IsInvalidNhsNumber(optOut.NhsNumber), Parameter: nameof(OptOut.NhsNumber)),

                (Rule: IsNotSame(
                    first: currentUserId,
                    second: optOut.CreatedBy),
                Parameter: nameof(OptOut.CreatedBy)),

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

                (Rule: IsEqualOrSmallerThan(
                    optOut.CreatedBy, 255), Parameter: nameof(optOut.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    optOut.UpdatedBy, 255), Parameter: nameof(optOut.UpdatedBy)),

                (Rule: await IsNotRecentAsync(optOut.CreatedDate), Parameter: nameof(OptOut.CreatedDate)));
        }

        private async ValueTask ValidateOptOutOnModifyAsync(OptOut optOut)
        {
            ValidateOptOutIsNotNull(optOut);
            string currentUserId = await this.securityAuditBroker.GetUserIdAsync();

            Validate(
                (Rule: IsInvalid(optOut.Id), Parameter: nameof(OptOut.Id)),
                (Rule: IsInvalid(optOut.NhsNumber), Parameter: nameof(OptOut.NhsNumber)),
                (Rule: IsInvalid(optOut.Status), Parameter: nameof(OptOut.Status)),
                (Rule: IsInvalid(optOut.CreatedDate), Parameter: nameof(OptOut.CreatedDate)),
                (Rule: IsInvalid(optOut.CreatedBy), Parameter: nameof(OptOut.CreatedBy)),
                (Rule: IsInvalid(optOut.UpdatedDate), Parameter: nameof(OptOut.UpdatedDate)),
                (Rule: IsInvalid(optOut.UpdatedBy), Parameter: nameof(OptOut.UpdatedBy)),
                (Rule: IsInvalidLength(optOut.NhsNumber, 10), Parameter: nameof(OptOut.NhsNumber)),
                (Rule: IsInvalidLength(optOut.Status, 50), Parameter: nameof(OptOut.Status)),
                (Rule: IsInvalidNhsNumber(optOut.NhsNumber), Parameter: nameof(OptOut.NhsNumber)),

                (Rule: IsNotSame(
                    first: currentUserId,
                    second: optOut.UpdatedBy),
                Parameter: nameof(OptOut.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: optOut.UpdatedDate,
                    secondDate: optOut.CreatedDate,
                    secondDateName: nameof(OptOut.CreatedDate)),
                Parameter: nameof(OptOut.UpdatedDate)),

                (Rule: IsEqualOrSmallerThan(
                    optOut.CreatedBy, 255), Parameter: nameof(optOut.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    optOut.UpdatedBy, 255), Parameter: nameof(optOut.UpdatedBy)),


                (Rule: await IsNotRecentAsync(optOut.UpdatedDate), Parameter: nameof(optOut.UpdatedDate)));
        }

        public void ValidateOptOutId(Guid optOutId) =>
            Validate((Rule: IsInvalid(optOutId), Parameter: nameof(OptOut.Id)));

        private static void ValidateStorageOptOut(OptOut maybeOptOut, Guid optOutId)
        {
            if (maybeOptOut is null)
            {
                throw new NotFoundOptOutException(message: $"Couldn't find optOut with optOutId: {optOutId}.");
            }
        }

        private static void ValidateOptOutIsNotNull(OptOut optOut)
        {
            if (optOut is null)
            {
                throw new NullOptOutException(message: "OptOut is null.");
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

        private async ValueTask ValidateAgainstStorageOptOutOnDeleteAsync(
            OptOut optOut,
            OptOut maybeOptOut)
        {
            string auditUserId = await this.securityAuditBroker.GetUserIdAsync();

            Validate(
                (Rule: IsNotSame(
                    optOut.CreatedDate,
                    maybeOptOut.CreatedDate,
                    nameof(maybeOptOut.CreatedDate)),
                 Parameter: nameof(OptOut.CreatedDate)),

                (Rule: IsNotSame(
                    optOut.CreatedBy,
                    maybeOptOut.CreatedBy,
                    nameof(maybeOptOut.CreatedBy)),
                 Parameter: nameof(OptOut.CreatedBy)),

                (Rule: IsNotSame(
                    maybeOptOut.UpdatedDate,
                    optOut.UpdatedDate,
                    nameof(OptOut.UpdatedDate)),
                 Parameter: nameof(OptOut.UpdatedDate)),

                (Rule: IsNotSame(
                    auditUserId,
                    optOut.UpdatedBy,
                    nameof(OptOut.UpdatedBy)),
                 Parameter: nameof(OptOut.UpdatedBy))
            );
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

        private static dynamic IsInvalidLength(string text, int maxLength) => new
        {
            Condition = text != null && text.Length > maxLength,
            Message = $"Text length should not be greater than {maxLength}"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsEqualOrSmallerThan(string text, int maxLength) => new
        {
            Condition = (text ?? string.Empty).Length > maxLength,
            Message = "Text is exceeding max length"
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
            string first,
            string second) => new
            {
                Condition = first != second,
                Message = $"Expected value to be '{first}' but found '{second}'."
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

        private async ValueTask<dynamic> IsNotRecentAsync(DateTimeOffset date) => new
        {
            Condition = await IsDateNotRecentAsync(date),
            Message = "Date is not recent"
        };

        private async ValueTask<bool> IsDateNotRecentAsync(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static dynamic IsInvalidNhsNumber(string nhsNumber) => new
        {
            Condition = IsNhsNumberInvalid(nhsNumber),
            Message = "NHS Number invalid"
        };

        private static bool IsNhsNumberInvalid(string nhsNumber)
        {
            if (nhsNumber == null || nhsNumber.Length != 10)
            {
                return true;
            }

            int[] multiplers = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int currentNumber = 0;
            int currentSum = 0;
            int currentMultipler = 0;
            string currentString = "";
            string checkDigit = nhsNumber.Substring(nhsNumber.Length - 1, 1);
            int checkNumber = Convert.ToInt16(checkDigit);
            int remainder = 0;
            int total = 0;

            for (int i = 0; i <= 8; i++)
            {
                currentString = nhsNumber.Substring(i, 1);

                currentNumber = Convert.ToInt16(currentString);
                currentMultipler = multiplers[i];
                currentSum = currentSum + (currentNumber * currentMultipler);
            }

            remainder = currentSum % 11;
            total = 11 - remainder;

            if (total.Equals(11))
            {
                total = 0;
            }

            if (total.Equals(checkNumber))
            {
                return false;
            }

            return true;
        }

        private void ValidateOnBulkAddOptOuts(
            List<OptOut> optOuts,
            string fileName)
        {
            Validate<InvalidOptOutException>(
                createException: () => new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors"
                        + " and try again."),

                (Rule: IsInvalid(optOuts), Parameter: nameof(optOuts)),
                (Rule: IsInvalid(fileName), Parameter: nameof(fileName)));
        }

        private void ValidateOnBulkModifyOptOuts(
            List<OptOut> optOuts,
            string fileName)
        {
            Validate<InvalidOptOutException>(
                createException: () => new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors"
                        + " and try again."),

                (Rule: IsInvalid(optOuts), Parameter: nameof(optOuts)),
                (Rule: IsInvalid(fileName), Parameter: nameof(fileName)));
        }

        private static dynamic IsInvalid(List<OptOut> optOuts) => new
        {
            Condition = optOuts == null,
            Message = "OptOuts is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidOptOutException = new InvalidOptOutException(
                message: "Invalid optOut. Please correct the errors and try again.");

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

        private static void Validate<T>(
            Func<T> createException,
            params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            T invalidDataException = createException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataException.ThrowIfContainsErrors();
        }
    }
}