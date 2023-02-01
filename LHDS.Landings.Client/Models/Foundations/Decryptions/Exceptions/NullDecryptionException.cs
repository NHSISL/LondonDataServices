// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.Decryptions.Exceptions
{
    public class NullDecryptionException : Xeption
    {
        public NullDecryptionException()
            : base(message: "Decryption is null.")
        { }
    }
}