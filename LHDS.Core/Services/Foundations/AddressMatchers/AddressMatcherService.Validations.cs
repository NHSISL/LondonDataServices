// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Foundations.AddressMatchers;
using LHDS.Core.Models.Foundations.AddressMatchers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.AddressMatchers
{
    public partial class AddressMatcherService
    {
        virtual internal void ValidateCheckForBestMatchArguments(
            HashSet<AddressMatch> matchedAddresses) =>
                Validate<InvalidArgumentAddressMatcherException>(
                    message: "Invalid address matcher argument(s), please correct the errors and try again.",
                    (Rule: IsInvalid(matchedAddresses), Parameter: "MatchedAddresses"));

        virtual internal void ValidateAddressComponents(
            IList<KeyValuePair<string, string>> addressComponents) =>
                Validate<InvalidArgumentAddressMatcherException>(
                    message: "Invalid address matcher argument(s), please correct the errors and try again.",
                    (Rule: IsInvalid(addressComponents), Parameter: "AddressComponents"));

        virtual internal void ValidateCalculateArguments(
            IList<KeyValuePair<string, string>> incomingAddressComponents,
            HashSet<AddressMatch> possibleAddresses) =>
                Validate<InvalidArgumentAddressMatcherException>(
                    message: "Invalid address matcher argument(s), please correct the errors and try again.",
                    (Rule: IsInvalid(incomingAddressComponents), Parameter: "IncomingAddressComponents"),
                    (Rule: IsInvalid(possibleAddresses), Parameter: "PossibleAddresses"));

        private static dynamic IsInvalid(HashSet<AddressMatch> data) => new
        {
            Condition = data == null || data.Count == 0,
            Message = "Values is required"
        };

        private static dynamic IsInvalid(IList<KeyValuePair<string, string>> data) => new
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
