// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Securities;

namespace LHDS.Core.Brokers.Securities
{
    public interface ISecurityBroker
    {
        ValueTask<EntraUser> GetCurrentUser();
        ValueTask<bool> IsCurrentUserAuthenticated();
        ValueTask<bool> IsInRole(string roleName);
        ValueTask<bool> HasClaimType(string claimType, string claimValue);
        ValueTask<bool> HasClaimType(string claimType);
    }
}
