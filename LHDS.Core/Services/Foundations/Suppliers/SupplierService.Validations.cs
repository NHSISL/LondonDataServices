// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Foundations.Suppliers.Exceptions;

namespace LHDS.Core.Services.Foundations.Suppliers
{
    public partial class SupplierService
    {
        private async ValueTask ValidateSupplierOnAddAsync(Supplier supplier)
        {
            EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();

            Validate(
                (Rule: IsInvalid(supplier.Id), Parameter: nameof(Supplier.Id)),
                (Rule: IsInvalid(supplier.Name), Parameter: nameof(Supplier.Name)),
                (Rule: IsInvalid(supplier.FriendlyName), Parameter: nameof(Supplier.FriendlyName)),
                (Rule: IsInvalid(supplier.Description), Parameter: nameof(Supplier.Description)),
                (Rule: IsInvalid(supplier.CreatedDate), Parameter: nameof(Supplier.CreatedDate)),
                (Rule: IsInvalid(supplier.CreatedBy), Parameter: nameof(Supplier.CreatedBy)),
                (Rule: IsInvalid(supplier.UpdatedDate), Parameter: nameof(Supplier.UpdatedDate)),
                (Rule: IsInvalid(supplier.UpdatedBy), Parameter: nameof(Supplier.UpdatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    supplier.CreatedBy, 255), Parameter: nameof(supplier.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    supplier.UpdatedBy, 255), Parameter: nameof(supplier.UpdatedBy)),

                (Rule: IsNotSame(
                    first: currentUser.EntraUserId,
                    second: supplier.CreatedBy),
                Parameter: nameof(Supplier.CreatedBy)),

                (Rule: IsNotSame(
                    firstDate: supplier.UpdatedDate,
                    secondDate: supplier.CreatedDate,
                    secondDateName: nameof(Supplier.CreatedDate)),
                Parameter: nameof(Supplier.UpdatedDate)),

                (Rule: IsNotSame(
                    first: supplier.UpdatedBy,
                    second: supplier.CreatedBy,
                    secondName: nameof(Supplier.CreatedBy)),
                Parameter: nameof(Supplier.UpdatedBy)),

                (Rule: await IsNotRecentAsync(supplier.CreatedDate), Parameter: nameof(Supplier.CreatedDate)));
        }

        private async ValueTask ValidateSupplierOnModifyAsync(Supplier supplier)
        {
            EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();

            Validate(
                (Rule: IsInvalid(supplier.Id), Parameter: nameof(Supplier.Id)),
                (Rule: IsInvalid(supplier.Name), Parameter: nameof(Supplier.Name)),
                (Rule: IsInvalid(supplier.FriendlyName), Parameter: nameof(Supplier.FriendlyName)),
                (Rule: IsInvalid(supplier.Description), Parameter: nameof(Supplier.Description)),
                (Rule: IsInvalid(supplier.CreatedDate), Parameter: nameof(Supplier.CreatedDate)),
                (Rule: IsInvalid(supplier.CreatedBy), Parameter: nameof(Supplier.CreatedBy)),
                (Rule: IsInvalid(supplier.UpdatedDate), Parameter: nameof(Supplier.UpdatedDate)),
                (Rule: IsInvalid(supplier.UpdatedBy), Parameter: nameof(Supplier.UpdatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    supplier.CreatedBy, 255), Parameter: nameof(supplier.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    supplier.UpdatedBy, 255), Parameter: nameof(supplier.UpdatedBy)),

                (Rule: IsNotSame(
                    first: currentUser.EntraUserId,
                    second: supplier.UpdatedBy),
                Parameter: nameof(Supplier.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: supplier.UpdatedDate,
                    secondDate: supplier.CreatedDate,
                    secondDateName: nameof(Supplier.CreatedDate)),
                Parameter: nameof(Supplier.UpdatedDate)),

                (Rule: await IsNotRecentAsync(supplier.UpdatedDate), Parameter: nameof(supplier.UpdatedDate)));
        }

        public void ValidateSupplierId(Guid supplierId) =>
            Validate((Rule: IsInvalid(supplierId), Parameter: nameof(Supplier.Id)));

        private static void ValidateStorageSupplier(Supplier maybeSupplier, Guid supplierId)
        {
            if (maybeSupplier is null)
            {
                throw new NotFoundSupplierException(supplierId);
            }
        }

        private static void ValidateSupplierIsNotNull(Supplier supplier)
        {
            if (supplier is null)
            {
                throw new NullSupplierException(message: "Supplier is null.");
            }
        }

        private static void ValidateStorageSupplierExist(Supplier maybeSupplier, Guid supplierId)
        {
            if (maybeSupplier is not null)
            {
                //throw new NotFoundSupplierException(supplierId);
            }
        }

        private static void ValidateAgainstStorageSupplierOnModify(Supplier inputSupplier, Supplier storageSupplier)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputSupplier.CreatedDate,
                    secondDate: storageSupplier.CreatedDate,
                    secondDateName: nameof(Supplier.CreatedDate)),
                Parameter: nameof(Supplier.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputSupplier.CreatedBy,
                    second: storageSupplier.CreatedBy,
                    secondName: nameof(Supplier.CreatedBy)),
                Parameter: nameof(Supplier.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputSupplier.UpdatedDate,
                    secondDate: storageSupplier.UpdatedDate,
                    secondDateName: nameof(Supplier.UpdatedDate)),
                Parameter: nameof(Supplier.UpdatedDate)));
        }

        private async ValueTask ValidateAgainstStorageSupplierOnDeleteAsync(
            Supplier supplier,
            Supplier maybeSupplier)
        {
            EntraUser auditUser = await this.securityBroker.GetCurrentUserAsync();

            Validate(
                (Rule: IsNotSame(
                    supplier.CreatedDate,
                    maybeSupplier.CreatedDate,
                    nameof(maybeSupplier.CreatedDate)),
                 Parameter: nameof(Supplier.CreatedDate)),

                (Rule: IsNotSame(
                    supplier.CreatedBy,
                    maybeSupplier.CreatedBy,
                    nameof(maybeSupplier.CreatedBy)),
                 Parameter: nameof(Supplier.CreatedBy)),

                (Rule: IsNotSame(
                    maybeSupplier.UpdatedDate,
                    supplier.UpdatedDate,
                    nameof(Supplier.UpdatedDate)),
                 Parameter: nameof(Supplier.UpdatedDate)),

                (Rule: IsNotSame(
                    auditUser.EntraUserId.ToString(),
                    supplier.UpdatedBy,
                    nameof(Supplier.UpdatedBy)),
                 Parameter: nameof(Supplier.UpdatedBy))
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

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidSupplierException = new InvalidSupplierException(
                message: "Invalid supplier. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidSupplierException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidSupplierException.ThrowIfContainsErrors();
        }
    }
}