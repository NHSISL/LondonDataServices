// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldAddOrModifySecureDataAsync()
        {
            // given
            dynamic randomCredential = CreateRandomDynamicSharingAgreementCredential();

            List<string> keyTypes = new List<string>
            {
                "FtpPassword",
                "FtpPassPhrase",
                "FtpPrivateKey",
                "GpgPassPhrase",
                "GpgPrivateKey",
            };

            SubscriberCredential inputSubscriberCredential =
                CreateSubscriberCredentialFromDynamic(credential: randomCredential);

            SubscriberCredential expectedSubscriberCredential = inputSubscriberCredential.DeepClone();

            foreach (string keyType in keyTypes)
            {
                SecureData inputSecureData =
                    CreateSecretDataFromDynamic(credential: randomCredential, property: keyType);

                SecureData outputSecureData = inputSecureData.DeepClone();

                this.secureDataServiceMock.Setup(service =>
                    service.AddOrModifySecureData(It.Is(SameSecureDataAs(inputSecureData))))
                        .ReturnsAsync(outputSecureData);
            }

            // when
            SubscriberCredential actualSubscriberCredential =
                await this.secureDataProcessingService.AddOrModifySecureDataAsync(inputSubscriberCredential);

            // then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);

            foreach (string keyType in keyTypes)
            {
                SecureData inputSecureData =
                    CreateSecretDataFromDynamic(credential: randomCredential, property: keyType);

                this.secureDataServiceMock.Verify(service =>
                    service.AddOrModifySecureData(It.Is(SameSecureDataAs(inputSecureData))),
                        Times.Once);
            }

            this.secureDataServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}