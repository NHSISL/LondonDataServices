// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;

namespace LHDS.Core.Tests.Integration.Decryptions
{
    public partial class DecryptionTests
    {
        [ReleaseCandidateFact]
        public async Task ShouldDecryptAsync()
        {
            // given
            string decryptedFileContainer = "ingress";

            var items = ingestionTrackingService.RetrieveAllIngestionTrackings()
                .Where(ingestionTrackingService => ingestionTrackingService.Decrypted == false);

            // when
            foreach (IngestionTracking item in items)
            {
                string fileName = await decryptionClient.DecryptAsync(item.EncryptedFileName);

                // then
                fileName.Should().NotBeNullOrWhiteSpace();
                item.DecryptedFileName.Should().BeEquivalentTo(fileName);
                Stream document = new MemoryStream();

                await this.documentService
                    .RetrieveDocumentByFileNameAsync(document, fileName, decryptedFileContainer);

                document.Should().NotBeNull();
                document.Length.Should().BeGreaterThan(0);
            }
        }
    }
}
