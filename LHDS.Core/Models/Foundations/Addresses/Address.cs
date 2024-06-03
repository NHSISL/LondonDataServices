// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Bases;

namespace LHDS.Core.Models.Foundations.Addresses
{
    public class Address : IKey, IAudit
    {
        public Guid Id { get; set; }
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
        public string? PostalAddress { get; set; }
        public string? JsonPostalAddress { get; set; }
        public bool IsNormalised { get; set; }
        public bool IsErrored { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
