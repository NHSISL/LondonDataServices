// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Controllers
{
    [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators,ISL.LDS.AdminSpa.IngestionTracking")]
    [ApiController]
    [Route("api/[controller]")]
    public class IngestionTrackingsController : RESTFulController
    {
        private readonly IIngestionTrackingService ingestionTrackingService;

        public IngestionTrackingsController(IIngestionTrackingService ingestionTrackingService) =>
            this.ingestionTrackingService = ingestionTrackingService;

        [HttpPost]
        [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators,ISL.LDS.AdminSpa.IngestionTracking")]
        public async ValueTask<ActionResult<IngestionTracking>> PostIngestionTrackingAsync(
            IngestionTracking ingestionTracking)
        {
            try
            {
                IngestionTracking addedIngestionTracking =
                    await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);

                return Created(addedIngestionTracking);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                return BadRequest(ingestionTrackingValidationException.InnerException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingValidationException)
                when (ingestionTrackingValidationException.InnerException is InvalidIngestionTrackingReferenceException)
            {
                return FailedDependency(ingestionTrackingValidationException.InnerException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
                when (ingestionTrackingDependencyValidationException.InnerException
                    is AlreadyExistsIngestionTrackingException)
            {
                return Conflict(ingestionTrackingDependencyValidationException.InnerException);
            }
            catch (IngestionTrackingDependencyException ingestionTrackingDependencyException)
            {
                return InternalServerError(ingestionTrackingDependencyException);
            }
            catch (IngestionTrackingServiceException ingestionTrackingServiceException)
            {
                return InternalServerError(ingestionTrackingServiceException);
            }
        }

        [HttpGet]
#if !DEBUG
        [EnableQuery(PageSize = 50)]
#endif
#if DEBUG
        [EnableQuery(PageSize = 25)]
#endif
        [Authorize(Roles =
            "ISL.LDS.AdminSpa.Administrators,ISL.LDS.AdminSpa.IngestionTracking,ISL.LDS.AdminSpa.ReadOnly")]
        public async ValueTask<ActionResult<IQueryable<IngestionTracking>>> Get()
        {
            try
            {
                IQueryable<IngestionTracking> retrievedIngestionTrackings =
                    await this.ingestionTrackingService.RetrieveAllIngestionTrackingsAsync();

                return Ok(retrievedIngestionTrackings);
            }
            catch (IngestionTrackingDependencyException ingestionTrackingDependencyException)
            {
                return InternalServerError(ingestionTrackingDependencyException);
            }
            catch (IngestionTrackingServiceException ingestionTrackingServiceException)
            {
                return InternalServerError(ingestionTrackingServiceException);
            }
        }

        [HttpGet("{ingestionTrackingId}")]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.IngestionTracking, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<IngestionTracking>> GetIngestionTrackingByIdAsync(Guid ingestionTrackingId)
        {
            try
            {
                IngestionTracking ingestionTracking =
                    await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(ingestionTrackingId);

                return Ok(ingestionTracking);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
                when (ingestionTrackingValidationException.InnerException is NotFoundIngestionTrackingException)
            {
                return NotFound(ingestionTrackingValidationException.InnerException);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                return BadRequest(ingestionTrackingValidationException.InnerException);
            }
            catch (IngestionTrackingDependencyException ingestionTrackingDependencyException)
            {
                return InternalServerError(ingestionTrackingDependencyException);
            }
            catch (IngestionTrackingServiceException ingestionTrackingServiceException)
            {
                return InternalServerError(ingestionTrackingServiceException);
            }
        }

        [HttpGet("byfilename/{filename}")]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.AdminApi.IngestionTracking, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<IngestionTracking>> GetIngestionTrackingByFileNameAsync(string fileName)
        {
            try
            {
                string decodedFileName = HttpUtility.UrlDecode(fileName);

                IngestionTracking ingestionTracking =
                    await this.ingestionTrackingService.RetrieveIngestionTrackingByFileNameAsync(decodedFileName);

                return Ok(ingestionTracking);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
                when (ingestionTrackingValidationException.InnerException is NotFoundIngestionTrackingException)
            {
                return NotFound(ingestionTrackingValidationException.InnerException);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                return BadRequest(ingestionTrackingValidationException.InnerException);
            }
            catch (IngestionTrackingDependencyException ingestionTrackingDependencyException)
            {
                return InternalServerError(ingestionTrackingDependencyException);
            }
            catch (IngestionTrackingServiceException ingestionTrackingServiceException)
            {
                return InternalServerError(ingestionTrackingServiceException);
            }
        }

        [HttpPut]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.AdminApi.IngestionTracking")]
#endif
        public async ValueTask<ActionResult<IngestionTracking>> PutIngestionTrackingAsync(
            IngestionTracking ingestionTracking)
        {
            try
            {
                IngestionTracking modifiedIngestionTracking =
                    await this.ingestionTrackingService.ModifyIngestionTrackingAsync(ingestionTracking);

                return Ok(modifiedIngestionTracking);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
                when (ingestionTrackingValidationException.InnerException is NotFoundIngestionTrackingException)
            {
                return NotFound(ingestionTrackingValidationException.InnerException);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                return BadRequest(ingestionTrackingValidationException.InnerException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingValidationException)
                when (ingestionTrackingValidationException.InnerException is InvalidIngestionTrackingReferenceException)
            {
                return FailedDependency(ingestionTrackingValidationException.InnerException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
                when (ingestionTrackingDependencyValidationException.InnerException
                    is AlreadyExistsIngestionTrackingException)
            {
                return Conflict(ingestionTrackingDependencyValidationException.InnerException);
            }
            catch (IngestionTrackingDependencyException ingestionTrackingDependencyException)
            {
                return InternalServerError(ingestionTrackingDependencyException);
            }
            catch (IngestionTrackingServiceException ingestionTrackingServiceException)
            {
                return InternalServerError(ingestionTrackingServiceException);
            }
        }

        [HttpDelete("{ingestionTrackingId}")]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.AdminApi.IngestionTracking")]
#endif
        public async ValueTask<ActionResult<IngestionTracking>> DeleteIngestionTrackingByIdAsync(
            Guid ingestionTrackingId)
        {
            try
            {
                IngestionTracking deletedIngestionTracking =
                    await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTrackingId);

                return Ok(deletedIngestionTracking);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
                when (ingestionTrackingValidationException.InnerException is NotFoundIngestionTrackingException)
            {
                return NotFound(ingestionTrackingValidationException.InnerException);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                return BadRequest(ingestionTrackingValidationException.InnerException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
                when (ingestionTrackingDependencyValidationException.InnerException is LockedIngestionTrackingException)
            {
                return Locked(ingestionTrackingDependencyValidationException.InnerException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
            {
                return BadRequest(ingestionTrackingDependencyValidationException);
            }
            catch (IngestionTrackingDependencyException ingestionTrackingDependencyException)
            {
                return InternalServerError(ingestionTrackingDependencyException);
            }
            catch (IngestionTrackingServiceException ingestionTrackingServiceException)
            {
                return InternalServerError(ingestionTrackingServiceException);
            }
        }
    }
}