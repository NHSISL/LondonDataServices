// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string cryptographyRelativeUrl = "api/cryptography";

        public async ValueTask<byte[]> PostEncryptDataAsync(Guid subscriberAgreemnetId, byte[] data) =>
            await this.apiFactoryClient
                .PostContentAsync($"{cryptographyRelativeUrl}/encrypt/{subscriberAgreemnetId}", data);

        public async ValueTask<byte[]> PostDecryptDataAsync(Guid subscriberAgreemnetId, byte[] data) =>
            await this.apiFactoryClient
                .PostContentAsync($"{cryptographyRelativeUrl}/decrypt/{subscriberAgreemnetId}", data);
    }
}
