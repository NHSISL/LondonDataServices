// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using LHDS.Core.Services.Foundations.Audits;
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
    public class AuditsController : RESTFulController
    {
        private readonly IIngestionTrackingAuditService auditService;

        public AuditsController(IIngestionTrackingAuditService auditService) =>
            this.auditService = auditService;

        [HttpPost]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.IngestionTracking")]
#endif
        public async ValueTask<ActionResult<IngestionTrackingAudit>> PostAuditAsync(IngestionTrackingAudit audit)
        {
            try
            {
                IngestionTrackingAudit addedAudit =
                    await this.auditService.AddIngestionTrackingAuditAsync(audit);

                return Created(addedAudit);
            }
            catch (IngestionTrackingAuditValidationException auditValidationException)
            {
                return BadRequest(auditValidationException.InnerException);
            }
            catch (IngestionTrackingAuditDependencyValidationException auditValidationException)
                when (auditValidationException.InnerException is InvalidIngestionTrackingAuditReferenceException)
            {
                return FailedDependency(auditValidationException.InnerException);
            }
            catch (IngestionTrackingAuditDependencyValidationException auditDependencyValidationException)
               when (auditDependencyValidationException.InnerException is AlreadyExistsAuditException)
            {
                return Conflict(auditDependencyValidationException.InnerException);
            }
            catch (IngestionTrackingAuditDependencyException auditDependencyException)
            {
                return InternalServerError(auditDependencyException);
            }
            catch (IngestionTrackingAuditServiceException auditServiceException)
            {
                return InternalServerError(auditServiceException);
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
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.IngestionTracking, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public ActionResult<IQueryable<IngestionTrackingAudit>> Get()
        {
            try
            {
                IQueryable<IngestionTrackingAudit> retrievedAudits =
                    this.auditService.RetrieveAllIngestionTrackingAudits();

                return Ok(retrievedAudits);
            }
            catch (IngestionTrackingAuditDependencyException auditDependencyException)
            {
                return InternalServerError(auditDependencyException);
            }
            catch (IngestionTrackingAuditServiceException auditServiceException)
            {
                return InternalServerError(auditServiceException);
            }
        }

        [HttpGet("{auditId}")]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.IngestionTracking, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<IngestionTrackingAudit>> GetAuditByIdAsync(Guid auditId)
        {
            try
            {
                IngestionTrackingAudit audit = await this.auditService.RetrieveIngestionTrackingAuditByIdAsync(auditId);

                return Ok(audit);
            }
            catch (IngestionTrackingAuditValidationException auditValidationException)
                when (auditValidationException.InnerException is NotFoundIngestionTrackingAuditException)
            {
                return NotFound(auditValidationException.InnerException);
            }
            catch (IngestionTrackingAuditValidationException auditValidationException)
            {
                return BadRequest(auditValidationException.InnerException);
            }
            catch (IngestionTrackingAuditDependencyException auditDependencyException)
            {
                return InternalServerError(auditDependencyException);
            }
            catch (IngestionTrackingAuditServiceException auditServiceException)
            {
                return InternalServerError(auditServiceException);
            }
        }

        [HttpPut]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.IngestionTracking")]
#endif
        public async ValueTask<ActionResult<IngestionTrackingAudit>> PutAuditAsync(IngestionTrackingAudit audit)
        {
            try
            {
                IngestionTrackingAudit modifiedAudit =
                    await this.auditService.ModifyIngestionTrackingAuditAsync(audit);

                return Ok(modifiedAudit);
            }
            catch (IngestionTrackingAuditValidationException auditValidationException)
                when (auditValidationException.InnerException is NotFoundIngestionTrackingAuditException)
            {
                return NotFound(auditValidationException.InnerException);
            }
            catch (IngestionTrackingAuditValidationException auditValidationException)
            {
                return BadRequest(auditValidationException.InnerException);
            }
            catch (IngestionTrackingAuditDependencyValidationException auditValidationException)
                when (auditValidationException.InnerException is InvalidIngestionTrackingAuditReferenceException)
            {
                return FailedDependency(auditValidationException.InnerException);
            }
            catch (IngestionTrackingAuditDependencyValidationException auditDependencyValidationException)
               when (auditDependencyValidationException.InnerException is AlreadyExistsAuditException)
            {
                return Conflict(auditDependencyValidationException.InnerException);
            }
            catch (IngestionTrackingAuditDependencyException auditDependencyException)
            {
                return InternalServerError(auditDependencyException);
            }
            catch (IngestionTrackingAuditServiceException auditServiceException)
            {
                return InternalServerError(auditServiceException);
            }
        }

        [HttpDelete("{auditId}")]
#if RELEASE
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.IngestionTracking")]
#endif
        public async ValueTask<ActionResult<IngestionTrackingAudit>> DeleteAuditByIdAsync(Guid auditId)
        {
            try
            {
                IngestionTrackingAudit deletedAudit =
                    await this.auditService.RemoveIngestionTrackingAuditByIdAsync(auditId);

                return Ok(deletedAudit);
            }
            catch (IngestionTrackingAuditValidationException auditValidationException)
                when (auditValidationException.InnerException is NotFoundIngestionTrackingAuditException)
            {
                return NotFound(auditValidationException.InnerException);
            }
            catch (IngestionTrackingAuditValidationException auditValidationException)
            {
                return BadRequest(auditValidationException.InnerException);
            }
            catch (IngestionTrackingAuditDependencyValidationException auditDependencyValidationException)
                when (auditDependencyValidationException.InnerException is LockedIngestionTrackingAuditException)
            {
                return Locked(auditDependencyValidationException.InnerException);
            }
            catch (IngestionTrackingAuditDependencyValidationException auditDependencyValidationException)
            {
                return BadRequest(auditDependencyValidationException);
            }
            catch (IngestionTrackingAuditDependencyException auditDependencyException)
            {
                return InternalServerError(auditDependencyException);
            }
            catch (IngestionTrackingAuditServiceException auditServiceException)
            {
                return InternalServerError(auditServiceException);
            }
        }
    }
}