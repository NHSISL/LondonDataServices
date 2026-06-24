// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<SpecificationObject> SpecificationObjects { get; set; }

        public async ValueTask<SpecificationObject> InsertSpecificationObjectAsync(
            SpecificationObject specificationObject,
            CancellationToken cancellationToken = default) =>
                await InsertAsync(specificationObject, cancellationToken);

        public async ValueTask<IQueryable<SpecificationObject>> SelectAllSpecificationObjectsAsync(
            CancellationToken cancellationToken = default) =>
                await SelectAllAsync<SpecificationObject>(cancellationToken);

        public async ValueTask<SpecificationObject> SelectSpecificationObjectByIdAsync(
            Guid specificationObjectId,
            CancellationToken cancellationToken = default) =>
                await SelectAsync<SpecificationObject>(new object[] { specificationObjectId }, cancellationToken);

        public async ValueTask<SpecificationObject> UpdateSpecificationObjectAsync(
            SpecificationObject specificationObject,
            CancellationToken cancellationToken = default) =>
                await UpdateAsync(specificationObject, cancellationToken);

        public async ValueTask<SpecificationObject> DeleteSpecificationObjectAsync(
            SpecificationObject specificationObject,
            CancellationToken cancellationToken = default) =>
                await DeleteAsync(specificationObject, cancellationToken);
    }
}