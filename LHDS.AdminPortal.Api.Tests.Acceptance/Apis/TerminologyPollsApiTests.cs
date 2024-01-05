using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.TerminologyPolls;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.TerminologyPolls
{
    [Collection(nameof(ApiTestCollection))]
    public partial class TerminologyPollsApiTests
    {
        private readonly ApiBroker apiBroker;

        public TerminologyPollsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private async ValueTask<TerminologyPoll> PostRandomTerminologyPollAsync()
        {
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll();
            await this.apiBroker.PostTerminologyPollAsync(randomTerminologyPoll);

            return randomTerminologyPoll;
        }

        private async ValueTask<List<TerminologyPoll>> PostRandomTerminologyPollsAsync()
        {
            int randomNumber = GetRandomNumber();
            var randomTerminologyPolls = new List<TerminologyPoll>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomTerminologyPolls.Add(await PostRandomTerminologyPollAsync());
            }

            return randomTerminologyPolls;
        }

        private static TerminologyPoll CreateRandomTerminologyPoll() =>
            CreateRandomTerminologyPollFiller().Create();

        private static Filler<TerminologyPoll> CreateRandomTerminologyPollFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<TerminologyPoll>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(terminologyPoll => terminologyPoll.CreatedDate).Use(now)
                .OnProperty(terminologyPoll => terminologyPoll.CreatedBy).Use(user)
                .OnProperty(terminologyPoll => terminologyPoll.UpdatedDate).Use(now)
                .OnProperty(terminologyPoll => terminologyPoll.UpdatedBy).Use(user);

            return filler;
        }
    }
}