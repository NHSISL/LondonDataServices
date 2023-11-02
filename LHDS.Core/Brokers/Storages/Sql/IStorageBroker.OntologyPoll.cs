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
        ValueTask<OntologyPoll> InsertOntologyPollAsync(
            OntologyPoll ontologyPoll);

        IQueryable<OntologyPoll> SelectAllOntologyPolls();
        ValueTask<OntologyPoll> SelectOntologyPollByIdAsync(Guid ontologyPollId);

        ValueTask<OntologyPoll> UpdateOntologyPollAsync(
            OntologyPoll ontologyPoll);

        ValueTask<OntologyPoll> DeleteOntologyPollAsync(
            OntologyPoll ontologyPoll);
    }
}
