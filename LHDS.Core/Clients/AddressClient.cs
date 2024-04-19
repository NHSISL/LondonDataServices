// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Clients.AddressClient.Exceptions;
using LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Coordinations.AddressCoordinations;
using Xeptions;

namespace LHDS.Core.Clients
{
    public class AddressClient : IAddressClient
    {
        private readonly IAddressCoordinationService addressCoordinationService;

        public AddressClient(IAddressCoordinationService addressCoordinationService)
        {
            this.addressCoordinationService = addressCoordinationService;
        }

        public async ValueTask<List<Address>> LoadAddressData(byte[] data, string filename)
        {
            try
            {
                return await this.addressCoordinationService.LoadAddressData(data, filename);
            }
            catch (AddressCoordinationValidationException addressCoordinationValidationException)
            {
                throw new AddressClientValidationException(
                    message: "Address client validation error occurred, fix errors and try again.",
                    innerException: addressCoordinationValidationException.InnerException as Xeption);
            }
            catch (AddressCoordinationDependencyValidationException
                addressCoordinationDependencyValidationException)
            {
                throw new AddressClientValidationException(
                    message: "Address client validation error occurred, fix errors and try again.",
                    innerException: addressCoordinationDependencyValidationException.InnerException as Xeption);
            }
            catch (AddressCoordinationDependencyException
                addressCoordinationDependencyException)
            {
                throw new AddressClientDependencyException(
                    message: "Address client dependency error occurred, contact support.",
                    innerException: addressCoordinationDependencyException.InnerException as Xeption);
            }
            catch (AddressCoordinationServiceException
                addressCoordinationServiceException)
            {
                throw new AddressClientServiceException(
                    message: "Address client service error occurred, fix errors and try again.",
                    addressCoordinationServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask<List<ResolvedAddress>> MatchPatientAddressData(byte[] data, string filename)
        {
            try
            {
                return await this.addressCoordinationService.MatchAddressDataAsync(data, filename);
            }
            catch (AddressCoordinationValidationException addressCoordinationValidationException)
            {
                throw new AddressClientValidationException(
                    message: "Address client validation error occurred, fix errors and try again.",
                    innerException: addressCoordinationValidationException.InnerException as Xeption);
            }
            catch (AddressCoordinationDependencyValidationException
                addressCoordinationDependencyValidationException)
            {
                throw new AddressClientValidationException(
                    message: "Address client validation error occurred, fix errors and try again.",
                    innerException: addressCoordinationDependencyValidationException.InnerException as Xeption);
            }
            catch (AddressCoordinationDependencyException
                addressCoordinationDependencyException)
            {
                throw new AddressClientDependencyException(
                    message: "Address client dependency error occurred, contact support.",
                    innerException: addressCoordinationDependencyException.InnerException as Xeption);
            }
            catch (AddressCoordinationServiceException
                addressCoordinationServiceException)
            {
                throw new AddressClientServiceException(
                    message: "Address client service error occurred, fix errors and try again.",
                    addressCoordinationServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask<List<Address>> ProcessResolvedAddressData()
        {
            try
            {
                return await this.addressCoordinationService.UploadResolvedAddresses();
            }
            catch (AddressCoordinationValidationException addressCoordinationValidationException)
            {
                throw new AddressClientValidationException(
                    message: "Address client validation error occurred, fix errors and try again.",
                    innerException: addressCoordinationValidationException.InnerException as Xeption);
            }
            catch (AddressCoordinationDependencyValidationException
                addressCoordinationDependencyValidationException)
            {
                throw new AddressClientValidationException(
                    message: "Address client validation error occurred, fix errors and try again.",
                    innerException: addressCoordinationDependencyValidationException.InnerException as Xeption);
            }
            catch (AddressCoordinationDependencyException
                addressCoordinationDependencyException)
            {
                throw new AddressClientDependencyException(
                    message: "Address client dependency error occurred, contact support.",
                    innerException: addressCoordinationDependencyException.InnerException as Xeption);
            }
            catch (AddressCoordinationServiceException
                addressCoordinationServiceException)
            {
                throw new AddressClientServiceException(
                    message: "Address client service error occurred, fix errors and try again.",
                    addressCoordinationServiceException.InnerException as Xeption);
            }
        }
    }
}
