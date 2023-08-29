using System;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;

namespace LHDS.Core.Services.Foundations.DataSets
{
    public partial class DataSetService
    {
        private void ValidateDataSetOnAdd(DataSet dataSet)
        {
            ValidateDataSetIsNotNull(dataSet);

            Validate(
                (Rule: IsInvalid(dataSet.Id), Parameter: nameof(DataSet.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(dataSet.CreatedDate), Parameter: nameof(DataSet.CreatedDate)),
                (Rule: IsInvalid(dataSet.CreatedBy), Parameter: nameof(DataSet.CreatedBy)),
                (Rule: IsInvalid(dataSet.UpdatedDate), Parameter: nameof(DataSet.UpdatedDate)),
                (Rule: IsInvalid(dataSet.UpdatedBy), Parameter: nameof(DataSet.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: dataSet.UpdatedDate,
                    secondDate: dataSet.CreatedDate,
                    secondDateName: nameof(DataSet.CreatedDate)),
                Parameter: nameof(DataSet.UpdatedDate)));
        }

        private static void ValidateDataSetIsNotNull(DataSet dataSet)
        {
            if (dataSet is null)
            {
                throw new NullDataSetException(message: "DataSet is null.");
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
            var invalidDataSetException = 
                new InvalidDataSetException(
                    message: "Invalid dataSet. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataSetException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataSetException.ThrowIfContainsErrors();
        }
    }
}