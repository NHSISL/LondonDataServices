// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using LHDS.Core.Models.Orchestrations.TppLandings.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.Tpp
{
    public partial class TppLandingOrchestrationService
    {
        private static void ValidateArgumentsOnProcess(string fileName, Guid supplierId)
        {
            Validate(
                createException: () => new InvalidArgumentTppLandingOrchestrationException(
                    message: "Invalid TPP landing orchestration argument(s), please correct the errors and try again."),

                (Rule: IsInvalid(fileName), Parameter: "FileName"),
                (Rule: IsInvalid(supplierId), Parameter: "SupplierId"));
        }

        private static void ValidateArgumentsOnReProcess(Guid supplierId)
        {
            Validate(
                createException: () => new InvalidArgumentTppLandingOrchestrationException(
                    message: "Invalid TPP landing orchestration argument(s), please correct the errors and try again."),


                (Rule: IsInvalid(supplierId), Parameter: "SupplierId"));
        }
        private static void ValidateConfigurationForReland(int relandIntervalInMinutes)
        {
            Validate(
                createException: () => new InvalidArgumentTppLandingOrchestrationException(
                    message: "Invalid TPP Reland orchestration configuration, please correct the errors and try again."),

                (Rule: IsInvalid(relandIntervalInMinutes), Parameter: nameof(relandIntervalInMinutes)));
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

        private static dynamic IsInvalid(int value) => new
        {
            Condition = value <= 0,
            Message = "Value must be greater than zero"
        };

        private static dynamic IsInvalidInputStream(Stream? stream) => new
        {
            Condition = stream is null || stream.Length == 0,
            Message = "Stream is required"
        };

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