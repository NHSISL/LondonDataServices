// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Brokers.Assigns;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.AssignAddresses;

namespace LHDS.Core.Services.Foundations.Assigns
{
    internal class AssignService : IAssignService
    {
        private readonly IAssignBroker assignBroker;
        private readonly ILoggingBroker loggingBroker;

        public AssignService(
            IAssignBroker assignBroker,
            ILoggingBroker loggingBroker)
        {
            this.assignBroker = assignBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<AssignAddress> MatchAddressAsync(string address) =>
            await this.MatchAddressAsync(address);
    }
}
