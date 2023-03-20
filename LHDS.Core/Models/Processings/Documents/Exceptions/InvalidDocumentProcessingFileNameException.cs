// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Documents.Exceptions
{
    public class InvalidDocumentProcessingFileNameException : Xeption
    {
        public InvalidDocumentProcessingFileNameException()
            : base(message: "Invalid document processing file name. Please correct the errors and try again.")
        { }
    }
}
