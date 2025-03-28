// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.OptOuts.Exceptions
{
    public class NullOptOutConsentedItemsListProcessingException : Xeption
    {
        public NullOptOutConsentedItemsListProcessingException(string message)
            : base(message)
        { }
    }
}
