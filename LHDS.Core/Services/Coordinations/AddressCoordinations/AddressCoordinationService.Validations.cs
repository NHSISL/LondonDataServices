// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationService
    {
        private void ValidateDataOnProcessData(byte[] data)
        {
            ValidateDataIsNotNull(data);
        }

        private static void ValidateDataIsNotNull(byte[] data)
        {
            if (data is null)
            {
                throw new InvalidArgumentAddressCoordinationException(
                    message: "Invalid address coordination argument, please correct the errors and try again.");
            }
        }

        private void ValidateAddressListIsNotNull(List<Address> addresses)
        {
            if (addresses == null)
            {
                throw new NullAddressCoordinationListException(
                    message: "Address list is null, please correct the errors and try again.");
            }
        }
    }
}