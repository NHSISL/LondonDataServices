// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string cryptographyRelativeUrl = "api/cryptography";

        public async ValueTask<byte[]> PostEncryptDataAsync(byte[] data)
        {
            try
            {
                var x = await this.apiFactoryClient.PostContentAsync($"{cryptographyRelativeUrl}/encrypt", data);
                return x;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async ValueTask<byte[]> PostDecryptDataAsync(byte[] data) =>
            await this.apiFactoryClient.PostContentAsync($"{cryptographyRelativeUrl}/decrypt", data);
    }
}
