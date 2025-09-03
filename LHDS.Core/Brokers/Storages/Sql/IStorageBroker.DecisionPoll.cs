// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DecisionPolls;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<DecisionPoll> InsertDecisionPollAsync(DecisionPoll decisionPoll);
        ValueTask<IQueryable<DecisionPoll>> SelectAllDecisionPollsAsync();
        ValueTask<DecisionPoll> SelectDecisionPollByIdAsync(Guid decisionPollId);
        ValueTask<DecisionPoll> UpdateDecisionPollAsync(DecisionPoll decisionPoll);
        ValueTask<DecisionPoll> DeleteDecisionPollAsync(DecisionPoll decisionPoll);
    }
}
