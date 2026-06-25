// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;

namespace LHDS.Core.Services.Foundations.AddressToUprnFileLogs
{
    public partial class AddressToUprnFileLogService : IAddressToUprnFileLogService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityAuditBroker securityAuditBroker;
        private readonly ILoggingBroker loggingBroker;

        public AddressToUprnFileLogService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ISecurityAuditBroker securityAuditBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.securityAuditBroker = securityAuditBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<AddressToUprnFileLog> AddAddressToUprnFileLogAsync(
            AddressToUprnFileLog addressToUprnFileLog) =>
            TryCatch(async () =>
            {
                AddressToUprnFileLog addressToUprnFileLogWithAddAuditApplied =
                    await this.securityAuditBroker.ApplyAddAuditValuesAsync(addressToUprnFileLog);

                await ValidateAddressToUprnFileLogOnAddAsync(addressToUprnFileLogWithAddAuditApplied);

                return await this.storageBroker.InsertAddressToUprnFileLogAsync(addressToUprnFileLog);
            });

        public ValueTask<IQueryable<AddressToUprnFileLog>> RetrieveAllAddressToUprnFileLogsAsync() =>
            TryCatch(async () => await this.storageBroker.SelectAllAddressToUprnFileLogsAsync());

        public ValueTask<AddressToUprnFileLog> RetrieveAddressToUprnFileLogByIdAsync(
            Guid addressToUprnFileLogId) =>
            TryCatch(async () =>
            {
                ValidateAddressToUprnFileLogId(addressToUprnFileLogId);

                AddressToUprnFileLog maybeAddressToUprnFileLog =
                    await this.storageBroker.SelectAddressToUprnFileLogByIdAsync(addressToUprnFileLogId);

                ValidateStorageAddressToUprnFileLog(maybeAddressToUprnFileLog, addressToUprnFileLogId);

                return maybeAddressToUprnFileLog;
            });

        public ValueTask<AddressToUprnFileLog> ModifyAddressToUprnFileLogAsync(
            AddressToUprnFileLog addressToUprnFileLog) =>
            TryCatch(async () =>
            {
                AddressToUprnFileLog addressToUprnFileLogWithModifyAuditApplied =
                    await this.securityAuditBroker.ApplyModifyAuditValuesAsync(addressToUprnFileLog);

                await ValidateAddressToUprnFileLogOnModifyAsync(addressToUprnFileLogWithModifyAuditApplied);

                AddressToUprnFileLog maybeAddressToUprnFileLog =
                    await this.storageBroker.SelectAddressToUprnFileLogByIdAsync(addressToUprnFileLog.Id);

                ValidateStorageAddressToUprnFileLog(maybeAddressToUprnFileLog, addressToUprnFileLog.Id);

                ValidateAgainstStorageAddressToUprnFileLogOnModify(
                    inputAddressToUprnFileLog: addressToUprnFileLogWithModifyAuditApplied,
                    storageAddressToUprnFileLog: maybeAddressToUprnFileLog);

                return await this.storageBroker.UpdateAddressToUprnFileLogAsync(
                    addressToUprnFileLogWithModifyAuditApplied);
            });

        public ValueTask<AddressToUprnFileLog> RemoveAddressToUprnFileLogByIdAsync(
            Guid addressToUprnFileLogId) =>
            TryCatch(async () =>
            {
                ValidateAddressToUprnFileLogId(addressToUprnFileLogId);

                AddressToUprnFileLog maybeAddressToUprnFileLog =
                    await this.storageBroker.SelectAddressToUprnFileLogByIdAsync(addressToUprnFileLogId);

                ValidateStorageAddressToUprnFileLog(maybeAddressToUprnFileLog, addressToUprnFileLogId);

                return await this.storageBroker.DeleteAddressToUprnFileLogAsync(maybeAddressToUprnFileLog);
            });
    }
}
