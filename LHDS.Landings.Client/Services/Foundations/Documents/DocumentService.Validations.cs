// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Models.Foundations.Documents.Exceptions;

namespace LHDS.Landings.Client.Services.Foundations.Downloads
{
    public partial class DocumentService
    {
        private static void ValidateDocumentIsNotNull(Document document)
        {
            if (document is null)
            {
                throw new NullDocumentException();
            }
        }
    }
}
