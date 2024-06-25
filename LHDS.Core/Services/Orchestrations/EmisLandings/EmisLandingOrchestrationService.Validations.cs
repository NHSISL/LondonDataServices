// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Services.Orchestrations.Downloads
{
    public partial class EmisLandingOrchestrationService
    {
        private void ValidateConfigurationSettings()
        {
            this.ValidateLandingConfigurationIsNotNull();
            this.ValidateBlobContainersIsNotNull();

            Validate((Rule: IsInvalid(this.landingConfiguration.LandingSupplierId),
                Parameter: "LandingConfiguration.SupplierId"));
        }

        private void ValidateSubscriberCredentials(SubscriberCredential subscriberCredential)
        {
            if (subscriberCredential is null)
            {
                throw new NullSubscriberCredentialEmisLandingOrchestrationException(
                    message: "Null subscriber credential EMIS landing orchestration exception, " +
                        "please correct the errors and try again.");
            }
        }

        public void ValidateIngestionTrackingId(Guid ingestionTrackingId)
        {
            Validate((Rule: IsInvalid(ingestionTrackingId), Parameter: "ingestionTrackingId"));
        }

        public void ValidateProcessArguments(Guid supplierId)
        {
            Validate((Rule: IsInvalid(supplierId), Parameter: "SupplierId"));
        }

        private void ValidateLandingConfigurationIsNotNull()
        {
            if (this.landingConfiguration is null)
            {
                throw new NullLandingConfigurationEmisLandingOrchestrationException(
                    message: "Null landing configuration EMIS landing orchestration exception, " +
                        "please correct the errors and try again.");
            }
        }

        private void ValidateBlobContainersIsNotNull()
        {
            if (this.blobContainers is null)
            {
                throw new NullBlobContainersEmisLandingOrchestrationException(
                    message: "Null blob container EMIS landing orchestration exception, " +
                        "please correct the errors and try again.");
            }
        }

        private static void ValidateProcessFileArguments(string fileName, Guid supplierId)
        {
            Validate(
                (Rule: IsInvalid(fileName), Parameter: "FileName"),
                (Rule: IsInvalid(supplierId), Parameter: "SupplierId"));
        }

        private static void ValidateRetrieveDownloadByFileNameArguments(Stream output, string fileName)
        {
            Validate(
                (Rule: IsInvalidOutputStream(output), Parameter: "Output"),
                (Rule: IsInvalid(fileName), Parameter: "FileName"));
        }

        private static void ValidateStorageDownload(Stream output, string fileName)
        {
            if (output is null || output.Length == 0)
            {
                throw new NotFoundEmisLandingOrchestrationException(
                    message: $"Couldn't find download with file name: {fileName}.");
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalidInputStream(Stream? stream) => new
        {
            Condition = stream is null || stream.Length == 0,
            Message = "Stream is required"
        };

        private static dynamic IsInvalidOutputStream(Stream? stream) => new
        {
            Condition = stream is null || stream.Length > 0,
            Message = "Stream is required"
        };

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentEmisLandingOrchestrationException = new InvalidArgumentEmisLandingOrchestrationException(
                message: "Invalid EMIS landing orchestration argument(s), please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentEmisLandingOrchestrationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentEmisLandingOrchestrationException.ThrowIfContainsErrors();
        }
    }
}