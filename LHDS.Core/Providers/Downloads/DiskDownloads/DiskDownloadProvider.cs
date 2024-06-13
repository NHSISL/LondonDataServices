// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads;

namespace LHDS.Core.Providers.Downloads.DiskDownloads
{
    public class DiskDownloadProvider : IDownloadProvider
    {
        private readonly DiskDownloadProviderSettings diskDownloadProviderSettings;

        public string Name { get; private set; } = "DiskDownloadProvider";
        public bool IsOfflineProvider { get; private set; }

        public DiskDownloadProvider()
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
            string defaultFolderPath = Path.Combine(assemblyPath, "temp", "downloads");

            this.diskDownloadProviderSettings = new DiskDownloadProviderSettings
            {
                LocalRootFolder = defaultFolderPath,
                IncludeSubDirectories = true
            };
        }

        public DiskDownloadProvider(DiskDownloadProviderSettings diskDownloadProviderSettings)
        {
            this.diskDownloadProviderSettings = diskDownloadProviderSettings;
        }

        public async ValueTask<Download> GetDocumentByFileNameAsync(Download download)
        {
            string docFileName = download?.Document?.FileName ?? "";
            string relativePath = docFileName.Replace("/", "\\");
            string filePath = Path.Combine(diskDownloadProviderSettings.LocalRootFolder, relativePath);
            byte[] data = await File.ReadAllBytesAsync(filePath);

            var document = new Document()
            {
                FileName = docFileName,
                DocumentData = data
            };

            var downloadedItem = new Download
            {
                Document = document,
                SubscriberCredential = download?.SubscriberCredential
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

            List<string> relativePaths = files.Select(file =>
                Path.GetRelativePath(diskDownloadProviderSettings.LocalRootFolder, file).Replace("\\", "/")).ToList();

            return await ValueTask.FromResult(relativePaths);
        }
    }
}
