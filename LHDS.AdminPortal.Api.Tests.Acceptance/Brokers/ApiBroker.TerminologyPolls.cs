// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.TerminologyPolls;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string TerminologyPollsRelativeUrl = "api/terminologyPolls";

        public async ValueTask<TerminologyPoll> PostTerminologyPollAsync(TerminologyPoll terminologyPoll) =>
            await this.apiFactoryClient.PostContentAsync(TerminologyPollsRelativeUrl, terminologyPoll);

        public async ValueTask<TerminologyPoll> GetTerminologyPollByIdAsync(Guid terminologyPollId) =>
            await this.apiFactoryClient.GetContentAsync<TerminologyPoll>($"{TerminologyPollsRelativeUrl}/{terminologyPollId}");

        public async ValueTask<List<TerminologyPoll>> GetAllTerminologyPollsAsync() =>
          await this.apiFactoryClient.GetContentAsync<List<TerminologyPoll>>($"{TerminologyPollsRelativeUrl}/");

        public async ValueTask<TerminologyPoll> PutTerminologyPollAsync(TerminologyPoll terminologyPoll) =>
            await this.apiFactoryClient.PutContentAsync(TerminologyPollsRelativeUrl, terminologyPoll);

        public async ValueTask<TerminologyPoll> DeleteTerminologyPollByIdAsync(Guid terminologyPollId) =>
            await this.apiFactoryClient.DeleteContentAsync<TerminologyPoll>($"{TerminologyPollsRelativeUrl}/{terminologyPollId}");
    }
}
