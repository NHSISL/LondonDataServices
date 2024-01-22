// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackings;

namespace LHDS.Core.Tests.Integration.Decryptions
{
    public partial class DecryptionTests
    {
        [ReleaseCandidateFact]
        public async Task ShouldDecryptAsync()
        {
            // given
            string encryptedFileContainer = "emislanding";

            var items = ingestionTrackingService.RetrieveAllIngestionTrackings()
                .Where(ingestionTrackingService => ingestionTrackingService.Decrypted == false);

            // when
            foreach (IngestionTracking item in items)
            {
                string filename = await decryptionClient.DecryptAsync(item.FileName);

                // then
                filename.Should().NotBeNullOrWhiteSpace();
                item.DecryptedFileName.Should().BeEquivalentTo(filename);

                Document document = await this.documentService
                    .RetrieveDocumentByFileNameAsync(filename, encryptedFileContainer);

                document.Should().NotBeNull();
                document.FileName.Should().BeEquivalentTo(filename);
            }
        }
    }
}
