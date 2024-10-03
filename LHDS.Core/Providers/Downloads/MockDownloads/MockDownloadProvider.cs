// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads;

namespace LHDS.Core.Providers.Downloads.MockDownloads
{
    public class MockDownloadProvider : IDownloadProvider
    {
        public string Name { get; private set; }
        public bool IsOfflineProvider { get; private set; }

        public MockDownloadProvider()
        {
            Name = "MockDownloadProvider";
            IsOfflineProvider = true;
        }

        private static string GetRandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            int length = random.Next(5, 16);

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async ValueTask GetDocumentByFileNameAsync(Download download)
        {
            string randomString = GetRandomString();
            byte[] data = Encoding.UTF8.GetBytes(randomString);

            var document = new Document()
            {
                FileName = download?.Document?.FileName ?? "",
                DocumentData = download?.Document?.DocumentData
            };

            var downloadedItem = new Download
            {
                Document = document,
                SubscriberCredential = download?.SubscriberCredential
            };

            await ValueTask.FromResult(true);
        }

        public async ValueTask<List<string>> GetListOfDocumentsToProcessAsync(Download download)
        {
            List<string> downloads = new List<string>();

            for (int i = 0; i < 1; i++)
            {
                downloads.Add(GetRandomString());
            }

            return await ValueTask.FromResult(downloads);
        }
    }
}
