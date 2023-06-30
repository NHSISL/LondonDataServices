// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.Downloads.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis
{
    public partial class DecryptionsApiTests
    {
        [Fact]
        public async Task ShouldDecryptFileAsync()
        {
            // given
            string randomFileName = GetRandomGuidString();
            string inputFileName = randomFileName;
            string expectedFileName = inputFileName;

            // when
            await this.apiBroker.DecryptFileAsync(inputFileName);

            // Check that the file is decrypted successfully. This might be a method in your service 
            // that confirms the file has been decrypted, depending on your specific implementation.
            bool isDecrypted = await this.apiBroker.IsFileDecryptedAsync(inputFileName);

            // then
            isDecrypted.Should().BeTrue();

            // Clean up any necessary resources here.
        }

        [Fact]
        public async Task ShouldNotDecryptNonExistentFileAsync()
        {
            // given
            string nonExistentFileName = GetRandomGuidString();

            // when
            Func<Task> decryptFunc = async () => { await this.apiBroker.DecryptFileAsync(nonExistentFileName); };

            // then
            await decryptFunc.Should().ThrowAsync<DownloadOrchestrationValidationException>();
        }

        [Fact]
        public async Task ShouldThrowOnDecryptionServiceExceptionAsync()
        {
            // given
            string fileNameCausingServiceException = GetRandomGuidString();

            // when
            Func<Task> decryptFunc = async () => { await this.apiBroker.DecryptFileAsync(fileNameCausingServiceException); };

            // then
            await decryptFunc.Should().ThrowAsync<DownloadOrchestrationServiceException>();
        }

        [Fact]
        public async Task ShouldThrowOnDecryptionDependencyExceptionAsync()
        {
            // given
            string fileNameCausingDependencyException = GetRandomGuidString();

            // when
            Func<Task> decryptFunc = async () => { await this.apiBroker.DecryptFileAsync(fileNameCausingDependencyException); };

            // then
            await decryptFunc.Should().ThrowAsync<DownloadOrchestrationDependencyException>();
        }

       
    }

}
