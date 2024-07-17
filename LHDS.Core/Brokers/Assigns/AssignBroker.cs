// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AssignAddresses;

namespace LHDS.Core.Brokers.Assigns
{
    public class AssignBroker : IAssignBroker
    {
        public ValueTask<AssignAddress> MatchAddressAsync(string address) =>
            throw new System.NotImplementedException();
    }
}
