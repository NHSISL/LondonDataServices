// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
using LHDS.Core.Services.Foundations.TerminologyArtifacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers
{
    [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.Configurations")]
    [Route("api/[controller]")]
    public class TerminologyArtifactsController : RESTFulController
    {
        private readonly ITerminologyArtifactService terminologyArtifactService;

        public TerminologyArtifactsController(ITerminologyArtifactService terminologyArtifactService) =>
            this.terminologyArtifactService = terminologyArtifactService;

        [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.Configurations")]
        [HttpPost]
        public async ValueTask<ActionResult<TerminologyArtifact>> PostTerminologyArtifactAsync(
            TerminologyArtifact terminologyArtifact)
        {
            try
            {
                TerminologyArtifact addedTerminologyArtifact =
                    await this.terminologyArtifactService.AddTerminologyArtifactAsync(terminologyArtifact);

                return Created(addedTerminologyArtifact);
            }
            catch (TerminologyArtifactValidationException terminologyArtifactValidationException)
            {
                return BadRequest(terminologyArtifactValidationException.InnerException);
            }
            catch (TerminologyArtifactDependencyValidationException terminologyArtifactValidationException)
                when (terminologyArtifactValidationException.InnerException
                    is InvalidTerminologyArtifactReferenceException)
            {
                return FailedDependency(terminologyArtifactValidationException.InnerException);
            }
            catch (TerminologyArtifactDependencyValidationException terminologyArtifactDependencyValidationException)
                when (terminologyArtifactDependencyValidationException.InnerException
                    is AlreadyExistsTerminologyArtifactException)
            {
                return Conflict(terminologyArtifactDependencyValidationException.InnerException);
            }
            catch (TerminologyArtifactDependencyException terminologyArtifactDependencyException)
            {
                return InternalServerError(terminologyArtifactDependencyException);
            }
            catch (TerminologyArtifactServiceException terminologyArtifactServiceException)
            {
                return InternalServerError(terminologyArtifactServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminSpa.TerminologyArtifact,ISL.LDS.AdminSpa.Administrators,ISL.LDS.AdminSpa.ReadOnly")]
        [HttpGet]
#if !DEBUG
        [EnableQuery(PageSize = 50)]
#endif
#if DEBUG
        [EnableQuery(PageSize = 50)]
#endif
        public async ValueTask<ActionResult<IQueryable<TerminologyArtifact>>> Get()
        {
            try
            {
                IQueryable<TerminologyArtifact> retrievedTerminologyArtifacts =
                    await this.terminologyArtifactService.RetrieveAllTerminologyArtifactsAsync();

                return Ok(retrievedTerminologyArtifacts);
            }
            catch (TerminologyArtifactDependencyException terminologyArtifactDependencyException)
            {
                return InternalServerError(terminologyArtifactDependencyException);
            }
            catch (TerminologyArtifactServiceException terminologyArtifactServiceException)
            {
                return InternalServerError(terminologyArtifactServiceException);
            }
        }

        [Authorize(Roles = 
            "ISL.LDS.AdminSpa.TerminologyArtifact,ISL.LDS.AdminSpa.Administrators,ISL.LDS.AdminSpa.ReadOnly")]
        [HttpGet("{terminologyArtifactId}")]
        public async ValueTask<ActionResult<TerminologyArtifact>> GetTerminologyArtifactByIdAsync(
            Guid terminologyArtifactId)
        {
            try
            {
                TerminologyArtifact terminologyArtifact = await this.terminologyArtifactService
                    .RetrieveTerminologyArtifactByIdAsync(terminologyArtifactId);

                return Ok(terminologyArtifact);
            }
            catch (TerminologyArtifactValidationException terminologyArtifactValidationException)
                when (terminologyArtifactValidationException.InnerException is NotFoundTerminologyArtifactException)
            {
                return NotFound(terminologyArtifactValidationException.InnerException);
            }
            catch (TerminologyArtifactValidationException terminologyArtifactValidationException)
            {
                return BadRequest(terminologyArtifactValidationException.InnerException);
            }
            catch (TerminologyArtifactDependencyException terminologyArtifactDependencyException)
            {
                return InternalServerError(terminologyArtifactDependencyException);
            }
            catch (TerminologyArtifactServiceException terminologyArtifactServiceException)
            {
                return InternalServerError(terminologyArtifactServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.Configurations")]
        [HttpPut]
        public async ValueTask<ActionResult<TerminologyArtifact>> PutTerminologyArtifactAsync(
            TerminologyArtifact terminologyArtifact)
        {
            try
            {
                TerminologyArtifact modifiedTerminologyArtifact =
                    await this.terminologyArtifactService.ModifyTerminologyArtifactAsync(terminologyArtifact);

                return Ok(modifiedTerminologyArtifact);
            }
            catch (TerminologyArtifactValidationException terminologyArtifactValidationException)
                when (terminologyArtifactValidationException.InnerException is NotFoundTerminologyArtifactException)
            {
                return NotFound(terminologyArtifactValidationException.InnerException);
            }
            catch (TerminologyArtifactValidationException terminologyArtifactValidationException)
            {
                return BadRequest(terminologyArtifactValidationException.InnerException);
            }
            catch (TerminologyArtifactDependencyValidationException terminologyArtifactValidationException)
                when (terminologyArtifactValidationException.InnerException
                    is InvalidTerminologyArtifactReferenceException)
            {
                return FailedDependency(terminologyArtifactValidationException.InnerException);
            }
            catch (TerminologyArtifactDependencyValidationException terminologyArtifactDependencyValidationException)
                when (terminologyArtifactDependencyValidationException.InnerException
                    is AlreadyExistsTerminologyArtifactException)
            {
                return Conflict(terminologyArtifactDependencyValidationException.InnerException);
            }
            catch (TerminologyArtifactDependencyException terminologyArtifactDependencyException)
            {
                return InternalServerError(terminologyArtifactDependencyException);
            }
            catch (TerminologyArtifactServiceException terminologyArtifactServiceException)
            {
                return InternalServerError(terminologyArtifactServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.Configurations")]
        [HttpDelete("{terminologyArtifactId}")]
        public async ValueTask<ActionResult<TerminologyArtifact>> DeleteTerminologyArtifactByIdAsync(
            Guid terminologyArtifactId)
        {
            try
            {
                TerminologyArtifact deletedTerminologyArtifact =
                    await this.terminologyArtifactService.RemoveTerminologyArtifactByIdAsync(terminologyArtifactId);

                return Ok(deletedTerminologyArtifact);
            }
            catch (TerminologyArtifactValidationException terminologyArtifactValidationException)
                when (terminologyArtifactValidationException.InnerException is NotFoundTerminologyArtifactException)
            {
                return NotFound(terminologyArtifactValidationException.InnerException);
            }
            catch (TerminologyArtifactValidationException terminologyArtifactValidationException)
            {
                return BadRequest(terminologyArtifactValidationException.InnerException);
            }
            catch (TerminologyArtifactDependencyValidationException terminologyArtifactDependencyValidationException)
                when (terminologyArtifactDependencyValidationException.InnerException
                    is LockedTerminologyArtifactException)
            {
                return Locked(terminologyArtifactDependencyValidationException.InnerException);
            }
            catch (TerminologyArtifactDependencyValidationException terminologyArtifactDependencyValidationException)
            {
                return BadRequest(terminologyArtifactDependencyValidationException);
            }
            catch (TerminologyArtifactDependencyException terminologyArtifactDependencyException)
            {
                return InternalServerError(terminologyArtifactDependencyException);
            }
            catch (TerminologyArtifactServiceException terminologyArtifactServiceException)
            {
                return InternalServerError(terminologyArtifactServiceException);
            }
        }
    }
}