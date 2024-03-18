// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Providers.Downloads.DiskDownloads
{
    internal interface IDiskDownloadProviderSettings
    {
        string LocalRootFolder { get; }
        bool IncludeSubDirectories { get; }
    }
}
