// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LHDS.Core.Models.Orchestrations.Addresses.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationService : IAddressOrchestrationService
    {
        private void ValidateDataOnBulkAddAddresses(Stream input, string fileName)
        {
            Validate<InvalidArgumentAddressOrchestrationException>(
                message: "Invalid argument address orchestration exception, " +
                        "please correct the errors and try again.",
                    (Rule: IsInvalidInputStream(input), Parameter: nameof(input)),
                    (Rule: IsInvalid(fileName), Parameter: nameof(fileName)));
        }

        private void ValidateCsvFiles(List<string> csvFiles)
        {
            List<string> validFiles = csvFiles
                .Where(file => file.Contains("ID28")
                    || file.Contains("ID24")
                    || file.Contains("ID21")
                    || file.Contains("ID15"))
                .ToList();

            if (validFiles.Count != 4)
            {
                throw new InvalidFileAddressOrchestrationException(
                    message: $"The zip file does not contain the required csv files. " +
                        $"Please correct the errors and try again.");
            }
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalidInputStream(Stream? stream) => new
        {
            Condition = stream is null || stream.Length == 0,
            Message = "Stream is required"
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
