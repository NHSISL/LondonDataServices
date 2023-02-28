// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Documents.Exceptions
{
    public class NullDocumentException : Xeption
    {
        public NullDocumentException()
            : base(message: $"Document is Null") { }

        public NullDocumentException(Document document)
            : base(message: $"Couldn't find document with fileName: {document}") { }
    }
}
