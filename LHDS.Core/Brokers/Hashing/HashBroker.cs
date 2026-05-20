// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace LHDS.Core.Brokers.Hashing
{
    public class HashBroker : IHashBroker
    {

        public async ValueTask<string> GenerateMd5HashAsync(
            Stream? data,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (data is null)
            {
                return string.Empty;
            }

            if (data.CanSeek)
            {
                data.Seek(0, SeekOrigin.Begin);
            }

            using var md5 =
                IncrementalHash.CreateHash(HashAlgorithmName.MD5);

            byte[] buffer = new byte[81920];
            int bytesRead;

            while ((bytesRead =
                await data.ReadAsync(buffer, cancellationToken)) > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                md5.AppendData(buffer, 0, bytesRead);
            }

            byte[] hashBytes = md5.GetHashAndReset();

            return Convert.ToHexString(hashBytes)
                .ToLowerInvariant();
        }

        public async ValueTask<string> GenerateSha256HashAsync(
            Stream? data,
            string? pepper = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (data is null)
            {
                return string.Empty;
            }

            if (data.CanSeek)
            {
                data.Seek(0, SeekOrigin.Begin);
            }

            using var sha256 =
                IncrementalHash.CreateHash(HashAlgorithmName.SHA256);

            byte[] buffer = new byte[81920];
            int bytesRead;

            while ((bytesRead =
                await data.ReadAsync(buffer, cancellationToken)) > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                sha256.AppendData(buffer, 0, bytesRead);
            }

            if (!string.IsNullOrWhiteSpace(pepper))
            {
                sha256.AppendData(
                    Encoding.UTF8.GetBytes(pepper));
            }

            byte[] hashBytes = sha256.GetHashAndReset();

            return Convert.ToHexString(hashBytes)
                .ToLowerInvariant();
        }

        public async ValueTask<string> GenerateSha256HashAsync(string? data, string? pepper = null)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }

            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            byte[] pepperBytes = !string.IsNullOrEmpty(pepper)
                ? Encoding.UTF8.GetBytes(pepper)
                : Array.Empty<byte>();

            byte[] combined = dataBytes.Concat(pepperBytes).ToArray();
            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(combined);

            var sha256Hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

            return sha256Hash;
        }
    }
}
