using System;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;

namespace LHDS.Core.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationService
    {
        private void ValidateDataSetSpecificationOnAdd(DataSetSpecification dataSetSpecification)
        {
            ValidateDataSetSpecificationIsNotNull(dataSetSpecification);

            Validate(
                (Rule: IsInvalid(dataSetSpecification.Id), Parameter: nameof(DataSetSpecification.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(dataSetSpecification.CreatedDate), Parameter: nameof(DataSetSpecification.CreatedDate)),
                (Rule: IsInvalid(dataSetSpecification.CreatedBy), Parameter: nameof(DataSetSpecification.CreatedBy)),
                (Rule: IsInvalid(dataSetSpecification.UpdatedDate), Parameter: nameof(DataSetSpecification.UpdatedDate)),
                (Rule: IsInvalid(dataSetSpecification.UpdatedBy), Parameter: nameof(DataSetSpecification.UpdatedBy)));
        }

        private static void ValidateDataSetSpecificationIsNotNull(DataSetSpecification dataSetSpecification)
        {
            if (dataSetSpecification is null)
            {
                throw new NullDataSetSpecificationException(message: "DataSetSpecification is null.");
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

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidDataSetSpecificationException = 
                new InvalidDataSetSpecificationException(
                    message: "Invalid dataSetSpecification. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataSetSpecificationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataSetSpecificationException.ThrowIfContainsErrors();
        }
    }
}