// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LHDS.Core.Models.Foundations.AddressMatchers;
using LHDS.Core.Models.Processings.AddressMatchers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingService : IAddressMatcherProcessingService
    {
        virtual internal void ValidateAddress(string? address) =>
            Validate<InvalidArgumentAddressMatcherProcessingException>(
                message: "Invalid address matcher processing argument(s), please correct the errors and try again.",
                (Rule: IsInvalid(address), Parameter: "address"));

        virtual internal void ValidateCalculateArguments(
            IList<KeyValuePair<string, string>> incomingAddressComponents,
            HashSet<AddressMatch> possibleAddresses) =>
                Validate<InvalidArgumentAddressMatcherProcessingException>(
                    message: "Invalid address matcher processing argument(s), please correct the errors and try again.",
                    (Rule: IsInvalid(incomingAddressComponents), Parameter: "IncomingAddressComponents"),
                    (Rule: IsInvalid(possibleAddresses), Parameter: "PossibleAddresses"));

        private void ValidateMatches(MatchCollection matches)
        {
            ValidateMultiplePostCodes(matches);
            ValidateNoPostCodesFound(matches);
        }

        private void ValidateMultiplePostCodes(MatchCollection matches)
        {
            if (matches.Count > 1)
            {
                throw new MultiplePostCodesAddressMatcherProcessingServiceException(
                    message: "Multiple Postcodes validation error occurred, please try again.");
            }
        }

        private void ValidateNoPostCodesFound(MatchCollection matches)
        {
            if (matches.Count == 0)
            {
                throw new PostCodeNotFoundAddressMatcherProcessingServiceException(
                message: "No Postcodes found validation error occurred, please try again.");
            }
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(IList<KeyValuePair<string, string>> data) => new
        {
            Condition = data == null || data.Count == 0,
            Message = "Values is required"
        };

        private static dynamic IsInvalid(HashSet<AddressMatch> data) => new
        {
            Condition = data == null || data.Count == 0,
            Message = "Values is required"
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
