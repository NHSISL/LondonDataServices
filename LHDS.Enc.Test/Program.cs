// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
using LHDS.Landings.Client.Providers.Cryptography;
using LHDS.Landings.Client.Providers.Cryptography.Gpg;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;

namespace LHDS.Enc.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Input file
            string inputFilePath = "C:\\temp\\TEST1\\input.txt";
            string outputFilePath = "C:\\temp\\TEST1\\output.txt";
            // Output file
            string encryptedFilePath = "C:\\temp\\TEST1\\encrypted.gpg";
            // Public key file
            string publicKeyFilePath = "C:\\temp\\TEST1\\public.key";
            using (Stream inputFileStream = File.OpenRead(inputFilePath))
            using (Stream publicKeyFileStream = File.OpenRead(publicKeyFilePath))
            using (Stream encryptedFileStream = File.Create(encryptedFilePath))
            {
                // Load the public key
                PgpPublicKey publicKey = ReadPublicKey(publicKeyFileStream);
                // Encrypt the file
                PgpEncryptedDataGenerator encryptedDataGenerator = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Cast5, true, new SecureRandom());

                encryptedDataGenerator.AddMethod(publicKey);
                Stream encryptedStream = encryptedDataGenerator.Open(encryptedFileStream, new byte[1 << 16]);
                PgpCompressedDataGenerator compressedDataGenerator = new PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip);
                Stream compressedStream = compressedDataGenerator.Open(encryptedStream);
                PgpLiteralDataGenerator literalDataGenerator = new PgpLiteralDataGenerator();
                Stream literalStream = literalDataGenerator.Open(compressedStream, PgpLiteralData.Binary, Path.GetFileName(inputFilePath), inputFileStream.Length, DateTime.UtcNow);
                inputFileStream.CopyTo(literalStream);
                literalStream.Close();
                compressedStream.Close();
                encryptedStream.Close();
            }

            var appSettingsStub = new Dictionary<string, string> {
                {"cryptography:gpgPrivateKey", "LS0tLS1CRUdJTiBQR1AgUFJJVkFURSBLRVkgQkxPQ0stLS0tLQoKbElZRVkzWVJhUllKS3dZQkJBSGFSdzhCQVFkQUdFdHdrR0pvb0hsWThXNmNPK0czYWlQY0szdEJ4cHk4TDNyYgptbDhNVFlqK0J3TUM1UTFYMFJ5STFRSEhpb0gyWE9LZy9FbHRURUNaNTlxcjBOMW9iY2NmMDRRSlUzU0xrSUdZCjRwRDhUYzlxSkhXRzI2cnVkbE42MGM3UXhIZWh2aytlRzhFWUpvRmM3NTVIOGdxcThGL2xRTFF2UTNWdWJHbG0KWm1Vc0lFUmhkbWxrSUR4a1lYWnBaQzVqZFc1c2FXWm1aVEpBYm1Wc1kzTjFMbTVvY3k1MWF6NkltUVFURmdvQQpRUlloQkpiS0hsR3B0bmxaK2ZIcmdLc1RSWitJNDh0cEJRSmpkaEZwQWhzREJRa0R3OGxYQlFzSkNBY0NBaUlDCkJoVUtDUWdMQWdRV0FnTUJBaDRIQWhlQUFBb0pFS3NUUlorSTQ4dHBFRVFBL0FvTXVXOElNblhUR0RmbFFsWncKVWZOUkNMYzhaVzF6UThOMmJXQkZPWWVQQVFDODRXV2VqTnFPcXFOb2N6MXBLQVV1Qmg3OFFHQmJZRmRRVDg5NgpzYzhJQjV5TEJHTjJFV2tTQ2lzR0FRUUJsMVVCQlFFQkIwQ281U0ZGQzZoVlgrZVBjNFVRN1BQZFVJejNoR0hOCkNCYVlaS1kvQ2x6akd3TUJDQWYrQndNQzVFelFqWk5lenQ3SGl5cVQrbXF4aUJxVEZNcnNUYVo2c0FTZnhGQ3oKMVZnV2ZjRm1QMkhtWXFiWXRJSkc0QWllRitNdU4xc3puVXZUdEJxMnp0UUVsUGozaWJETG1mZzNxT2VhRTlzSgpzWWgrQkJnV0NnQW1GaUVFbHNvZVVhbTJlVm41OGV1QXF4TkZuNGpqeTJrRkFtTjJFV2tDR3d3RkNRUER5VmNBCkNna1FxeE5GbjRqankybVU5UUVBbk5venM4UzBicGx0TGIzaER3b0ZGdVk5M2R6SURSQ09yZnNqUkpaTS95b0IKQVBDWngzWmh4Z0hBdHdEK0lPRkg5Y205MlYzeHJrY25NOXY3VHNDMFlkZ00KPU9nckgKLS0tLS1FTkQgUEdQIFBSSVZBVEUgS0VZIEJMT0NLLS0tLS0K"},
                {"cryptography:gpgPublicKey", "LS0tLS1CRUdJTiBQR1AgUFVCTElDIEtFWSBCTE9DSy0tLS0tCgptRE1FWTNZUmFSWUpLd1lCQkFIYVJ3OEJBUWRBR0V0d2tHSm9vSGxZOFc2Y08rRzNhaVBjSzN0QnhweThMM3JiCm1sOE1UWWkwTDBOMWJteHBabVpsTENCRVlYWnBaQ0E4WkdGMmFXUXVZM1Z1YkdsbVptVXlRRzVsYkdOemRTNXUKYUhNdWRXcytpSmtFRXhZS0FFRVdJUVNXeWg1UnFiWjVXZm54NjRDckUwV2ZpT1BMYVFVQ1kzWVJhUUliQXdVSgpBOFBKVndVTENRZ0hBZ0lpQWdZVkNna0lDd0lFRmdJREFRSWVCd0lYZ0FBS0NSQ3JFMFdmaU9QTGFSQkVBUHdLCkRMbHZDREoxMHhnMzVVSldjRkh6VVFpM1BHVnRjMFBEZG0xZ1JUbUhqd0VBdk9GbG5vemFqcXFqYUhNOWFTZ0YKTGdZZS9FQmdXMkJYVUUvUGVySFBDQWU0T0FSamRoRnBFZ29yQmdFRUFaZFZBUVVCQVFkQXFPVWhSUXVvVlYvbgpqM09GRU96ejNWQ005NFJoelFnV21HU21Qd3BjNHhzREFRZ0hpSDRFR0JZS0FDWVdJUVNXeWg1UnFiWjVXZm54CjY0Q3JFMFdmaU9QTGFRVUNZM1lSYVFJYkRBVUpBOFBKVndBS0NSQ3JFMFdmaU9QTGFaVDFBUUNjMmpPenhMUnUKbVcwdHZlRVBDZ1VXNWozZDNNZ05FSTZ0K3lORWxrei9LZ0VBOEpuSGRtSEdBY0MzQVA0ZzRVZjF5YjNaWGZHdQpSeWN6Mi90T3dMUmgyQXc9Cj1PZ3Y0Ci0tLS0tRU5EIFBHUCBQVUJMSUMgS0VZIEJMT0NLLS0tLS0K"},
                {"cryptography:gpgPassphrase", "Vh68@KIiQ7^JM@l1vVRpmVHZpolmAhmHAnd!nPa0MdBkKT8p$N&XRU63!CakUMfVHwe9K@q7bszmUQyR@pOqFfdnKvoT^xA&3ZG"},
            };

            var inMemoryConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            var gpgCryptographyProviderSettings = new GpgCryptographyProviderSettings(inMemoryConfiguration);
            var cryptographyProvider = new GpgCryptographyProvider(gpgCryptographyProviderSettings);

            var res = cryptographyProvider.DecryptAsync(File.ReadAllBytes(encryptedFilePath)).Result;

            File.WriteAllBytes(outputFilePath, res);
        }
        private static PgpPublicKey ReadPublicKey(Stream publicKeyStream)
        {
            PgpPublicKeyRingBundle publicKeyRingBundle = new PgpPublicKeyRingBundle(PgpUtilities.GetDecoderStream(publicKeyStream));
            foreach (PgpPublicKeyRing publicKeyRing in publicKeyRingBundle.GetKeyRings())
            {
                foreach (PgpPublicKey publicKey in publicKeyRing.GetPublicKeys())
                {
                    if (publicKey.IsEncryptionKey)
                    {
                        return publicKey;
                    }
                }
            }
            throw new ArgumentException("No encryption key found in the public key file");
        }
    }
}