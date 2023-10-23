// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Services.Foundations.AddressParsers
{
    public interface IAddressParserService
    {
        List<Address> ProcessCSV(byte[] data);
    }
}