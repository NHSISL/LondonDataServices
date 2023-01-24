// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xeptions;

namespace LHDS.Landings.Client.Models.IngestionTracking.Exceptions
{
    public class NullIngestionTrackingException : Xeption
    {
        public NullIngestionTrackingException() 
            : base(message: "Ingestion tracking is null.")
        { }
    }
}
