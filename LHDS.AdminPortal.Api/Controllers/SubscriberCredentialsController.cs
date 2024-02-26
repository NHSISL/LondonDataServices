// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriberCredentialsController : RESTFulController
    {
        private readonly ISubscriberCredentialOrchestration subscriberCredentialOrchestration;

        public SubscriberCredentialsController(ISubscriberCredentialOrchestration subscriberCredentialOrchestration) =>
            this.subscriberCredentialOrchestration = subscriberCredentialOrchestration;

        [HttpPost]
        public async ValueTask<ActionResult<SubscriberCredential>> PostSubscriberCredentialAsync(
            SubscriberCredential subscriberCredential)
        {
            try
            {
                SubscriberCredential addedSubscriberCredential =
                    await this.subscriberCredentialOrchestration
                        .ModifyOrAddSubscriberCredentialAsync(subscriberCredential);

                return Created(addedSubscriberCredential);
            }
            catch (SubscriberCredentialValidationOrchestrationException
                subscriberCredentialValidationOrchestrationException)
            {
                return BadRequest(subscriberCredentialValidationOrchestrationException.InnerException);
            }
            catch (SubscriberCredentialOrchestrationDependencyValidationException
                subscriberCredentialOrchestrationDependencyValidationException)
            {
                return FailedDependency(subscriberCredentialOrchestrationDependencyValidationException.InnerException);
            }
            catch (SubscriberCredentialDependencyOrchestrationException
                subscriberCredentialDependencyOrchestrationException)
            {
                return InternalServerError(subscriberCredentialDependencyOrchestrationException);
            }
            catch (SubscriberCredentialOrchestrationServiceException
                subscriberCredentialOrchestrationServiceException)
            {
                return InternalServerError(subscriberCredentialOrchestrationServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<SubscriberCredential>> GetAllSubscriberCredentials()
        {
            try
            {
                IQueryable<SubscriberCredential> retrievedSubscriberCredentials =
                    this.subscriberCredentialOrchestration.RetrieveAllSubscriberCredentials();

                return Ok(retrievedSubscriberCredentials);
            }
            catch (SubscriberCredentialDependencyOrchestrationException
                subscriberCredentialDependencyOrchestrationException)
            {
                return InternalServerError(subscriberCredentialDependencyOrchestrationException);
            }
            catch (SubscriberCredentialOrchestrationServiceException
                subscriberCredentialOrchestrationServiceException)
            {
                return InternalServerError(subscriberCredentialOrchestrationServiceException);
            }
        }

        [HttpGet("{subscriberCredentialId}")]
        public async ValueTask<ActionResult<SubscriberCredential>> GetSubscriberCredentialByIdAsync(
            Guid subscriberCredentialId)
        {
            try
            {
                SubscriberCredential subscriberCredential = await this.subscriberCredentialOrchestration
                    .RetrieveSubscriberCredentialByIdAsync(subscriberCredentialId);

                return Ok(subscriberCredential);
            }
            catch (SubscriberCredentialValidationOrchestrationException subscriberCredentialValidationException)
                when (subscriberCredentialValidationException.InnerException is NotFoundSubscriberAgreementException)
            {
                return NotFound(subscriberCredentialValidationException.InnerException);
            }
            catch (SubscriberCredentialValidationOrchestrationException
                subscriberCredentialValidationOrchestrationException)
            {
                return BadRequest(subscriberCredentialValidationOrchestrationException.InnerException);
            }
            catch (SubscriberCredentialDependencyOrchestrationException
                subscriberCredentialDependencyOrchestrationException)
            {
                return InternalServerError(subscriberCredentialDependencyOrchestrationException);
            }
            catch (SubscriberCredentialOrchestrationServiceException
                subscriberCredentialOrchestrationServiceException)
            {
                return InternalServerError(subscriberCredentialOrchestrationServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<SubscriberCredential>> PutSubscriberCredentialAsync(
            SubscriberCredential subscriberCredential)
        {
            try
            {
                SubscriberCredential modifiedSubscriberCredential =
                    await this.subscriberCredentialOrchestration
                        .ModifyOrAddSubscriberCredentialAsync(subscriberCredential);

                return Ok(modifiedSubscriberCredential);
            }
            catch (SubscriberCredentialValidationOrchestrationException subscriberCredentialValidationException)
                when (subscriberCredentialValidationException.InnerException is NotFoundSubscriberAgreementException)
            {
                return NotFound(subscriberCredentialValidationException.InnerException);
            }
            catch (SubscriberCredentialValidationOrchestrationException
                subscriberCredentialValidationOrchestrationException)
            {
                return BadRequest(subscriberCredentialValidationOrchestrationException.InnerException);
            }
            catch (SubscriberCredentialOrchestrationDependencyValidationException
                subscriberCredentialOrchestrationDependencyValidationException)
            {
                return FailedDependency(subscriberCredentialOrchestrationDependencyValidationException.InnerException);
            }
            catch (SubscriberCredentialDependencyOrchestrationException
                subscriberCredentialDependencyOrchestrationException)
            {
                return InternalServerError(subscriberCredentialDependencyOrchestrationException);
            }
            catch (SubscriberCredentialOrchestrationServiceException
                subscriberCredentialOrchestrationServiceException)
            {
                return InternalServerError(subscriberCredentialOrchestrationServiceException);
            }
        }

        [HttpDelete("{subscriberCredentialId}")]
        public async ValueTask<ActionResult<SubscriberCredential>> DeleteSubscriberCredentialByIdAsync(
            Guid subscriberCredentialId)
        {
            try
            {
                SubscriberCredential deletedSubscriberCredential =
                    await this.subscriberCredentialOrchestration
                        .RemoveSubscriberCredentialByIdAsync(subscriberCredentialId);

                return Ok(deletedSubscriberCredential);
            }
            catch (SubscriberCredentialValidationOrchestrationException subscriberCredentialValidationException)
                when (subscriberCredentialValidationException.InnerException is NotFoundSubscriberAgreementException)
            {
                return NotFound(subscriberCredentialValidationException.InnerException);
            }
            catch (SubscriberCredentialValidationOrchestrationException
                subscriberCredentialValidationOrchestrationException)
            {
                return BadRequest(subscriberCredentialValidationOrchestrationException.InnerException);
            }
            catch (SubscriberCredentialOrchestrationDependencyValidationException
                subscriberCredentialOrchestrationDependencyValidationException)
            {
                return FailedDependency(subscriberCredentialOrchestrationDependencyValidationException.InnerException);
            }
            catch (SubscriberCredentialDependencyOrchestrationException
                subscriberCredentialDependencyOrchestrationException)
            {
                return InternalServerError(subscriberCredentialDependencyOrchestrationException);
            }
            catch (SubscriberCredentialOrchestrationServiceException
                subscriberCredentialOrchestrationServiceException)
            {
                return InternalServerError(subscriberCredentialOrchestrationServiceException);
            }
        }
    }
}