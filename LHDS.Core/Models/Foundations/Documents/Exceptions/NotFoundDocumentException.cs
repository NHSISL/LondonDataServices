// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Documents.Exceptions
{
    public class NotFoundDocumentException : Xeption
    {
        public NotFoundDocumentException(string fileName)
            : base(message: $"Couldn't find documents with fileName: {fileName}.") { }
    }
}