// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string decryptionsRelativeUrl = "api/decryptions";

        public async ValueTask GetDocumentByFileNameToDecryptAsync(string fileName) =>
            await this.apiFactoryClient.GetContentStringAsync($"{decryptionsRelativeUrl}/{fileName}");
    }
}
