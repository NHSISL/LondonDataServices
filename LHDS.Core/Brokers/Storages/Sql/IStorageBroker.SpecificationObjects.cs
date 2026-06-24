// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SpecificationObjects;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<SpecificationObject> InsertSpecificationObjectAsync(
            SpecificationObject specificationObject,
            CancellationToken cancellationToken = default);

        ValueTask<IQueryable<SpecificationObject>> SelectAllSpecificationObjectsAsync(
            CancellationToken cancellationToken = default);

        ValueTask<SpecificationObject> SelectSpecificationObjectByIdAsync(
            Guid specificationObjectId,
            CancellationToken cancellationToken = default);

        ValueTask<SpecificationObject> UpdateSpecificationObjectAsync(
            SpecificationObject specificationObject,
            CancellationToken cancellationToken = default);

        ValueTask<SpecificationObject> DeleteSpecificationObjectAsync(
            SpecificationObject specificationObject,
            CancellationToken cancellationToken = default);
    }
}