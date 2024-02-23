// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using LHDS.Core.Services.Foundations.SubscriberAgreements;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriberAgreementsController : RESTFulController
    {
        private readonly ISubscriberAgreementService SubscriberAgreementService;

        public SubscriberAgreementsController(ISubscriberAgreementService SubscriberAgreementService) =>
            this.SubscriberAgreementService = SubscriberAgreementService;

        [HttpPost]
        public async ValueTask<ActionResult<SubscriberAgreement>> PostSubscriberAgreementAsync(SubscriberAgreement SubscriberAgreement)
        {
            try
            {
                SubscriberAgreement addedSubscriberAgreement =
                    await this.SubscriberAgreementService.AddSubscriberAgreementAsync(SubscriberAgreement);

                return Created(addedSubscriberAgreement);
            }
            catch (SubscriberAgreementValidationException SubscriberAgreementValidationException)
            {
                return BadRequest(SubscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyValidationException SubscriberAgreementValidationException)
                when (SubscriberAgreementValidationException.InnerException is InvalidSubscriberAgreementReferenceException)
            {
                return FailedDependency(SubscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyValidationException SubscriberAgreementDependencyValidationException)
               when (SubscriberAgreementDependencyValidationException.InnerException is AlreadyExistsSubscriberAgreementException)
            {
                return Conflict(SubscriberAgreementDependencyValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyException SubscriberAgreementDependencyException)
            {
                return InternalServerError(SubscriberAgreementDependencyException);
            }
            catch (SubscriberAgreementServiceException SubscriberAgreementServiceException)
            {
                return InternalServerError(SubscriberAgreementServiceException);
            }
        }

        [HttpGet]
#if !DEBUG
        [EnableQuery(PageSize = 50)]
#endif
#if DEBUG
        [EnableQuery(PageSize = 5000)]
#endif
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.SubscriberAgreements, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public ActionResult<IQueryable<SubscriberAgreement>> Get()
        {
            try
            {
                IQueryable<SubscriberAgreement> retrievedSubscriberAgreements =
                    this.SubscriberAgreementService.RetrieveAllSubscriberAgreements();

                return Ok(retrievedSubscriberAgreements);
            }
            catch (SubscriberAgreementDependencyException SubscriberAgreementDependencyException)
            {
                return InternalServerError(SubscriberAgreementDependencyException);
            }
            catch (SubscriberAgreementServiceException SubscriberAgreementServiceException)
            {
                return InternalServerError(SubscriberAgreementServiceException);
            }
        }

        [HttpGet("{SubscriberAgreementId}")]
        public async ValueTask<ActionResult<SubscriberAgreement>> GetSubscriberAgreementByIdAsync(Guid SubscriberAgreementId)
        {
            try
            {
                SubscriberAgreement SubscriberAgreement = await this.SubscriberAgreementService.RetrieveSubscriberAgreementByIdAsync(SubscriberAgreementId);

                return Ok(SubscriberAgreement);
            }
            catch (SubscriberAgreementValidationException SubscriberAgreementValidationException)
                when (SubscriberAgreementValidationException.InnerException is NotFoundSubscriberAgreementException)
            {
                return NotFound(SubscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementValidationException SubscriberAgreementValidationException)
            {
                return BadRequest(SubscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyException SubscriberAgreementDependencyException)
            {
                return InternalServerError(SubscriberAgreementDependencyException);
            }
            catch (SubscriberAgreementServiceException SubscriberAgreementServiceException)
            {
                return InternalServerError(SubscriberAgreementServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<SubscriberAgreement>> PutSubscriberAgreementAsync(SubscriberAgreement SubscriberAgreement)
        {
            try
            {
                SubscriberAgreement modifiedSubscriberAgreement =
                    await this.SubscriberAgreementService.ModifySubscriberAgreementAsync(SubscriberAgreement);

                return Ok(modifiedSubscriberAgreement);
            }
            catch (SubscriberAgreementValidationException SubscriberAgreementValidationException)
                when (SubscriberAgreementValidationException.InnerException is NotFoundSubscriberAgreementException)
            {
                return NotFound(SubscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementValidationException SubscriberAgreementValidationException)
            {
                return BadRequest(SubscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyValidationException SubscriberAgreementValidationException)
                when (SubscriberAgreementValidationException.InnerException is InvalidSubscriberAgreementReferenceException)
            {
                return FailedDependency(SubscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyValidationException SubscriberAgreementDependencyValidationException)
               when (SubscriberAgreementDependencyValidationException.InnerException is AlreadyExistsSubscriberAgreementException)
            {
                return Conflict(SubscriberAgreementDependencyValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyException SubscriberAgreementDependencyException)
            {
                return InternalServerError(SubscriberAgreementDependencyException);
            }
            catch (SubscriberAgreementServiceException SubscriberAgreementServiceException)
            {
                return InternalServerError(SubscriberAgreementServiceException);
            }
        }

        [HttpDelete("{SubscriberAgreementId}")]
        public async ValueTask<ActionResult<SubscriberAgreement>> DeleteSubscriberAgreementByIdAsync(Guid SubscriberAgreementId)
        {
            try
            {
                SubscriberAgreement deletedSubscriberAgreement =
                    await this.SubscriberAgreementService.RemoveSubscriberAgreementByIdAsync(SubscriberAgreementId);

                return Ok(deletedSubscriberAgreement);
            }
            catch (SubscriberAgreementValidationException SubscriberAgreementValidationException)
                when (SubscriberAgreementValidationException.InnerException is NotFoundSubscriberAgreementException)
            {
                return NotFound(SubscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementValidationException SubscriberAgreementValidationException)
            {
                return BadRequest(SubscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyValidationException SubscriberAgreementDependencyValidationException)
                when (SubscriberAgreementDependencyValidationException.InnerException is LockedSubscriberAgreementException)
            {
                return Locked(SubscriberAgreementDependencyValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyValidationException SubscriberAgreementDependencyValidationException)
            {
                return BadRequest(SubscriberAgreementDependencyValidationException);
            }
            catch (SubscriberAgreementDependencyException SubscriberAgreementDependencyException)
            {
                return InternalServerError(SubscriberAgreementDependencyException);
            }
            catch (SubscriberAgreementServiceException SubscriberAgreementServiceException)
            {
                return InternalServerError(SubscriberAgreementServiceException);
            }
        }
    }
}
