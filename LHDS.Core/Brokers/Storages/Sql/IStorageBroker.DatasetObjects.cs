// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SpecificationObjects;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<SpecificationObject> InsertDataSetObjectAsync(SpecificationObject dataSetObject);
        IQueryable<SpecificationObject> SelectAllDataSetObjects();
        ValueTask<SpecificationObject> SelectDataSetObjectByIdAsync(Guid dataSetObjectId);
        ValueTask<SpecificationObject> UpdateDataSetObjectAsync(SpecificationObject dataSetObject);
        ValueTask<SpecificationObject> DeleteDataSetObjectAsync(SpecificationObject dataSetObject);
    }
}