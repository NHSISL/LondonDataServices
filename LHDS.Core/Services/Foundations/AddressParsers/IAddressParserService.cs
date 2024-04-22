// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Services.Foundations.AddressParsers
{
    public interface IAddressParserService
    {
        ValueTask<List<Address>> ProcessCsvAsync(byte[] data, string filename);
        ValueTask<List<Address>> ProcessCsvAsync(string data, string filename);
    }
}