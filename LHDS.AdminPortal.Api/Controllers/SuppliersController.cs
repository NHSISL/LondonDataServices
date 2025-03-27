// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Foundations.Suppliers.Exceptions;
using LHDS.Core.Services.Foundations.Suppliers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers
{
    [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.Configurations")]
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : RESTFulController
    {
        private readonly ISupplierService supplierService;

        public SuppliersController(ISupplierService supplierService) =>
            this.supplierService = supplierService;

        [HttpPost]
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.Suppliers")]
        public async ValueTask<ActionResult<Supplier>> PostSupplierAsync(Supplier supplier)
        {
            try
            {
                Supplier addedSupplier =
                    await this.supplierService.AddSupplierAsync(supplier);

                return Created(addedSupplier);
            }
            catch (SupplierValidationException supplierValidationException)
            {
                return BadRequest(supplierValidationException.InnerException);
            }
            catch (SupplierDependencyValidationException supplierValidationException)
                when (supplierValidationException.InnerException is InvalidSupplierReferenceException)
            {
                return FailedDependency(supplierValidationException.InnerException);
            }
            catch (SupplierDependencyValidationException supplierDependencyValidationException)
               when (supplierDependencyValidationException.InnerException is AlreadyExistsSupplierException)
            {
                return Conflict(supplierDependencyValidationException.InnerException);
            }
            catch (SupplierDependencyException supplierDependencyException)
            {
                return InternalServerError(supplierDependencyException);
            }
            catch (SupplierServiceException supplierServiceException)
            {
                return InternalServerError(supplierServiceException);
            }
        }

        [HttpGet]
#if !DEBUG
        [EnableQuery(PageSize = 50)]
#endif
#if DEBUG
        [EnableQuery(PageSize = 5000)]
#endif
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.Suppliers, ISL.LDS.AdminApi.ReadOnly")]
        public async ValueTask<ActionResult<IQueryable<Supplier>>> Get()
        {
            try
            {
                IQueryable<Supplier> retrievedSuppliers =
                    await this.supplierService.RetrieveAllSuppliersAsync();

                return Ok(retrievedSuppliers);
            }
            catch (SupplierDependencyException supplierDependencyException)
            {
                return InternalServerError(supplierDependencyException);
            }
            catch (SupplierServiceException supplierServiceException)
            {
                return InternalServerError(supplierServiceException);
            }
        }

        [HttpGet("{supplierId}")]
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.Suppliers, ISL.LDS.AdminApi.ReadOnly")]
        public async ValueTask<ActionResult<Supplier>> GetSupplierByIdAsync(Guid supplierId)
        {
            try
            {
                Supplier supplier = await this.supplierService.RetrieveSupplierByIdAsync(supplierId);

                return Ok(supplier);
            }
            catch (SupplierValidationException supplierValidationException)
                when (supplierValidationException.InnerException is NotFoundSupplierException)
            {
                return NotFound(supplierValidationException.InnerException);
            }
            catch (SupplierValidationException supplierValidationException)
            {
                return BadRequest(supplierValidationException.InnerException);
            }
            catch (SupplierDependencyException supplierDependencyException)
            {
                return InternalServerError(supplierDependencyException);
            }
            catch (SupplierServiceException supplierServiceException)
            {
                return InternalServerError(supplierServiceException);
            }
        }

        [HttpPut]
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.Suppliers")]
        public async ValueTask<ActionResult<Supplier>> PutSupplierAsync(Supplier supplier)
        {
            try
            {
                Supplier modifiedSupplier =
                    await this.supplierService.ModifySupplierAsync(supplier);

                return Ok(modifiedSupplier);
            }
            catch (SupplierValidationException supplierValidationException)
                when (supplierValidationException.InnerException is NotFoundSupplierException)
            {
                return NotFound(supplierValidationException.InnerException);
            }
            catch (SupplierValidationException supplierValidationException)
            {
                return BadRequest(supplierValidationException.InnerException);
            }
            catch (SupplierDependencyValidationException supplierValidationException)
                when (supplierValidationException.InnerException is InvalidSupplierReferenceException)
            {
                return FailedDependency(supplierValidationException.InnerException);
            }
            catch (SupplierDependencyValidationException supplierDependencyValidationException)
               when (supplierDependencyValidationException.InnerException is AlreadyExistsSupplierException)
            {
                return Conflict(supplierDependencyValidationException.InnerException);
            }
            catch (SupplierDependencyException supplierDependencyException)
            {
                return InternalServerError(supplierDependencyException);
            }
            catch (SupplierServiceException supplierServiceException)
            {
                return InternalServerError(supplierServiceException);
            }
        }

        [HttpDelete("{supplierId}")]
        [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.Suppliers")]
        public async ValueTask<ActionResult<Supplier>> DeleteSupplierByIdAsync(Guid supplierId)
        {
            try
            {
                Supplier deletedSupplier =
                    await this.supplierService.RemoveSupplierByIdAsync(supplierId);

                return Ok(deletedSupplier);
            }
            catch (SupplierValidationException supplierValidationException)
                when (supplierValidationException.InnerException is NotFoundSupplierException)
            {
                return NotFound(supplierValidationException.InnerException);
            }
            catch (SupplierValidationException supplierValidationException)
            {
                return BadRequest(supplierValidationException.InnerException);
            }
            catch (SupplierDependencyValidationException supplierDependencyValidationException)
                when (supplierDependencyValidationException.InnerException is LockedSupplierException)
            {
                return Locked(supplierDependencyValidationException.InnerException);
            }
            catch (SupplierDependencyValidationException supplierDependencyValidationException)
            {
                return BadRequest(supplierDependencyValidationException);
            }
            catch (SupplierDependencyException supplierDependencyException)
            {
                return InternalServerError(supplierDependencyException);
            }
            catch (SupplierServiceException supplierServiceException)
            {
                return InternalServerError(supplierServiceException);
            }
        }
    }
}