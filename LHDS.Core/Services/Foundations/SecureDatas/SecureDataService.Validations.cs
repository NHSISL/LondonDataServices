// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.SecureData;
using LHDS.Core.Models.Foundations.SecureData.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.SecureDatas
{
    public partial class SecureDataService
    {
        private void ValidateSecureDataOnAdd(SecureData secureData)
        {
            ValidateSecureDataIsNotNull(secureData);

            Validate<InvalidSecureDataException>(
                message: "Invalid secure data errors occured. Please correct the errors and try again.",
                (Rule: IsInvalid(secureData.Name), Parameter: nameof(SecureData.Name)));
        }

        private void ValidateArgumentOnRetrieve(string secretName)
        {
            Validate<InvalidArgumentSecureDataException>(
                message: "Invalid secure data argument. Please correct the errors and try again.",
                    (Rule: IsInvalid(secretName), Parameter: "secretName"));
        }

        private void ValidateArgumentOnRemove(string secretName)
        {
            Validate<InvalidArgumentSecureDataException>(
                message: "Invalid secure data argument. Please correct the errors and try again.",
                    (Rule: IsInvalid(secretName), Parameter: "secretName"));
        }

        private static void ValidateSecureDataIsNotNull(SecureData secureData)
        {
            if (secureData is null)
            {
                throw new NullSecureDataException(message: "Secure data is null.");
            }
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