// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------


using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Documents;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberCredentials;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.Downloads
{
    public class Download
    {
        public Document Document { get; set; }
        public SubscriberCredential SubscriberCredential { get; set; }
    }
}
