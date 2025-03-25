// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.StreetDescriptors;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<StreetDescriptor> StreetDescriptors { get; set; }

        public async ValueTask BulkInsertStreetDescriptorsAsync(List<StreetDescriptor> streetDescriptors) =>
            await BulkInsertAsync(streetDescriptors);

        public async ValueTask<StreetDescriptor> InsertStreetDescriptorAsync(StreetDescriptor streetDescriptor) =>
            await InsertAsync(streetDescriptor);

        public async ValueTask<IQueryable<StreetDescriptor>> SelectAllStreetDescriptorsAsync() => await SelectAllAsync<StreetDescriptor>();

        public async ValueTask<StreetDescriptor> SelectStreetDescriptorByIdAsync(Guid streetDescriptorId) =>
            await SelectAsync<StreetDescriptor>(streetDescriptorId);

        public async ValueTask<StreetDescriptor> UpdateStreetDescriptorAsync(StreetDescriptor streetDescriptor) =>
            await UpdateAsync(streetDescriptor);

        public async ValueTask BulkUpdateStreetDescriptorsAsync(List<StreetDescriptor> streetDescriptors) =>
            await BulkUpdateAsync(streetDescriptors);

        public async ValueTask<StreetDescriptor> DeleteStreetDescriptorAsync(StreetDescriptor streetDescriptor) =>
            await DeleteAsync(streetDescriptor);
    }
}
