// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Services.Foundations.TerminologyPolls;

namespace LHDS.Core.Services.Processings.TerminologyPolls
{
    public partial class TerminologyPollProcessingService : ITerminologyPollProcessingService
    {
        private readonly ITerminologyPollService terminologyPollService;
        private readonly ILoggingBroker loggingBroker;

        public TerminologyPollProcessingService(
            ITerminologyPollService terminologyPollService,
            ILoggingBroker loggingBroker)
        {
            this.terminologyPollService = terminologyPollService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<TerminologyPoll> AddTerminologyPollAsync(TerminologyPoll terminologyPoll) =>
            TryCatch(async () =>
            {
                ValidateTerminologyPollOnAdd(terminologyPoll);

                return await this.terminologyPollService.AddTerminologyPollAsync(terminologyPoll);
            });

        public IQueryable<TerminologyPoll> RetrieveAllTerminologyPolls() =>
            throw new System.NotImplementedException();
    }
}