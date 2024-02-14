// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Models.Foundations.Downloads
{
    public class Download
    {
        public Document? Document { get; set; }
        public SubscriberCredential? SubscriberCredential { get; set; }
    }
}
