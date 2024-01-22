// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.OptOuts.Exceptions
{
    public class NullOptOutConsentedItemsListProcessingException : Xeption
    {
        public NullOptOutConsentedItemsListProcessingException()
            : base(message: $"Opt out processing consented items list is Null")
        { }
    }
}
