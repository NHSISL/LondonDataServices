// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AssignAddresses;

namespace LHDS.Core.Brokers.Assigns
{
    public interface IAssignBroker
    {
        ValueTask<AssignAddress> MatchAddressAsync(string address);
    }
}
