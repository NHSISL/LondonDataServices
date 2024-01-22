// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Providers.FtpDownloads.Exceptions;
using Renci.SshNet;

namespace LHDS.Core.Providers.Downloads.FtpDownloads
{
    public class FtpDownloadProvider : IDownloadProvider
    {
        private readonly Renci.SshNet.SftpClient client;
        private readonly IFtpDownloadProviderSettings ftpDownloadProviderSettings;
        public string Name { get; private set; }
        public bool IsMock { get; private set; }

        public FtpDownloadProvider(IFtpDownloadProviderSettings ftpDownloadProviderSettings)
        {

            this.Name = "FtpDownloadProvider";
            this.ftpDownloadProviderSettings = ftpDownloadProviderSettings;
            client = new SftpClient(GetConnectionInfo(ftpDownloadProviderSettings));
        }

        public async ValueTask<Document> GetDocumentByFileNameAsync(string fileName)
        {
            this.EnsureClientIsConnected();

            var attrs = client.GetAttributes(fileName);
            MemoryStream stream = new MemoryStream();
            this.client.DownloadFile(fileName, stream);
            byte[] data = stream.ToArray();

            var document = new Document()
            {
                FileName = fileName,
                DocumentData = data
            };

            return await ValueTask.FromResult(document);
        }

        public async ValueTask<List<Document>> GetListOfDocumentsToProcessAsync()
        {
            this.EnsureClientIsConnected();

            return await this.GetListOfDocumentsToProcessAsync(
                path: ftpDownloadProviderSettings.FtpRootFolder,
                includeSubDirectories: ftpDownloadProviderSettings.IncludeSubDirectories);
        }

        private async ValueTask<List<Document>> GetListOfDocumentsToProcessAsync(
            string path,
            bool includeSubDirectories)
        {
            var documents = new List<Document>();

            var currDir = path == "/" ? this.client.WorkingDirectory : path;
            var files = this.client.ListDirectory(currDir).ToList();

            if (!includeSubDirectories)
            {
                return files.Select(x => new Document { FileName = x.FullName }).ToList();
            }

            foreach (var remotefile in files)
            {
                if (remotefile.Name == "." || remotefile.Name == "..")
                {
                    continue;
                }

                if (remotefile.IsDirectory)
                {
                    var items = await this.GetListOfDocumentsToProcessAsync(
                        path: remotefile.FullName,
                        includeSubDirectories);

                    documents.AddRange(items);
                }
                else
                {
                    documents.Add(new Document { FileName = remotefile.FullName });
                }
            }

            return await ValueTask.FromResult(documents);
        }

        private void EnsureClientIsConnected()
        {
            var attempts = 0;

            while (true)
            {
                attempts++;

                if (client.IsConnected)
                {
                    return;
                }

                client.Connect();
                Thread.Sleep(500);

                if (!client.IsConnected && attempts > 3)
                {
                    throw new FailedToConnectSftpClientException();
                }
            }
        }

        private ConnectionInfo GetConnectionInfo(IFtpDownloadProviderSettings ftpDownloadProviderSettings)
        {
            List<AuthenticationMethod> methods = new();

            if (ftpDownloadProviderSettings.FtpPassword != null)
            {
                methods.Add(new PasswordAuthenticationMethod(
                    username: ftpDownloadProviderSettings.FtpUserName,
                    password: ftpDownloadProviderSettings.FtpPassword));
            }

            if (ftpDownloadProviderSettings.FtpKey != null && ftpDownloadProviderSettings.FtpPassPhrase != null)
            {
                var pkf = GetKeyFromB64(ftpDownloadProviderSettings.FtpKey, ftpDownloadProviderSettings.FtpPassPhrase);
                methods.Add(new PrivateKeyAuthenticationMethod(ftpDownloadProviderSettings.FtpUserName, pkf));
            }

            return new ConnectionInfo(
                                    ftpDownloadProviderSettings.FtpServer,
                                    ftpDownloadProviderSettings.FtpPort,
                                    ftpDownloadProviderSettings.FtpUserName,
                                    methods.ToArray());
        }

        private PrivateKeyFile GetKeyFromB64(string encodedKey, string passPhrase)
        {
            byte[] bytes = Convert.FromBase64String(encodedKey);

            using (MemoryStream stream = new(bytes))
            {
                var privateKeyFile = new PrivateKeyFile(stream, passPhrase);
                return privateKeyFile;
            }
        }
    }
}
