// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.TerminologyArtifacts;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string TerminologyArtifactsRelativeUrl = "api/terminologyArtifacts";

        public async ValueTask<TerminologyArtifact> PostTerminologyArtifactAsync(TerminologyArtifact terminologyArtifact) =>
            await this.apiFactoryClient.PostContentAsync(TerminologyArtifactsRelativeUrl, terminologyArtifact);

        public async ValueTask<TerminologyArtifact> GetTerminologyArtifactByIdAsync(Guid terminologyArtifactId) =>
            await this.apiFactoryClient.GetContentAsync<TerminologyArtifact>($"{TerminologyArtifactsRelativeUrl}/{terminologyArtifactId}");

        public async ValueTask<List<TerminologyArtifact>> GetAllTerminologyArtifactsAsync() =>
          await this.apiFactoryClient.GetContentAsync<List<TerminologyArtifact>>($"{TerminologyArtifactsRelativeUrl}/");

        public async ValueTask<TerminologyArtifact> PutTerminologyArtifactAsync(TerminologyArtifact terminologyArtifact) =>
            await this.apiFactoryClient.PutContentAsync(TerminologyArtifactsRelativeUrl, terminologyArtifact);

        public async ValueTask<TerminologyArtifact> DeleteTerminologyArtifactByIdAsync(Guid terminologyArtifactId) =>
            await this.apiFactoryClient.DeleteContentAsync<TerminologyArtifact>($"{TerminologyArtifactsRelativeUrl}/{terminologyArtifactId}");
    }
}
