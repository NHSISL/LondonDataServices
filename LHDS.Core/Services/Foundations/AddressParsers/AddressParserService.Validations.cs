// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.AddressParsers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.AddressParsers
{
    public partial class AddressParserService
    {
        virtual internal void ValidateAddressParserOnProcessCSV(byte[] data, string filename)
        {
            Validate<InvalidArgumentAddressParserException>(
                message: "Invalid arguments. Please correct the errors and try again.",
                    (Rule: IsInvalid(data), Parameter: "data"),
                    (Rule: IsInvalid(filename), Parameter: "filename"));
        }

        virtual internal void ValidateAddressParserOnProcessCSV(string data, string filename)
        {
            Validate<InvalidArgumentAddressParserException>(
                message: "Invalid arguments. Please correct the errors and try again.",
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
            Condition = data is null || data.Length == 0,
            Message = "Data is required"
        };

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