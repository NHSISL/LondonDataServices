// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Coordinations.Decryptions.Exceptions;

namespace LHDS.Core.Services.Coordinations.Decryptions
{
    public partial class DecryptionCoordinationService
    {
        private void ValidateOnProcessDecryptedItemsForBatchComplete(Guid supplierId)
        {
            Validate((Rule: IsInvalid(supplierId), Parameter: "supplierId"));
        }

        private void ValidateFileNameOnDecrypt(string fileName)
        {
            ValidateDataIsNotNull(fileName);
        }

        private static void ValidateDataIsNotNull(string fileName)
        {
            Validate((Rule: IsInvalid(fileName), Parameter: "fileName"));
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentDecryptionCoordinationException =
                new InvalidArgumentDecryptionCoordinationException(
                    message: "Invalid decryption coordination argument, please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentDecryptionCoordinationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentDecryptionCoordinationException.ThrowIfContainsErrors();
        }
    }
}