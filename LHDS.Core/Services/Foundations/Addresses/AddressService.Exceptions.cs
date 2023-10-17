using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Addresses
{
    public partial class AddressService
    {
        private delegate ValueTask<Address> ReturningAddressFunction();

        private async ValueTask<Address> TryCatch(ReturningAddressFunction returningAddressFunction)
        {
            try
            {
                return await returningAddressFunction();
            }
            catch (NullAddressException nullAddressException)
            {
                throw CreateAndLogValidationException(nullAddressException);
            }
        }

        private AddressValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressValidationException);

            return addressValidationException;
        }
    }
}