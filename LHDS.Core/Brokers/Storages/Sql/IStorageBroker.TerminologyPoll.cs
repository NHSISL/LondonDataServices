// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyPolls;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<TerminologyPoll> InsertTerminologyPollAsync(
            TerminologyPoll terminologyPoll);

        IQueryable<TerminologyPoll> SelectAllTerminologyPollsAsync();
        ValueTask<TerminologyPoll> SelectTerminologyPollByIdAsync(Guid terminologyPollId);

        ValueTask<TerminologyPoll> UpdateTerminologyPollAsync(
            TerminologyPoll terminologyPoll);

        ValueTask<TerminologyPoll> DeleteTerminologyPollAsync(
            TerminologyPoll terminologyPoll);
    }
}
