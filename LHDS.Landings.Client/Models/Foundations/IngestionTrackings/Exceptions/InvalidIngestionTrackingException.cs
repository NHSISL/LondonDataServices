// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.IngestionTrackings.Exceptions
{
    public class InvalidIngestionTrackingException : Xeption
    {
        public InvalidIngestionTrackingException()
            : base(message: "Invalid ingestion tracking. Please investigate.")
        { }
    }
}
