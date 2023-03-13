// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Documents.Exceptions
{
    public class NullDocumentProcessingFileNameException : Xeption
    {
        public NullDocumentProcessingFileNameException()
            : base(message: "Null document processing file name. Please correct the errors and try again.") { }
    }
}
