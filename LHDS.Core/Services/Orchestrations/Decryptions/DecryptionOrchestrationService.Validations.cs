// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using LHDS.Core.Models.Orchestrations.Decryptions.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Services.Orchestrations.Decryptions
{
    public partial class DecryptionOrchestrationService
    {
        private void ValidateBlobContainersIsNotNull()
        {
            if (this.blobContainers is null)
            {
                throw new NullBlobContainersDecryptionOrchestrationException(
                    message: "Null blob container decryption orchestration exception, " +
                        "please correct the errors and try again.");
            }
        }

        private void ValidateSubscriberCredentials(SubscriberCredential subscriberCredential)
        {
            if (subscriberCredential is null)
            {
                throw new NullSubscriberCredentialDecryptionOrchestrationException(
                    message: "Null subscriber credential decryption orchestration exception, " +
                        "please correct the errors and try again.");
            }
        }

        private static void ValidateStorageDocumentIsNotNull(Stream document, string encryptedFileName)
        {
            if (document is null)
            {
                throw new NotFoundDecryptionOrchestrationException(
                    message: $"Couldn't find document with file name: {encryptedFileName}.");
            }
        }

        private static void ValidateFileNameIsNotNull(string fileName)
        {
            Validate((Rule: IsInvalid(fileName), Parameter: "FileName"));
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentDecryptionOrchestrationException =
                new InvalidArgumentDecryptionOrchestrationException(
                    message: "Invalid decryption orchestration argument(s), please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentDecryptionOrchestrationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentDecryptionOrchestrationException.ThrowIfContainsErrors();
        }
    }
}