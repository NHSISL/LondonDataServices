// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using LHDS.Core.Models.Foundations.SecureData;
using LHDS.Core.Models.Foundations.SecureData.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.SecureDatas
{
    public partial class SecureDataProcessingService
    {
        private void ValidateSubscriberCredentialOnAdd(SubscriberCredential subscriberCredential)
        {
            ValidateSubscriberCredentialIsNotNull(subscriberCredential);
        }

        private void ValidateSubscriberCredentialOnRetrieve(SubscriberCredential subscriberCredential)
        {
            ValidateSubscriberCredentialIsNotNull(subscriberCredential);

            Validate<InvalidSubscriberCredentialException>(
                message: "Invalid subscriber credential errors occurred. Please correct the errors and try again.",
                (Rule: IsInvalid(subscriberCredential.Id), Parameter: nameof(SubscriberCredential.Id)));
        }

        private void ValidateSubscriberCredentialIdOnRemove(Guid subscriberCredentialId)
        {
            Validate<InvalidArgumentSubscriberCredentialProcessingException>(
                 message: "Invalid argument subscriber credential processing error occurred, please contact support.",
                 (Rule: IsInvalid(subscriberCredentialId), Parameter: "subscriberCredentialId"));
        }

        private void ValidateSecureData(SecureData secureData)
        {
            ValidateSecureDataIsNotNull(secureData);

            Validate<InvalidSecureDataException>(
                message: "Invalid secure data errors occured. Please correct the errors and try again.",
                (Rule: IsInvalid(secureData.Name), Parameter: nameof(SecureData.Name)));
        }

        private void ValidateKeysExist(List<string> keyTypes, SubscriberCredential subscriberCredential)
        {
            ValidateSubscriberCredentialIsNotNull(subscriberCredential);
            List<(dynamic Rule, string Parameter)> rules = new List<(dynamic Rule, string Parameter)>();

            foreach (var keyType in keyTypes)
            {
                var rule = (Rule: IsInvalidProperty(keyType, subscriberCredential), Parameter: keyType);
                rules.Add(rule);
            }

            Validate<InvalidArgumentSubscriberCredentialProcessingException>(
                message: "Invalid argument subscriber credential processing error occurred, please contact support.",
                rules.ToArray());
        }

        private static void ValidateSubscriberCredentialIsNotNull(SubscriberCredential subscriberCredential)
        {
            if (subscriberCredential is null)
            {
                throw new NullSubscriberCredentialException(message: "Subscriber credential is null.");
            }
        }

        private static void ValidateSecureDataIsNotNull(SecureData secureData)
        {
            if (secureData is null)
            {
                throw new NullSecureDataException(message: "Secure data is null.");
            }
        }

        private static dynamic IsInvalid(Guid someId) => new
        {
            Condition = someId == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalidProperty(string keyType, SubscriberCredential subscriberCredential)
        {
            Type type = subscriberCredential.GetType();
            PropertyInfo? property = type.GetProperty(keyType);

            return new
            {
                Condition = property == null,
                Message = "Invalid property"
            };
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