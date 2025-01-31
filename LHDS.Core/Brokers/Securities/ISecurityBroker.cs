// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Securities;

namespace LHDS.Core.Brokers.Securities
{
    public interface ISecurityBroker
    {
        ValueTask<EntraUser> GetCurrentUserAsync();
        ValueTask<bool> IsCurrentUserAuthenticatedAsync();
        ValueTask<bool> IsInRoleAsync(string roleName);
        ValueTask<bool> HasClaimTypeAsync(string claimType, string claimValue);
        ValueTask<bool> HasClaimTypeAsync(string claimType);
    }
}
