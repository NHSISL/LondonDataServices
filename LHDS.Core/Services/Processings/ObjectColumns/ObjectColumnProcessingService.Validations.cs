// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Processings.ObjectColumns.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.ObjectColumns
{
    public partial class ObjectColumnProcessingService : IObjectColumnProcessingService
    {
        private void ValidateObjectColumn(ObjectColumn objectColumn)
        {
            ValidateObjectColumnIsNotNull(objectColumn);
        }

        private static void ValidateObjectColumnIsNotNull(ObjectColumn objectColumn)
        {
            if (objectColumn is null)
            {
                throw new NullObjectColumnProcessingException(message: "ObjectColumn is null.");
            }
        }

        public void ValidateObjectColumnId(Guid objectColumnId) =>
            Validate<InvalidArgumentObjectColumnProcessingException>(
                message: "Invalid argument(s). Please correct the errors and try again.",
                (Rule: IsInvalid(objectColumnId), Parameter: nameof(ObjectColumn.Id)));

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
