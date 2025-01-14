// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
                await ValidateTerminologyPollOnAddAsync(terminologyPoll);

                return await this.storageBroker.InsertTerminologyPollAsync(terminologyPoll);
            });

        public ValueTask<IQueryable<TerminologyPoll>> RetrieveAllTerminologyPollsAsync() =>
            TryCatch(async () => await this.storageBroker.SelectAllTerminologyPollsAsync());

        public ValueTask<TerminologyPoll> RetrieveTerminologyPollByIdAsync(Guid terminologyPollId) =>
            TryCatch(async () =>
            {
                ValidateTerminologyPollId(terminologyPollId);

                TerminologyPoll maybeTerminologyPoll = await this.storageBroker
                    .SelectTerminologyPollByIdAsync(terminologyPollId);

                ValidateStorageTerminologyPoll(maybeTerminologyPoll, terminologyPollId);

                return maybeTerminologyPoll;
            });

        public ValueTask<TerminologyPoll> ModifyTerminologyPollAsync(TerminologyPoll terminologyPoll) =>
            TryCatch(async () =>
            {
                await ValidateTerminologyPollOnModifyAsync(terminologyPoll);

                TerminologyPoll maybeTerminologyPoll =
                    await this.storageBroker.SelectTerminologyPollByIdAsync(terminologyPoll.Id);

                ValidateStorageTerminologyPoll(maybeTerminologyPoll, terminologyPoll.Id);

                ValidateAgainstStorageTerminologyPollOnModify(
                    inputTerminologyPoll: terminologyPoll,
                    storageTerminologyPoll: maybeTerminologyPoll);

                return await this.storageBroker.UpdateTerminologyPollAsync(terminologyPoll);
            });

        public ValueTask<TerminologyPoll> RemoveTerminologyPollByIdAsync(Guid terminologyPollId) =>
            TryCatch(async () =>
            {
                ValidateTerminologyPollId(terminologyPollId);

                TerminologyPoll maybeTerminologyPoll = await this.storageBroker
                    .SelectTerminologyPollByIdAsync(terminologyPollId);

                ValidateStorageTerminologyPoll(maybeTerminologyPoll, terminologyPollId);

                return await this.storageBroker.DeleteTerminologyPollAsync(maybeTerminologyPoll);
            });
    }
}