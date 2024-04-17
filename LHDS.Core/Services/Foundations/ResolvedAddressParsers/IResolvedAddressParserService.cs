// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;

namespace LHDS.Core.Services.Foundations.ResolvedAddressParsers
{
    public interface IResolvedAddressParserService
    {
        ValueTask<List<ResolvedAddress>> ProcessCsvAsync(byte[] data);
        ValueTask<List<ResolvedAddress>> ProcessCsvAsync(string data);
    }
}