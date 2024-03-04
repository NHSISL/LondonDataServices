// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Cryptographies.Exceptions
{
    public class NullDataCryptographyException : Xeption
    {
        public NullDataCryptographyException(string message)
            : base(message)
        { }
    }
}