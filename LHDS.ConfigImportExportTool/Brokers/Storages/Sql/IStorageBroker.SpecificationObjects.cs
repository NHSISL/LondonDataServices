// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;

namespace LHDS.ConfigImportExportTool.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<SpecificationObject> InsertSpecificationObjectAsync(SpecificationObject specificationObject);
        ValueTask<IQueryable<SpecificationObject>> SelectAllSpecificationObjectsAsync();
        ValueTask<SpecificationObject> SelectSpecificationObjectByIdAsync(Guid specificationObjectId);
        ValueTask<SpecificationObject> UpdateSpecificationObjectAsync(SpecificationObject specificationObject);
        ValueTask<SpecificationObject> DeleteSpecificationObjectAsync(SpecificationObject specificationObject);
    }
}