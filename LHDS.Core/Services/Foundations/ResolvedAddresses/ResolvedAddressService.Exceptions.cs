using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
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
            catch (SqlException sqlException)
            {
                var failedResolvedAddressStorageException =
                    new FailedResolvedAddressStorageException(
                        message: "Failed resolvedAddress storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedResolvedAddressStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsResolvedAddressException =
                    new AlreadyExistsResolvedAddressException(
                        message: "ResolvedAddress with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsResolvedAddressException);
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

        private ResolvedAddressDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var resolvedAddressDependencyException = 
                new ResolvedAddressDependencyException(
                    message: "ResolvedAddress dependency error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogCritical(resolvedAddressDependencyException);

            return resolvedAddressDependencyException;
        }

        private ResolvedAddressDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var resolvedAddressDependencyValidationException =
                new ResolvedAddressDependencyValidationException(
                    message: "ResolvedAddress dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(resolvedAddressDependencyValidationException);

            return resolvedAddressDependencyValidationException;
        }
    }
}