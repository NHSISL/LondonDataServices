// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Landings.Client.Models.Downloads.Exceptions;
using LHDS.Landings.Client.Models.Foundations.Documents;

namespace LHDS.Landings.Client.Services.Foundations.Downloads
{
    public partial class DownloadService
    {
        private static void ValidateDownloadIsNotNull(Document document)
        {
            if (document is null)
            {
                throw new NullDownloadException();
            }
        }
    }
}