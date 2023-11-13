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
    }
}