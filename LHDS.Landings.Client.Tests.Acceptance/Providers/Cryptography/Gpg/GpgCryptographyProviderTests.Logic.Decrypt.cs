// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Documents
{
    public partial class GpgCryptographyProviderTests
    {
        [Fact]
        public async Task ShouldEncryptAndDecryptStringAsync()
        {
            // Given
            string randomString = GetRandomString();
            byte[] randomBytes = Encoding.UTF8.GetBytes(randomString);
            string expectedString = randomString;

            // When
            byte[] encryptedData = await this.cryptographyProvider.EncryptAsync(randomBytes);
            byte[] decryptedData = await this.cryptographyProvider.DecryptAsync(encryptedData);
            string actualString = Encoding.UTF8.GetString(decryptedData);

            // Then
            actualString.Should().BeEquivalentTo(expectedString);
        }

        //[Fact]
        //public async Task ShouldEncryptFileAsync()
        //{
        //    try
        //    {
        //        // Given
        //        var inputFilePath = "C:\\Temp\\TEST1\\delta_original.csv";
        //        var outputFilePath = "C:\\Temp\\TEST1\\delta_original_encrypted.csv.gpg";

        //        var data = File.ReadAllBytes(inputFilePath);

        //        // When
        //        byte[] encryptedData = await this.cryptographyProvider.EncryptAsync(data);

        //        // Then
        //        File.WriteAllBytes(outputFilePath, encryptedData);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //[Fact]
        //public async Task ShouldDecryptFileAsync()
        //{
        //    try
        //    {
        //        // Given
        //        var inputFilePath = "C:\\Temp\\TEST1\\delta_original.csv.gpg";
        //        var outputFilePath = "C:\\Temp\\TEST1\\delta_original_decrypted.csv";
        //        var encryptedData = File.ReadAllBytes(inputFilePath);

        //        // When
        //        byte[] decryptedData = await this.cryptographyProvider.DecryptAsync(encryptedData);

        //        // Then
        //        File.WriteAllBytes(outputFilePath, decryptedData);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}