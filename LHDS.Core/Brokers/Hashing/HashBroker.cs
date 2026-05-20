// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.Hashing
{
    public class HashBroker : IHashBroker
    {

        public async ValueTask<string> GenerateMd5HashAsync(Stream? data)
        {
            if (data == null)
            {
                return string.Empty;
            }

            using (MD5 md5 = MD5.Create())
            {
                data.Position = 0;
                byte[] hashBytes = md5.ComputeHash(data);
                var md5Hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return md5Hash;
            }
        }

        public async ValueTask<string> GenerateSha256HashAsync(Stream? data, string? pepper = null)
        {
            if (data == null || data.Length == 0)
            {
                return string.Empty;
            }

            data.Position = 0;
            using var sha256 = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
            byte[] buffer = new byte[81920];
            int bytesRead;

            while ((bytesRead = await data.ReadAsync(buffer)) > 0)
            {
                sha256.AppendData(buffer, 0, bytesRead);
            }

            if (!string.IsNullOrEmpty(pepper))
            {
                sha256.AppendData(System.Text.Encoding.UTF8.GetBytes(pepper));
            }

            byte[] hashBytes = sha256.GetHashAndReset();

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }

        public async ValueTask<string> GenerateSha256HashAsync(string? data, string? pepper = null)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }

            byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(data);

            byte[] pepperBytes = !string.IsNullOrEmpty(pepper)
                ? System.Text.Encoding.UTF8.GetBytes(pepper)
                : Array.Empty<byte>();

            byte[] combined = dataBytes.Concat(pepperBytes).ToArray();
            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(combined);

            var sha256Hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

            return sha256Hash;
        }
    }
}
