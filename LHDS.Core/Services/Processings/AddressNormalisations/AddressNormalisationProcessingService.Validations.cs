// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Processings.AddressNormalisations.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.AddressNormalisations
{
    public partial class AddressNormalisationProcessingService
    {
        public void ValidateAddressNormalisationArgs(string address)
        {
            Validate<InvalidArgumentAddressNormalisationProcessingException>(
                message: "Invalid address normalisation processing argument. Please correct the errors and try again.",
                (Rule: IsInvalid(address), Parameter: "address"));
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
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