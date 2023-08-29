using System;
using LHDS.Core.Models.Foundations.DataSetObjects;
using LHDS.Core.Models.Foundations.DataSetObjects.Exceptions;

namespace LHDS.Core.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectService
    {
        private void ValidateDataSetObjectOnAdd(DataSetObject dataSetObject)
        {
            ValidateDataSetObjectIsNotNull(dataSetObject);

            Validate(
                (Rule: IsInvalid(dataSetObject.Id), Parameter: nameof(DataSetObject.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(dataSetObject.CreatedDate), Parameter: nameof(DataSetObject.CreatedDate)),
                (Rule: IsInvalid(dataSetObject.CreatedBy), Parameter: nameof(DataSetObject.CreatedBy)),
                (Rule: IsInvalid(dataSetObject.UpdatedDate), Parameter: nameof(DataSetObject.UpdatedDate)),
                (Rule: IsInvalid(dataSetObject.UpdatedBy), Parameter: nameof(DataSetObject.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: dataSetObject.UpdatedDate,
                    secondDate: dataSetObject.CreatedDate,
                    secondDateName: nameof(DataSetObject.CreatedDate)),
                Parameter: nameof(DataSetObject.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: dataSetObject.UpdatedBy,
                    secondId: dataSetObject.CreatedBy,
                    secondIdName: nameof(DataSetObject.CreatedBy)),
                Parameter: nameof(DataSetObject.UpdatedBy)));
        }

        private static void ValidateDataSetObjectIsNotNull(DataSetObject dataSetObject)
        {
            if (dataSetObject is null)
            {
                throw new NullDataSetObjectException(message: "DataSetObject is null.");
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

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidDataSetObjectException = 
                new InvalidDataSetObjectException(
                    message: "Invalid dataSetObject. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataSetObjectException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataSetObjectException.ThrowIfContainsErrors();
        }
    }
}