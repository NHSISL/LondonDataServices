// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
                throw new InvalidArgumentSubscriberCredentialOrchestrationException(
                    message: "Invalid argument subscriber credential orchestration exception, " +
                        "please correct the errors and try again.");
            }
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
