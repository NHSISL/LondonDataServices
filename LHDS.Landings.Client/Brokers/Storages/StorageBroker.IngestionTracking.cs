// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Security.Policy;
using LHDS.Landings.Client.Models.IngestionTracking;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Landings.Client.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<IngestionTracking> IngestionTrackings { get; set; }
    }
}
