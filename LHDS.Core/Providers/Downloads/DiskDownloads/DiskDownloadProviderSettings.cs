// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Providers.Downloads.DiskDownloads
{
    public class DiskDownloadProviderSettings : IDiskDownloadProviderSettings
    {
        public string LocalRootFolder { get; set; }
        public bool IncludeSubDirectories { get; set; }
    }
}
