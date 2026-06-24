// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.AddressToUprnFileLogs
{
    public partial class AddressToUprnFileLogService
    {
        private async ValueTask ValidateAddressToUprnFileLogOnAddAsync(
            AddressToUprnFileLog addressToUprnFileLog)
        {
            ValidateAddressToUprnFileLogIsNotNull(addressToUprnFileLog);
            string currentUserId = await this.securityAuditBroker.GetUserIdAsync();

            Validate<InvalidAddressToUprnFileLogException>(
                createException: () => new InvalidAddressToUprnFileLogException(
                    message: "Invalid address to UPRN file log. Please correct the errors and try again."),

                (Rule: IsInvalid(addressToUprnFileLog.Id), Parameter: nameof(AddressToUprnFileLog.Id)),
                (Rule: IsInvalid(addressToUprnFileLog.FileName), Parameter: nameof(AddressToUprnFileLog.FileName)),
                (Rule: IsInvalid(addressToUprnFileLog.CreatedWhen),
                    Parameter: nameof(AddressToUprnFileLog.CreatedWhen)),
                (Rule: IsInvalid(addressToUprnFileLog.CreatedBy), Parameter: nameof(AddressToUprnFileLog.CreatedBy)),
                (Rule: IsInvalid(addressToUprnFileLog.UpdatedWhen),
                    Parameter: nameof(AddressToUprnFileLog.UpdatedWhen)),
                (Rule: IsInvalid(addressToUprnFileLog.UpdatedBy), Parameter: nameof(AddressToUprnFileLog.UpdatedBy)),

                (Rule: IsExceedingLength(addressToUprnFileLog.FileName, 450),
                    Parameter: nameof(AddressToUprnFileLog.FileName)),

                (Rule: IsNotSame(
                    first: currentUserId,
                    second: addressToUprnFileLog.CreatedBy),
                Parameter: nameof(AddressToUprnFileLog.CreatedBy)),

                (Rule: IsNotSame(
                    firstDate: addressToUprnFileLog.UpdatedWhen,
                    secondDate: addressToUprnFileLog.CreatedWhen,
                    secondDateName: nameof(AddressToUprnFileLog.CreatedWhen)),
                Parameter: nameof(AddressToUprnFileLog.UpdatedWhen)),

                (Rule: IsNotSame(
                    first: addressToUprnFileLog.UpdatedBy,
                    second: addressToUprnFileLog.CreatedBy,
                    secondName: nameof(AddressToUprnFileLog.CreatedBy)),
                Parameter: nameof(AddressToUprnFileLog.UpdatedBy)),

                (Rule: IsExceedingLength(addressToUprnFileLog.CreatedBy, 255),
                    Parameter: nameof(AddressToUprnFileLog.CreatedBy)),

                (Rule: IsExceedingLength(addressToUprnFileLog.UpdatedBy, 255),
                    Parameter: nameof(AddressToUprnFileLog.UpdatedBy)),

                (Rule: await IsNotRecentAsync(addressToUprnFileLog.CreatedWhen),
                    Parameter: nameof(AddressToUprnFileLog.CreatedWhen)));
        }

        private async ValueTask ValidateAddressToUprnFileLogOnModifyAsync(
            AddressToUprnFileLog addressToUprnFileLog)
        {
            ValidateAddressToUprnFileLogIsNotNull(addressToUprnFileLog);
            string currentUserId = await this.securityAuditBroker.GetUserIdAsync();

            Validate<InvalidAddressToUprnFileLogException>(
                createException: () => new InvalidAddressToUprnFileLogException(
                    message: "Invalid address to UPRN file log. Please correct the errors and try again."),

                (Rule: IsInvalid(addressToUprnFileLog.Id), Parameter: nameof(AddressToUprnFileLog.Id)),
                (Rule: IsInvalid(addressToUprnFileLog.FileName), Parameter: nameof(AddressToUprnFileLog.FileName)),
                (Rule: IsInvalid(addressToUprnFileLog.CreatedWhen),
                    Parameter: nameof(AddressToUprnFileLog.CreatedWhen)),
                (Rule: IsInvalid(addressToUprnFileLog.CreatedBy), Parameter: nameof(AddressToUprnFileLog.CreatedBy)),
                (Rule: IsInvalid(addressToUprnFileLog.UpdatedWhen),
                    Parameter: nameof(AddressToUprnFileLog.UpdatedWhen)),
                (Rule: IsInvalid(addressToUprnFileLog.UpdatedBy), Parameter: nameof(AddressToUprnFileLog.UpdatedBy)),

                (Rule: IsExceedingLength(addressToUprnFileLog.FileName, 450),
                    Parameter: nameof(AddressToUprnFileLog.FileName)),

                (Rule: IsNotSame(
                    first: currentUserId,
                    second: addressToUprnFileLog.UpdatedBy),
                Parameter: nameof(AddressToUprnFileLog.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: addressToUprnFileLog.UpdatedWhen,
                    secondDate: addressToUprnFileLog.CreatedWhen,
                    secondDateName: nameof(AddressToUprnFileLog.CreatedWhen)),
                Parameter: nameof(AddressToUprnFileLog.UpdatedWhen)),

                (Rule: IsExceedingLength(addressToUprnFileLog.CreatedBy, 255),
                    Parameter: nameof(AddressToUprnFileLog.CreatedBy)),

                (Rule: IsExceedingLength(addressToUprnFileLog.UpdatedBy, 255),
                    Parameter: nameof(AddressToUprnFileLog.UpdatedBy)),

                (Rule: await IsNotRecentAsync(addressToUprnFileLog.UpdatedWhen),
                    Parameter: nameof(AddressToUprnFileLog.UpdatedWhen)));
        }

        private void ValidateAddressToUprnFileLogId(Guid addressToUprnFileLogId)
        {
            Validate<InvalidAddressToUprnFileLogException>(
                createException: () => new InvalidAddressToUprnFileLogException(
                    message: "Invalid address to UPRN file log. Please correct the errors and try again."),

                (Rule: IsInvalid(addressToUprnFileLogId), Parameter: nameof(AddressToUprnFileLog.Id)));
        }

        private static void ValidateStorageAddressToUprnFileLog(
            AddressToUprnFileLog maybeAddressToUprnFileLog,
            Guid addressToUprnFileLogId)
        {
            if (maybeAddressToUprnFileLog is null)
            {
                throw new NotFoundAddressToUprnFileLogException(addressToUprnFileLogId);
            }
        }

        private static void ValidateAddressToUprnFileLogIsNotNull(AddressToUprnFileLog addressToUprnFileLog)
        {
            if (addressToUprnFileLog is null)
            {
                throw new NullAddressToUprnFileLogException(message: "Address to UPRN file log is null.");
            }
        }

        private void ValidateAgainstStorageAddressToUprnFileLogOnModify(
            AddressToUprnFileLog inputAddressToUprnFileLog,
            AddressToUprnFileLog storageAddressToUprnFileLog)
        {
            Validate<InvalidAddressToUprnFileLogException>(
                createException: () => new InvalidAddressToUprnFileLogException(
                    message: "Invalid address to UPRN file log. Please correct the errors and try again."),

                (Rule: IsNotSame(
                    firstDate: inputAddressToUprnFileLog.CreatedWhen,
                    secondDate: storageAddressToUprnFileLog.CreatedWhen,
                    secondDateName: nameof(AddressToUprnFileLog.CreatedWhen)),
                Parameter: nameof(AddressToUprnFileLog.CreatedWhen)),

                (Rule: IsNotSame(
                    first: inputAddressToUprnFileLog.CreatedBy,
                    second: storageAddressToUprnFileLog.CreatedBy,
                    secondName: nameof(AddressToUprnFileLog.CreatedBy)),
                Parameter: nameof(AddressToUprnFileLog.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputAddressToUprnFileLog.UpdatedWhen,
                    secondDate: storageAddressToUprnFileLog.UpdatedWhen,
                    secondDateName: nameof(AddressToUprnFileLog.UpdatedWhen)),
                Parameter: nameof(AddressToUprnFileLog.UpdatedWhen)));
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

        private static dynamic IsExceedingLength(string? text, int maxLength) => new
        {
            Condition = (text ?? string.Empty).Length > maxLength,
            Message = "Text is exceeding max length"
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
