// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Processings.IngestionTrackingAudits.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingAuditProcessingService
    {
        private void ValidateIngestionTrackingAudit(IngestionTrackingAudit audit)
        {
            ValidateIngestionTrackingAuditIsNotNull(audit);
        }

        private static void ValidateIngestionTrackingAuditIsNotNull(IngestionTrackingAudit audit)
        {
            if (audit is null)
            {
                throw new NullIngestionTrackingAuditProcessingException(message: "IngestionTrackingAudit is null.");
            }
        }

        public void ValidateIngestionTrackingAuditId(Guid auditId) =>
            Validate<InvalidArgumentIngestionTrackingAuditProcessingException>(
                message: "Invalid argument(s). Please correct the errors and try again.",
                (Rule: IsInvalid(auditId), Parameter: nameof(IngestionTrackingAudit.Id)));

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
