// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Models.Orchestrations.AddressToUprns
{
    public class AddressToUprnConfiguration
    {
        public string InboxFolder { get; set; } = "Inbox";
        public string OutboxFolder { get; set; } = "Outbox";
        public string ErrorFolder { get; set; } = "Error";
    }
}
