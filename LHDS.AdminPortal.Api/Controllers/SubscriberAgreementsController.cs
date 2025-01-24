// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using LHDS.Core.Services.Foundations.SubscriberAgreements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriberAgreementsController : RESTFulController
    {
        private readonly ISubscriberAgreementService subscriberAgreementService;

        public SubscriberAgreementsController(ISubscriberAgreementService subscriberAgreementService) =>
            this.subscriberAgreementService = subscriberAgreementService;

        [HttpPost]
        public async ValueTask<ActionResult<SubscriberAgreement>> PostSubscriberAgreementAsync(SubscriberAgreement subscriberAgreement)
        {
            try
            {
                SubscriberAgreement addedSubscriberAgreement =
                    await this.subscriberAgreementService.AddSubscriberAgreementAsync(subscriberAgreement);

                return Created(addedSubscriberAgreement);
            }
            catch (SubscriberAgreementValidationException subscriberAgreementValidationException)
            {
                return BadRequest(subscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyValidationException subscriberAgreementValidationException)
                when (subscriberAgreementValidationException.InnerException is InvalidSubscriberAgreementReferenceException)
            {
                return FailedDependency(subscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyValidationException subscriberAgreementDependencyValidationException)
               when (subscriberAgreementDependencyValidationException.InnerException is AlreadyExistsSubscriberAgreementException)
            {
                return Conflict(subscriberAgreementDependencyValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyException subscriberAgreementDependencyException)
            {
                return InternalServerError(subscriberAgreementDependencyException);
            }
            catch (SubscriberAgreementServiceException subscriberAgreementServiceException)
            {
                return InternalServerError(subscriberAgreementServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.ReadOnly")]
        [HttpGet]
#if !DEBUG
                [EnableQuery(PageSize = 50)]
#endif
#if DEBUG
        [EnableQuery(PageSize = 5000)]
#endif
        public async ValueTask<ActionResult<IQueryable<SubscriberAgreement>>> Get()
        {
            try
            {
                IQueryable<SubscriberAgreement> retrievedSubscriberAgreements =
                    await this.subscriberAgreementService.RetrieveAllSubscriberAgreementsAsync();

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

        //[HttpGet]
        //public ActionResult<IQueryable<SubscriberAgreement>> GetAllSubscriberAgreements()
        //{
        //    try
        //    {
        //        IQueryable<SubscriberAgreement> retrievedSubscriberAgreements =
        //            this.subscriberAgreementService.RetrieveAllSubscriberAgreements();

        //        return Ok(retrievedSubscriberAgreements);
        //    }
        //    catch (SubscriberAgreementDependencyException subscriberAgreementDependencyException)
        //    {
        //        return InternalServerError(subscriberAgreementDependencyException);
        //    }
        //    catch (SubscriberAgreementServiceException subscriberAgreementServiceException)
        //    {
        //        return InternalServerError(subscriberAgreementServiceException);
        //    }
        //}

        [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.ReadOnly")]
        [HttpGet("{subscriberAgreementId}")]
        public async ValueTask<ActionResult<SubscriberAgreement>> GetSubscriberAgreementByIdAsync(Guid subscriberAgreementId)
        {
            try
            {
                SubscriberAgreement subscriberAgreement = await this.subscriberAgreementService.RetrieveSubscriberAgreementByIdAsync(subscriberAgreementId);

                return Ok(subscriberAgreement);
            }
            catch (SubscriberAgreementValidationException subscriberAgreementValidationException)
                when (subscriberAgreementValidationException.InnerException is NotFoundSubscriberAgreementException)
            {
                return NotFound(subscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementValidationException subscriberAgreementValidationException)
            {
                return BadRequest(subscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyException subscriberAgreementDependencyException)
            {
                return InternalServerError(subscriberAgreementDependencyException);
            }
            catch (SubscriberAgreementServiceException subscriberAgreementServiceException)
            {
                return InternalServerError(subscriberAgreementServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators")]
        [HttpPut]
        public async ValueTask<ActionResult<SubscriberAgreement>> PutSubscriberAgreementAsync(SubscriberAgreement subscriberAgreement)
        {
            try
            {
                SubscriberAgreement modifiedSubscriberAgreement =
                    await this.subscriberAgreementService.ModifySubscriberAgreementAsync(subscriberAgreement);

                return Ok(modifiedSubscriberAgreement);
            }
            catch (SubscriberAgreementValidationException subscriberAgreementValidationException)
                when (subscriberAgreementValidationException.InnerException is NotFoundSubscriberAgreementException)
            {
                return NotFound(subscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementValidationException subscriberAgreementValidationException)
            {
                return BadRequest(subscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyValidationException subscriberAgreementValidationException)
                when (subscriberAgreementValidationException.InnerException is InvalidSubscriberAgreementReferenceException)
            {
                return FailedDependency(subscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyValidationException subscriberAgreementDependencyValidationException)
               when (subscriberAgreementDependencyValidationException.InnerException is AlreadyExistsSubscriberAgreementException)
            {
                return Conflict(subscriberAgreementDependencyValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyException subscriberAgreementDependencyException)
            {
                return InternalServerError(subscriberAgreementDependencyException);
            }
            catch (SubscriberAgreementServiceException subscriberAgreementServiceException)
            {
                return InternalServerError(subscriberAgreementServiceException);
            }
        }

        [HttpDelete("{subscriberAgreementId}")]
        public async ValueTask<ActionResult<SubscriberAgreement>> DeleteSubscriberAgreementByIdAsync(Guid subscriberAgreementId)
        {
            try
            {
                SubscriberAgreement deletedSubscriberAgreement =
                    await this.subscriberAgreementService.RemoveSubscriberAgreementByIdAsync(subscriberAgreementId);

                return Ok(deletedSubscriberAgreement);
            }
            catch (SubscriberAgreementValidationException subscriberAgreementValidationException)
                when (subscriberAgreementValidationException.InnerException is NotFoundSubscriberAgreementException)
            {
                return NotFound(subscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementValidationException subscriberAgreementValidationException)
            {
                return BadRequest(subscriberAgreementValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyValidationException subscriberAgreementDependencyValidationException)
                when (subscriberAgreementDependencyValidationException.InnerException is LockedSubscriberAgreementException)
            {
                return Locked(subscriberAgreementDependencyValidationException.InnerException);
            }
            catch (SubscriberAgreementDependencyValidationException subscriberAgreementDependencyValidationException)
            {
                return BadRequest(subscriberAgreementDependencyValidationException);
            }
            catch (SubscriberAgreementDependencyException subscriberAgreementDependencyException)
            {
                return InternalServerError(subscriberAgreementDependencyException);
            }
            catch (SubscriberAgreementServiceException subscriberAgreementServiceException)
            {
                return InternalServerError(subscriberAgreementServiceException);
            }
        }
    }
}