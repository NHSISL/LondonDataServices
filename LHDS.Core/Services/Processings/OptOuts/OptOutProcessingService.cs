// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Services.Foundations.OptOuts;

namespace LHDS.Core.Services.Processings.OptOuts
{
    public partial class OptOutProcessingService : IOptOutProcessingService
    {
        private readonly IOptOutService optOutService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public OptOutProcessingService(
            IOptOutService optOutService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.optOutService = optOutService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<OptOut> RetrieveOrAddOptOutAsync(OptOut optOut) =>
            TryCatch(async () =>
            {
                ValidateOptOutProcessingOnRetrieveOrAdd(optOut);

                OptOut maybeOptOut = this.optOutService.RetrieveAllOptOuts()
                    .FirstOrDefault(item => item.NhsNumber == optOut.NhsNumber);

                if (maybeOptOut == null)
                {
                    return await this.optOutService.AddOptOutAsync(optOut);
                }

                return maybeOptOut;
            });

        public ValueTask<OptOut> AddOrModifyOptOutAsync(OptOut optOut) =>
            TryCatch(async () =>
            {
                ValidateOptOutProcessingOnModify(optOut);

                IQueryable<OptOut> allOptOuts = this.optOutService.RetrieveAllOptOuts();

                OptOut maybeOptOut = allOptOuts.FirstOrDefault(item =>
                    item.NhsNumber == optOut.NhsNumber);

                if (maybeOptOut == null)
                {
                    return await this.optOutService.AddOptOutAsync(optOut);
                }

                maybeOptOut.OptOutStatus = optOut.OptOutStatus;
                maybeOptOut.BatchReference = optOut.BatchReference;
                maybeOptOut.CacheTime = optOut.CacheTime;
                maybeOptOut.LastSentToMesh = optOut.LastSentToMesh;
                maybeOptOut.UpdatedDate = dateTimeBroker.GetCurrentDateTimeOffset();

                return await this.optOutService.ModifyOptOutAsync(maybeOptOut);
            });

        public ValueTask<OptOut> RemoveOptOutByIdAsync(Guid optOutId) =>
            TryCatch(async () =>
            {
                ValidateOptOutId(optOutId);

                return await this.optOutService.RemoveOptOutByIdAsync(optOutId);
            });

        public ValueTask<OptOut> RetrieveOptOutByIdAsync(Guid optOutId) =>
            TryCatch(async () =>
            {
                ValidateOptOutId(optOutId);

                return await this.optOutService.RetrieveOptOutByIdAsync(optOutId);
            });

        public ValueTask<OptOut> RetrieveOptOutByNhsNumberAsync(string optOutNhsNumber) =>
            TryCatch(async () =>
            {
                ValidateOptOutNhsNumber(optOutNhsNumber);

                IQueryable<OptOut> allOptOuts = this.optOutService.RetrieveAllOptOuts();

                OptOut foundOptOut = allOptOuts.FirstOrDefault(optOut =>
                    optOut.NhsNumber == optOutNhsNumber);

                return await ValueTask.FromResult(foundOptOut);
            });

        public ValueTask<List<OptOut>> RetrieveAllExpiredOptOutsAsync(int olderThanDays) =>
            TryCatch(async () =>
            {
                ValidateOlderThanDays(olderThanDays);

                var expirationDate = this.dateTimeBroker.
                    GetCurrentDateTimeOffset().AddDays(-olderThanDays);

                IQueryable<OptOut> allOptOuts = this.optOutService.RetrieveAllOptOuts();

                List<OptOut> expiredOptOuts = allOptOuts
                    .Where(optOut => optOut.CacheTime < expirationDate)
                        .ToList();

                return await ValueTask.FromResult(expiredOptOuts);
            });

        public ValueTask<List<OptOut>> RetrieveAllOptOutsByBatchReferenceAsync(string batchReference) =>
            TryCatch(async () =>
            {
                ValidateOptOutBatchReference(batchReference);

                IQueryable<OptOut> allOptOuts = this.optOutService.RetrieveAllOptOuts();

                List<OptOut> foundOptOuts = allOptOuts.Where(optOut =>
                    optOut.BatchReference == batchReference)
                        .ToList();

                return await ValueTask.FromResult(foundOptOuts);
            });
    }
}

