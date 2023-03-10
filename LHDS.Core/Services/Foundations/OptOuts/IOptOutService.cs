using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.OptOuts;

namespace LHDS.Core.Services.Foundations.OptOuts
{
    public interface IOptOutService
    {
        ValueTask<OptOut> AddOptOutAsync(OptOut optOut);
        IQueryable<OptOut> RetrieveAllOptOuts();
        ValueTask<OptOut> RetrieveOptOutByIdAsync(Guid optOutId);
        ValueTask<OptOut> ModifyOptOutAsync(OptOut optOut);
        ValueTask<OptOut> RemoveOptOutByIdAsync(Guid optOutId);
    }
}