// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using LHDS.Core.Models.Coordinations.EmisLandings.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Coordinations.EmisLandings
{
    public partial class EmisLandingCoordinationService
    {
        private static void ValidateProcessArgs(Guid supplierId)
        {
            Validate(
                createException: () => new InvalidArgumentEmisLandingCoordinationException(
                    message: "Invalid Emis Landing coordination argument, please correct the errors and try again."),

                (Rule: IsInvalid(supplierId), Parameter: "SupplierId"));
        }

        private static void ValidateProcessFileArgs(string fileName, Guid supplierId)
        {
            Validate(
                createException: () => new InvalidArgumentEmisLandingCoordinationException(
                    message: "Invalid Emis Landing coordination argument, please correct the errors and try again."),

                (Rule: IsInvalid(fileName), Parameter: "FileName"),
                (Rule: IsInvalid(supplierId), Parameter: "SupplierId"));
        }

        private static void ValidateFileNameOnRetrieve(Stream output, string fileName)
        {
            Validate(
                createException: () => new InvalidArgumentEmisLandingCoordinationException(
                    message: "Invalid Emis Landing coordination argument, please correct the errors and try again."),

                (Rule: IsInvalidOutputStream(output), Parameter: "Output"),
                (Rule: IsInvalid(fileName), Parameter: "FileName"),
                (Rule: IsInvalidFileNameSegments(fileName), Parameter: "FileName"));
        }

        private static void ValidateArgsOnRetrieveListOfDocumentsToProcess(Guid subscriberAgreementId)
        {
            Validate(
                createException: () => new InvalidArgumentEmisLandingCoordinationException(
                    message: "Invalid Emis Landing coordination argument, please correct the errors and try again."),

                (Rule: IsInvalid(subscriberAgreementId), Parameter: "SubscriberAgreementId"));
        }

        private static void ValidateArgsOnRedecryptDocumentByIngestionId(Guid ingestionTrackingId)
        {
            Validate(
                createException: () => new InvalidArgumentEmisLandingCoordinationException(
                    message: "Invalid Emis Landing coordination argument, please correct the errors and try again."),

                (Rule: IsInvalid(ingestionTrackingId), Parameter: "ingestionTrackingId"));
        }

        private static void ValidateArgsOnReLandDocumentByFileName(string fileName)
        {
            Validate(
                createException: () => new InvalidArgumentEmisLandingCoordinationException(
                    message: "Invalid Emis Landing coordination argument, please correct the errors and try again."),

                (Rule: IsInvalid(fileName), Parameter: "FileName"));
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalidFileNameSegments(string value) => new
        {
            Condition = value == null ? true : value.Split("/").Length < 6,
            Message = "File name is not valid"
        };

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