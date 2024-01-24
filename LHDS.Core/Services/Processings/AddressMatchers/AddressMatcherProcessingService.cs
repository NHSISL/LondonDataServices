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
            throw new System.NotImplementedException();

        public string ExtractPostCode(string address) =>
            TryCatch(() =>
            {
                ValidateAddress(address);
                string pattern = @"\b([A-Z]{1,2}\d{1,2}[A-Z]?\s*\d[A-Z]{2})\b";
                MatchCollection matches = Regex.Matches(address, pattern);

                string extractedPostCode = matches[0].Groups[1].Value;

                return extractedPostCode;
            });
    }
}
