// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationService
    {
        public void ValidateResolvedAddressArgsOnAdd(byte[] data, string fileName, string container)
        {
            Validate<InvalidArgumentResolvedAddressOrchestrationException>(
                message: "Invalid argument resolved address orchestration exception, " +
                    "please correct the errors and try again.",
                (Rule: IsInvalid(data), Parameter: "data"),
                (Rule: IsInvalid(fileName), Parameter: nameof(fileName)),
                (Rule: IsInvalid(container), Parameter: nameof(container)));
        }

        public void ValidateResolvedAddressArgsOnRemove(string fileName, string container)
        {
            Validate<InvalidArgumentResolvedAddressOrchestrationException>(
                message: "Invalid argument resolved address orchestration exception, " +
                    "please correct the errors and try again.",
                (Rule: IsInvalid(fileName), Parameter: nameof(fileName)),
                (Rule: IsInvalid(container), Parameter: nameof(container)));
        }

        public void ValidateOnUploadAddressesToResolve(Stream input, string fileName)
        {
            Validate<InvalidArgumentResolvedAddressOrchestrationException>(
                message: "Invalid argument resolved address orchestration exception, " +
                    "please correct the errors and try again.",
                (Rule: IsInvalidInputStream(input), Parameter: nameof(input)),
                (Rule: IsInvalid(fileName), Parameter: nameof(fileName)));
        }

        private static void ValidateNewResolvedAddress(ResolvedAddress maybeResolvedAddress)
        {
            if (maybeResolvedAddress is null)
            {
                var nullResolvedAddressOrchestrationException =
                    new NullResolvedAddressOrchestrationException(
                        message: "Null Resolved Address orchestration exception, " +
                            "please correct the errors and try again.");

                throw nullResolvedAddressOrchestrationException;
            }
        }

        private static dynamic IsInvalidInputStream(Stream? stream) => new
        {
            Condition = stream is null || stream.Length == 0,
            Message = "Stream is required"
        };

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(byte[] data) => new
        {
            Condition = data == null || data.Length == 0,
            Message = "Data is required"
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
