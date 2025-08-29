// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DecisionPolls;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<DecisionPoll> DecisionPolls { get; set; }

        public async ValueTask<DecisionPoll> InsertDecisionPollAsync(DecisionPoll decisionPoll) =>
            await InsertAsync(decisionPoll);

        public async ValueTask<IQueryable<DecisionPoll>> SelectAllDecisionPollsAsync() =>
            await SelectAllAsync<DecisionPoll>();

        public async ValueTask<DecisionPoll> SelectDecisionPollByIdAsync(Guid decisionPollId) =>
            await SelectAsync<DecisionPoll>(decisionPollId);

        public async ValueTask<DecisionPoll> UpdateDecisionPollAsync(DecisionPoll decisionPoll) =>
            await UpdateAsync(decisionPoll);

        public async ValueTask<DecisionPoll> DeleteDecisionPollAsync(DecisionPoll decisionPoll) =>
            await DeleteAsync(decisionPoll);
    }
}
