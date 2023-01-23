// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.IngestionTracking;

namespace LHDS.Landings.Client.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<IngestionTracking> InsertIngestionTrackingAsync(IngestionTracking ingestionTracking);
        IQueryable<Discussion> SelectAllDiscussions();
    }
}
