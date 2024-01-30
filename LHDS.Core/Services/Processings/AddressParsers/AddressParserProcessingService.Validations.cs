// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Processings.AddressParsers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.AddressParsers
{
    public partial class AddressParserProcessingService
    {
        public void ValidateAddressParserArgs(string address)
        {
            Validate<InvalidArgumentAddressParserProcessingException>(
                message: "Invalid address parser processing argument. Please correct the errors and try again.",
                (Rule: IsInvalid(address), Parameter: "address"));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
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