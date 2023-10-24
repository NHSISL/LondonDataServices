// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.AddressParsers.Exceptions;

namespace LHDS.Core.Services.Foundations.AddressParsers
{
    public partial class AddressParserService
    {
        private void ValidateAddressParserOnProcessCSV(byte[] data)
        {
            ValidateAddressParserIsNotNull(data);
        }

        private static void ValidateAddressParserIsNotNull(byte[] data)
        {
            if (data is null)
            {
                throw new InvalidArgumentAddressParserException(message: "Address parser is null.");
            }
        }
    }
}