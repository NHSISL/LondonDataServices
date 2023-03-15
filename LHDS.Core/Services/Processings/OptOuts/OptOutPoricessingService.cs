// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Services.Foundations.OptOuts;

namespace LHDS.Core.Services.Processings.OptOuts
{
    public class OptOutProcessingService : IOptOutProcessingService
    {
        private readonly IOptOutService optOutService;
        private readonly ILoggingBroker loggingBroker;

        public OptOutProcessingService(IOptOutService optOutService, ILoggingBroker loggingBroker)
        {
            this.optOutService = optOutService;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<OptOut> RetrieveOrAddOptOutAsync(OptOut optOut)
        {
            OptOut maybeOptOut = await this.optOutService.RetrieveOptOutByIdAsync(optOut.Id);

            if (maybeOptOut == null)
            {
                return await this.optOutService.AddOptOutAsync(optOut);
            }

            return maybeOptOut;
        }
    }
}

