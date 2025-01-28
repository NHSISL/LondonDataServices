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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Controllers
{
    [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.Configurations")]
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriberCredentialsController : RESTFulController
    {
        private readonly ISubscriberCredentialOrchestration subscriberCredentialOrchestration;

        public SubscriberCredentialsController(ISubscriberCredentialOrchestration subscriberCredentialOrchestration) =>
            this.subscriberCredentialOrchestration = subscriberCredentialOrchestration;

        [HttpPost]
        public async ValueTask<ActionResult<SubscriberCredential>> PostSubscriberCredentialAsync(
            [FromBody] SubscriberCredential subscriberCredential)
        {
            try
            {
                return Ok(await this.subscriberCredentialOrchestration
                    .ModifyOrAddSubscriberCredentialAsync(
                        subscriberCredential,
                        regenerateKeys: false,
                        externalUse: true));
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

        [HttpPost("regeneratekeys")]
        public async ValueTask<ActionResult<SubscriberCredential>> PostSubscriberCredentialAndRegenerateKeysAsync(
            [FromBody] SubscriberCredential subscriberCredential)
        {
            try
            {
                return Ok(await this.subscriberCredentialOrchestration
                    .ModifyOrAddSubscriberCredentialAsync(
                        subscriberCredential,
                        regenerateKeys: true,
                        externalUse: true));
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
#if !DEBUG
[EnableQuery(PageSize = 50)]
#endif
#if DEBUG
        [EnableQuery(PageSize = 5000)]
#endif
        [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.SubscriberCredentials, ISL.LDS.AdminSpa.ReadOnly")]
        public async ValueTask<ActionResult<IQueryable<SubscriberCredential>>> Get()
        {
            try
            {
                IQueryable<SubscriberCredential> retrievedSubscriberCredentials =
                    await this.subscriberCredentialOrchestration.RetrieveAllSubscriberCredentialsAsync();

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

        [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.SubscriberCredentials, ISL.LDS.AdminSpa.ReadOnly")]
        [HttpGet("{subscriberCredentialId}")]
        public async ValueTask<ActionResult<SubscriberCredential>> GetSubscriberCredentialByIdAsync(
            Guid subscriberCredentialId)
        {
            try
            {
                SubscriberCredential subscriberCredential = await this.subscriberCredentialOrchestration
                    .RetrieveSubscriberCredentialByIdAsync(subscriberCredentialId, externalUse: true);

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
            [FromBody] SubscriberCredential subscriberCredential)
        {
            try
            {
                return Ok(await this.subscriberCredentialOrchestration
                    .ModifyOrAddSubscriberCredentialAsync(
                        subscriberCredential,
                        regenerateKeys: false,
                        externalUse: true));
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

        [HttpPut("regeneratekeys")]
        public async ValueTask<ActionResult<SubscriberCredential>> PutSubscriberCredentialAndRegenerateKeysAsync(
            [FromBody] SubscriberCredential subscriberCredential)
        {
            try
            {
                return Ok(await this.subscriberCredentialOrchestration
                    .ModifyOrAddSubscriberCredentialAsync(
                    subscriberCredential,
                    regenerateKeys: true,
                    externalUse: true));
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
                await this.subscriberCredentialOrchestration
                        .RemoveSubscriberCredentialByIdAsync(subscriberCredentialId);

                return Ok();
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