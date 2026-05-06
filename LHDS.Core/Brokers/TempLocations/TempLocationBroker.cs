// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;

namespace LHDS.Core.Brokers.TempLocations
{
    internal class TempLocationBroker : ITempLocationBroker
    {
        private const string DefaultExtension = ".tmp";
        private const string HomeSubDirectoryName = "tempfiles";
        private readonly string homeEnvironmentVariableName;

        public TempLocationBroker(string homeEnvironmentVariableName = "HOME")
        {
            this.homeEnvironmentVariableName = homeEnvironmentVariableName;
        }

        public string GetTempDirectory() => Path.GetTempPath();

        public string GetHomeDirectory()
        {
            string homeDirectory = ResolveHomeDirectory();
            Directory.CreateDirectory(homeDirectory);
            string subDirectory = Path.Combine(homeDirectory, HomeSubDirectoryName);
            Directory.CreateDirectory(subDirectory);

            return subDirectory;
        }

        public string GetTempFilePath(string fileName)
        {
            ValidateFileName(fileName);
            string tempDirectory = GetTempDirectory();

            return Path.Combine(tempDirectory, fileName);
        }

        public string GetHomeFilePath(string fileName)
        {
            ValidateFileName(fileName);
            string homeDirectory = GetHomeDirectory();

            return Path.Combine(homeDirectory, fileName);
        }

        public string GetUniqueTempFilePath(string extension = DefaultExtension)
        {
            string normalizedExtension = NormalizeExtension(extension);
            string fileName = $"{Guid.NewGuid()}{normalizedExtension}";

            return GetTempFilePath(fileName);
        }

        public string GetUniqueHomeFilePath(string extension = DefaultExtension)
        {
            string normalizedExtension = NormalizeExtension(extension);
            string fileName = $"{Guid.NewGuid()}{normalizedExtension}";

            return GetHomeFilePath(fileName);
        }

        private string ResolveHomeDirectory()
        {
            string homeDirectory =
                Environment.GetEnvironmentVariable(this.homeEnvironmentVariableName);

            if (string.IsNullOrWhiteSpace(homeDirectory))
            {
                homeDirectory =
                    Environment.GetEnvironmentVariable("HOME") ??
                    Environment.GetEnvironmentVariable("USERPROFILE") ??
                    Path.GetTempPath();
            }

            return homeDirectory;
        }

        private static string NormalizeExtension(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                return DefaultExtension;
            }

            return extension.StartsWith('.')
                ? extension
                : $".{extension}";
        }

        private static void ValidateFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(
                    message: "File name is required.",
                    paramName: nameof(fileName));
            }

            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                throw new ArgumentException(
                    message: "File name contains invalid characters.",
                    paramName: nameof(fileName));
            }
        }
    }
}
