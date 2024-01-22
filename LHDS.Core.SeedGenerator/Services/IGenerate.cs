// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace LHDS.Core.SeedGenerator.Services
{
    public interface IGenerate
    {
        ValueTask GenerateRecords(int supplierCount, int recordCount, int auditCount);
    }
}
