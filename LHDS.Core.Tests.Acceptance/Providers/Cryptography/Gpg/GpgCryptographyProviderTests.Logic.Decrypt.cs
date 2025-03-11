// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Providers.Cryptography.Gpg
{
    public partial class GpgCryptographyProviderTests
    {
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
            int sizeInGb = GetRandomNumber();
            long fileSize = sizeInGb * 1024L * 1024L * 1024L;
            string tempFilePath = Path.GetTempFileName();
            string encryptedFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".enc");
            string decryptedFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".dec");

            byte[] buffer = new byte[8192];
            Random random = new Random();

            // Generate large file efficiently
            using (FileStream fileStream = new FileStream(
                path: tempFilePath,
                mode: FileMode.Create,
                access: FileAccess.Write,
                share: FileShare.None,
                bufferSize: buffer.Length,
                options: FileOptions.SequentialScan))
            {
                long bytesWritten = 0;
                while (bytesWritten < fileSize)
                {
                    random.NextBytes(buffer);
                    long bytesToWrite = Math.Min(buffer.Length, fileSize - bytesWritten);
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

            // Ensure the encrypted file exists before proceeding
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
            // Wrap the encrypted stream to prevent premature disposal
            using (Stream nonDisposingEncryptedStream = new NonDisposingStreamWrapper(encryptedStream))
            using (BufferedStream bufferedEncryptedStream = new BufferedStream(nonDisposingEncryptedStream))
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

            // Cleanup
            File.Delete(tempFilePath);
            File.Delete(encryptedFilePath);
            File.Delete(decryptedFilePath);
        }

        // A stream wrapper that does not dispose its inner stream, including asynchronous disposal.
        public class NonDisposingStreamWrapper : Stream
        {
            private readonly Stream innerStream;
            public NonDisposingStreamWrapper(Stream innerStream)
            {
                this.innerStream = innerStream;
            }
            public override bool CanRead => innerStream.CanRead;
            public override bool CanSeek => innerStream.CanSeek;
            public override bool CanWrite => innerStream.CanWrite;
            public override long Length => innerStream.Length;
            public override long Position { get => innerStream.Position; set => innerStream.Position = value; }
            public override void Flush() => innerStream.Flush();
            public override int Read(byte[] buffer, int offset, int count) => innerStream.Read(buffer, offset, count);
            public override long Seek(long offset, SeekOrigin origin) => innerStream.Seek(offset, origin);
            public override void SetLength(long value) => innerStream.SetLength(value);
            public override void Write(byte[] buffer, int offset, int count) => innerStream.Write(buffer, offset, count);

            protected override void Dispose(bool disposing)
            {
                // Prevent disposal of the inner stream.
            }
            public override async ValueTask DisposeAsync()
            {
                // Prevent asynchronous disposal of the inner stream.
                await Task.CompletedTask;
            }
            public override void Close()
            {
                // Prevent closing the inner stream.
            }
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
    }
}