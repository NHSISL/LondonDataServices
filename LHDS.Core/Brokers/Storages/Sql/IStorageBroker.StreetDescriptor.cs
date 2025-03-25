// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.StreetDescriptors;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask BulkInsertStreetDescriptorsAsync(List<StreetDescriptor> streetDescriptor);
        ValueTask<StreetDescriptor> InsertStreetDescriptorAsync(StreetDescriptor streetDescriptor);
        ValueTask<IQueryable<StreetDescriptor>> SelectAllStreetDescriptorsAsync();
        ValueTask<StreetDescriptor> SelectStreetDescriptorByIdAsync(Guid streetDescriptorId);
        ValueTask<StreetDescriptor> UpdateStreetDescriptorAsync(StreetDescriptor streetDescriptor);
        ValueTask BulkUpdateStreetDescriptorsAsync(List<StreetDescriptor> streetDescriptor);
        ValueTask<StreetDescriptor> DeleteStreetDescriptorAsync(StreetDescriptor streetDescriptor);
    }
}
