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
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Providers.FtpDownloads.Exceptions;
using Renci.SshNet;

namespace LHDS.Core.Providers.Downloads.FtpDownloads
{
    public class FtpDownloadProvider : IDownloadProvider
    {
        private readonly IFtpDownloadProviderSettings ftpDownloadProviderSettings;
        public string Name { get; private set; }
        public bool IsOfflineProvider { get; private set; }

        public FtpDownloadProvider(IFtpDownloadProviderSettings ftpDownloadProviderSettings)
        {
            this.ftpDownloadProviderSettings = ftpDownloadProviderSettings;
            this.Name = "FtpDownloadProvider";
            this.IsOfflineProvider = false;
        }

        public async ValueTask<Download> GetDocumentByFileNameAsync(Download download)
        {
            IFtpDownloadProviderSettings settings = new FtpDownloadProviderSettings
            {
                FtpServer = this.ftpDownloadProviderSettings.FtpServer,
                FtpPort = this.ftpDownloadProviderSettings.FtpPort,
                FtpRootFolder = this.ftpDownloadProviderSettings.FtpRootFolder,
                IncludeSubDirectories = this.ftpDownloadProviderSettings.IncludeSubDirectories,
                FtpUserName = download.SubscriberCredential.FtpUserName,
                FtpPassword = download.SubscriberCredential.FtpPassword,
                FtpPassPhrase = download.SubscriberCredential.FtpPassPhrase,
                FtpPrivateKey = download.SubscriberCredential.FtpPrivateKey
            };

            using (SftpClient client = new SftpClient(GetConnectionInfo(settings)))
            {
                this.EnsureClientIsConnected(client);

                var attrs = client.GetAttributes(download.Document.FileName);
                MemoryStream stream = new MemoryStream();
                client.DownloadFile(download.Document.FileName, stream);
                byte[] data = stream.ToArray();

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

                client.Disconnect();
                client.Dispose();

                return await ValueTask.FromResult(downloadedItem);
            }
        }

        public async ValueTask<List<string>> GetListOfDocumentsToProcessAsync(Download download)
        {
            IFtpDownloadProviderSettings settings = new FtpDownloadProviderSettings
            {
                FtpServer = this.ftpDownloadProviderSettings.FtpServer,
                FtpPort = this.ftpDownloadProviderSettings.FtpPort,
                FtpRootFolder = this.ftpDownloadProviderSettings.FtpRootFolder,
                IncludeSubDirectories = this.ftpDownloadProviderSettings.IncludeSubDirectories,
                FtpUserName = download.SubscriberCredential.FtpUserName,
                FtpPassword = download.SubscriberCredential.FtpPassword,
                FtpPassPhrase = download.SubscriberCredential.FtpPassPhrase,
                FtpPrivateKey = download.SubscriberCredential.FtpPrivateKey
            };

            using (SftpClient client = new SftpClient(GetConnectionInfo(settings)))
            {
                this.EnsureClientIsConnected(client);

                var items = await this.GetListOfDocumentsToProcessAsync(
                    client,
                    download,
                    path: ftpDownloadProviderSettings.FtpRootFolder,
                    includeSubDirectories: ftpDownloadProviderSettings.IncludeSubDirectories);

                client.Disconnect();
                client.Dispose();

                return items;
            }
        }

        private async ValueTask<List<string>> GetListOfDocumentsToProcessAsync(
            SftpClient client,
            Download download,
            string path,
            bool includeSubDirectories)
        {
            var downloads = new List<string>();

            var currDir = path == "/" ? client.WorkingDirectory : path;
            var files = client.ListDirectory(currDir).ToList();

            if (!includeSubDirectories)
            {
                return files.Select(x => x.FullName).ToList();
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
                        client,
                        download,
                        path: remotefile.FullName,
                        includeSubDirectories);

                    downloads.AddRange(items);
                }
                else
                {
                    downloads.Add(remotefile.FullName);
                }
            }

            return await ValueTask.FromResult(downloads);
        }

        private void EnsureClientIsConnected(SftpClient client)
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

            if (ftpDownloadProviderSettings.FtpPrivateKey != null && ftpDownloadProviderSettings.FtpPassPhrase != null)
            {
                var pkf = GetKeyFromB64(
                    encodedKey: ftpDownloadProviderSettings.FtpPrivateKey,
                    passPhrase: ftpDownloadProviderSettings.FtpPassPhrase);

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
                var privateKeyFile = passPhrase switch
                {
                    "" => new PrivateKeyFile(stream),
                    _ => new PrivateKeyFile(stream, passPhrase)
                };

                return privateKeyFile;
            }
        }
    }
}
