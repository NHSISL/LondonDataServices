// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.SecureData;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.SecureDatas
{
    public partial class SecureDataProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveSecretsByKeyVaultKeyNameAsync()
        {
            // given
            dynamic randomCredential = CreateRandomDynamicSharingAgreementCredential();

            List<string> keyTypes = new List<string>
            {
                "FtpPassword",
                "FtpPassPhrase",
                "FtpPrivateKey",
                "GpgPassPhrase",
                "GpgPrivateKey"
            };

            SubscriberCredential inputSubscriberCredential =
                CreateSubscriberCredentialFromDynamic(credential: randomCredential);

            inputSubscriberCredential.FtpPassword = string.Empty;
            inputSubscriberCredential.FtpPassPhrase = string.Empty;
            inputSubscriberCredential.GpgPassPhrase = string.Empty; 
            inputSubscriberCredential.FtpPrivateKey = string.Empty;
            inputSubscriberCredential.GpgPrivateKey = string.Empty;

            SubscriberCredential expectedSubscriberCredential = inputSubscriberCredential.DeepClone();

            foreach (string keyType in keyTypes)
            {
                string secretName = $"{inputSubscriberCredential.Id}-{keyType}";

                SecureData inputSecureData =
                    CreateSecretDataFromDynamic(credential: randomCredential, property: keyType);

                SecureData outputSecureData = inputSecureData.DeepClone();

                this.secureDataServiceMock.Setup(service =>
                    service.RetrieveSecretDataByNameAsync(secretName))
                        .ReturnsAsync(outputSecureData);
            }

            // when
            SubscriberCredential actualSubscriberCredential =
                await this.secureDataProcessingService.RetrieveSecretsByKeyVaultKeyNameAsync(inputSubscriberCredential);

            // then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);

            foreach (string keyType in keyTypes)
            {
                string secretName = $"{inputSubscriberCredential.Id}-{keyType}";

                SecureData inputSecureData =
                    CreateSecretDataFromDynamic(credential: randomCredential, property: keyType);

                this.secureDataServiceMock.Verify(service =>
                    service.RetrieveSecretDataByNameAsync(secretName),
                        Times.Once);
            }

            this.secureDataServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}