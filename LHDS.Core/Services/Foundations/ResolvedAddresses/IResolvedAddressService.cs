using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;

namespace LHDS.Core.Services.Foundations.ResolvedAddresses
{
    public interface IResolvedAddressService
    {
        ValueTask<ResolvedAddress> AddResolvedAddressAsync(ResolvedAddress resolvedAddress);
    }
}