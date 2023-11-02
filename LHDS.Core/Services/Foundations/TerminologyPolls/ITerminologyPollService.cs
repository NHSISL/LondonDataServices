using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyPolls;

namespace LHDS.Core.Services.Foundations.TerminologyPolls
{
    public interface ITerminologyPollService
    {
        ValueTask<TerminologyPoll> AddTerminologyPollAsync(TerminologyPoll terminologyPoll);
    }
}