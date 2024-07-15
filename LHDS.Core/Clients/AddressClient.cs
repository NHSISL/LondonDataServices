// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Models.Clients.AddressClient.Exceptions;
using LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions;
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

        public async ValueTask LoadAddressDataAsync(Stream data, string filename)
        {
            try
            {
                await this.addressCoordinationService.LoadAddressDataAsync(data, filename);
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
                    message: "Address client dependency error occurred, please contact support.",
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

        public async ValueTask SyncAddressesWithAssign()
        {
            try
            {
                await this.addressCoordinationService.SyncAddressesWithAssignAsync();
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
                    message: "Address client dependency error occurred, please contact support.",
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

        public async ValueTask LoadAddressesToResolveAsync(Stream data, string filename)
        {
            try
            {
                await this.addressCoordinationService.LoadAddressesToResolveAsync(data, filename);
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
                    message: "Address client dependency error occurred, please contact support.",
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

        public async ValueTask MatchAddressDataAsync()
        {
            try
            {
                await this.addressCoordinationService.MatchAddressDataAsync();
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
                    message: "Address client dependency error occurred, please contact support.",
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

        public async ValueTask<List<Guid>> ExportResolvedAddressesAsync()
        {
            try
            {
                return await this.addressCoordinationService.ExportResolvedAddressesAsync();
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
                    message: "Address client dependency error occurred, please contact support.",
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
