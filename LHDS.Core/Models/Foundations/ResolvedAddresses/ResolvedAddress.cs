// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Bases;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses
{
    public class ResolvedAddress : IKey, IAudit
    {
        public Guid Id { get; set; }
        public Guid UniqueReference { get; set; }
        public Guid? BatchReference { get; set; }
        public string UnstructuredPostalAddress { get; set; } = string.Empty;
        public string? PostCode { get; set; }
        public string? PostalAddress { get; set; }
        public string? JsonPostalAddress { get; set; }
        public MatchAlgorithmEnum MatchAlgorithmEnum { get; set; }
        public bool IsMatched { get; set; }
        public string? MatchedPostalAddress { get; set; }
        public string? MatchedJsonPostalAddress { get; set; }
        public string? MatchedUPRN { get; set; }
        public string? MatchedUPSN { get; set; }
        public string? MatchedOrganisationName { get; set; }
        public string? MatchedDepartmentName { get; set; }
        public string? MatchedSubBuildingName { get; set; }
        public string? MatchedBuildingName { get; set; }
        public string? MatchedBuildingNumber { get; set; }
        public string? MatchedDependentThoroughfare { get; set; }
        public string? MatchedThoroughfare { get; set; }
        public string? MatchedDoubleDependentLocality { get; set; }
        public string? MatchedDependentLocality { get; set; }
        public string? MatchedPostTown { get; set; }
        public string? MatchedPostCode { get; set; }
        public bool IsProcessed { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
