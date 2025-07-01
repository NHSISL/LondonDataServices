// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.Suppliers;
using Xunit;

namespace LHDS.Core.Tests.Integration.TppLandings
{
    public partial class TppLandingTests
    {
        [Fact]
        public async Task ShouldLandTPPFileAsync()
        {
            // given
            string filePath = @"Resources\TppLandingTests\ShouldLandTPPFileAsync.txt";
            byte[] fileBytes = File.ReadAllBytes(filePath);
            FileInfo fi = new FileInfo(filePath);
            Stream fileStream = new MemoryStream(fileBytes);
            var fileNameWithoutExtension = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
            string sha256Hash = CalculateSHA256Hash(fileBytes);

            string encryptedFileContainer = blobContainers.TppLanding;
            Supplier supplier = await GetTppSupplier();

            // when
            Guid actualGuid = await this.tppLandingClient.ProcessAsync(
                fileName: fileNameWithoutExtension,
                supplierId: supplier.Id);

            // then
            actualGuid.Should().NotBe(Guid.Empty);

            IngestionTracking actualIngestionTracking =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(actualGuid);

            IQueryable<IngestionTrackingAudit> allAudits =
                await this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAuditsAsync();

            var audits = allAudits.Where(audit => audit.IngestionTrackingId == actualGuid).ToList();

            foreach (var audit in audits)
            {
                await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(audit.Id);
            }

            await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(actualGuid);
        }
    }
}
