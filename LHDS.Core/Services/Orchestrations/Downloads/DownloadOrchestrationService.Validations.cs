// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Orchestrations.Downloads.Exceptions;

namespace LHDS.Core.Services.Orchestrations.Downloads
{
    public partial class DownloadOrchestrationService
    {
        private void ValidateConfigurationSettings()
        {
            this.ValidateLandingConfigurationIsNotNull();
            this.ValidateBlobContainersIsNotNull();

            Validate((Rule: IsInvalid(this.landingConfiguration.LandingSupplierId),
                Parameter: "LandingConfiguration.SupplierId"));
        }

        private void ValidateLandingConfigurationIsNotNull()
        {
            if (this.landingConfiguration is null)
            {
                throw new NullLandingConfigurationDownloadOrchestrationException(
                    message: "Null landing configuration download orchestration exception, " +
                        "please correct the errors and try again.");
            }
        }

        private void ValidateBlobContainersIsNotNull()
        {
            if (this.blobContainers is null)
            {
                throw new NullBlobContainersDownloadOrchestrationException(
                    message: "Null blob container download orchestration exception, " +
                        "please correct the errors and try again.");
            }
        }

        private static void ValidateFileName(string fileName)
        {
            Validate((Rule: IsInvalid(fileName), Parameter: "FileName"));
        }

        private static void ValidateStorageDownload(Document maybeDocument, string fileName)
        {
            if (maybeDocument is null)
            {
                throw new NotFoundDownloadOrchestrationException(
                    message: $"Couldn't find download with file name: {fileName}.");
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
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentDownloadOrchestrationException = new InvalidArgumentDownloadOrchestrationException(
                message: "Invalid download orchestration argument(s), please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentDownloadOrchestrationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentDownloadOrchestrationException.ThrowIfContainsErrors();
        }
    }
}