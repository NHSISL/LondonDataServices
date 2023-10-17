using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAddressException =
                    new AlreadyExistsAddressException(
                        message: "Address with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsAddressException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidAddressReferenceException =
                    new InvalidAddressReferenceException(
                        message: "Invalid address reference error occurred.", 
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidAddressReferenceException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedAddressStorageException =
                    new FailedAddressStorageException(
                    message: "Failed address storage error occurred, contact support.",
                    innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedAddressStorageException);
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

        private AddressDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var addressDependencyValidationException =
                new AddressDependencyValidationException(
                    message: "Address dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressDependencyValidationException);

            return addressDependencyValidationException;
        }

        private AddressDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var addressDependencyException = 
                new AddressDependencyException(
                    message: "Address dependency error occurred, contact support.",
                    innerException: exception); 

            this.loggingBroker.LogError(addressDependencyException);

            return addressDependencyException;
        }
    }
}