// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Processings.SpecificationObjects
{
    public interface ISpecificationObjectProcessingService
    {
        ValueTask<List<string>> RetrieveSpecificationObjectsByDataSetSpecificationId(Guid dataSetSpecificationId);
    }
}
