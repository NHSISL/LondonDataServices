// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Services.Foundations.Assigns;

namespace LHDS.Core.Services.Processings.Assigns
{
    public class AssignProcessingService : IAssignProcessingService
    {
        private readonly IAssignService assignService;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IAuditBroker auditBroker;

        public AssignProcessingService(
            IAssignService assignService,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker,
            IAuditBroker auditBroker)
        {
            this.assignService = assignService;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
            this.auditBroker = auditBroker;
        }

        public ValueTask SyncAddressesAsync(List<Address> addresses) =>
            throw new NotImplementedException();
    }
}
