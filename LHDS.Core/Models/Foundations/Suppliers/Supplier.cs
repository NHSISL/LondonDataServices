// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace LHDS.Core.Models.Foundations.Suppliers
{
    public class Supplier
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public string LandingManualTriggerUrl { get; set; }
    }
}
