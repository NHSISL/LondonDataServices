// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using System.IO.Compression;
using LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions;

namespace LHDS.Core.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationService
    {
        private void ValidateDataOnProcessData(byte[] data)
        {
            ValidateDataIsNotNull(data);
        }

        private static void ValidateDataIsNotNull(byte[] data)
        {
            if (data is null)
            {
                throw new InvalidArgumentAddressExtractionOrchestrationException(
                    message: "Invalid argument address extraction orchestration exception, " +
                    "please correct the errors and try again.");
            }
        }

        private static void ValidateZipFileIsNotEmpty(MemoryStream memoryStream)
        {
            Validate((Rule: IsInvalid(memoryStream), Parameter: "MemoryStream"));
        }

        private static dynamic IsInvalid(MemoryStream memoryStream) => new
        {
            Condition = memoryStream == null,
            Message = "File(s) required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArchiveAddressExctractionOrchestrationException =
                new InvalidArchiveAddressExtractionOrchestrationException(
                    message: "Invalid address extraction orchestration archive, " +
                        "please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArchiveAddressExctractionOrchestrationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArchiveAddressExctractionOrchestrationException.ThrowIfContainsErrors();
        }
    }
}