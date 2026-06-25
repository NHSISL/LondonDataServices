// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using LHDS.Core.Models.Orchestrations.AddressToUprns.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.AddressToUprns
{
    public partial class AddressToUprnOrchestrationService
    {
        private void ValidateMatchAddressToUprnArguments(Stream input, string fileName, Guid correlationId)
        {
            Validate<InvalidArgumentAddressToUprnOrchestrationException>(
                () => new InvalidArgumentAddressToUprnOrchestrationException(
                    "Invalid address to UPRN orchestration argument(s), please correct the errors and try again."),
                (Rule: IsInvalid(input), Parameter: nameof(input)),
                (Rule: IsInvalid(fileName), Parameter: nameof(fileName)),
                (Rule: IsInvalid(correlationId), Parameter: nameof(correlationId)));
        }

        private static dynamic IsInvalid(Stream stream) => new
        {
            Condition = stream is null,
            Message = "Stream is required"
        };

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        internal virtual void Validate<T>(
            Func<T> exceptionFactory,
            params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            T invalidException = exceptionFactory();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidException.ThrowIfContainsErrors();
        }
    }
}
