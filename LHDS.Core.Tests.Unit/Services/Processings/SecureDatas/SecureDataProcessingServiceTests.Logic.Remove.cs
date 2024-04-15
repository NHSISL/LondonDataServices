// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            Guid randomId = Guid.NewGuid();

            List<string> keyTypes = new List<string>
            {
                "FtpPassword",
                "FtpPassPhrase",
                "FtpPrivateKey",
                "GpgPassPhrase",
                "GpgPrivateKey"
            };

            // when
            await this.secureDataProcessingService.RemoveSecureDataByIdAsync(randomId);

            // then

            foreach (string keyType in keyTypes)
            {
                string secretName = $"{randomId}-{keyType}";

                this.secureDataServiceMock.Verify(service =>
                    service.RemoveSecureDataAsync(secretName),
                        Times.Once);
            }

            this.secureDataServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}