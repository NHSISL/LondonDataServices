// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationService
    {
        private void ValidateDataOnProcessData(Stream input, string filename)
        {
            Validate<InvalidArgumentAddressCoordinationException>(
                message: "Invalid address coordination argument, please correct the errors and try again.",
                    (Rule: IsInvalidInputStream(input), Parameter: nameof(input)),
                    (Rule: IsInvalid(filename), Parameter: nameof(filename)));
        }

        private void ValidateFolderPathOnProcessData(string folderPath)
        {
            Validate<InvalidArgumentAddressCoordinationException>(
                message: "Invalid address coordination argument, please correct the errors and try again.",
                    (Rule: IsInvalid(folderPath), Parameter: nameof(folderPath)));
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalidInputStream(Stream? stream) => new
        {
            Condition = stream is null || stream.Length == 0,
            Message = "Stream is required"
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