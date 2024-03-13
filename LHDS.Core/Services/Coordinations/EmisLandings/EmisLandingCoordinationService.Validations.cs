// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Coordinations.EmisLandings.Exceptions;

namespace LHDS.Core.Services.Coordinations.EmisLandings
{
    public partial class EmisLandingCoordinationService
    {
        private static void ValidateFileNameOnLand(string fileName)
        {
            Validate((Rule: IsInvalid(fileName), Parameter: "FileName"));
        }

        private static void ValidateFileNameOnRetrieve(string fileName)
        {
            Validate(
                (Rule: IsInvalid(fileName), Parameter: "FileName"),
                (Rule: IsInvalidArray(fileName), Parameter: "FileName"));
        }

        private static void ValidateArgsOnRetrieveListOfDocumentsToProcess(Guid subscriberAgreementId)
        {
            Validate((Rule: IsInvalid(subscriberAgreementId), Parameter: "SubscriberAgreementId"));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalidArray(string value) => new
        {
            Condition = value == null ? true : value.Split("/").Length < 6,
            Message = "File name is not valid"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentEmisLandingCoordinationException =
                new InvalidArgumentEmisLandingCoordinationException(
                    message: "Invalid Emis Landing coordination argument, please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentEmisLandingCoordinationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentEmisLandingCoordinationException.ThrowIfContainsErrors();
        }
    }
}