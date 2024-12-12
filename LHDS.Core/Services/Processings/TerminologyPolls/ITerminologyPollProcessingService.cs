// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyPolls;

namespace LHDS.Core.Services.Processings.TerminologyPolls
{
    public interface ITerminologyPollProcessingService
    {
        ValueTask<TerminologyPoll> AddTerminologyPollAsync(TerminologyPoll terminologyPoll);
        IQueryable<TerminologyPoll> RetrieveAllTerminologyPolls();
        ValueTask<TerminologyPoll> RetrieveTerminologyPollByIdAsync(Guid terminologyPollId);
        ValueTask<TerminologyPoll> ModifyTerminologyPollAsync(TerminologyPoll terminologyPoll);
        ValueTask<TerminologyPoll> RemoveTerminologyPollByIdAsync(Guid terminologyPollId);
        ValueTask<TerminologyPoll> RetrieveOrAddTerminologyPollAsync(string resourceType);
    }
}