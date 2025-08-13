// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses
{
    public class ResolvedAddressReturn
    {
        public Guid UniqueReference { get; set; }
        public string? UPRN { get; set; }
        public string? USRN { get; set; }
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
    }
}
