// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Clients.IDecideClient.Exceptions;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Models.Orchestrations.Decisions.Exceptions;
using LHDS.Core.Services.Orchestrations.Decisions;
using Xeptions;

namespace LHDS.Core.Clients
{
    public class IDecideClient : IIDecideClient
    {
        private readonly IDecisionOrchestrationService decisionOrchestrationService;

        public IDecideClient(IDecisionOrchestrationService decisionOrchestrationService)
        {
            this.decisionOrchestrationService = decisionOrchestrationService;
        }

        public async ValueTask<List<Decision>> GetPatientDecisions()
        {
            try
            {
                return await this.decisionOrchestrationService.GetPatientDecisions();
            }
            catch (DecisionOrchestrationValidationException decisionOrchestrationValidationException)
            {
                throw new IDecideClientValidationException(
                    message: "IDecide client validation error occurred, fix errors and try again.",
                    innerException: decisionOrchestrationValidationException.InnerException as Xeption);
            }
            catch (DecisionOrchestrationDependencyValidationException
                decisionOrchestrationDependencyValidationException)
            {
                throw new IDecideClientValidationException(
                    message: "IDecide client validation error occurred, fix errors and try again.",
                    innerException:
                        decisionOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (DecisionOrchestrationDependencyException decisionOrchestrationDependencyException)
            {
                throw new IDecideClientDependencyException(
                    message: "IDecide client dependency error occurred, please contact support.",
                    innerException: decisionOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (DecisionOrchestrationServiceException decisionOrchestrationServiceException)
            {
                throw new IDecideClientServiceException(
                    message: "IDecide client service error occurred, please contact support.",
                    innerException: decisionOrchestrationServiceException.InnerException as Xeption);
            }
        }
    }
}
