// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads;

namespace LHDS.Core.Providers.Downloads.DiskDownloads
{
    public class DiskDownloadProvider : IDownloadProvider
    {
        private readonly DiskDownloadProviderSettings diskDownloadProviderSettings;

        public string Name { get; private set; }
        public bool IsOfflineProvider { get; private set; }

        public DiskDownloadProvider(DiskDownloadProviderSettings diskDownloadProviderSettings)
        {
            this.diskDownloadProviderSettings = diskDownloadProviderSettings;
            this.Name = "DiskDownloadProvider";
        }

        public async ValueTask<Download> GetDocumentByFileNameAsync(Download download)
        {
            string filePath = Path.Combine(diskDownloadProviderSettings.LocalRootFolder, download.Document.FileName);
            byte[] data = await File.ReadAllBytesAsync(filePath);

            var document = new Document()
            {
                FileName = download.Document.FileName,
                DocumentData = data
            };

            var downloadedItem = new Download
            {
                Document = document,
                SubscriberCredential = download.SubscriberCredential
            };

            return downloadedItem;
        }

        public async ValueTask<List<string>> GetListOfDocumentsToProcessAsync(Download download)
        {
            string[] files = Directory.GetFiles(
                diskDownloadProviderSettings.LocalRootFolder, "*",
                diskDownloadProviderSettings.IncludeSubDirectories
                    ? SearchOption.AllDirectories
                    : SearchOption.TopDirectoryOnly);

            return new List<string>(files);
        }
    }
}
