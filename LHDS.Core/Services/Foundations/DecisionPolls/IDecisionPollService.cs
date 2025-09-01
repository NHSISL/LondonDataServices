// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DecisionPolls;

namespace LHDS.Core.Services.Foundations.DecisionPolls
{
    public interface IDecisionPollService
    {
        ValueTask<DecisionPoll> AddDecisionPollAsync(DecisionPoll decisionPoll);
        ValueTask<DecisionPoll> ModifyDecisionPollAsync(DecisionPoll decisionPoll);
    }
}
