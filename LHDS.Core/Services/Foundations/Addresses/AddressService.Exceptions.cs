using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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
            catch (InvalidAddressException invalidAddressException)
            {
                throw CreateAndLogValidationException(invalidAddressException);
            }
            catch (SqlException sqlException)
            {
                var failedAddressStorageException =
                    new FailedAddressStorageException(
                        message: "Failed address storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedAddressStorageException);
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

        private AddressDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var addressDependencyException = 
                new AddressDependencyException(
                    message: "Address dependency error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogCritical(addressDependencyException);

            return addressDependencyException;
        }
    }
}