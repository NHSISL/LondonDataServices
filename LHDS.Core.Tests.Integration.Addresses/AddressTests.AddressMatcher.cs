// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Clients;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Xunit;

namespace LHDS.Core.Tests.Integration.Addresses
{
    public partial class AddressTests
    {
        [Fact]
        public async Task AddressMatcherDataAsync()
        {
            byte[] fileBytes =
               File.ReadAllBytes(
                   @"Resources\DataEngineering\IncomingFileToMatch.csv");

            FileInfo fi =
                new FileInfo(
                    @"Resources\DataEngineering\IncomingFileToMatch.csv");

            var fileName = fi.Name;

            // When
            await addressClient.MatchPatientAddressDataAsync(fileBytes, fileName);

            // Then

        }
    }
}
