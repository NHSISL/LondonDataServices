// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Text.RegularExpressions;
using CsvHelper;
using LHDS.Core.Brokers.Loggings;

namespace LHDS.Core.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingService : IAddressMatcherProcessingService
    {
        private readonly ILoggingBroker loggingBroker;

        public AddressMatcherProcessingService(ILoggingBroker loggingBroker)
        {
            this.loggingBroker = loggingBroker;
        }

        public string CleanAddress(string address) =>
            TryCatch(() =>
            {
                ValidateAddress(address);
                this.loggingBroker.LogNothing();
                var cleanAddress = address.ToLower().Trim();
                var punctuationPattern = @"\s[,.!?-]|[,.!?;:'""](?![ ])";
                var regexPunctuation = new Regex(punctuationPattern, RegexOptions.Compiled);
                var regexSpaces = new Regex(@"[ ]{2,}", RegexOptions.Compiled);
                string previousAddress;

                do
                {
                    previousAddress = cleanAddress;

                    cleanAddress = regexPunctuation.Replace(cleanAddress, match =>
                    {
                        if (match.Value.StartsWith(" ") || match.Value.EndsWith(" "))
                        {
                            return match.Value.Trim();
                        }

                        return match.Value + " ";
                    });

                    cleanAddress = regexSpaces.Replace(cleanAddress, " ");

                } while (cleanAddress != previousAddress);

                return cleanAddress;
            });

        public string ExtractPostCode(string address) =>
            TryCatch(() =>
            {
                ValidateAddress(address);
                string pattern = @"\b([A-Z]{1,2}\d{1,2}[A-Z]?\s*\d[A-Z]{2})\b";
                MatchCollection matches = Regex.Matches(address, pattern);
                ValidateMatches(matches);
                string extractedPostCode = matches[0].Groups[1].Value;
                

                return extractedPostCode;
            });
    }
}
