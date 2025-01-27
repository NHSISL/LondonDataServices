// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Foundations.PdsAudits.Exceptions;
using LHDS.Core.Services.Foundations.PdsAudits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdsAuditsController : RESTFulController
    {
        private readonly IPdsAuditService pdsAuditService;

        public PdsAuditsController(IPdsAuditService pdsAuditService) =>
            this.pdsAuditService = pdsAuditService;

        [HttpPost]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.Pds")]
#endif
        public async ValueTask<ActionResult<PdsAudit>> PostPdsAuditAsync(PdsAudit pdsAudit)
        {
            try
            {
                PdsAudit addedPdsAudit =
                    await this.pdsAuditService.AddPdsAuditAsync(pdsAudit);

                return Created(addedPdsAudit);
            }
            catch (PdsAuditValidationException pdsAuditValidationException)
            {
                return BadRequest(pdsAuditValidationException.InnerException);
            }
            catch (PdsAuditDependencyValidationException pdsAuditValidationException)
                when (pdsAuditValidationException.InnerException is InvalidPdsAuditReferenceException)
            {
                return FailedDependency(pdsAuditValidationException.InnerException);
            }
            catch (PdsAuditDependencyValidationException pdsAuditDependencyValidationException)
               when (pdsAuditDependencyValidationException.InnerException is AlreadyExistsPdsAuditException)
            {
                return Conflict(pdsAuditDependencyValidationException.InnerException);
            }
            catch (PdsAuditDependencyException pdsAuditDependencyException)
            {
                return InternalServerError(pdsAuditDependencyException);
            }
            catch (PdsAuditServiceException pdsAuditServiceException)
            {
                return InternalServerError(pdsAuditServiceException);
            }
        }

        [HttpGet]
#if !DEBUG
        [EnableQuery(PageSize = 50)]
#endif
#if DEBUG
        [EnableQuery(PageSize = 5000)]
#endif
        [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.Pds, ISL.LDS.AdminSpa.ReadOnly")]
        public async ValueTask<ActionResult<IQueryable<PdsAudit>>> Get()
        {
            try
            {
                IQueryable<PdsAudit> retrievedPdsAudits =
                    await this.pdsAuditService.RetrieveAllPdsAuditsAsync();

                return Ok(retrievedPdsAudits);
            }
            catch (PdsAuditDependencyException pdsAuditDependencyException)
            {
                return InternalServerError(pdsAuditDependencyException);
            }
            catch (PdsAuditServiceException pdsAuditServiceException)
            {
                return InternalServerError(pdsAuditServiceException);
            }
        }

        [HttpGet("{pdsAuditId}")]
        [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.Pds, ISL.LDS.AdminSpa.ReadOnly")]
        public async ValueTask<ActionResult<PdsAudit>> GetPdsAuditByIdAsync(Guid pdsAuditId)
        {
            try
            {
                PdsAudit pdsAudit =
                    await this.pdsAuditService.RetrievePdsAuditByIdAsync(pdsAuditId);

                return Ok(pdsAudit);
            }
            catch (PdsAuditValidationException pdsAuditValidationException)
                when (pdsAuditValidationException.InnerException is NotFoundPdsAuditException)
            {
                return NotFound(pdsAuditValidationException.InnerException);
            }
            catch (PdsAuditValidationException pdsAuditValidationException)
            {
                return BadRequest(pdsAuditValidationException.InnerException);
            }
            catch (PdsAuditDependencyException pdsAuditDependencyException)
            {
                return InternalServerError(pdsAuditDependencyException);
            }
            catch (PdsAuditServiceException pdsAuditServiceException)
            {
                return InternalServerError(pdsAuditServiceException);
            }
        }

        [HttpPut]
        [Authorize(Roles = "ISL.LDS.AdminSpa.Administrators, ISL.LDS.AdminSpa.Pds")]
        public async ValueTask<ActionResult<PdsAudit>> PutPdsAuditAsync(PdsAudit pdsAudit)
        {
            try
            {
                PdsAudit modifiedPdsAudit =
                    await this.pdsAuditService.ModifyPdsAuditAsync(pdsAudit);

                return Ok(modifiedPdsAudit);
            }
            catch (PdsAuditValidationException pdsAuditValidationException)
                when (pdsAuditValidationException.InnerException is NotFoundPdsAuditException)
            {
                return NotFound(pdsAuditValidationException.InnerException);
            }
            catch (PdsAuditValidationException pdsAuditValidationException)
            {
                return BadRequest(pdsAuditValidationException.InnerException);
            }
            catch (PdsAuditDependencyValidationException pdsAuditValidationException)
                when (pdsAuditValidationException.InnerException is InvalidPdsAuditReferenceException)
            {
                return FailedDependency(pdsAuditValidationException.InnerException);
            }
            catch (PdsAuditDependencyValidationException pdsAuditDependencyValidationException)
               when (pdsAuditDependencyValidationException.InnerException is AlreadyExistsPdsAuditException)
            {
                return Conflict(pdsAuditDependencyValidationException.InnerException);
            }
            catch (PdsAuditDependencyException pdsAuditDependencyException)
            {
                return InternalServerError(pdsAuditDependencyException);
            }
            catch (PdsAuditServiceException pdsAuditServiceException)
            {
                return InternalServerError(pdsAuditServiceException);
            }
        }

        [HttpDelete("{pdsAuditId}")]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.Pds")]
#endif
        public async ValueTask<ActionResult<PdsAudit>> DeletePdsAuditByIdAsync(Guid pdsAuditId)
        {
            try
            {
                PdsAudit deletedPdsAudit =
                    await this.pdsAuditService.RemovePdsAuditByIdAsync(pdsAuditId);

                return Ok(deletedPdsAudit);
            }
            catch (PdsAuditValidationException pdsAuditValidationException)
                when (pdsAuditValidationException.InnerException is NotFoundPdsAuditException)
            {
                return NotFound(pdsAuditValidationException.InnerException);
            }
            catch (PdsAuditValidationException pdsAuditValidationException)
            {
                return BadRequest(pdsAuditValidationException.InnerException);
            }
            catch (PdsAuditDependencyValidationException pdsAuditDependencyValidationException)
                when (pdsAuditDependencyValidationException.InnerException is LockedPdsAuditException)
            {
                return Locked(pdsAuditDependencyValidationException.InnerException);
            }
            catch (PdsAuditDependencyValidationException pdsAuditDependencyValidationException)
            {
                return BadRequest(pdsAuditDependencyValidationException);
            }
            catch (PdsAuditDependencyException pdsAuditDependencyException)
            {
                return InternalServerError(pdsAuditDependencyException);
            }
            catch (PdsAuditServiceException pdsAuditServiceException)
            {
                return InternalServerError(pdsAuditServiceException);
            }
        }
    }
}