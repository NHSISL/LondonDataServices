// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Models.Providers.FtpDownloads.Exceptions;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace LHDS.Landings.Client.Providers.Downloads.FtpDownloads
{
    public class FtpDownloadProvider : IDownloadProvider
    {
        private readonly Renci.SshNet.SftpClient client;
        private readonly IFtpDownloadProviderSettings ftpDownloadProviderSettings;

        public FtpDownloadProvider(IFtpDownloadProviderSettings ftpDownloadProviderSettings)
        {
            this.ftpDownloadProviderSettings = ftpDownloadProviderSettings;
            client = new SftpClient(GetConnectionInfo(ftpDownloadProviderSettings));
            //this.EnsureClientIsConnected();
        }

        public async ValueTask<Document> GetDocumentByFileNameAsync(string fileName)
        {
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
            try
            {
                if (client.IsConnected)
                {
                    return;
                }

                client.Connect();

                if (!client.IsConnected)
                {
                    throw new FailedToConnectSftpClientException();
                }
            }
            catch (SshOperationTimeoutException ex)
            {
                EnsureClientIsConnected();
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
