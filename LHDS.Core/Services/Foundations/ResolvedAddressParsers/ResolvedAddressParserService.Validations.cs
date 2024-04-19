// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.ResolvedAddressParsers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.ResolvedAddressParsers
{
    public partial class ResolvedAddressParserService
    {
        virtual internal void ValidateResolvedAddressParserOnProcessCSV(byte[] data, string filename)
        {
            Validate<InvalidArgumentResolvedAddressParserException>(
                message: "Invalid argument. Please correct the errors and try again.",
                    (Rule: IsInvalid(data), Parameter: "data"),
                    (Rule: IsInvalid(filename), Parameter: "filename"));
        }

        virtual internal void ValidateResolvedAddressParserOnProcessCSV(string data, string filename)
        {
            Validate<InvalidArgumentResolvedAddressParserException>(
                message: "Invalid argument. Please correct the errors and try again.",
                    (Rule: IsInvalid(data), Parameter: "data"),
                    (Rule: IsInvalid(filename), Parameter: "filename"));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(byte[] data) => new
        {
            Condition = data == null || data.Length == 0,
            Message = "Data is required"
        };

        private static void ValidateResolvedAddressParserIsNotNull(byte[] data)
        {
            if (data is null)
            {
                throw new InvalidArgumentResolvedAddressParserException(message: "ResolvedAddress parser is null.");
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