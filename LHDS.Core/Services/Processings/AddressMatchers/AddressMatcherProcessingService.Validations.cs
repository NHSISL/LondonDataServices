// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Text.RegularExpressions;
using LHDS.Core.Models.Processings.Addresses.Exceptions;
using LHDS.Core.Models.Processings.AddressMatcher.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingService : IAddressMatcherProcessingService
    {

        public void ValidateAddress(string address) =>
            Validate<InvalidArgumentAddressMatcherProcessingException>(
                message: "Invalid address matcher processing argument(s), please correct the errors and try again.",
                (Rule: IsInvalid(address), Parameter: "address"));

        private void ValidateMultiplePostCodes(MatchCollection matches)
        {
            if (matches.Count > 1)
            {
                throw new MultiplePostCodesAddressMatcherProcessingServiceException(
                    message: "Multiple Postcodes validation error occurred, please try again.");
            }
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