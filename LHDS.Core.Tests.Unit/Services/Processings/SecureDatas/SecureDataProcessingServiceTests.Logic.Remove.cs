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
        public async Task ShouldRemoveSecretsAsync()
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

            SubscriberCredential expectedSubscriberCredential = inputSubscriberCredential.DeepClone();


            // when
            SubscriberCredential actualSubscriberCredential =
                await this.secureDataProcessingService.RemoveSecureDataAsync(inputSubscriberCredential);

            // then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);

            foreach (string keyType in keyTypes)
            {
                string secretName = $"{inputSubscriberCredential.Id}-{keyType}";

                this.secureDataServiceMock.Verify(service =>
                    service.RemoveSecureDataAsync(secretName),
                        Times.Once);
            }

            this.secureDataServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}