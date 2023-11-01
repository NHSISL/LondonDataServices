// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OntologyCodeSystems;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<OntologyCodeSystem> InsertOntologyCodeSystemAsync(
            OntologyCodeSystem ontologyCodeSystem);

        IQueryable<OntologyCodeSystem> SelectAllOntologyCodeSystems();
        ValueTask<OntologyCodeSystem> SelectOntologyCodeSystemByIdAsync(Guid ontologyCodeSystemId);

        ValueTask<OntologyCodeSystem> UpdateOntologyCodeSystemAsync(
            OntologyCodeSystem ontologyCodeSystem);

        ValueTask<OntologyCodeSystem> DeleteOntologyCodeSystemAsync(
            OntologyCodeSystem ontologyCodeSystem);
    }
}
