using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;

namespace LHDS.Core.Services.Foundations.ResolvedAddressAudits
{
    public class ResolvedAddressAuditService : IResolvedAddressAuditService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityBroker securityBroker;

        public ResolvedAddressAuditService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ISecurityBroker securityBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.securityBroker = securityBroker;
        }

        public ValueTask<ResolvedAddressAudit> AddResolvedAddressAuditAsync(ResolvedAddressAudit resolvedAddressAudit)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResolvedAddressAudit> ModifyResolvedAddressAuditAsync(ResolvedAddressAudit resolvedAddressAudit)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResolvedAddressAudit> RemoveResolvedAddressAuditByIdAsync(Guid resolvedAddressAuditId)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IQueryable<ResolvedAddressAudit>> RetrieveAllResolvedAddressAuditsAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResolvedAddressAudit> RetrieveResolvedAddressAuditByIdAsync(Guid resolvedAddressAuditId)
        {
            throw new NotImplementedException();
        }
    }
}
