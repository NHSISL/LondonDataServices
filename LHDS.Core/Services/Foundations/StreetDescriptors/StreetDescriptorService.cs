// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.StreetDescriptors;

namespace LHDS.Core.Services.Foundations.StreetDescriptors
{
    public partial class StreetDescriptorService : IStreetDescriptorService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityBroker securityBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IAuditBroker auditBroker;

        public StreetDescriptorService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ISecurityBroker securityBroker,
            IIdentifierBroker identifierBroker,
            ILoggingBroker loggingBroker,
            IAuditBroker auditBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.securityBroker = securityBroker;
            this.identifierBroker = identifierBroker;
            this.loggingBroker = loggingBroker;
            this.auditBroker = auditBroker;
        }

        public async ValueTask<StreetDescriptor> AddStreetDescriptorAsync(StreetDescriptor streetDescriptor)
        {
            StreetDescriptor streetDescriptorWithAddAuditApplied = await ApplyAddAuditAsync(streetDescriptor);

            return await this.storageBroker.InsertStreetDescriptorAsync(streetDescriptor);
        }

        public ValueTask BulkAddStreetDescriptorsAsync(List<StreetDescriptor> streetDescriptors, string fileName)
        {
            throw new NotImplementedException();
        }

        public ValueTask<StreetDescriptor> ModifyStreetDescriptorAsync(StreetDescriptor address)
        {
            throw new NotImplementedException();
        }

        public ValueTask<StreetDescriptor> RemoveStreetDescriptorByIdAsync(Guid addressId)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IQueryable<StreetDescriptor>> RetrieveAllStreetDescriptorsAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<StreetDescriptor> RetrieveStreetDescriptorByIdAsync(Guid addressId)
        {
            throw new NotImplementedException();
        }

        public ValueTask<List<StreetDescriptor>> RetrieveStreetDescriptorsByUSRNAsync(string urrn)
        {
            throw new NotImplementedException();
        }

        virtual internal async ValueTask<StreetDescriptor> ApplyAddAuditAsync(
            StreetDescriptor streetDescriptor)
        {
            
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            streetDescriptor.CreatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            streetDescriptor.CreatedDate = auditDateTimeOffset;
            streetDescriptor.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            streetDescriptor.UpdatedDate = auditDateTimeOffset;

            return streetDescriptor;
        }
    }
}