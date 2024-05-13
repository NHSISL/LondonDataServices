// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Brokers.CryptographyKeys;
using LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.CryptographicKeys
{
    public partial class CryptographyKeyService
    {
        private void ValidateInputArguments(string cryptographyType)
        {
            Validate<InvalidArgumentCryptographyKeyException>(
                message: "Invalid cryptography key argument(s), please correct the errors and try again.",
                (Rule: IsInvalid(cryptographyType), Parameter: nameof(cryptographyType)));
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private void ValidateBrokerNotNull(ICryptographyKeyBroker? broker)
        {
            ValidateCryptographyBrokerIsNotNull(broker);
        }

        private static void ValidateCryptographyBrokerIsNotNull(ICryptographyKeyBroker? broker)
        {
            if (broker is null)
            {
                throw new NullBrokerCryptographyKeyException(message: "Broker is null.");
            }
        }

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