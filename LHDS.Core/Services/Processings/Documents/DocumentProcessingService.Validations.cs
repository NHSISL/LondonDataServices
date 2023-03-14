// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Processings.Documents.Exceptions;

namespace LHDS.Core.Services.Processings.Documents
{
    public partial class DocumentProcessingService
    {
        private static void ValidateDocumentProcessingOnAdd(Document document)
        {
            ValidateDocumentProcessingIsNotNull(document);
        }

        private static void ValidateDocumentProcessingIsNotNull(Document document)
        {
            if (document is null)
            {
                throw new NullDocumentProcessingException();
            }
        }
    }
}
