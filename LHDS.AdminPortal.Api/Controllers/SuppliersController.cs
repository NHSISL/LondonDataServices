// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Foundations.Suppliers.Exceptions;
using LHDS.Core.Services.Foundations.Suppliers;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : RESTFulController
    {
        private readonly ISupplierService supplierService;

        public SuppliersController(ISupplierService supplierService) =>
            this.supplierService = supplierService;

        [HttpPost]
#if RELEASE
        [Authorize(Roles = "lhdsApi.Administrators, lhdsApi.Suppliers")]
#endif
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
#if RELEASE
        [Authorize(Roles = "lhdsApi.Administrators, lhdsApi.Suppliers, lhdsApi.ReadOnly")]
#endif
        public ActionResult<IQueryable<Supplier>> GetAllSuppliers()
        {
            try
            {
                IQueryable<Supplier> retrievedSuppliers =
                    this.supplierService.RetrieveAllSuppliers();

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
#if RELEASE
        [Authorize(Roles = "lhdsApi.Administrators, lhdsApi.Suppliers, lhdsApi.ReadOnly")]
#endif
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
#if RELEASE
        [Authorize(Roles = "lhdsApi.Administrators, lhdsApi.Suppliers")]
#endif
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
#if RELEASE
        [Authorize(Roles = "lhdsApi.Administrators, lhdsApi.Suppliers")]
#endif
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