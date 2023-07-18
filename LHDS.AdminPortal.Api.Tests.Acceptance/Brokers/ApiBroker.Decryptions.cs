// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string DecryptionsRelativeUrl = "api/decryptions";

        public async ValueTask DecryptFileAsync(string fileName) =>
            await this.apiFactoryClient.GetContentStringAsync($"{DecryptionsRelativeUrl}/{fileName}");
    }
}
