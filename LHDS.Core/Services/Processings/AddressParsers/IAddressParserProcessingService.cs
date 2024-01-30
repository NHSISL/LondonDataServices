// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Services.Processings.AddressParsers
{
    public interface IAddressParserProcessingService
    {
        ValueTask<List<Address>> ProcessCsvAsync(byte[] data);
        ValueTask<List<Address>> ProcessCsvAsync(string data);
    }
}
