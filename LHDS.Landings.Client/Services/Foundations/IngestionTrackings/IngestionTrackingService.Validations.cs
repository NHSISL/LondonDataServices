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
        private void ValidateIngestionTrackingOnAdd(IngestionTracking ingestionTracking)
        {
            ValidateIngestionTrackingIsNotNull(ingestionTracking);

            Validate(
                (Rule: IsInvalid(ingestionTracking.Id), Parameter: nameof(IngestionTracking.Id)),

            // TODO: Add any other required validation rules

                (Rule: IsInvalid(ingestionTracking.CreatedDate), Parameter: nameof(IngestionTracking.CreatedDate)),
                (Rule: IsInvalid(ingestionTracking.CreatedBy), Parameter: nameof(IngestionTracking.CreatedBy)),
                (Rule: IsInvalid(ingestionTracking.UpdatedDate), Parameter: nameof(IngestionTracking.UpdatedDate)),
                (Rule: IsInvalid(ingestionTracking.UpdatedBy), Parameter: nameof(IngestionTracking.UpdatedBy)),
            
                (Rule: IsNotSame(
                    firstDate: ingestionTracking.UpdatedDate,
                    secondDate: ingestionTracking.CreatedDate,
                    secondDateName: nameof(IngestionTracking.CreatedDate)),
                Parameter: nameof(IngestionTracking.UpdatedDate)),

                (Rule: IsNotSame(
                    firstUser: ingestionTracking.UpdatedBy,
                    secondUser: ingestionTracking.CreatedBy,
                    secondUserName: nameof(ingestionTracking.CreatedBy)),
                Parameter: nameof(ingestionTracking.UpdatedBy)));

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

        private static dynamic IsNotSame(
                 string firstUser,
                 string secondUser,
                 string secondUserName) => new
                 {
                     Condition = firstUser != secondUser,
                     Message = $"User is not the same as {secondUserName}"
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
