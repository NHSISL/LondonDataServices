// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.Documents.Exceptions
{
    public class NullDocumentException : Xeption
    {
        public NullDocumentException(Document document)
            : base(message: $"Couldn't find document with fileName: {document}")
        { }
    }
}
