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
        public string? AlternateUnstructuredPostalAddress { get; set; } = string.Empty;
        public string? UPRN { get; set; }
        public string? UPSN { get; set; }
        public string? OrganisationName { get; set; }
        public string? DepartmentName { get; set; }
        public string? SubBuildingName { get; set; }
        public string? BuildingName { get; set; }
        public string? BuildingNumber { get; set; }
        public string? DependentThoroughfare { get; set; }
        public string? Thoroughfare { get; set; }
        public string? DoubleDependentLocality { get; set; }
        public string? DependentLocality { get; set; }
        public string? PostTown { get; set; }
        public string? PostCode { get; set; }
        public string? AddressFormatQuality { get; set; }
        public string? PostCodeQuality { get; set; }
        public bool MatchedWithAssign { get; set; }
        public bool IsProcessed { get; set; }
        public string? Qualifier { get; set; }
        public string? Classification { get; set; }
        public string? Algorithm { get; set; }
        public string? MatchPattern { get; set; }
        public bool IsProcessing { get; set; }
        public bool IsExported { get; set; }
        public int RetryCount { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
