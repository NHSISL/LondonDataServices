// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Brokers.Hashing;
using Xunit;

namespace LHDS.Core.Tests.Unit.Brokers.Hashing
{
    public class HashBrokerTests
    {
        [Fact]
        public async Task ShouldHashNhsNumberWithSha256_BytesMatchSqlServer()
        {
            // Arrange
            string nhsNumber = "0000000000";
            string expectedHex = "0x3C37528723BD5259CDCE3EB7D3C4565C1C89A358D23608A1466996AFE035BDAC";
            byte[] expectedHashBytes = ConvertHexStringToByteArray(expectedHex);
            var broker = new HashBroker();
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(nhsNumber));

            // Act
            string actualHashString = await broker.GenerateSha256HashAsync(stream);
            byte[] actualHashBytes = Convert.FromHexString(actualHashString);

            // Assert
            actualHashBytes.Should().BeEquivalentTo(expectedHashBytes);
        }

        private static byte[] ConvertHexStringToByteArray(string hex)
        {
            if (hex.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                hex = hex.Substring(2);

            return Convert.FromHexString(hex);
        }
    }
}
