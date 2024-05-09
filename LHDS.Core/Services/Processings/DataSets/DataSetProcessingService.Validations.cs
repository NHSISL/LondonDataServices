// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Processings.DataSets.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.DataSets
{
    public partial class DataSetProcessingService : IDataSetProcessingService
    {
        private void ValidateDataSet(DataSet dataSet)
        {
            ValidateDataSetIsNotNull(dataSet);
        }

        private static void ValidateDataSetIsNotNull(DataSet dataSet)
        {
            if (dataSet is null)
            {
                throw new NullDataSetProcessingException(message: "DataSet is null.");
            }
        }

        public void ValidateDataSetId(Guid dataSetId) =>
            Validate<InvalidArgumentDataSetProcessingException>(
                message: "Invalid argument(s). Please correct the errors and try again.",
                (Rule: IsInvalid(dataSetId), Parameter: nameof(DataSet.Id)));

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
