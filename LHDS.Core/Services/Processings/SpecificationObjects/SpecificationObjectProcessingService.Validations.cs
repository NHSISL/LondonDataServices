// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Processings.SpecificationObjects.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.SpecificationObjects
{
    public partial class SpecificationObjectProcessingService
    {
        public void ValidateOnRetrieveSpecificationObjectsByDataSetSpecificationId(Guid dataSetSpecificationId) =>
            Validate<InvalidArgumentSpecificationObjectProcessingException>(
                message: "Invalid argument(s). Please correct the errors and try again.",
                (Rule: IsInvalid(dataSetSpecificationId), Parameter: nameof(SpecificationObject.Id)));

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
