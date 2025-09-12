// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.ObjectColumns.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnService
    {
        private async ValueTask ValidateObjectColumnOnAddAsync(ObjectColumn objectColumn)
        {
            EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();

            Validate<InvalidObjectColumnException>(
                createException: () => new InvalidObjectColumnException(
                    message: "Invalid objectColumn. Please correct the errors and try again."),

                (Rule: IsInvalid(objectColumn.Id), Parameter: nameof(ObjectColumn.Id)),

                (Rule: IsInvalid(objectColumn.SpecificationObjectId), 
                Parameter: nameof(ObjectColumn.SpecificationObjectId)),

                (Rule: IsInvalid(objectColumn.SupplierColumnName), Parameter: nameof(ObjectColumn.SupplierColumnName)),
                (Rule: IsInvalid(objectColumn.OurColumnName), Parameter: nameof(ObjectColumn.OurColumnName)),
                (Rule: IsInvalid(objectColumn.SqlDataType), Parameter: nameof(ObjectColumn.SqlDataType)),
                (Rule: IsInvalid(objectColumn.CodeSystem), Parameter: nameof(ObjectColumn.CodeSystem)),
                (Rule: IsInvalid(objectColumn.CreatedDate), Parameter: nameof(ObjectColumn.CreatedDate)),
                (Rule: IsInvalid(objectColumn.CreatedBy), Parameter: nameof(ObjectColumn.CreatedBy)),
                (Rule: IsInvalid(objectColumn.UpdatedDate), Parameter: nameof(ObjectColumn.UpdatedDate)),
                (Rule: IsInvalid(objectColumn.UpdatedBy), Parameter: nameof(ObjectColumn.UpdatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.SupplierColumnName, 255), Parameter: nameof(objectColumn.SupplierColumnName)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.OurColumnName, 255), Parameter: nameof(objectColumn.OurColumnName)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.ColumnDescription, 500), Parameter: nameof(objectColumn.ColumnDescription)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.PopulatedBy, 255), Parameter: nameof(objectColumn.PopulatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.SqlDataType, 50), Parameter: nameof(objectColumn.SqlDataType)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.FhirDataType, 255), Parameter: nameof(objectColumn.FhirDataType)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.SupplierDateFormat, 255), Parameter: nameof(objectColumn.SupplierDateFormat)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.PersonConfidentialDataType, 255),
                Parameter: nameof(objectColumn.PersonConfidentialDataType)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.MaskingMethod, 255), Parameter: nameof(objectColumn.MaskingMethod)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.CodeSystem, 255), Parameter: nameof(objectColumn.CodeSystem)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.PartitionColumnLevel, 255), Parameter: nameof(objectColumn.PartitionColumnLevel)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.CreatedBy, 255), Parameter: nameof(objectColumn.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.UpdatedBy, 255), Parameter: nameof(objectColumn.UpdatedBy)),

                (Rule: IsNotSame(
                    first: currentUser.EntraUserId,
                    second: objectColumn.CreatedBy),
                Parameter: nameof(ObjectColumn.CreatedBy)),

                (Rule: IsNotSame(
                    firstDate: objectColumn.UpdatedDate,
                    secondDate: objectColumn.CreatedDate,
                    secondDateName: nameof(ObjectColumn.CreatedDate)),
                Parameter: nameof(ObjectColumn.UpdatedDate)),

                (Rule: IsNotSame(
                    first: objectColumn.UpdatedBy,
                    second: objectColumn.CreatedBy,
                    secondName: nameof(ObjectColumn.CreatedBy)),
                Parameter: nameof(ObjectColumn.UpdatedBy)),

                (Rule: await IsNotRecentAsync(objectColumn.CreatedDate), Parameter: nameof(ObjectColumn.CreatedDate)));
        }

        private async ValueTask ValidateObjectColumnOnModifyAsync(ObjectColumn objectColumn)
        {
            EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();

            Validate<InvalidObjectColumnException>(
                createException: () => new InvalidObjectColumnException(
                    message: "Invalid objectColumn. Please correct the errors and try again."),

                (Rule: IsInvalid(objectColumn.Id), Parameter: nameof(ObjectColumn.Id)),

                (Rule: IsInvalid(objectColumn.SpecificationObjectId), 
                Parameter: nameof(ObjectColumn.SpecificationObjectId)),

                (Rule: IsInvalid(objectColumn.SupplierColumnName), Parameter: nameof(ObjectColumn.SupplierColumnName)),
                (Rule: IsInvalid(objectColumn.OurColumnName), Parameter: nameof(ObjectColumn.OurColumnName)),
                (Rule: IsInvalid(objectColumn.SqlDataType), Parameter: nameof(ObjectColumn.SqlDataType)),
                (Rule: IsInvalid(objectColumn.CodeSystem), Parameter: nameof(ObjectColumn.CodeSystem)),
                (Rule: IsInvalid(objectColumn.CreatedDate), Parameter: nameof(ObjectColumn.CreatedDate)),
                (Rule: IsInvalid(objectColumn.CreatedBy), Parameter: nameof(ObjectColumn.CreatedBy)),
                (Rule: IsInvalid(objectColumn.UpdatedDate), Parameter: nameof(ObjectColumn.UpdatedDate)),
                (Rule: IsInvalid(objectColumn.UpdatedBy), Parameter: nameof(ObjectColumn.UpdatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.SupplierColumnName, 255), Parameter: nameof(objectColumn.SupplierColumnName)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.OurColumnName, 255), Parameter: nameof(objectColumn.OurColumnName)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.ColumnDescription, 500), Parameter: nameof(objectColumn.ColumnDescription)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.PopulatedBy, 255), Parameter: nameof(objectColumn.PopulatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.SqlDataType, 50), Parameter: nameof(objectColumn.SqlDataType)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.FhirDataType, 255), Parameter: nameof(objectColumn.FhirDataType)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.SupplierDateFormat, 255), Parameter: nameof(objectColumn.SupplierDateFormat)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.PersonConfidentialDataType, 255),
                Parameter: nameof(objectColumn.PersonConfidentialDataType)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.MaskingMethod, 255), Parameter: nameof(objectColumn.MaskingMethod)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.CodeSystem, 255), Parameter: nameof(objectColumn.CodeSystem)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.PartitionColumnLevel, 255), Parameter: nameof(objectColumn.PartitionColumnLevel)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.CreatedBy, 255), Parameter: nameof(objectColumn.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    objectColumn.UpdatedBy, 255), Parameter: nameof(objectColumn.UpdatedBy)),

                (Rule: IsNotSame(
                    first: currentUser.EntraUserId,
                    second: objectColumn.UpdatedBy),
                Parameter: nameof(ObjectColumn.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: objectColumn.UpdatedDate,
                    secondDate: objectColumn.CreatedDate,
                    secondDateName: nameof(ObjectColumn.CreatedDate)),
                Parameter: nameof(ObjectColumn.UpdatedDate)),

                (Rule: await IsNotRecentAsync(objectColumn.UpdatedDate), Parameter: nameof(objectColumn.UpdatedDate)));
        }

        public void ValidateObjectColumnId(Guid objectColumnId)
        {

            Validate<InvalidObjectColumnException>(

                createException: () => new InvalidObjectColumnException(
                    message: "Invalid objectColumn. Please correct the errors and try again."),
                
                (Rule: IsInvalid(objectColumnId), Parameter: nameof(ObjectColumn.Id)));
        }

        private static void ValidateStorageObjectColumn(ObjectColumn maybeObjectColumn, Guid objectColumnId)
        {
            if (maybeObjectColumn is null)
            {
                throw new NotFoundObjectColumnException(objectColumnId);
            }
        }

        private static void ValidateObjectColumnIsNotNull(ObjectColumn objectColumn)
        {
            if (objectColumn is null)
            {
                throw new NullObjectColumnException(message: "ObjectColumn is null.");
            }
        }

        private static void ValidateAgainstStorageObjectColumnOnModify(
            ObjectColumn inputObjectColumn,
            ObjectColumn storageObjectColumn)
        {
            Validate<InvalidObjectColumnException>(
                createException: () => new InvalidObjectColumnException(
                    message: "Invalid objectColumn. Please correct the errors and try again."),

                (Rule: IsNotSame(
                    firstDate: inputObjectColumn.CreatedDate,
                    secondDate: storageObjectColumn.CreatedDate,
                    secondDateName: nameof(ObjectColumn.CreatedDate)),
                Parameter: nameof(ObjectColumn.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputObjectColumn.CreatedBy,
                    second: storageObjectColumn.CreatedBy,
                    secondName: nameof(ObjectColumn.CreatedBy)),
                Parameter: nameof(ObjectColumn.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputObjectColumn.UpdatedDate,
                    secondDate: storageObjectColumn.UpdatedDate,
                    secondDateName: nameof(ObjectColumn.UpdatedDate)),
                Parameter: nameof(ObjectColumn.UpdatedDate)));
        }

        private async ValueTask ValidateAgainstStorageObjectColumnOnDeleteAsync(
            ObjectColumn objectColumn,
            ObjectColumn maybeObjectColumn)
        {
            EntraUser auditUser = await this.securityBroker.GetCurrentUserAsync();

            Validate<InvalidObjectColumnException>(
                createException: () => new InvalidObjectColumnException(
                    message: "Invalid objectColumn. Please correct the errors and try again."),

                (Rule: IsNotSame(
                    objectColumn.CreatedDate,
                    maybeObjectColumn.CreatedDate,
                    nameof(maybeObjectColumn.CreatedDate)),
                 Parameter: nameof(ObjectColumn.CreatedDate)),

                (Rule: IsNotSame(
                    objectColumn.CreatedBy,
                    maybeObjectColumn.CreatedBy,
                    nameof(maybeObjectColumn.CreatedBy)),
                 Parameter: nameof(ObjectColumn.CreatedBy)),

                (Rule: IsNotSame(
                    maybeObjectColumn.UpdatedDate,
                    objectColumn.UpdatedDate,
                    nameof(ObjectColumn.UpdatedDate)),
                 Parameter: nameof(ObjectColumn.UpdatedDate)),

                (Rule: IsNotSame(
                    auditUser.EntraUserId.ToString(),
                    objectColumn.UpdatedBy,
                    nameof(ObjectColumn.UpdatedBy)),
                 Parameter: nameof(ObjectColumn.UpdatedBy))
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

        private static dynamic IsEqualOrSmallerThan(string text, int maxLength) => new
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