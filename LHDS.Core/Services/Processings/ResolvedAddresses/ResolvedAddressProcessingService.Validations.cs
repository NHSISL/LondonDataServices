// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Processings.ResolvedAddresses.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.ResolvedAddresses
{
    public partial class ResolvedAddressProcessingService : IResolvedAddressProcessingService
    {
        private void ValidateResolvedAddress(ResolvedAddress resolvedAddress)
        {
            ValidateResolvedAddressIsNotNull(resolvedAddress);
        }

        private void ValidateArguments(List<ResolvedAddress> resolvedAddresses, string fileName)
        {
            Validate<InvalidArgumentResolvedAddressProcessingException>(
                message: "Invalid argument(s). Please correct the errors and try again.",
                (Rule: IsInvalid(resolvedAddresses), Parameter: nameof(resolvedAddresses)),
                (Rule: IsInvalid(fileName), Parameter: nameof(fileName)));
        }

        private void ValidateAddress(string address)
        {
            Validate<InvalidArgumentResolvedAddressProcessingException>(
                message: "Invalid argument(s). Please correct the errors and try again.",
                (Rule: IsInvalid(address), Parameter: "Address"));
        }

        private static void ValidateResolvedAddressIsNotNull(ResolvedAddress resolvedAddress)
        {
            if (resolvedAddress is null)
            {
                throw new NullResolvedAddressProcessingException(message: "ResolvedAddress is null.");
            }
        }

        public void ValidateResolvedAddressId(Guid resolvedAddressId) =>
            Validate<InvalidArgumentResolvedAddressProcessingException>(
                message: "Invalid argument(s). Please correct the errors and try again.",
                (Rule: IsInvalid(resolvedAddressId), Parameter: nameof(ResolvedAddress.Id)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(List<ResolvedAddress>? resolvedAddresses) => new
        {
            Condition = resolvedAddresses is null,
            Message = "Resolved addresses is required"
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
