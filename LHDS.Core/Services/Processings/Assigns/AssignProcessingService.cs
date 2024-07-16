// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Services.Foundations.Assigns;

namespace LHDS.Core.Services.Processings.Assigns
{
    public partial class AssignProcessingService : IAssignProcessingService
    {
        private readonly IAssignService assignService;
        private readonly ILoggingBroker loggingBroker;

        public AssignProcessingService(
            IAssignService assignService,
            ILoggingBroker loggingBroker)
        {
            this.assignService = assignService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<AssignAddress> MatchAddressAsync(string address) =>
            TryCatch(async () =>
            {
                ValidateOnMatchAddress(address);

                return await this.assignService.MatchAddressAsync(address);
            });
    }
}
