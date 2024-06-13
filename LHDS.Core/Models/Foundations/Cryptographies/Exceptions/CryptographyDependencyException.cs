// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Cryptographies.Exceptions
{
    public class CryptographyDependencyException : Xeption
    {
        public CryptographyDependencyException(string message, Xeption? innerException) :
            base(message, innerException)
        { }
    }
}