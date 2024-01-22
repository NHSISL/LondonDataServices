// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Providers.Downloads.FtpDownloads;

namespace LHDS.Core.Providers.Downloads.Builders
{
    public class FtpProviderRegistrationBuilder
    {
        private readonly List<IDownloadProvider> downloadProviderRegistrations;
        private readonly IFtpDownloadProviderSettings ftpDownloadProviderSettings;

        public FtpProviderRegistrationBuilder(IFtpDownloadProviderSettings ftpDownloadProviderSettings)
        {
            downloadProviderRegistrations = new List<IDownloadProvider>();
            this.ftpDownloadProviderSettings = ftpDownloadProviderSettings;
        }

        public void AddFtpDownloadProvider()
        {
            var ftpDownloadProvider = new FtpDownloadProvider(ftpDownloadProviderSettings);

            if (downloadProviderRegistrations.Count > 0)
            {
                throw new Exception("Only one download provider can be registered at a time");
            }

            downloadProviderRegistrations.Add(ftpDownloadProvider);
        }

        public List<IDownloadProvider> Build()
        {
            return downloadProviderRegistrations;
        }
    }
}
