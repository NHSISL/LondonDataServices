using System;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;

namespace LHDS.Core.Services.Foundations.DataTypes
{
    public partial class DataTypeService
    {
        private void ValidateDataTypeOnAdd(DataType dataType)
        {
            ValidateDataTypeIsNotNull(dataType);

            Validate(
                (Rule: IsInvalid(dataType.Id), Parameter: nameof(DataType.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(dataType.CreatedDate), Parameter: nameof(DataType.CreatedDate)),
                (Rule: IsInvalid(dataType.CreatedBy), Parameter: nameof(DataType.CreatedBy)),
                (Rule: IsInvalid(dataType.UpdatedDate), Parameter: nameof(DataType.UpdatedDate)),
                (Rule: IsInvalid(dataType.UpdatedBy), Parameter: nameof(DataType.UpdatedBy)),

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

                (Rule: IsNotRecent(dataType.CreatedDate), Parameter: nameof(DataType.CreatedDate)));
        }

        public void ValidateDataTypeId(Guid dataTypeId) =>
            Validate((Rule: IsInvalid(dataTypeId), Parameter: nameof(DataType.Id)));

        private static void ValidateDataTypeIsNotNull(DataType dataType)
        {
            if (dataType is null)
            {
                throw new NullDataTypeException();
            }
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
            var invalidDataTypeException = new InvalidDataTypeException();

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