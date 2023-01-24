// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace NEL.Premises.Api.Models.Bases
{
    public interface IAudit
    {
        string CreatedByUser { get; set; }
        DateTimeOffset CreatedDate { get; set; }
        string UpdatedByUser { get; set; }
        DateTimeOffset UpdatedDate { get; set; }
    }
}
