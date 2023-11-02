// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OntologyPolls;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<TerminologyPoll> InsertOntologyPollAsync(
            TerminologyPoll ontologyPoll);

        IQueryable<TerminologyPoll> SelectAllOntologyPolls();
        ValueTask<TerminologyPoll> SelectOntologyPollByIdAsync(Guid ontologyPollId);

        ValueTask<TerminologyPoll> UpdateOntologyPollAsync(
            TerminologyPoll ontologyPoll);

        ValueTask<TerminologyPoll> DeleteOntologyPollAsync(
            TerminologyPoll ontologyPoll);
    }
}
