// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Suppliers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LHDS.Core.Tests.Integration.EmisLandings
{
    public partial class LandingTests
    {
        [ReleaseCandidateFact]
        public async Task ShouldLandFilesAsync()
        {
            try
            {
                // given
                string encryptedFileContainer = blobContainers.EmisLanding;
                Supplier supplier = await GetEmisSupplier();

                // when
                List<string> files = await landingClient.ProcessAsync(supplierId: supplier.Id);

                // then
                files.Should().NotBeNull();

                //foreach (var file in files)
                //{
                //    IngestionTracking ingestionTracking =
                //        ingestionTrackingService.RetrieveAllIngestionTrackings()
                //            .FirstOrDefault(item => item.DecryptedFileName == file);

                //    ingestionTracking.Should().NotBeNull();

                //    List<IngestionTrackingAudit> audits = auditService.RetrieveAllIngestionTrackingAudits()
                //        .Where(item => item.IngestionTrackingId == ingestionTracking.Id).ToList();

                //    foreach (IngestionTrackingAudit item in audits)
                //    {
                //        await auditService.RemoveIngestionTrackingAuditByIdAsync(item.Id);
                //    }

                //    await ingestionTrackingService
                //        .RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);

                //    await blobStorageBroker.DeleteFileAsync(
                //        fileName: ingestionTracking.EncryptedFileName, container: encryptedFileContainer);
                // }
            }
            catch (Exception ex)
            {
                loggingBroker.LogError(ex);
                Console.WriteLine(ex.Message);
                Assert.Fail($"{ex.Message}, {ex?.InnerException?.Message}");
            }
        }
    }
}