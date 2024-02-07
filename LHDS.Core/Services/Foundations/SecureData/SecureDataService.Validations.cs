// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.SecureData;
using LHDS.Core.Models.Foundations.SecureData.Exceptions;

namespace LHDS.Core.Services.Foundations.SecureDatas
{
    public partial class SecureDataService
    {
        private void ValidateSecureDataOnAdd(SecureData secureData)
        {
            ValidateSecureDataIsNotNull(secureData);

            Validate(
                (Rule: IsInvalid(secureData.Name), Parameter: nameof(SecureData.Name)),
                (Rule: IsInvalid(secureData.Value), Parameter: nameof(SecureData.Value)));
        }

        private void ValidateSecureDataOnRetrieve(string secretName)
        {

            Validate(
                (Rule: IsInvalid(secretName), Parameter: "secretName"));
        }

        private static void ValidateSecureDataIsNotNull(SecureData secureData)
        {
            if (secureData is null)
            {
                throw new NullSecureDataException(message: "Secure data is null.");
            }
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidSecureDataException = new InvalidSecureDataException(
                message: "Invalid secure data. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidSecureDataException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidSecureDataException.ThrowIfContainsErrors();
        }
    }
}