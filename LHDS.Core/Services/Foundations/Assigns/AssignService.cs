// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Brokers.Assigns;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;

namespace LHDS.Core.Services.Foundations.Assigns
{
    internal class AssignService : IAssignService
    {
        private readonly IAssignBroker assignBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IAuditBroker auditBroker;

        public AssignService(
            IAssignBroker assignBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker,
            IAuditBroker auditBroker)
        {
            this.assignBroker = assignBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
            this.auditBroker = auditBroker;
        }
    }
}
