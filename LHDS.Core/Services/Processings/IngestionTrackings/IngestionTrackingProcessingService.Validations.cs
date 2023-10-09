// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Processings.IngestionTrackings.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingProcessingService : IIngestionTrackingProcessingService
    {
        private void ValidateIngestionTracking(IngestionTracking objectColumn)
        {
            ValidateIngestionTrackingIsNotNull(objectColumn);
        }

        private static void ValidateIngestionTrackingIsNotNull(IngestionTracking objectColumn)
        {
            if (objectColumn is null)
            {
                throw new NullIngestionTrackingProcessingException(message: "IngestionTracking is null.");
            }
        }

        public void ValidateIngestionTrackingId(Guid objectColumnId) =>
            Validate<InvalidArgumentIngestionTrackingProcessingException>(
                message: "Invalid argument(s). Please correct the errors and try again.",
                (Rule: IsInvalid(objectColumnId), Parameter: nameof(IngestionTracking.Id)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void Validate<T>(string message, params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            var invalidDataException = (T)Activator.CreateInstance(typeof(T), message);

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataException.ThrowIfContainsErrors();
        }
    }
}
