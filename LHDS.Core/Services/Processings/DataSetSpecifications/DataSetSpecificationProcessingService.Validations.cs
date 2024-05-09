// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Processings.DataSetSpecifications.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.DataSetSpecifications
{
    public partial class DataSetSpecificationProcessingService : IDataSetSpecificationProcessingService
    {
        private void ValidateDataSetSpecification(DataSetSpecification dataSetSpecification)
        {
            ValidateDataSetSpecificationIsNotNull(dataSetSpecification);
        }

        private static void ValidateDataSetSpecificationIsNotNull(DataSetSpecification dataSetSpecification)
        {
            if (dataSetSpecification is null)
            {
                throw new NullDataSetSpecificationProcessingException(message: "DataSetSpecification is null.");
            }
        }

        public void ValidateDataSetSpecificationId(Guid dataSetSpecificationId) =>
            Validate<InvalidArgumentDataSetSpecificationProcessingException>(
                message: "Invalid argument(s). Please correct the errors and try again.",
                (Rule: IsInvalid(dataSetSpecificationId), Parameter: nameof(DataSetSpecification.Id)));

        public void ValidateSupplierId(Guid supplierId) =>
            Validate<InvalidArgumentDataSetSpecificationProcessingException>(
                message: "Invalid argument(s). Please correct the errors and try again.",
                (Rule: IsInvalid(supplierId), Parameter: nameof(DataSetSpecification.DataSet.SupplierId)));

        public void ValidateDataSetSpecificationCount(int count)
        {
            if (count != 1)
            {
                throw new InvalidCountDataSetSpecificationProcessingException(
                    message: "Expected DataSetSpecification count to be one.");
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void Validate<T>(string message, params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            var invalidDataException = (T?)Activator.CreateInstance(typeof(T), message);

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataException?.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataException?.ThrowIfContainsErrors();
        }
    }
}
