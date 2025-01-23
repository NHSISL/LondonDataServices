// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DataSetSpecificationsController : RESTFulController
    {
        private readonly IDataSetSpecificationService dataSetSpecificationService;

        public DataSetSpecificationsController(IDataSetSpecificationService dataSetSpecificationService) =>
            this.dataSetSpecificationService = dataSetSpecificationService;

        [Authorize(Roles = "ISL.LDS.AdminSpa.Configurations,ISL.LDS.AdminSpa.Administrators")]
        [HttpPost]
        public async ValueTask<ActionResult<DataSetSpecification>>
            PostDataSetSpecificationAsync(DataSetSpecification dataSetSpecification)
        {
            try
            {
                DataSetSpecification addedDataSetSpecification =
                    await this.dataSetSpecificationService.AddDataSetSpecificationAsync(dataSetSpecification);

                return Created(addedDataSetSpecification);
            }
            catch (DataSetSpecificationValidationException dataSetSpecificationValidationException)
            {
                return BadRequest(dataSetSpecificationValidationException.InnerException);
            }
            catch (DataSetSpecificationDependencyValidationException dataSetSpecificationValidationException)
                when (dataSetSpecificationValidationException.InnerException is InvalidDataSetSpecificationReferenceException)
            {
                return FailedDependency(dataSetSpecificationValidationException.InnerException);
            }
            catch (DataSetSpecificationDependencyValidationException dataSetSpecificationDependencyValidationException)
               when (dataSetSpecificationDependencyValidationException.InnerException is AlreadyExistsDataSetSpecificationException)
            {
                return Conflict(dataSetSpecificationDependencyValidationException.InnerException);
            }
            catch (DataSetSpecificationDependencyException dataSetSpecificationDependencyException)
            {
                return InternalServerError(dataSetSpecificationDependencyException);
            }
            catch (DataSetSpecificationServiceException dataSetSpecificationServiceException)
            {
                return InternalServerError(dataSetSpecificationServiceException);
            }
        }

        [HttpGet]
#if !DEBUG
        [EnableQuery(PageSize = 50)]
#endif
#if DEBUG
        [EnableQuery(PageSize = 5000)]
#endif
        [Authorize(Roles = "ISL.LDS.AdminSpa.Configurations,ISL.LDS.AdminSpa.Administrators,ISL.LDS.AdminSpa.ReadOnly")]
        public async ValueTask<ActionResult<IQueryable<DataSetSpecification>>> Get()
        {
            try
            {
                IQueryable<DataSetSpecification> retrievedDataSetSpecifications =
                    await this.dataSetSpecificationService.RetrieveAllDataSetSpecificationsAsync();

                return Ok(retrievedDataSetSpecifications);
            }
            catch (DataSetSpecificationDependencyException dataSetSpecificationDependencyException)
            {
                return InternalServerError(dataSetSpecificationDependencyException);
            }
            catch (DataSetSpecificationServiceException dataSetSpecificationServiceException)
            {
                return InternalServerError(dataSetSpecificationServiceException);
            }
        }

        [Authorize(Roles = "ISL.LDS.AdminSpa.Configurations,ISL.LDS.AdminSpa.Administrators,ISL.LDS.AdminSpa.ReadOnly")]
        [HttpGet("{dataSetSpecificationId}")]
        public async ValueTask<ActionResult<DataSetSpecification>>
            GetDataSetSpecificationByIdAsync(Guid dataSetSpecificationId)
        {
            try
            {
                DataSetSpecification dataSetSpecification = await this.dataSetSpecificationService
                    .RetrieveDataSetSpecificationByIdAsync(dataSetSpecificationId);

                return Ok(dataSetSpecification);
            }
            catch (DataSetSpecificationValidationException dataSetSpecificationValidationException)
                when (dataSetSpecificationValidationException.InnerException is NotFoundDataSetSpecificationException)
            {
                return NotFound(dataSetSpecificationValidationException.InnerException);
            }
            catch (DataSetSpecificationValidationException dataSetSpecificationValidationException)
            {
                return BadRequest(dataSetSpecificationValidationException.InnerException);
            }
            catch (DataSetSpecificationDependencyException dataSetSpecificationDependencyException)
            {
                return InternalServerError(dataSetSpecificationDependencyException);
            }
            catch (DataSetSpecificationServiceException dataSetSpecificationServiceException)
            {
                return InternalServerError(dataSetSpecificationServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<DataSetSpecification>> PutDataSetSpecificationAsync(DataSetSpecification dataSetSpecification)
        {
            try
            {
                DataSetSpecification modifiedDataSetSpecification =
                    await this.dataSetSpecificationService.ModifyDataSetSpecificationAsync(dataSetSpecification);

                return Ok(modifiedDataSetSpecification);
            }
            catch (DataSetSpecificationValidationException dataSetSpecificationValidationException)
                when (dataSetSpecificationValidationException.InnerException is NotFoundDataSetSpecificationException)
            {
                return NotFound(dataSetSpecificationValidationException.InnerException);
            }
            catch (DataSetSpecificationValidationException dataSetSpecificationValidationException)
            {
                return BadRequest(dataSetSpecificationValidationException.InnerException);
            }
            catch (DataSetSpecificationDependencyValidationException dataSetSpecificationValidationException)
                when (dataSetSpecificationValidationException.InnerException is InvalidDataSetSpecificationReferenceException)
            {
                return FailedDependency(dataSetSpecificationValidationException.InnerException);
            }
            catch (DataSetSpecificationDependencyValidationException dataSetSpecificationDependencyValidationException)
               when (dataSetSpecificationDependencyValidationException.InnerException is AlreadyExistsDataSetSpecificationException)
            {
                return Conflict(dataSetSpecificationDependencyValidationException.InnerException);
            }
            catch (DataSetSpecificationDependencyException dataSetSpecificationDependencyException)
            {
                return InternalServerError(dataSetSpecificationDependencyException);
            }
            catch (DataSetSpecificationServiceException dataSetSpecificationServiceException)
            {
                return InternalServerError(dataSetSpecificationServiceException);
            }
        }

        [HttpDelete("{dataSetSpecificationId}")]
        public async ValueTask<ActionResult<DataSetSpecification>> DeleteDataSetSpecificationByIdAsync(Guid dataSetSpecificationId)
        {
            try
            {
                DataSetSpecification deletedDataSetSpecification =
                    await this.dataSetSpecificationService.RemoveDataSetSpecificationByIdAsync(dataSetSpecificationId);

                return Ok(deletedDataSetSpecification);
            }
            catch (DataSetSpecificationValidationException dataSetSpecificationValidationException)
                when (dataSetSpecificationValidationException.InnerException is NotFoundDataSetSpecificationException)
            {
                return NotFound(dataSetSpecificationValidationException.InnerException);
            }
            catch (DataSetSpecificationValidationException dataSetSpecificationValidationException)
            {
                return BadRequest(dataSetSpecificationValidationException.InnerException);
            }
            catch (DataSetSpecificationDependencyValidationException dataSetSpecificationDependencyValidationException)
                when (dataSetSpecificationDependencyValidationException.InnerException is LockedDataSetSpecificationException)
            {
                return Locked(dataSetSpecificationDependencyValidationException.InnerException);
            }
            catch (DataSetSpecificationDependencyValidationException dataSetSpecificationDependencyValidationException)
            {
                return BadRequest(dataSetSpecificationDependencyValidationException);
            }
            catch (DataSetSpecificationDependencyException dataSetSpecificationDependencyException)
            {
                return InternalServerError(dataSetSpecificationDependencyException);
            }
            catch (DataSetSpecificationServiceException dataSetSpecificationServiceException)
            {
                return InternalServerError(dataSetSpecificationServiceException);
            }
        }
    }
}