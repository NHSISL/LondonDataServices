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
                throw CreateAndLogValidationException(invalidArgumentTppLandingCoordinationException);
            }
            catch (TppLandingOrchestrationValidationException
                tppLandingOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(tppLandingOrchestrationValidationException);
            }
            catch (TppLandingOrchestrationDependencyValidationException
                tppLandingOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    tppLandingOrchestrationDependencyValidationException);
            }
            catch (IngresOrchestrationValidationException
                ingresOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingresOrchestrationValidationException);
            }
            catch (IngresOrchestrationDependencyValidationException
                ingresOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingresOrchestrationDependencyValidationException);
            }
            catch (TppLandingOrchestrationDependencyException
                tppLandingOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(tppLandingOrchestrationDependencyException);
            }
            catch (TppLandingOrchestrationServiceException
                tppLandingOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(tppLandingOrchestrationServiceException);
            }
            catch (IngresOrchestrationDependencyException
                ingresOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(ingresOrchestrationDependencyException);
            }
            catch (IngresOrchestrationServiceException
                ingresOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(ingresOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedTppLandingCoordinationServiceException =
                    new FailedTppLandingCoordinationServiceException(
                        message: "Failed TPP landing coordination service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedTppLandingCoordinationServiceException);
            }
        }

        private TppLandingCoordinationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var tppLandingCoordinationValidationException =
                new TppLandingCoordinationValidationException(
                    message: "TPP landing coordination validation errors occured, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(tppLandingCoordinationValidationException);

            return tppLandingCoordinationValidationException;
        }

        private TppLandingCoordinationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var tppLandingCoordinationDependencyValidationException =
                new TppLandingCoordinationDependencyValidationException(
                    message: "TPP landing coordination dependency validation error occurred, fix the errors and try again.",
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(tppLandingCoordinationDependencyValidationException);

            return tppLandingCoordinationDependencyValidationException;
        }

        private TppLandingCoordinationDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var tppLandingCoordinationDependencyException =
                new TppLandingCoordinationDependencyException(
                    message: "TPP landing coordination dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(tppLandingCoordinationDependencyException);

            throw tppLandingCoordinationDependencyException;
        }

        private TppLandingCoordinationServiceException
           CreateAndLogServiceException(Xeption exception)
        {
            var
                tppLandingCoordinationServiceException =
                new TppLandingCoordinationServiceException(
                    message: "TPP landing coordination service error occurred, please contact support.",
                    exception);

            this.loggingBroker.LogError(tppLandingCoordinationServiceException);

            throw tppLandingCoordinationServiceException;
        }

    }
}
