// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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

        public ValueTask<IQueryable<OptOut>> RetrieveAllOptOutsAsync() =>
            TryCatch(async () => await this.optOutService.RetrieveAllOptOutsAsync());

        public ValueTask<OptOut> RetrieveOrAddOptOutAsync(OptOut optOut) =>
            TryCatch(async () =>
            {
                ValidateOptOutProcessingOnRetrieveOrAdd(optOut);
                IQueryable<OptOut> allOptOuts = await this.optOutService.RetrieveAllOptOutsAsync();

                OptOut? maybeOptOut = allOptOuts.FirstOrDefault(item =>
                    item.NhsNumber == optOut.NhsNumber);

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
                IQueryable<OptOut> allOptOuts = await this.optOutService.RetrieveAllOptOutsAsync();

                OptOut? maybeOptOut = allOptOuts.FirstOrDefault(item =>
                    item.NhsNumber == optOut.NhsNumber);

                if (maybeOptOut == null)
                {
                    return await this.optOutService.AddOptOutAsync(optOut);
                }

                maybeOptOut.Status = string.IsNullOrEmpty(optOut.Status)
                    ? maybeOptOut.Status
                    : optOut.Status;

                maybeOptOut.BatchReference =
                    optOut.BatchReference ?? maybeOptOut.BatchReference;

                maybeOptOut.CacheTime =
                    optOut.CacheTime == default ? maybeOptOut.CacheTime : optOut.CacheTime;

                maybeOptOut.LastSentToMesh =
                    optOut.LastSentToMesh == default ? maybeOptOut.LastSentToMesh : optOut.LastSentToMesh;

                maybeOptOut.UpdatedDate =
                    optOut.UpdatedDate == default ? maybeOptOut.UpdatedDate : optOut.UpdatedDate;

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

        public ValueTask<OptOut?> RetrieveOptOutByNhsNumberAsync(string optOutNhsNumber) =>
            TryCatch(async () =>
            {
                ValidateOptOutNhsNumber(optOutNhsNumber);
                IQueryable<OptOut> allOptOuts = await this.optOutService.RetrieveAllOptOutsAsync();

                OptOut? foundOptOut = allOptOuts.FirstOrDefault(optOut =>
                    optOut.NhsNumber == optOutNhsNumber);

                return await ValueTask.FromResult(foundOptOut);
            });

        public ValueTask<List<string>> RetrieveAllExpiredOptOutsAsync(int olderThanDays) =>
            TryCatch(async () =>
            {
                ValidateOlderThanDays(olderThanDays);
                var currentDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                var expirationDate = currentDateTimeOffset.AddDays(-olderThanDays);
                var lastSentExpirationDate = currentDateTimeOffset.AddDays(-2);
                IQueryable<OptOut> allOptOuts = await this.optOutService.RetrieveAllOptOutsAsync();

                List<string> expiredOptOuts = allOptOuts
                    .Where(optOut =>
                        optOut.CacheTime < expirationDate
                        && optOut.LastSentToMesh < lastSentExpirationDate)
                    .OrderBy(optOut => optOut.LastSentToMesh)
                    .Select(optOut => optOut.NhsNumber)
                    .Take(10000)
                    .ToList();

                return await ValueTask.FromResult(expiredOptOuts);
            });

        public ValueTask<List<OptOut>> RetrieveAllOptOutsByBatchReferenceAsync(string batchReference) =>
            TryCatch(async () =>
            {
                ValidateOptOutBatchReference(batchReference);
                IQueryable<OptOut> allOptOuts = await this.optOutService.RetrieveAllOptOutsAsync();

                List<OptOut> foundOptOuts = allOptOuts.Where(optOut =>
                    optOut.BatchReference == batchReference)
                        .ToList();

                return await ValueTask.FromResult(foundOptOuts);
            });

        public ValueTask<List<OptOut>> ConsolidateOptOutChangesAndReturnChangesOnly(
            List<OptOut> currentOptOutList,
            List<string> consentedIdentifiers) =>
            TryCatch(async () =>
        {
            ValidateCurrentOptOutListProcessingOnConsolidate(currentOptOutList, consentedIdentifiers);

            if (!currentOptOutList.Any())
            {
                return new List<OptOut>();
            }

            var consentedSet = new HashSet<string>(consentedIdentifiers);

            List<OptOut> consentedList = currentOptOutList
                .Where(optOut => consentedSet.Contains(optOut.NhsNumber))
                    .ToList();

            List<OptOut> nonConsentedList = currentOptOutList
                .Except(consentedList).ToList();

            List<OptOut> delta = new List<OptOut>();

            if (!consentedList.Any() && !nonConsentedList.Any())
            {
                return delta;
            }

            var dateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            foreach (var item in consentedList)
            {
                if (item == null)
                {
                    continue;
                }

                if (item.Status != "Opt-In")
                {
                    delta.Add(item);
                }

                item.CacheTime = dateTime;
                item.LastSentToMesh = dateTime;
                item.Status = "Opt-In";
            }

            foreach (var nonConsentedListItem in nonConsentedList)
            {
                if (nonConsentedListItem.Status != "Opt-Out")
                {
                    delta.Add(nonConsentedListItem);
                }

                nonConsentedListItem.CacheTime = dateTime;
                nonConsentedListItem.LastSentToMesh = dateTime;
                nonConsentedListItem.Status = "Opt-Out";
            }

            List<OptOut> allModifiedOptOuts = new List<OptOut>();
            allModifiedOptOuts.AddRange(consentedList);
            allModifiedOptOuts.AddRange(nonConsentedList);

            await this.optOutService.BulkModifyOptOutsAsync(
                allModifiedOptOuts,
                "ConsolidateOptOutChanges");

            return delta;
        });
    }
            }

