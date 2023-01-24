// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Reflection.Metadata;
using System.Xml.Linq;
using LHDS.Landings.Client.Models.IngestionTracking;
using LHDS.Landings.Client.Models.IngestionTracking.Exceptions;

namespace LHDS.Landings.Client.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingService
    {
        private void ValidateIngestionTrackingOnAdd(IngestionTracking IngestionTracking)
        {
            ValidateIngestionTrackingIsNotNull(IngestionTracking);

            Validate(
                (Rule: IsInvalid(IngestionTracking.Id), Parameter: nameof(IngestionTracking.Id)),

            // TODO: Add any other required validation rules

                (Rule: IsInvalid(IngestionTracking.CreatedDate), Parameter: nameof(IngestionTracking.CreatedDate)),
                (Rule: IsInvalid(IngestionTracking.CreatedBy), Parameter: nameof(IngestionTracking.CreatedBy)),
                (Rule: IsInvalid(IngestionTracking.UpdatedDate), Parameter: nameof(IngestionTracking.UpdatedDate)),
                (Rule: IsInvalid(IngestionTracking.UpdatedBy), Parameter: nameof(IngestionTracking.UpdatedBy)),
            
                (Rule: IsNotSame(
                    firstDate: IngestionTracking.UpdatedDate,
                    secondDate: IngestionTracking.CreatedDate,
                    secondDateName: nameof(IngestionTracking.CreatedDate)),
                Parameter: nameof(IngestionTracking.UpdatedDate)));

        }

        private static void ValidateIngestionTrackingIsNotNull(IngestionTracking ingestionTracking)
        {
            if (ingestionTracking is null)
            {
                throw new NullIngestionTrackingException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "User is required"
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
            var invalidIngestionTrackingException = new InvalidIngestionTrackingException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidIngestionTrackingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidIngestionTrackingException.ThrowIfContainsErrors();
        }
    }
}
