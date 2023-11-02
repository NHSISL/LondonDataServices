using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.TerminologyPolls;

namespace LHDS.Core.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollService : ITerminologyPollService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public TerminologyPollService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<TerminologyPoll> AddTerminologyPollAsync(TerminologyPoll terminologyPoll) =>
            TryCatch(async () =>
            {
                ValidateTerminologyPollOnAdd(terminologyPoll);

                return await this.storageBroker.InsertTerminologyPollAsync(terminologyPoll);
            });

        public IQueryable<TerminologyPoll> RetrieveAllTerminologyPolls() =>
            TryCatch(() => this.storageBroker.SelectAllTerminologyPolls());

        public async ValueTask<TerminologyPoll> RetrieveTerminologyPollByIdAsync(Guid terminologyPollId) =>
            await this.storageBroker.SelectTerminologyPollByIdAsync(terminologyPollId);
    }
}