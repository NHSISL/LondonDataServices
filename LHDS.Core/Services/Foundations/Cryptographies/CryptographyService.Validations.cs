// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using LHDS.Core.Models.Foundations.Cryptographies.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Services.Foundations.Cryptographies
{
    public partial class CryptographyService
    {
        private void ValidateInputs(Stream input, Stream output, SubscriberCredential subscriberCredential)
        {
            Validate(
                (Rule: IsInvalid(input), Parameter: nameof(input)),
                (Rule: IsInvalid(output), Parameter: nameof(output)),
                (Rule: IsInvalid(subscriberCredential), Parameter: nameof(subscriberCredential)));
        }

        private static dynamic IsInvalid(Stream? stream) => new
        {
            Condition = stream is null,
            Message = "Stream is required"
        };

        private static dynamic IsInvalid(SubscriberCredential? subscriberCredential) => new
        {
            Condition = subscriberCredential is null,
            Message = "SubscriberCredential is required"
        };

        private static void ValidateDataIsNotNull(Stream input)
        {
            if (input is null)
            {
                throw new InvalidArgumentCryptographyException(message: "Data is null.");
            }
        }

        private static void ValidateSubscriberCredentialsIsNotNull(SubscriberCredential subscriberCredential)
        {
            if (subscriberCredential is null)
            {
                throw new NullSubscriberCredentialCryptographyException(message: "Subscriber credential is null.");
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidDocumentProcessingException = new InvalidArgumentCryptographyException(
                message: "Invalid cryptography arguments. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDocumentProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDocumentProcessingException.ThrowIfContainsErrors();
        }
    }
}