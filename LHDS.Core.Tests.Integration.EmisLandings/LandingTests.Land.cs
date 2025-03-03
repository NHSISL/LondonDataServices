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
                Supplier supplier = await GetEmisSupplierAsync();

                // when
                List<string> files = await landingClient.ProcessAsync(supplierId: supplier.Id);

                // then
                files.Should().NotBeNull();
            }
            catch (Exception ex)
            {
                await loggingBroker.LogErrorAsync(ex);
                Console.WriteLine(ex.Message);
                Assert.Fail($"{ex.Message}, {ex?.InnerException?.Message}");
            }
        }
    }
}