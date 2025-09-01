// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DecisionPolls;

namespace LHDS.Core.Services.Foundations.DecisionPolls
{
    public interface IDecisionPollService
    {
        ValueTask<DecisionPoll> AddDecisionPollAsync(DecisionPoll decisionPoll);
        ValueTask<IQueryable<DecisionPoll>> RetrieveAllDecisionPollsAsync();
        ValueTask<DecisionPoll> ModifyDecisionPollAsync(DecisionPoll decisionPoll);
    }
}
