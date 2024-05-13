// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.SubscriberCredentials
{
    public partial class SubscriberCredentialOrchestration
    {
        private static void ValidateSubscriberCredential(SubscriberCredential subscriberCredential)
        {
            ValidateSubscriberCredentialIsNotNull(subscriberCredential);
        }

        private static void ValidateSubscriberCredentialIsNotNull(SubscriberCredential subscriberCredential)
        {
            if (subscriberCredential == null)
            {
                throw new InvalidSubscriberCredentialOrchestrationException(
                    message: "Null subscriber credential orchestration exception, " +
                        "please correct the errors and try again.");
            }
        }

        private static void ValidateSubscriberAgreementIsNotNull(SubscriberAgreement subscriberAgreement)
        {
            if (subscriberAgreement == null)
            {
                throw new InvalidSubscriberAgreementOrchestrationException(
                    message: "Invalid subscriber agreement orchestration exception, " +
                        "please correct the errors and try again.");
            }
        }

        private void ValidateSubscriberCredentialId(Guid subscriberCredentialId)
        {
            Validate<InvalidArgumentSubscriberCredentialOrchestrationException>(
                 message: "Invalid argument subscriber credential orchestration error occurred, please contact support.",
                 (Rule: IsInvalid(subscriberCredentialId), Parameter: "subscriberCredentialId"));
        }

        private static dynamic IsInvalid(Guid someId) => new
        {
            Condition = someId == Guid.Empty,
            Message = "Id is required"
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
