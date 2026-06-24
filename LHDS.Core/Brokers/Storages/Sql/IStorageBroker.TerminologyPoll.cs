// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyPolls;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<TerminologyPoll> InsertTerminologyPollAsync(
            TerminologyPoll terminologyPoll,
            CancellationToken cancellationToken = default);

        ValueTask<IQueryable<TerminologyPoll>> SelectAllTerminologyPollsAsync(
            CancellationToken cancellationToken = default);

        ValueTask<TerminologyPoll> SelectTerminologyPollByIdAsync(
            Guid terminologyPollId,
            CancellationToken cancellationToken = default);

        ValueTask<TerminologyPoll> UpdateTerminologyPollAsync(
            TerminologyPoll terminologyPoll,
            CancellationToken cancellationToken = default);

        ValueTask<TerminologyPoll> DeleteTerminologyPollAsync(
            TerminologyPoll terminologyPoll,
            CancellationToken cancellationToken = default);
    }
}
