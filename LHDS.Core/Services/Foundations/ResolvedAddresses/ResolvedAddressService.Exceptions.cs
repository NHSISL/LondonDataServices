using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressService
    {
        private delegate ValueTask<ResolvedAddress> ReturningResolvedAddressFunction();

        private async ValueTask<ResolvedAddress> TryCatch(ReturningResolvedAddressFunction returningResolvedAddressFunction)
        {
            try
            {
                return await returningResolvedAddressFunction();
            }
            catch (NullResolvedAddressException nullResolvedAddressException)
            {
                throw CreateAndLogValidationException(nullResolvedAddressException);
            }
            catch (InvalidResolvedAddressException invalidResolvedAddressException)
            {
                throw CreateAndLogValidationException(invalidResolvedAddressException);
            }
        }

        private ResolvedAddressValidationException CreateAndLogValidationException(Xeption exception)
        {
            var resolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "ResolvedAddress validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(resolvedAddressValidationException);

            return resolvedAddressValidationException;
        }
    }
}