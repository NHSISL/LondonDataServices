// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Services.Foundations.OptOuts;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Services.Processings.OptOuts
{
    public partial class OptOutProcessingService : IOptOutProcessingService
    {
        private readonly IOptOutService optOutService;
        private readonly ILoggingBroker loggingBroker;

        public OptOutProcessingService(
            IOptOutService optOutService,
            ILoggingBroker loggingBroker)
        {
            this.optOutService = optOutService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<OptOut> RetrieveOrAddOptOutAsync(OptOut optOut) =>
            TryCatch(async () =>
            {
                ValidateOptOutProcessingOnRetrieveOrAdd(optOut);

                OptOut maybeOptOut = await this.optOutService.RetrieveOptOutByIdAsync(optOut.Id);

                if (maybeOptOut == null)
                {
                    return await this.optOutService.AddOptOutAsync(optOut);
                }

                return maybeOptOut;
            });

        public ValueTask<OptOut> ModifyOptOutAsync(OptOut optOut) =>
            TryCatch(async () =>
            {
                ValidateOptOutProcessingOnModify(optOut);

                return await this.optOutService.ModifyOptOutAsync(optOut);
            });

        public ValueTask<OptOut> RemoveOptOutByIdAsync(Guid optOutId) =>
            TryCatch(async () =>
            {
                ValidateOptOutId(optOutId);

                return await this.optOutService.RemoveOptOutByIdAsync(optOutId);
            });

        public  ValueTask<OptOut> RetrieveOptOutByIdAsync(Guid optOutId) =>
            TryCatch(async () =>
            {
                ValidateOptOutId(optOutId);

                return await this.optOutService.RetrieveOptOutByIdAsync(optOutId);
            });

        public async ValueTask<OptOut> RetrieveOptOutByNhsNumberAsync(string optOutNhsNumber) =>
            throw new NotImplementedException();

    }
}

