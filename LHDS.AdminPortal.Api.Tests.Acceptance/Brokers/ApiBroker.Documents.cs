// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Audits;
using LHDS.Core.Models.Foundations.Documents;
using Microsoft.AspNetCore.Mvc;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string DocumentsRelativeUrl = "api/documents";
        public async ValueTask<ActionResult<Document>> GetDownloadLinkAsync(string fileName) =>
            await this.apiFactoryClient.GetContentAsync<ActionResult<Document>>($"{DocumentsRelativeUrl}/{fileName}");
    }

}
