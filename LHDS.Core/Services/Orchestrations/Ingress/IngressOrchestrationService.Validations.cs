// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.Ingres.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.Ingress
{
    public partial class IngressOrchestrationService
    {
        private void ValidateOnProcessDecryptedItemsForBatchComplete(Guid supplierId)
        {
            Validate(
                createException: () => new InvalidArgumentIngressOrchestrationException(
                    message: "Invalid ingress orchestration argument(s), please correct the errors and try again."),

                (Rule: IsInvalid(supplierId), Parameter: "supplierId"));
        }

        private static void ValidateOnCheckForBatchComplete(Guid ingestionTrackingId)
        {
            Validate(
                 createException: () => new InvalidArgumentIngressOrchestrationException(
                     message: "Invalid ingress orchestration argument(s), please correct the errors and try again."),

                 (Rule: IsInvalid(ingestionTrackingId),
                Parameter: nameof(ingestionTrackingId)));
        }

        private static void ValidateOnRollbackIngestionTrackingItem(string encryptedFilePath)
        {
            Validate(
                createException: () => new InvalidArgumentIngressOrchestrationException(
                    message: "Invalid ingress orchestration argument(s), please correct the errors and try again."),

                (Rule: IsInvalid(encryptedFilePath),
                Parameter: nameof(IngestionTracking.EncryptedFileName)));
        }

        private static void ValidateStorageIngestionTracking(
            IngestionTracking ingestionTracking,
            Guid ingestionTrackingId)
        {
            if (ingestionTracking is null)
            {
                throw new NotFoundIngressOrchestrationException(
                    $"Couldn't find ingestion tracking with Id: {ingestionTrackingId}.");
            }
        }

        private static void ValidateStorageIngestionTracking(
            IngestionTracking ingestionTracking,
            string encryptedFileName)
        {
            if (ingestionTracking is null)
            {
                throw new NotFoundIngressOrchestrationException(
                    $"Couldn't find ingestion tracking with {nameof(IngestionTracking.EncryptedFileName)}: " +
                        $"{encryptedFileName}.");
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string input) => new
        {
            Condition = string.IsNullOrWhiteSpace(input),
            Message = "Text is required"
        };

        private static void Validate<T>(
            Func<T> createException,
            params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            T invalidDataException = createException();

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
