// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;

namespace LHDS.Core.Services.Foundations.AddressToUprnFileLogs
{
    public interface IAddressToUprnFileLogService
    {
        ValueTask<AddressToUprnFileLog> AddAddressToUprnFileLogAsync(AddressToUprnFileLog addressToUprnFileLog);
        ValueTask<IQueryable<AddressToUprnFileLog>> RetrieveAllAddressToUprnFileLogsAsync();
        ValueTask<AddressToUprnFileLog> RetrieveAddressToUprnFileLogByIdAsync(Guid addressToUprnFileLogId);
        ValueTask<AddressToUprnFileLog> ModifyAddressToUprnFileLogAsync(AddressToUprnFileLog addressToUprnFileLog);
        ValueTask<AddressToUprnFileLog> RemoveAddressToUprnFileLogByIdAsync(Guid addressToUprnFileLogId);
    }
}