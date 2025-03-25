// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.StreetDescriptors;

namespace LHDS.Core.Services.Foundations.StreetDescriptors
{
    public partial class StreetDescriptorService : IStreetDescriptorService
    {
        public StreetDescriptorService()
        { }

        public ValueTask<StreetDescriptor> AddStreetDescriptorAsync(StreetDescriptor address)
        {
            throw new NotImplementedException();
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
    }
}