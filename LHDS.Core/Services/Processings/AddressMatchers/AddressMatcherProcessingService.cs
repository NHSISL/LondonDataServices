// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Text.RegularExpressions;
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
                        if (match.Value.StartsWith(" "))
                        {
                            return match.Value.TrimStart();
                        }

                        return match.Value + " ";
                    });

                    cleanAddress = regexSpaces.Replace(cleanAddress, " ");

                } while (cleanAddress != previousAddress);

                return cleanAddress;
            });

        public string ExtractPostCode(string address) =>
            throw new System.NotImplementedException();
    }
}
