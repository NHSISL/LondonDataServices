// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.TppLandings.Exceptions;
using LHDS.Core.Models.Orchestrations.Ingres.Exceptions;
using LHDS.Core.Models.Orchestrations.TppLandings.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Coordinations.TppLandings
{
    public partial class TppLandingCoordinationService
    {
        private delegate ValueTask<Guid> ReturningGuidFunction();

        private async ValueTask<Guid> TryCatch(ReturningGuidFunction returningGuidFunction)
        {
            try
            {
                return await returningGuidFunction();
            }
            catch (InvalidArgumentTppLandingCoordinationException invalidArgumentTppLandingCoordinationException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentTppLandingCoordinationException);
            }
            catch (TppLandingOrchestrationValidationException
                tppLandingOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(tppLandingOrchestrationValidationException);
            }
            catch (TppLandingOrchestrationDependencyValidationException
                tppLandingOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    tppLandingOrchestrationDependencyValidationException);
            }
            catch (IngressOrchestrationValidationException
                ingresOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ingresOrchestrationValidationException);
            }
            catch (IngressOrchestrationDependencyValidationException
                ingresOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ingresOrchestrationDependencyValidationException);
            }
            catch (TppLandingOrchestrationDependencyException
                tppLandingOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(tppLandingOrchestrationDependencyException);
            }
            catch (TppLandingOrchestrationServiceException
                tppLandingOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(tppLandingOrchestrationServiceException);
            }
            catch (IngressOrchestrationDependencyException
                ingresOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingresOrchestrationDependencyException);
            }
            catch (IngressOrchestrationServiceException
                ingresOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingresOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedTppLandingCoordinationServiceException =
                    new FailedTppLandingCoordinationServiceException(
                        message: "Failed TPP landing coordination service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedTppLandingCoordinationServiceException);
            }
        }

        private async ValueTask<TppLandingCoordinationValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var tppLandingCoordinationValidationException =
                new TppLandingCoordinationValidationException(
                    message: "TPP landing coordination validation errors occured, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(tppLandingCoordinationValidationException);

            return tppLandingCoordinationValidationException;
        }

        private async ValueTask<TppLandingCoordinationDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var tppLandingCoordinationDependencyValidationException =
                new TppLandingCoordinationDependencyValidationException(
                    message: "TPP landing coordination dependency validation error occurred, " +
                        "fix the errors and try again.",
                    exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(tppLandingCoordinationDependencyValidationException);

            return tppLandingCoordinationDependencyValidationException;
        }

        private async ValueTask<TppLandingCoordinationDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var tppLandingCoordinationDependencyException =
                new TppLandingCoordinationDependencyException(
                    message: "TPP landing coordination dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(tppLandingCoordinationDependencyException);

            return tppLandingCoordinationDependencyException;
        }

        private async ValueTask<TppLandingCoordinationServiceException>
           CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var
                tppLandingCoordinationServiceException =
                new TppLandingCoordinationServiceException(
                    message: "TPP landing coordination service error occurred, please contact support.",
                    exception);

            await this.loggingBroker.LogErrorAsync(tppLandingCoordinationServiceException);

            return tppLandingCoordinationServiceException;
        }

    }
}
