// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Xunit;

namespace LHDS.Core.Tests.Integration.Decryptions
{
    public partial class DecryptionTests
    {
        [Fact]
        public async Task ShouldDecryptAsync()
        {
            // given
            string decryptedFileContainer = "versioner";

            var items = ingestionTrackingService.RetrieveAllIngestionTrackings()
                .Where(ingestionTrackingService => ingestionTrackingService.Decrypted == false);

            // when
            foreach (IngestionTracking item in items)
            {
                string fileName = await decryptionClient.DecryptAsync(item.EncryptedFileName);

                // then
                fileName.Should().NotBeNullOrWhiteSpace();
                item.DecryptedFileName.Should().BeEquivalentTo(fileName);

                Document document = await this.documentService
                    .RetrieveDocumentByFileNameAsync(fileName, decryptedFileContainer);

                document.Should().NotBeNull();
                document.FileName.Should().BeEquivalentTo(fileName);
            }
        }
    }
}
