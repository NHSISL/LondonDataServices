// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Extensions
{
    public class ChildException : Xeption
    {
        public ChildException(string message, Exception innerException = null)
            : base(message, innerException) { }
    }
}