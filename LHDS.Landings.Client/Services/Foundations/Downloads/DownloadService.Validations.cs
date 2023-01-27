// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Models.Foundations.Downloads.Exceptions;

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