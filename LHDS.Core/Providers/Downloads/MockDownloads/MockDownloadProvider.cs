// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Providers.Downloads.FtpDownloads
{
    public class MockDownloadProvider : IDownloadProvider
    {
        private readonly Renci.SshNet.SftpClient client;
        private readonly IFtpDownloadProviderSettings ftpDownloadProviderSettings;

        public string Name { get; private set; }
        public bool IsMock { get; private set; }

        public MockDownloadProvider()
        {
            this.Name = "MockDownloadProvider";
            this.IsMock = true;
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        public async ValueTask<Document> GetDocumentByFileNameAsync(string fileName)
        {
            string randomString = GetRandomString();
            byte[] data = Encoding.ASCII.GetBytes(randomString);

            var document = new Document()
            {
                FileName = fileName,
                DocumentData = data
            };

            return await ValueTask.FromResult(document);
        }

        public async ValueTask<List<Document>> GetListOfDocumentsToProcessAsync()
        {
            List<Document> documents = new List<Document>();

            for (int i = 0; i < 1; i++)
            {
                Document document = await GetRandomDocumentAsync();
                documents.Add(document);
            }

            return documents;
        }

        private async ValueTask<Document> GetRandomDocumentAsync()
        {
            string randomString = GetRandomString();
            byte[] data = Encoding.ASCII.GetBytes(randomString);

            Document document = new Document()
            {
                FileName = GetRandomString(),
                DocumentData = data
            };

            return document;
        }
    }
}
