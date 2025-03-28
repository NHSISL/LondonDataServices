// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;

namespace LHDS.Core.Services.Foundations.DataTypes
{
    public partial class DataTypeService
    {
        private async ValueTask ValidateDataTypeOnAddAsync(DataType dataType)
        {
            ValidateDataTypeIsNotNull(dataType);
            EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();

            Validate(
                (Rule: IsInvalid(dataType.Id), Parameter: nameof(DataType.Id)),
                (Rule: IsInvalid(dataType.Name), Parameter: nameof(DataType.Name)),
                (Rule: IsInvalid(dataType.CreatedDate), Parameter: nameof(DataType.CreatedDate)),
                (Rule: IsInvalid(dataType.CreatedBy), Parameter: nameof(DataType.CreatedBy)),
                (Rule: IsInvalid(dataType.UpdatedDate), Parameter: nameof(DataType.UpdatedDate)),
                (Rule: IsInvalid(dataType.UpdatedBy), Parameter: nameof(DataType.UpdatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    dataType.Name, 50), Parameter: nameof(DataType.Name)),

                (Rule: IsNotSame(
                    first: currentUser.EntraUserId,
                    second: dataType.CreatedBy),
                Parameter: nameof(DataType.CreatedBy)),

                (Rule: IsNotSame(
                    firstDate: dataType.UpdatedDate,
                    secondDate: dataType.CreatedDate,
                    secondDateName: nameof(DataType.CreatedDate)),
                Parameter: nameof(DataType.UpdatedDate)),

                (Rule: IsNotSame(
                    first: dataType.UpdatedBy,
                    second: dataType.CreatedBy,
                    secondName: nameof(DataType.CreatedBy)),
                Parameter: nameof(DataType.UpdatedBy)),

                (Rule: await IsNotRecentAsync(dataType.CreatedDate), Parameter: nameof(DataType.CreatedDate)));
        }

        private async ValueTask ValidateDataTypeOnModifyAsync(DataType dataType)
        {
            ValidateDataTypeIsNotNull(dataType);
            EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();

            Validate(
                (Rule: IsInvalid(dataType.Id), Parameter: nameof(DataType.Id)),
                (Rule: IsInvalid(dataType.Name), Parameter: nameof(DataType.Name)),
                (Rule: IsInvalid(dataType.CreatedDate), Parameter: nameof(DataType.CreatedDate)),
                (Rule: IsInvalid(dataType.CreatedBy), Parameter: nameof(DataType.CreatedBy)),
                (Rule: IsInvalid(dataType.UpdatedDate), Parameter: nameof(DataType.UpdatedDate)),
                (Rule: IsInvalid(dataType.UpdatedBy), Parameter: nameof(DataType.UpdatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    dataType.Name, 50), Parameter: nameof(DataType.Name)),

                (Rule: IsNotSame(
                    first: currentUser.EntraUserId,
                    second: dataType.UpdatedBy),
                Parameter: nameof(DataType.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: dataType.UpdatedDate,
                    secondDate: dataType.CreatedDate,
                    secondDateName: nameof(DataType.CreatedDate)),
                Parameter: nameof(DataType.UpdatedDate)),

                (Rule: await IsNotRecentAsync(dataType.UpdatedDate), Parameter: nameof(dataType.UpdatedDate)));
        }

        public void ValidateDataTypeId(Guid dataTypeId) =>
            Validate((Rule: IsInvalid(dataTypeId), Parameter: nameof(DataType.Id)));

        private static void ValidateStorageDataType(DataType maybeDataType, Guid dataTypeId)
        {
            if (maybeDataType is null)
            {
                throw new NotFoundDataTypeException(dataTypeId);
            }
        }

        private static void ValidateDataTypeIsNotNull(DataType dataType)
        {
            if (dataType is null)
            {
                throw new NullDataTypeException(message: "DataType is null.");
            }
        }

        private static void ValidateAgainstStorageDataTypeOnModify(DataType inputDataType, DataType storageDataType)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputDataType.CreatedDate,
                    secondDate: storageDataType.CreatedDate,
                    secondDateName: nameof(DataType.CreatedDate)),
                Parameter: nameof(DataType.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputDataType.CreatedBy,
                    second: storageDataType.CreatedBy,
                    secondName: nameof(DataType.CreatedBy)),
                Parameter: nameof(DataType.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputDataType.UpdatedDate,
                    secondDate: storageDataType.UpdatedDate,
                    secondDateName: nameof(DataType.UpdatedDate)),
                Parameter: nameof(DataType.UpdatedDate)));
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

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidDataTypeException =
                new InvalidDataTypeException(
                    message: "Invalid dataType. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataTypeException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataTypeException.ThrowIfContainsErrors();
        }
    }
}