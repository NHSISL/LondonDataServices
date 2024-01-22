// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Providers.Downloads.RestDownloads;

namespace LHDS.Core.Providers.Downloads.Builders
{
    public class RestProviderRegistrationBuilder
    {
        private readonly List<IDownloadProvider> downloadProviderRegistrations;

        public RestProviderRegistrationBuilder()
        {
            downloadProviderRegistrations = new();
        }

        public void AddRestDownloadProvider()
        {
            RestDownloadProvider ftpDownloadProvider = new();

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
