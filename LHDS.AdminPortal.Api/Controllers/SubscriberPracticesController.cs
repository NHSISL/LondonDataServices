// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions;
using LHDS.Core.Services.Foundations.SubscriberPractices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Controllers
{
    [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.Configurations")]
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriberPracticesController : RESTFulController
    {
        private readonly ISubscriberPracticeService subscriberPracticeService;

        public SubscriberPracticesController(ISubscriberPracticeService subscriberPracticeService) =>
            this.subscriberPracticeService = subscriberPracticeService;

        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.Configurations")]
        [HttpPost]
        public async ValueTask<ActionResult<SubscriberPractice>> PostSubscriberPracticeAsync(
            SubscriberPractice subscriberPractice)
        {
            try
            {
                SubscriberPractice addedSubscriberPractice =
                    await this.subscriberPracticeService.AddSubscriberPracticeAsync(subscriberPractice);

                return Created(addedSubscriberPractice);
            }
            catch (SubscriberPracticeValidationException subscriberPracticeValidationException)
            {
                return BadRequest(subscriberPracticeValidationException.InnerException);
            }
            catch (SubscriberPracticeDependencyValidationException subscriberPracticeValidationException)
                when (subscriberPracticeValidationException.InnerException is
                InvalidSubscriberPracticeReferenceException)
            {
                return FailedDependency(subscriberPracticeValidationException.InnerException);
            }
            catch (SubscriberPracticeDependencyValidationException subscriberPracticeDependencyValidationException)
               when (subscriberPracticeDependencyValidationException.InnerException is
               AlreadyExistsSubscriberPracticeException)
            {
                return Conflict(subscriberPracticeDependencyValidationException.InnerException);
            }
            catch (SubscriberPracticeDependencyException subscriberPracticeDependencyException)
            {
                return InternalServerError(subscriberPracticeDependencyException);
            }
            catch (SubscriberPracticeServiceException subscriberPracticeServiceException)
            {
                return InternalServerError(subscriberPracticeServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.Configurations, ISL.LDS.AdminApi.ReadOnly")]
        [HttpGet]
#if !DEBUG
        [EnableQuery(PageSize = 50)]
#endif
#if DEBUG
        [EnableQuery(PageSize = 5000)]
#endif
        public async ValueTask<ActionResult<IQueryable<SubscriberPractice>>> Get()
        {
            try
            {
                IQueryable<SubscriberPractice> retrievedSubscriberPractices =
                    await this.subscriberPracticeService.RetrieveAllSubscriberPracticesAsync();

                return Ok(retrievedSubscriberPractices);
            }
            catch (SubscriberPracticeDependencyException SubscriberPracticeDependencyException)
            {
                return InternalServerError(SubscriberPracticeDependencyException);
            }
            catch (SubscriberPracticeServiceException SubscriberPracticeServiceException)
            {
                return InternalServerError(SubscriberPracticeServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.Configurations, ISL.LDS.AdminApi.ReadOnly")]
        [HttpGet("{subscriberPracticeId}")]
        public async ValueTask<ActionResult<SubscriberPractice>> GetSubscriberPracticeByIdAsync(
            Guid subscriberPracticeId)
        {
            try
            {
                SubscriberPractice subscriberPractice =
                    await this.subscriberPracticeService.RetrieveSubscriberPracticeByIdAsync(subscriberPracticeId);

                return Ok(subscriberPractice);
            }
            catch (SubscriberPracticeValidationException subscriberPracticeValidationException)
                when (subscriberPracticeValidationException.InnerException is NotFoundSubscriberPracticeException)
            {
                return NotFound(subscriberPracticeValidationException.InnerException);
            }
            catch (SubscriberPracticeValidationException subscriberPracticeValidationException)
            {
                return BadRequest(subscriberPracticeValidationException.InnerException);
            }
            catch (SubscriberPracticeDependencyException subscriberPracticeDependencyException)
            {
                return InternalServerError(subscriberPracticeDependencyException);
            }
            catch (SubscriberPracticeServiceException subscriberPracticeServiceException)
            {
                return InternalServerError(subscriberPracticeServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.Configurations")]
        [HttpPut]
        public async ValueTask<ActionResult<SubscriberPractice>> PutSubscriberPracticeAsync(
            SubscriberPractice subscriberPractice)
        {
            try
            {
                SubscriberPractice modifiedSubscriberPractice =
                    await this.subscriberPracticeService.ModifySubscriberPracticeAsync(subscriberPractice);

                return Ok(modifiedSubscriberPractice);
            }
            catch (SubscriberPracticeValidationException subscriberPracticeValidationException)
                when (subscriberPracticeValidationException.InnerException is NotFoundSubscriberPracticeException)
            {
                return NotFound(subscriberPracticeValidationException.InnerException);
            }
            catch (SubscriberPracticeValidationException subscriberPracticeValidationException)
            {
                return BadRequest(subscriberPracticeValidationException.InnerException);
            }
            catch (SubscriberPracticeDependencyValidationException subscriberPracticeValidationException)
                when (subscriberPracticeValidationException.InnerException is
                InvalidSubscriberPracticeReferenceException)
            {
                return FailedDependency(subscriberPracticeValidationException.InnerException);
            }
            catch (SubscriberPracticeDependencyValidationException subscriberPracticeDependencyValidationException)
               when (subscriberPracticeDependencyValidationException.InnerException is
               AlreadyExistsSubscriberPracticeException)
            {
                return Conflict(subscriberPracticeDependencyValidationException.InnerException);
            }
            catch (SubscriberPracticeDependencyException subscriberPracticeDependencyException)
            {
                return InternalServerError(subscriberPracticeDependencyException);
            }
            catch (SubscriberPracticeServiceException subscriberPracticeServiceException)
            {
                return InternalServerError(subscriberPracticeServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.Configurations")]
        [HttpDelete("{subscriberPracticeId}")]
        public async ValueTask<ActionResult<SubscriberPractice>> DeleteSubscriberPracticeByIdAsync(
            Guid subscriberPracticeId)
        {
            try
            {
                SubscriberPractice deletedSubscriberPractice =
                    await this.subscriberPracticeService.RemoveSubscriberPracticeByIdAsync(subscriberPracticeId);

                return Ok(deletedSubscriberPractice);
            }
            catch (SubscriberPracticeValidationException subscriberPracticeValidationException)
                when (subscriberPracticeValidationException.InnerException is NotFoundSubscriberPracticeException)
            {
                return NotFound(subscriberPracticeValidationException.InnerException);
            }
            catch (SubscriberPracticeValidationException subscriberPracticeValidationException)
            {
                return BadRequest(subscriberPracticeValidationException.InnerException);
            }
            catch (SubscriberPracticeDependencyValidationException subscriberPracticeDependencyValidationException)
                when (subscriberPracticeDependencyValidationException.InnerException is
                LockedSubscriberPracticeException)
            {
                return Locked(subscriberPracticeDependencyValidationException.InnerException);
            }
            catch (SubscriberPracticeDependencyValidationException subscriberPracticeDependencyValidationException)
            {
                return BadRequest(subscriberPracticeDependencyValidationException.InnerException);
            }
            catch (SubscriberPracticeDependencyException subscriberPracticeDependencyException)
            {
                return InternalServerError(subscriberPracticeDependencyException);
            }
            catch (SubscriberPracticeServiceException subscriberPracticeServiceException)
            {
                return InternalServerError(subscriberPracticeServiceException);
            }
        }
    }
}