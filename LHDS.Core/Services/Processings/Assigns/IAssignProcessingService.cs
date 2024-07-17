// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AssignAddresses;

namespace LHDS.Core.Services.Processings.Assigns
{
    public interface IAssignProcessingService
    {
        ValueTask<AssignAddress> MatchAddressAsync(string address);
    }
}
