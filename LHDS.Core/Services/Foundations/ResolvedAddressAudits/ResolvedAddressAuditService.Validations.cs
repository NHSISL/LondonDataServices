// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.ResolvedAddressAudits.Exceptions;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;

namespace LHDS.Core.Services.Foundations.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditService
    {
        private async ValueTask ValidateResolvedAddressAuditOnAddAsync(ResolvedAddressAudit resolvedAddressAudit)
        {
            EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();

            Validate(
                (Rule: IsInvalid(resolvedAddressAudit.Id), Parameter: nameof(ResolvedAddressAudit.Id)),
                (Rule: IsInvalid(resolvedAddressAudit.CorrelationId), Parameter: nameof(ResolvedAddressAudit.CorrelationId)),
                (Rule: IsInvalid(resolvedAddressAudit.Message), Parameter: nameof(ResolvedAddressAudit.Message)),
                (Rule: IsInvalid(resolvedAddressAudit.AuditType), Parameter: nameof(ResolvedAddressAudit.AuditType)),
                (Rule: IsInvalid(resolvedAddressAudit.CreatedDate), Parameter: nameof(ResolvedAddressAudit.CreatedDate)),
                (Rule: IsInvalid(resolvedAddressAudit.CreatedBy), Parameter: nameof(ResolvedAddressAudit.CreatedBy)),
                (Rule: IsInvalid(resolvedAddressAudit.UpdatedDate), Parameter: nameof(ResolvedAddressAudit.UpdatedDate)),
                (Rule: IsInvalid(resolvedAddressAudit.UpdatedBy), Parameter: nameof(ResolvedAddressAudit.UpdatedBy)),

                (Rule: IsNotSame(
                    first: currentUser.EntraUserId,
                    second: resolvedAddressAudit.CreatedBy),
                Parameter: nameof(ResolvedAddressAudit.CreatedBy)),

                (Rule: IsNotSame(
                    firstDate: resolvedAddressAudit.UpdatedDate,
                    secondDate: resolvedAddressAudit.CreatedDate,
                    secondDateName: nameof(ResolvedAddressAudit.CreatedDate)),
                Parameter: nameof(ResolvedAddressAudit.UpdatedDate)),

                (Rule: IsNotSame(
                    first: resolvedAddressAudit.UpdatedBy,
                    second: resolvedAddressAudit.CreatedBy,
                    secondName: nameof(ResolvedAddressAudit.CreatedBy)),
                Parameter: nameof(ResolvedAddressAudit.UpdatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    resolvedAddressAudit.CreatedBy, 255), Parameter: nameof(resolvedAddressAudit.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    resolvedAddressAudit.UpdatedBy, 255), Parameter: nameof(resolvedAddressAudit.UpdatedBy)),

                (Rule: await IsNotRecentAsync(resolvedAddressAudit.CreatedDate), Parameter: nameof(ResolvedAddressAudit.CreatedDate)));
        }

        private async ValueTask ValidateResolvedAddressAuditOnModifyAsync(ResolvedAddressAudit resolvedAddressAudit)
        {
            EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();

            Validate(
                (Rule: IsInvalid(resolvedAddressAudit.Id), Parameter: nameof(ResolvedAddressAudit.Id)),
                (Rule: IsInvalid(resolvedAddressAudit.CorrelationId), Parameter: nameof(ResolvedAddressAudit.CorrelationId)),
                (Rule: IsInvalid(resolvedAddressAudit.Message), Parameter: nameof(ResolvedAddressAudit.Message)),
                (Rule: IsInvalid(resolvedAddressAudit.AuditType), Parameter: nameof(ResolvedAddressAudit.AuditType)),
                (Rule: IsInvalid(resolvedAddressAudit.CreatedDate), Parameter: nameof(ResolvedAddressAudit.CreatedDate)),
                (Rule: IsInvalid(resolvedAddressAudit.CreatedBy), Parameter: nameof(ResolvedAddressAudit.CreatedBy)),
                (Rule: IsInvalid(resolvedAddressAudit.UpdatedDate), Parameter: nameof(ResolvedAddressAudit.UpdatedDate)),
                (Rule: IsInvalid(resolvedAddressAudit.UpdatedBy), Parameter: nameof(ResolvedAddressAudit.UpdatedBy)),

                (Rule: IsNotSame(
                    first: currentUser.EntraUserId,
                    second: resolvedAddressAudit.UpdatedBy),
                Parameter: nameof(ResolvedAddressAudit.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: resolvedAddressAudit.UpdatedDate,
                    secondDate: resolvedAddressAudit.CreatedDate,
                    secondDateName: nameof(ResolvedAddressAudit.CreatedDate)),
                Parameter: nameof(ResolvedAddressAudit.UpdatedDate)),

                (Rule: IsEqualOrSmallerThan(
                    resolvedAddressAudit.CreatedBy, 255), Parameter: nameof(resolvedAddressAudit.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    resolvedAddressAudit.UpdatedBy, 255), Parameter: nameof(resolvedAddressAudit.UpdatedBy)),

                (Rule: await IsNotRecentAsync(resolvedAddressAudit.UpdatedDate), Parameter: nameof(resolvedAddressAudit.UpdatedDate)));
        }

        public void ValidateResolvedAddressAuditId(Guid resolvedAddressAuditId) =>
            Validate((Rule: IsInvalid(resolvedAddressAuditId), Parameter: nameof(ResolvedAddressAudit.Id)));

        private static void ValidateStorageResolvedAddressAudit(ResolvedAddressAudit maybeResolvedAddressAudit, Guid resolvedAddressAuditId)
        {
            if (maybeResolvedAddressAudit is null)
            {
                throw new NotFoundResolvedAddressAuditException(resolvedAddressAuditId);
            }
        }

        private static void ValidateResolvedAddressAuditIsNotNull(ResolvedAddressAudit resolvedAddressAudit)
        {
            if (resolvedAddressAudit is null)
            {
                throw new NullResolvedAddressAuditException(message: "ResolvedAddressAudit is null.");
            }
        }

        private static void ValidateAgainstStorageResolvedAddressAuditOnModify(ResolvedAddressAudit inputResolvedAddressAudit, ResolvedAddressAudit storageResolvedAddressAudit)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputResolvedAddressAudit.CreatedDate,
                    secondDate: storageResolvedAddressAudit.CreatedDate,
                    secondDateName: nameof(ResolvedAddressAudit.CreatedDate)),
                Parameter: nameof(ResolvedAddressAudit.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputResolvedAddressAudit.CreatedBy,
                    second: storageResolvedAddressAudit.CreatedBy,
                    secondName: nameof(ResolvedAddressAudit.CreatedBy)),
                Parameter: nameof(ResolvedAddressAudit.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputResolvedAddressAudit.UpdatedDate,
                    secondDate: storageResolvedAddressAudit.UpdatedDate,
                    secondDateName: nameof(ResolvedAddressAudit.UpdatedDate)),
                Parameter: nameof(ResolvedAddressAudit.UpdatedDate)));
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

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidResolvedAddressAuditException = new InvalidResolvedAddressAuditException(
                message: "Invalid resolvedAddressAudit. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidResolvedAddressAuditException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidResolvedAddressAuditException.ThrowIfContainsErrors();
        }
    }
}