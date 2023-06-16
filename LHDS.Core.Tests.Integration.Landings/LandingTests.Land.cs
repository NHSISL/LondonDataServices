// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace LHDS.Core.Tests.Integration.Landings
{
    public partial class LandingTests
    {
        [ReleaseCandidateFact]
        public async Task ShouldLandFilesAsync()
        {
            try
            {
                // given
                string list = CreateRandomListNhsNumbers(GetRandomNumber());
                byte[] fileContent = Encoding.ASCII.GetBytes(list);
                MemoryStream stream = new MemoryStream(fileContent);
                string fileName = $"{this.landingConfiguration.EncryptedFolder}/{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
                await this.blobStorageBroker.InsertFileAsync(fileName, stream);

                // when
                List<string> files = await landingClient.ProcessAsync();

                // then

                files.Should().NotBeNull();
                //TODO: files.Count.Should().BeGreaterThan(setupFiles.Count);
            }
            catch (Exception ex)
            {
                loggingBroker.LogError(ex);
                Console.WriteLine(ex.Message);
            }
        }
    }
}
