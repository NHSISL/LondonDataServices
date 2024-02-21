// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.SecureData.Exceptions;
using LHDS.Core.Models.Foundations.SecureData;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions;
using Xeptions;
using System.Collections.Generic;

namespace LHDS.Core.Services.Processings.SecureDatas
{
    public partial class SecureDataProcessingService
    {
        private void ValidateSubscriberCredentialOnAdd(SubscriberCredential subscriberCredential)
        {
            ValidateSubscriberCredentialIsNotNull(subscriberCredential);

            Validate<InvalidSubscriberCredentialException>(
                message: "Invalid subscriber credential errors occured. Please correct the errors and try again.",
                (Rule: IsInvalid(subscriberCredential.Id), Parameter: nameof(SubscriberCredential.Id)),

                (Rule: IsInvalid(subscriberCredential.SupplierSharingAgreementShortName), Parameter: nameof(
                    SubscriberCredential.SupplierSharingAgreementShortName)),

                (Rule: IsInvalid(subscriberCredential.FtpUserName), Parameter: nameof(
                    SubscriberCredential.FtpUserName)),

                (Rule: IsInvalid(subscriberCredential.FtpPassword), Parameter: nameof(
                    SubscriberCredential.FtpPassword)),

                (Rule: IsInvalid(subscriberCredential.FtpPassPhrase), Parameter: nameof(
                    SubscriberCredential.FtpPassPhrase)),

                (Rule: IsInvalid(subscriberCredential.FtpPrivateKey), Parameter: nameof(
                    SubscriberCredential.FtpPrivateKey)),

                (Rule: IsInvalid(subscriberCredential.FtpPublicKey), Parameter: nameof(
                    SubscriberCredential.FtpPublicKey)),

                (Rule: IsInvalid(subscriberCredential.GpgPassPhrase), Parameter: nameof(
                    SubscriberCredential.GpgPassPhrase)),

                (Rule: IsInvalid(subscriberCredential.GpgPrivateKey), Parameter: nameof(
                    SubscriberCredential.GpgPrivateKey)),

                (Rule: IsInvalid(subscriberCredential.GpgPublicKey), Parameter: nameof(
                    SubscriberCredential.GpgPublicKey)));
        }

        private void ValidateSubscriberCredentialOnRetrieve(SubscriberCredential subscriberCredential)
        {
            ValidateSubscriberCredentialIsNotNull(subscriberCredential);

            Validate<InvalidSubscriberCredentialException>(
                message: "Invalid subscriber credential errors occurred. Please correct the errors and try again.",
                (Rule: IsInvalid(subscriberCredential.Id), Parameter: nameof(SubscriberCredential.Id)),

                (Rule: IsInvalid(subscriberCredential.SupplierSharingAgreementShortName), Parameter: nameof(
                    SubscriberCredential.SupplierSharingAgreementShortName)),

                (Rule: IsInvalid(subscriberCredential.FtpUserName), Parameter: nameof(
                    SubscriberCredential.FtpUserName)),

                (Rule: IsInvalid(subscriberCredential.FtpPublicKey), Parameter: nameof(
                    SubscriberCredential.FtpPublicKey)),

                (Rule: IsInvalid(subscriberCredential.GpgPublicKey), Parameter: nameof(
                    SubscriberCredential.GpgPublicKey)));
        }

        private void ValidateSecureData(SecureData secureData)
        {
            ValidateSecureDataIsNotNull(secureData);

            Validate<InvalidSecureDataException>(
                message: "Invalid secure data errors occured. Please correct the errors and try again.",
                (Rule: IsInvalid(secureData.Name), Parameter: nameof(SecureData.Name)),
                (Rule: IsInvalid(secureData.Value), Parameter: nameof(SecureData.Value)));
        }

        private void ValidateSecureDataProperty(string propertyName)
        {
            Validate<InvalidArgumentSubscriberCredentialProcessingException>(
                message: "Invalid argument subscriber credential processing error occurred, contact support.",
                (Rule: IsInvalidProperty(propertyName), Parameter:nameof(propertyName)));
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

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalidProperty(string property)
        {
            List<string> validProperties = new List<string>
            {
                "FtpPassword",
                "FtpPassPhrase",
                "FtpPrivateKey",
                "GpgPassPhrase",
                "GpgPrivateKey"
            };

            return new
            {
                Condition = !validProperties.Contains(property),
                Message = "Invalid property."
            };
        }

        private static void Validate<T>(string message, params (dynamic Rule, string Parameter)[] validations)
             where T : Xeption
        {
            var invalidDataException = (T)Activator.CreateInstance(typeof(T), message);

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