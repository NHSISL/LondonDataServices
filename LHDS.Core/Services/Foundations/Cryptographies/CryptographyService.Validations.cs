// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using LHDS.Core.Models.Foundations.Cryptographies.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Cryptographies
{
    public partial class CryptographyService
    {
        private void ValidateInputs(Stream input, Stream output, SubscriberCredential subscriberCredential)
        {
            Validate(
                createException: () => new InvalidArgumentCryptographyException(
                    message: "Invalid cryptography arguments. Please correct the errors and try again."),

                (Rule: IsInvalidInputStream(input), Parameter: nameof(input)),
                (Rule: IsInvalidOutputStream(output), Parameter: nameof(output)),
                (Rule: IsInvalid(subscriberCredential), Parameter: nameof(subscriberCredential)));
        }

        private static dynamic IsInvalidInputStream(Stream? stream) => new
        {
            Condition = stream is null || stream.Length == 0,
            Message = "Stream is required"
        };

        private static dynamic IsInvalidOutputStream(Stream? stream) => new
        {
            Condition = stream is null || stream.Length > 0,
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

        private static void Validate<T>(
            Func<T> createException,
            params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            T invalidDataException = createException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataException.ThrowIfContainsErrors();
        }
    }
}