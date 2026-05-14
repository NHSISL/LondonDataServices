// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.Files
{
    public class FileBroker : IFileBroker
    {
        public async ValueTask<bool> CheckIfFileExistsAsync(string path) =>
            File.Exists(path);

        public async ValueTask<bool> WriteToFileAsync(string path, string content)
        {
            File.WriteAllText(path, content);

            return true;
        }

        public async ValueTask WriteToFileAsync(string path, Stream inputStream)
        {
            using FileStream fileStream = new FileStream(
                path, FileMode.Create, FileAccess.Write);

            await inputStream.CopyToAsync(fileStream);
        }

        public async ValueTask<byte[]> ReadFileAsync(string path) =>
            await File.ReadAllBytesAsync(path);

        public async ValueTask<Stream> ReadFileStreamAsync(string path) =>
            new FileStream(
                path, FileMode.Open, FileAccess.Read, FileShare.Read);

        public async ValueTask<List<string>> ReadLinesBatchAsync(string path, int batchSize, int skipCounter) =>
            File.ReadLines(path).Skip(skipCounter).Take(batchSize).ToList();

        public async ValueTask<bool> DeleteFileAsync(string path)
        {
            File.Delete(path);

            return true;
        }

        public async ValueTask<List<string>> GetListOfFilesAsync(string path, string searchPattern = "*") =>
            Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories).ToList();

        public async ValueTask<bool> CheckIfDirectoryExistsAsync(string path) =>
            Directory.Exists(path);

        public async ValueTask<bool> CreateDirectoryAsync(string path)
        {
            Directory.CreateDirectory(path);

            return true;
        }

        public async ValueTask<bool> DeleteDirectoryAsync(string path, bool recursive = false)
        {
            Directory.Delete(path, recursive);

            return true;
        }

        public async ValueTask<string> GetTempFileName() =>
            Path.GetTempFileName();

        public async ValueTask<string> GetTempPath() =>
            Path.GetTempPath();
    }
}
