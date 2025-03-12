// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Providers.Cryptography.Gpg
{
    public partial class GpgCryptographyProviderTests : IDisposable
    {
        private readonly List<string> tempFiles = new();

        [Fact]
        public async Task ShouldEncryptAndDecryptStringAsync()
        {
            // Given
            string randomString = GetRandomString();
            byte[] randomBytes = Encoding.UTF8.GetBytes(randomString);
            string expectedString = randomString;
            byte[] decryptedBytes;

            // When
            using (Stream inputStream = new MemoryStream(randomBytes))
            using (Stream encryptedStream = new MemoryStream())
            using (Stream decryptedStream = new MemoryStream())
            {
                await this.cryptographyProvider
                    .EncryptAsync(input: inputStream, output: encryptedStream, subscriberCredential);

                await this.cryptographyProvider
                    .DecryptAsync(input: encryptedStream, output: decryptedStream, subscriberCredential);

                decryptedBytes = ReadAllBytesFromStream(stream: decryptedStream);
            }

            // Then
            string actualString = Encoding.UTF8.GetString(decryptedBytes);
            actualString.Should().BeEquivalentTo(expectedString);
        }

        [Fact]
        public async Task ShouldEncryptAndDecryptLargeFilesAsync()
        {
            // Given
            double sizeInGb = 1.5; // GetRandomNumber();
            output.WriteLine($"File size generated: {sizeInGb}Gb ");
            double fileSize = sizeInGb * 1024L * 1024L * 1024L;
            string tempFilePath = Path.GetTempFileName();
            string encryptedFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".enc");
            string decryptedFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".dec");

            tempFiles.Add(tempFilePath);
            tempFiles.Add(encryptedFilePath);
            tempFiles.Add(decryptedFilePath);

            byte[] buffer = new byte[8192];
            Random random = new Random();

            using (FileStream fileStream = new FileStream(
                path: tempFilePath,
                mode: FileMode.Create,
                access: FileAccess.Write,
                share: FileShare.None,
                bufferSize: buffer.Length,
                options: FileOptions.SequentialScan))
            {
                double bytesWritten = 0;
                while (bytesWritten < fileSize)
                {
                    random.NextBytes(buffer);
                    double bytesToWrite = Math.Min(buffer.Length, fileSize - bytesWritten);
                    await fileStream.WriteAsync(buffer.AsMemory(0, (int)bytesToWrite));
                    bytesWritten += bytesToWrite;
                }
            }

            // When - Encryption
            using (Stream inputStream = new FileStream(
                path: tempFilePath,
                mode: FileMode.Open,
                access: FileAccess.Read,
                share: FileShare.Read,
                bufferSize: buffer.Length,
                options: FileOptions.SequentialScan))
            using (Stream encryptedFileStream = new FileStream(
                path: encryptedFilePath,
                mode: FileMode.Create,
                access: FileAccess.Write,
                share: FileShare.None,
                bufferSize: buffer.Length,
                options: FileOptions.SequentialScan))
            {
                await this.cryptographyProvider.EncryptAsync(inputStream, encryptedFileStream, subscriberCredential);
                await encryptedFileStream.FlushAsync();  // Ensure the encryption stream is flushed and ready
            }

            if (!File.Exists(encryptedFilePath) || new FileInfo(encryptedFilePath).Length == 0)
                throw new InvalidOperationException("Encryption failed, encrypted file is empty.");

            // When - Decryption
            using (Stream encryptedStream = new FileStream(
                path: encryptedFilePath,
                mode: FileMode.Open,
                access: FileAccess.Read,
                share: FileShare.Read,
                bufferSize: buffer.Length,
                options: FileOptions.SequentialScan))
            using (BufferedStream bufferedEncryptedStream = new BufferedStream(encryptedStream))
            using (Stream decryptedStream = new FileStream(
                path: decryptedFilePath,
                mode: FileMode.Create,
                access: FileAccess.Write,
                share: FileShare.None,
                bufferSize: buffer.Length,
                options: FileOptions.SequentialScan))
            {
                await this.cryptographyProvider.DecryptAsync(bufferedEncryptedStream, decryptedStream, subscriberCredential);
                await decryptedStream.FlushAsync();  // Ensure the decrypted stream is flushed and ready
            }

            // Then
            if (!File.Exists(decryptedFilePath) || new FileInfo(decryptedFilePath).Length == 0)
                throw new InvalidOperationException("Decryption failed, decrypted file is empty.");

            bool filesMatch = CompareFiles(tempFilePath, decryptedFilePath);
            filesMatch.Should().BeTrue();
        }

        private bool CompareFiles(string path1, string path2)
        {
            const int bufferSize = 8192;
            using (FileStream fs1 = new FileStream(path1, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.SequentialScan))
            using (FileStream fs2 = new FileStream(path2, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.SequentialScan))
            {
                byte[] buffer1 = new byte[bufferSize];
                byte[] buffer2 = new byte[bufferSize];

                int bytesRead1, bytesRead2;
                while ((bytesRead1 = fs1.Read(buffer1, 0, bufferSize)) > 0 &&
                       (bytesRead2 = fs2.Read(buffer2, 0, bufferSize)) > 0)
                {
                    if (bytesRead1 != bytesRead2 || !buffer1.AsSpan(0, bytesRead1).SequenceEqual(buffer2.AsSpan(0, bytesRead2)))
                        return false;
                }
                return true;
            }
        }

        public void Dispose()
        {
            foreach (string file in tempFiles)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                    output.WriteLine($"Deleted: {file}");
                }
            }
        }
    }
}