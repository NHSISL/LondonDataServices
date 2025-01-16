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
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentIngressOrchestrationException);
            }
            catch (NotFoundIngressOrchestrationException notFoundIngressOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundIngressOrchestrationException);
            }
            catch (NoConfigIngressOrchestrationException noConfigIngressOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(noConfigIngressOrchestrationException);
            }
            catch (IngestionTrackingProcessingValidationException ingestionTrackingProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ingestionTrackingProcessingValidationException);
            }
            catch (IngestionTrackingProcessingDependencyValidationException
                ingestionTrackingProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ingestionTrackingProcessingDependencyValidationException);
            }
            catch (SpecificationObjectProcessingValidationException specificationObjectProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(specificationObjectProcessingValidationException);
            }
            catch (SpecificationObjectProcessingDependencyValidationException
                specificationObjectProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(specificationObjectProcessingDependencyValidationException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentDependencyValidationException);
            }
            catch (IngestionTrackingProcessingDependencyException ingestionTrackingProcessingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingestionTrackingProcessingDependencyException);
            }
            catch (IngestionTrackingProcessingServiceException
                ingestionTrackingProcessingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingestionTrackingProcessingServiceException);
            }
            catch (SpecificationObjectProcessingDependencyException specificationObjectProcessingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(specificationObjectProcessingDependencyException);
            }
            catch (SpecificationObjectProcessingServiceException
                specificationObjectProcessingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(specificationObjectProcessingServiceException);
            }
            catch (DocumentProcessingDependencyException documentProcessingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentProcessingDependencyException);
            }
            catch (DocumentProcessingServiceException documentProcessingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentProcessingServiceException);
            }
            catch (Exception exception)
            {
                var failedIngressOrchestrationServiceException =
                    new FailedIngressOrchestrationServiceException(
                        message: "Failed ingress orchestration service error occurred, please contact support.",
                        exception);

                throw await CreateAndLogServiceExceptionAsync(failedIngressOrchestrationServiceException);
            }
        }

        private async ValueTask<IngressOrchestrationValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var ingressOrchestrationValidationException =
                new IngressOrchestrationValidationException(
                    message: "Ingress orchestration validation errors occurred, please try again.",
                    exception);

            await this.loggingBroker.LogErrorAsync(ingressOrchestrationValidationException);

            return ingressOrchestrationValidationException;
        }

        private async ValueTask<IngressOrchestrationDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var ingressOrchestrationDependencyValidationException =
                new IngressOrchestrationDependencyValidationException(
                    message: "Ingress orchestration dependency validation error occurred, fix the errors and try again.",
                    exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(ingressOrchestrationDependencyValidationException);

            return ingressOrchestrationDependencyValidationException;
        }

        private async ValueTask<IngressOrchestrationDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var ingressOrchestrationDependencyException =
                new IngressOrchestrationDependencyException(
                    message: "Ingress orchestration dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(ingressOrchestrationDependencyException);

            return ingressOrchestrationDependencyException;
        }

        private async ValueTask<IngressOrchestrationServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var ingressOrchestrationServiceException =
                new IngressOrchestrationServiceException(
                    message: "Ingress orchestration service error occurred, please contact support.",
                    exception);

            await this.loggingBroker.LogErrorAsync(ingressOrchestrationServiceException);

            return ingressOrchestrationServiceException;
        }
    }
}
