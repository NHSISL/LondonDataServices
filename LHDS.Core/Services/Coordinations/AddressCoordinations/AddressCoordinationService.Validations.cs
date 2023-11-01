// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions;

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
    }
}