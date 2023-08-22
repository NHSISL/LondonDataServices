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
                Parameter: nameof(DataType.UpdatedDate)));
        }

        private static void ValidateDataTypeIsNotNull(DataType dataType)
        {
            if (dataType is null)
            {
                throw new NullDataTypeException(message: "DataType is null.");
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