// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AssignAddresses;

namespace LHDS.Core.Services.Foundations.Assigns
{
    public interface IAssignService
    {
        ValueTask<AssignAddress> MatchAddressAsync(string address);
    }
}
