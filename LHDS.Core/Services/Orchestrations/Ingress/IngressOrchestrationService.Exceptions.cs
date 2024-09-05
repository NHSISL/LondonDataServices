// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Orchestrations.Ingres.Exceptions;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using LHDS.Core.Models.Processings.IngestionTrackings.Exceptions;
using LHDS.Core.Models.Processings.SpecificationObjects.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.Ingress
{
    public partial class IngressOrchestrationService
    {
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentIngressOrchestrationException invalidArgumentIngressOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentIngressOrchestrationException);
            }
            catch (NotFoundIngressOrchestrationException notFoundIngressOrchestrationException)
            {
                throw CreateAndLogValidationException(notFoundIngressOrchestrationException);
            }
            catch (NoConfigIngressOrchestrationException noConfigIngressOrchestrationException)
            {
                throw CreateAndLogValidationException(noConfigIngressOrchestrationException);
            }
            catch (IngestionTrackingProcessingValidationException ingestionTrackingProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingestionTrackingProcessingValidationException);
            }
            catch (IngestionTrackingProcessingDependencyValidationException
                ingestionTrackingProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingestionTrackingProcessingDependencyValidationException);
            }
            catch (SpecificationObjectProcessingValidationException specificationObjectProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(specificationObjectProcessingValidationException);
            }
            catch (SpecificationObjectProcessingDependencyValidationException
                specificationObjectProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(specificationObjectProcessingDependencyValidationException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentDependencyValidationException);
            }
            catch (IngestionTrackingProcessingDependencyException ingestionTrackingProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(ingestionTrackingProcessingDependencyException);
            }
            catch (IngestionTrackingProcessingServiceException
                ingestionTrackingProcessingServiceException)
            {
                throw CreateAndLogDependencyException(ingestionTrackingProcessingServiceException);
            }
            catch (SpecificationObjectProcessingDependencyException specificationObjectProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(specificationObjectProcessingDependencyException);
            }
            catch (SpecificationObjectProcessingServiceException
                specificationObjectProcessingServiceException)
            {
                throw CreateAndLogDependencyException(specificationObjectProcessingServiceException);
            }
            catch (DocumentProcessingDependencyException documentProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(documentProcessingDependencyException);
            }
            catch (DocumentProcessingServiceException documentProcessingServiceException)
            {
                throw CreateAndLogDependencyException(documentProcessingServiceException);
            }
            catch (Exception exception)
            {
                var failedIngressOrchestrationServiceException =
                    new FailedIngressOrchestrationServiceException(
                        message: "Failed ingress orchestration service error occurred, please contact support.",
                        exception);

                throw CreateAndLogServiceException(failedIngressOrchestrationServiceException);
            }
        }

        private IngressOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ingressOrchestrationValidationException =
                new IngressOrchestrationValidationException(
                    message: "Ingress orchestration validation errors occurred, please try again.",
                    exception);

            this.loggingBroker.LogError(ingressOrchestrationValidationException);

            return ingressOrchestrationValidationException;
        }

        private IngressOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var ingressOrchestrationDependencyValidationException =
                new IngressOrchestrationDependencyValidationException(
                    message: "Ingress orchestration dependency validation error occurred, fix the errors and try again.",
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(ingressOrchestrationDependencyValidationException);

            return ingressOrchestrationDependencyValidationException;
        }

        private IngressOrchestrationDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var ingressOrchestrationDependencyException =
                new IngressOrchestrationDependencyException(
                    message: "Ingress orchestration dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(ingressOrchestrationDependencyException);

            throw ingressOrchestrationDependencyException;
        }

        private IngressOrchestrationServiceException
            CreateAndLogServiceException(Xeption exception)
        {
            var ingressOrchestrationServiceException =
                new IngressOrchestrationServiceException(
                    message: "Ingress orchestration service error occurred, please contact support.",
                    exception);

            this.loggingBroker.LogError(ingressOrchestrationServiceException);

            throw ingressOrchestrationServiceException;
        }
    }
}
