// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Services.Foundations.TerminologyPolls;

namespace LHDS.Core.Services.Processings.TerminologyPolls
{
    public partial class TerminologyPollProcessingService : ITerminologyPollProcessingService
    {
        private readonly ITerminologyPollService terminologyPollService;
        private readonly IIdentifierBroker identifierBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public TerminologyPollProcessingService(
            ITerminologyPollService terminologyPollService,
            IIdentifierBroker identifierBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.terminologyPollService = terminologyPollService;
            this.identifierBroker = identifierBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<TerminologyPoll> AddTerminologyPollAsync(TerminologyPoll terminologyPoll) =>
            TryCatch(async () =>
            {
                ValidateTerminologyPollIsNotNull(terminologyPoll);

                return await this.terminologyPollService.AddTerminologyPollAsync(terminologyPoll);
            });

        public IQueryable<TerminologyPoll> RetrieveAllTerminologyPolls() =>
            TryCatch(() =>
            {
                return this.terminologyPollService.RetrieveAllTerminologyPolls();
            });

        public ValueTask<TerminologyPoll> RetrieveTerminologyPollByIdAsync(Guid terminologyPollId) =>
            TryCatch(async () =>
            {
                ValidateTerminologyPollId(terminologyPollId);

                return await this.terminologyPollService.RetrieveTerminologyPollByIdAsync(terminologyPollId);
            });

        public ValueTask<TerminologyPoll> ModifyTerminologyPollAsync(TerminologyPoll terminologyPoll) =>
            TryCatch(async () =>
            {
                ValidateTerminologyPollIsNotNull(terminologyPoll);

                return await this.terminologyPollService.ModifyTerminologyPollAsync(terminologyPoll);
            });

        public ValueTask<TerminologyPoll> RemoveTerminologyPollByIdAsync(Guid terminologyPollId) =>
            TryCatch(async () =>
            {
                ValidateTerminologyPollId(terminologyPollId);

                return await this.terminologyPollService.RemoveTerminologyPollByIdAsync(terminologyPollId);
            });

        public ValueTask<TerminologyPoll> RetrieveOrAddTerminologyPollAsync(string resourceType) =>
            TryCatch(async () =>
            {
                ValidateResourceType(resourceType);

                IQueryable<TerminologyPoll> allTerminologyPolls =
                    this.terminologyPollService.RetrieveAllTerminologyPolls();

                TerminologyPoll? maybeTerminologyPoll = allTerminologyPolls
                    .Where(terminologyPoll => terminologyPoll.ResourceType == resourceType)
                        .FirstOrDefault();

                if (maybeTerminologyPoll == null)
                {
                    DateTimeOffset dateTimeOffset = this.dateTimeBroker.GetCurrentDateTimeOffset();

                    TerminologyPoll terminologyPoll = new TerminologyPoll
                    {
                        Id = await this.identifierBroker.GetIdentifierAsync(),
                        ResourceType = resourceType,
                        LastPoll = DateTimeOffset.MinValue.AddMilliseconds(1),
                        CreatedBy = "System",
                        UpdatedBy = "System",
                        UpdatedDate = dateTimeOffset,
                        CreatedDate = dateTimeOffset
                    };

                    TerminologyPoll addedTerminologyPoll =
                        await this.terminologyPollService.AddTerminologyPollAsync(terminologyPoll);

                    return addedTerminologyPoll;
                }
                else
                {
                    return maybeTerminologyPoll;
                }
            });
    }
}