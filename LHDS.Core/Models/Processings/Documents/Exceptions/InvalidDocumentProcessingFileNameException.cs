// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Documents.Exceptions
{
    public class InvalidDocumentProcessingFileNameException : Xeption
    {
        public InvalidDocumentProcessingFileNameException(string message)
            : base(message)
        { }
    }
}
