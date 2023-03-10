using System.Threading.Tasks;
using LHDS.Core.Models.OptOuts;

namespace LHDS.Core.Services.Foundations.OptOuts
{
    public interface IOptOutService
    {
        ValueTask<OptOut> AddOptOutAsync(OptOut optOut);
    }
}