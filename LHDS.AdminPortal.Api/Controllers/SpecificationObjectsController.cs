// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions;
using LHDS.Core.Services.Foundations.SpecificationObjects;
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
    public class SpecificationObjectsController : RESTFulController
    {
        private readonly ISpecificationObjectService specificationObjectService;

        public SpecificationObjectsController(ISpecificationObjectService specificationObjectService) =>
            this.specificationObjectService = specificationObjectService;

        [HttpPost]
        public async ValueTask<ActionResult<SpecificationObject>> PostSpecificationObjectAsync(
            SpecificationObject specificationObject)
        {
            try
            {
                SpecificationObject addedSpecificationObject =
                    await this.specificationObjectService.AddSpecificationObjectAsync(specificationObject);

                return Created(addedSpecificationObject);
            }
            catch (SpecificationObjectValidationException specificationObjectValidationException)
            {
                return BadRequest(specificationObjectValidationException.InnerException);
            }
            catch (SpecificationObjectDependencyValidationException specificationObjectValidationException)
                when (specificationObjectValidationException.InnerException
                    is InvalidSpecificationObjectReferenceException)
            {
                return FailedDependency(specificationObjectValidationException.InnerException);
            }
            catch (SpecificationObjectDependencyValidationException specificationObjectDependencyValidationException)
                when (specificationObjectDependencyValidationException.InnerException
                    is AlreadyExistsSpecificationObjectException)
            {
                return Conflict(specificationObjectDependencyValidationException.InnerException);
            }
            catch (SpecificationObjectDependencyException specificationObjectDependencyException)
            {
                return InternalServerError(specificationObjectDependencyException);
            }
            catch (SpecificationObjectServiceException specificationObjectServiceException)
            {
                return InternalServerError(specificationObjectServiceException);
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
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, lhds.Api.SpecificationObjects, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<IQueryable<SpecificationObject>>> GetAsync()
        {
            try
            {
                IQueryable<SpecificationObject> retrievedSpecificationObjects =
                    await this.specificationObjectService.RetrieveAllSpecificationObjectsAsync();

                return Ok(retrievedSpecificationObjects);
            }
            catch (SpecificationObjectDependencyException specificationObjectDependencyException)
            {
                return InternalServerError(specificationObjectDependencyException);
            }
            catch (SpecificationObjectServiceException specificationObjectServiceException)
            {
                return InternalServerError(specificationObjectServiceException);
            }
        }

        [HttpGet("{specificationObjectId}")]
        public async ValueTask<ActionResult<SpecificationObject>> GetSpecificationObjectByIdAsync(Guid specificationObjectId)
        {
            try
            {
                SpecificationObject specificationObject = await this.specificationObjectService.RetrieveSpecificationObjectByIdAsync(specificationObjectId);

                return Ok(specificationObject);
            }
            catch (SpecificationObjectValidationException specificationObjectValidationException)
                when (specificationObjectValidationException.InnerException is NotFoundSpecificationObjectException)
            {
                return NotFound(specificationObjectValidationException.InnerException);
            }
            catch (SpecificationObjectValidationException specificationObjectValidationException)
            {
                return BadRequest(specificationObjectValidationException.InnerException);
            }
            catch (SpecificationObjectDependencyException specificationObjectDependencyException)
            {
                return InternalServerError(specificationObjectDependencyException);
            }
            catch (SpecificationObjectServiceException specificationObjectServiceException)
            {
                return InternalServerError(specificationObjectServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<SpecificationObject>> PutSpecificationObjectAsync(SpecificationObject specificationObject)
        {
            try
            {
                SpecificationObject modifiedSpecificationObject =
                    await this.specificationObjectService.ModifySpecificationObjectAsync(specificationObject);

                return Ok(modifiedSpecificationObject);
            }
            catch (SpecificationObjectValidationException specificationObjectValidationException)
                when (specificationObjectValidationException.InnerException is NotFoundSpecificationObjectException)
            {
                return NotFound(specificationObjectValidationException.InnerException);
            }
            catch (SpecificationObjectValidationException specificationObjectValidationException)
            {
                return BadRequest(specificationObjectValidationException.InnerException);
            }
            catch (SpecificationObjectDependencyValidationException specificationObjectValidationException)
                when (specificationObjectValidationException.InnerException is InvalidSpecificationObjectReferenceException)
            {
                return FailedDependency(specificationObjectValidationException.InnerException);
            }
            catch (SpecificationObjectDependencyValidationException specificationObjectDependencyValidationException)
               when (specificationObjectDependencyValidationException.InnerException is AlreadyExistsSpecificationObjectException)
            {
                return Conflict(specificationObjectDependencyValidationException.InnerException);
            }
            catch (SpecificationObjectDependencyException specificationObjectDependencyException)
            {
                return InternalServerError(specificationObjectDependencyException);
            }
            catch (SpecificationObjectServiceException specificationObjectServiceException)
            {
                return InternalServerError(specificationObjectServiceException);
            }
        }

        [HttpDelete("{specificationObjectId}")]
        public async ValueTask<ActionResult<SpecificationObject>> DeleteSpecificationObjectByIdAsync(Guid specificationObjectId)
        {
            try
            {
                SpecificationObject deletedSpecificationObject =
                    await this.specificationObjectService.RemoveSpecificationObjectByIdAsync(specificationObjectId);

                return Ok(deletedSpecificationObject);
            }
            catch (SpecificationObjectValidationException specificationObjectValidationException)
                when (specificationObjectValidationException.InnerException is NotFoundSpecificationObjectException)
            {
                return NotFound(specificationObjectValidationException.InnerException);
            }
            catch (SpecificationObjectValidationException specificationObjectValidationException)
            {
                return BadRequest(specificationObjectValidationException.InnerException);
            }
            catch (SpecificationObjectDependencyValidationException specificationObjectDependencyValidationException)
                when (specificationObjectDependencyValidationException.InnerException is LockedSpecificationObjectException)
            {
                return Locked(specificationObjectDependencyValidationException.InnerException);
            }
            catch (SpecificationObjectDependencyValidationException specificationObjectDependencyValidationException)
            {
                return BadRequest(specificationObjectDependencyValidationException);
            }
            catch (SpecificationObjectDependencyException specificationObjectDependencyException)
            {
                return InternalServerError(specificationObjectDependencyException);
            }
            catch (SpecificationObjectServiceException specificationObjectServiceException)
            {
                return InternalServerError(specificationObjectServiceException);
            }
        }
    }
}