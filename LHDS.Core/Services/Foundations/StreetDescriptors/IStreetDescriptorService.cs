// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.StreetDescriptors;

namespace LHDS.Core.Services.Foundations.StreetDescriptors
{
    public interface IStreetDescriptorService
    {
        ValueTask BulkAddStreetDescriptorsAsync(List<StreetDescriptor> streetDescriptors, string fileName);
        ValueTask<StreetDescriptor> AddStreetDescriptorAsync(StreetDescriptor address);
        ValueTask<IQueryable<StreetDescriptor>> RetrieveAllStreetDescriptorsAsync();
        ValueTask<StreetDescriptor> RetrieveStreetDescriptorByIdAsync(Guid addressId);
        ValueTask<StreetDescriptor> ModifyStreetDescriptorAsync(StreetDescriptor address);
        ValueTask<StreetDescriptor> RemoveStreetDescriptorByIdAsync(Guid addressId);
        ValueTask<List<StreetDescriptor>> RetrieveStreetDescriptorsByUSRNAsync(string urrn);
    }
}