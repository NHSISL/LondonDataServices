using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;

namespace LHDS.Core.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressService
    {
        private void ValidateResolvedAddressOnAdd(ResolvedAddress resolvedAddress)
        {
            ValidateResolvedAddressIsNotNull(resolvedAddress);
        }

        private static void ValidateResolvedAddressIsNotNull(ResolvedAddress resolvedAddress)
        {
            if (resolvedAddress is null)
            {
                throw new NullResolvedAddressException(message: "ResolvedAddress is null.");
            }
        }
    }
}