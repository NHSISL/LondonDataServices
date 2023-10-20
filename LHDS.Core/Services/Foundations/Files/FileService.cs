// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Files;
using LHDS.Core.Models.Configurations.Retries;

namespace LHDS.Core.Services.Foundations.Files
{
    internal partial class FileService : IFileService
    {
        private readonly IFileBroker fileBroker;
        private readonly IRetryConfig retryConfig;

        public FileService(IFileBroker fileBroker, IRetryConfig retryConfig)
        {
            this.fileBroker = fileBroker;
            this.retryConfig = retryConfig;
        }

        public ValueTask<bool> CheckIfDirectoryExistsAsync(string path) =>
            throw new System.NotImplementedException();

        public ValueTask<bool> CheckIfFileExistsAsync(string path) =>
            throw new System.NotImplementedException();

        public ValueTask<bool> CreateDirectoryAsync(string path) =>
            throw new System.NotImplementedException();

        public ValueTask<bool> DeleteDirectoryAsync(string path, bool recursive = false) =>
            throw new System.NotImplementedException();

        public ValueTask<bool> DeleteFileAsync(string path) =>
            throw new System.NotImplementedException();

        public ValueTask<string> ReadFromFileAsync(string path) =>
            throw new System.NotImplementedException();

        public ValueTask<List<string>> RetrieveListOfFilesAsync(string path, string searchPattern = "*") =>
            throw new System.NotImplementedException();

        public ValueTask<bool> WriteToFileAsync(string path, string content) =>
            throw new System.NotImplementedException();
    }
}
