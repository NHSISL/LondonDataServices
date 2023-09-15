// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using Newtonsoft.Json;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.OdataResponses
{
    public class OdataResponce<TItem>
    {
        [JsonProperty("@odata.context")]
        public string ContextUrl { get; set; } = null!;

        [JsonProperty("@odata.count")]
        public int? TotalItemsCount { get; set; }

        [JsonProperty("value")]
        public List<TItem> Items { get; set; } = null!;
    }
}
