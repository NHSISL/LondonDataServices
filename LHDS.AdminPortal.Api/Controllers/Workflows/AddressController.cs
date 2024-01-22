// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Services.Coordinations.AddressCoordinations;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
#if RELEASE
using Microsoft.AspNetCore.Authorization;
#endif

namespace LHDS.AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("api/workflows/[controller]")]
    public class AddressController : RESTFulController
    {
        private readonly IAddressCoordinationService addressCoordinationService;

        public AddressController(IAddressCoordinationService addressCoordinationService) =>
            this.addressCoordinationService = addressCoordinationService;

        [HttpPost]
#if RELEASE
            [Authorize(Roles = "ISL.LDS.AdminApi.Administrators, ISL.LDS.AdminApi.ReadOnly")]
#endif
        public async ValueTask<ActionResult<List<Address>>> ProcessData([FromBody] byte[] data)
        {
            try
            {
                List<Address> addresses = await this.addressCoordinationService.ProcessData(data);

                return Ok(addresses);
            }
            catch (AddressCoordinationValidationException addressCoordinationValidationException)
            {
                return BadRequest(addressCoordinationValidationException.InnerException);
            }
            catch (AddressCoordinationDependencyException addressCoordinationDependencyException)
            {
                return InternalServerError(addressCoordinationDependencyException);
            }
            catch (AddressCoordinationServiceException terminologyDetailOrchestrationServiceException)
            {
                return InternalServerError(terminologyDetailOrchestrationServiceException);
            }
        }
    }
}