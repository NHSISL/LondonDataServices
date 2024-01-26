// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;

namespace LHDS.Core.Models.Foundations.AddressNormalisations
{
    public class AddressNormalisation
    {
        public string? PostalAddress { get; set; }
        public string? JsonPostalAddress { get; set; }
        public IList<KeyValuePair<string, string>> AddressComponents { get; set; }
            = new List<KeyValuePair<string, string>>();
    }
}
