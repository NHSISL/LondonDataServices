// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Newtonsoft.Json;

namespace LHDS.Core.Models.Brokers.Decisions
{
    public class DecisionAccessToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; } = "Bearer";

        [JsonProperty("not-before-policy")]
        public int NotBeforePolicy { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; } = string.Empty;

        public DateTimeOffset ExpiresAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
