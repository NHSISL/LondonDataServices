// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Documents.Exceptions
{
    public class NullDocumentProcessingFileNameException : Xeption
    {
        public NullDocumentProcessingFileNameException(string message)
            : base(message)
        { }
    }
}
