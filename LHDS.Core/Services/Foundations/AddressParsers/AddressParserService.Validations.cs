// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions;
using System;
using LHDS.Core.Models.Foundations.AddressParsers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.AddressParsers
{
    public partial class AddressParserService
    {
        virtual internal void ValidateAddressParserOnProcessCSV(byte[] data)
        {
            ValidateAddressParserIsNotNull(data);
        }

        virtual internal void ValidateAddressParserOnProcessCSV(string data)
        {
            Validate<InvalidArgumentAddressParserException>(
                message: "Invalid argument. Please correct the errors and try again.",
                    (Rule: IsInvalid(data), Parameter: "data"));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void ValidateAddressParserIsNotNull(byte[] data)
        {
            if (data is null)
            {
                throw new InvalidArgumentAddressParserException(message: "Address parser is null.");
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