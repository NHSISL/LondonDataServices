// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressService
    {
        private async ValueTask ValidateResolvedAddressOnAddAsync(ResolvedAddress resolvedAddress)
        {
            string currentEntraUserId = await this.securityAuditBroker.GetUserIdAsync();

            Validate(
                createException: () => new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again."),

                (Rule: IsInvalid(resolvedAddress.Id), Parameter: nameof(ResolvedAddress.Id)),
                (Rule: IsInvalid(resolvedAddress.UniqueReference), Parameter: nameof(ResolvedAddress.UniqueReference)),

                (Rule: IsInvalid(resolvedAddress.UnstructuredPostalAddress),
                    Parameter: nameof(ResolvedAddress.UnstructuredPostalAddress)),

                (Rule: IsInvalid(resolvedAddress.CreatedDate), Parameter: nameof(ResolvedAddress.CreatedDate)),
                (Rule: IsInvalid(resolvedAddress.CreatedBy), Parameter: nameof(ResolvedAddress.CreatedBy)),
                (Rule: IsInvalid(resolvedAddress.UpdatedDate), Parameter: nameof(ResolvedAddress.UpdatedDate)),
                (Rule: IsInvalid(resolvedAddress.UpdatedBy), Parameter: nameof(ResolvedAddress.UpdatedBy)),

                (Rule: IsNotSame(
                    first: currentEntraUserId,
                    second: resolvedAddress.CreatedBy),
                Parameter: nameof(ResolvedAddress.CreatedBy)),

                (Rule: IsNotSame(
                    firstDate: resolvedAddress.UpdatedDate,
                    secondDate: resolvedAddress.CreatedDate,
                    secondDateName: nameof(ResolvedAddress.CreatedDate)),
                Parameter: nameof(ResolvedAddress.UpdatedDate)),

                (Rule: IsNotSame(
                    first: resolvedAddress.UpdatedBy,
                    second: resolvedAddress.CreatedBy,
                    secondName: nameof(ResolvedAddress.CreatedBy)),
                Parameter: nameof(ResolvedAddress.UpdatedBy)),

                (Rule: await IsNotRecentAsync(resolvedAddress.CreatedDate),
                Parameter: nameof(ResolvedAddress.CreatedDate)));
        }

        private void ValidateOnBulkAddResolvedAddresses(List<ResolvedAddress> resolvedAddresses, string fileName)
        {
            Validate(
                createException: () => new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again."),

                (Rule: IsInvalid(resolvedAddresses), Parameter: nameof(resolvedAddresses)),
                (Rule: IsInvalid(fileName), Parameter: nameof(fileName)));
        }

        private void ValidateOnBulkModifyResolvedAddresses(List<ResolvedAddress> resolvedAddresses)
        {
            Validate(

                createException: () => new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again."),

                (Rule: IsInvalid(resolvedAddresses), Parameter: nameof(resolvedAddresses)));
        }

        private async ValueTask ValidateResolvedAddressOnModifyAsync(ResolvedAddress resolvedAddress)
        {
            string currentEntraUserId = await this.securityAuditBroker.GetUserIdAsync();

            Validate(
                createException: () => new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again."),

                (Rule: IsInvalid(resolvedAddress.Id), Parameter: nameof(ResolvedAddress.Id)),
                (Rule: IsInvalid(resolvedAddress.UniqueReference), Parameter: nameof(ResolvedAddress.UniqueReference)),

                (Rule: IsInvalid(resolvedAddress.UnstructuredPostalAddress),
                    Parameter: nameof(ResolvedAddress.UnstructuredPostalAddress)),

                (Rule: IsInvalid(resolvedAddress.CreatedDate), Parameter: nameof(ResolvedAddress.CreatedDate)),
                (Rule: IsInvalid(resolvedAddress.CreatedBy), Parameter: nameof(ResolvedAddress.CreatedBy)),
                (Rule: IsInvalid(resolvedAddress.UpdatedDate), Parameter: nameof(ResolvedAddress.UpdatedDate)),
                (Rule: IsInvalid(resolvedAddress.UpdatedBy), Parameter: nameof(ResolvedAddress.UpdatedBy)),

                (Rule: IsNotSame(
                    first: currentEntraUserId,
                    second: resolvedAddress.UpdatedBy),
                Parameter: nameof(ResolvedAddress.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: resolvedAddress.UpdatedDate,
                    secondDate: resolvedAddress.CreatedDate,
                    secondDateName: nameof(ResolvedAddress.CreatedDate)),
                Parameter: nameof(ResolvedAddress.UpdatedDate)),

                (Rule: await IsNotRecentAsync(resolvedAddress.UpdatedDate),
                    Parameter: nameof(resolvedAddress.UpdatedDate)));
        }

        public void ValidateResolvedAddressId(Guid resolvedAddressId)
        {
            Validate(
                createException: () => new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again."),

                (Rule: IsInvalid(resolvedAddressId), Parameter: nameof(ResolvedAddress.Id)));
        }

        private static void ValidateStorageResolvedAddress(ResolvedAddress maybeResolvedAddress, Guid resolvedAddressId)
        {
            if (maybeResolvedAddress is null)
            {
                throw new NotFoundResolvedAddressException(resolvedAddressId);
            }
        }

        private static void ValidateResolvedAddressIsNotNull(ResolvedAddress resolvedAddress)
        {
            if (resolvedAddress is null)
            {
                throw new NullResolvedAddressException(message: "Resolved address is null.");
            }
        }

        private static void ValidateAgainstStorageResolvedAddressOnModify(ResolvedAddress inputResolvedAddress, ResolvedAddress storageResolvedAddress)
        {
            Validate(

                createException: () => new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again."),

                (Rule: IsNotSame(
                    firstDate: inputResolvedAddress.CreatedDate,
                    secondDate: storageResolvedAddress.CreatedDate,
                    secondDateName: nameof(ResolvedAddress.CreatedDate)),
                Parameter: nameof(ResolvedAddress.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputResolvedAddress.CreatedBy,
                    second: storageResolvedAddress.CreatedBy,
                    secondName: nameof(ResolvedAddress.CreatedBy)),
                Parameter: nameof(ResolvedAddress.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputResolvedAddress.UpdatedDate,
                    secondDate: storageResolvedAddress.UpdatedDate,
                    secondDateName: nameof(ResolvedAddress.UpdatedDate)),
                Parameter: nameof(ResolvedAddress.UpdatedDate)));
        }

        private async ValueTask ValidateAgainstStorageResolvedAddressOnDeleteAsync(
            ResolvedAddress resolvedAddress,
            ResolvedAddress maybeResolvedAddress)
        {
            string auditUserId = await this.securityAuditBroker.GetUserIdAsync();

            Validate(

                createException: () => new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again."),

                (Rule: IsNotSame(
                    resolvedAddress.CreatedDate,
                    maybeResolvedAddress.CreatedDate,
                    nameof(maybeResolvedAddress.CreatedDate)),
                 Parameter: nameof(ResolvedAddress.CreatedDate)),

                (Rule: IsNotSame(
                    resolvedAddress.CreatedBy,
                    maybeResolvedAddress.CreatedBy,
                    nameof(maybeResolvedAddress.CreatedBy)),
                 Parameter: nameof(ResolvedAddress.CreatedBy)),

                (Rule: IsNotSame(
                    maybeResolvedAddress.UpdatedDate,
                    resolvedAddress.UpdatedDate,
                    nameof(ResolvedAddress.UpdatedDate)),
                 Parameter: nameof(ResolvedAddress.UpdatedDate)),

                (Rule: IsNotSame(
                    auditUserId,
                    resolvedAddress.UpdatedBy,
                    nameof(ResolvedAddress.UpdatedBy)),
                 Parameter: nameof(ResolvedAddress.UpdatedBy))
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

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsInvalid(List<ResolvedAddress> resolvedAddresses) => new
        {
            Condition = resolvedAddresses == null,
            Message = "List of resolved addresses is required"
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