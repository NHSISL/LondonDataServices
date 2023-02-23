using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using LHDS.AdminPortal.Api.Models.Audits;
using LHDS.AdminPortal.Api.Models.Audits.Exceptions;
using LHDS.AdminPortal.Api.Services.Foundations.Audits;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditsController : RESTFulController
    {
        private readonly IAuditService auditService;

        public AuditsController(IAuditService auditService) =>
            this.auditService = auditService;

        [HttpPost]
        public async ValueTask<ActionResult<Audit>> PostAuditAsync(Audit audit)
        {
            try
            {
                Audit addedAudit =
                    await this.auditService.AddAuditAsync(audit);

                return Created(addedAudit);
            }
            catch (AuditValidationException auditValidationException)
            {
                return BadRequest(auditValidationException.InnerException);
            }
            catch (AuditDependencyValidationException auditValidationException)
                when (auditValidationException.InnerException is InvalidAuditReferenceException)
            {
                return FailedDependency(auditValidationException.InnerException);
            }
            catch (AuditDependencyValidationException auditDependencyValidationException)
               when (auditDependencyValidationException.InnerException is AlreadyExistsAuditException)
            {
                return Conflict(auditDependencyValidationException.InnerException);
            }
            catch (AuditDependencyException auditDependencyException)
            {
                return InternalServerError(auditDependencyException);
            }
            catch (AuditServiceException auditServiceException)
            {
                return InternalServerError(auditServiceException);
            }
        }
    }
}