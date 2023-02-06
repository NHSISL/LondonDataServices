// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace LHDS.Landings.Client.Providers.Downloads.FtpDownloads
{
    public interface IBlobStorageBrokerSettings
    {
        string AzureBlobStoreUri { get; }

        string BlobContainerName { get; }
    }
}
