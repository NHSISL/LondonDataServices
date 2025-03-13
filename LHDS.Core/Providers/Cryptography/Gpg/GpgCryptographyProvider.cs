// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;

namespace LHDS.Core.Providers.Cryptography.Gpg
{
    public class GpgCryptographyProvider : ICryptographyProvider
    {
        public async ValueTask EncryptAsync(Stream input, Stream output, SubscriberCredential subscriberCredential)
        {
            var publicKeyDecoded = Convert.FromBase64String(subscriberCredential.GpgPublicKey ?? "");

            using (Stream publicKeyFileStream = new MemoryStream(publicKeyDecoded))
            {
                PgpPublicKey publicKey = ReadPublicKey(publicKeyFileStream);

                PgpEncryptedDataGenerator encryptedDataGenerator =
                    new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Cast5, true, new SecureRandom());

                encryptedDataGenerator.AddMethod(publicKey);
                Stream encryptedStream = encryptedDataGenerator.Open(output, new byte[1 << 16]);

                PgpCompressedDataGenerator compressedDataGenerator =
                    new PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip);

                Stream compressedStream = compressedDataGenerator.Open(encryptedStream);
                PgpLiteralDataGenerator literalDataGenerator = new PgpLiteralDataGenerator();
                input.Position = 0;

                Stream literalStream = literalDataGenerator
                    .Open(
                        outStr: compressedStream,
                        format: PgpLiteralData.Binary,
                        name: string.Empty,
                        length: input.Length,
                        modificationTime: DateTime.UtcNow);

                await input.CopyToAsync(literalStream);
                literalStream.Close();
                compressedStream.Close();
                encryptedStream.Close();
            }
        }

        public async ValueTask DecryptAsync(Stream input, Stream output, SubscriberCredential subscriberCredential)
        {
            using var bufferedInput = new BufferedStream(input);
            var privateKeyDecoded = Convert.FromBase64String(subscriberCredential.GpgPrivateKey ?? "");
            char[] privateKeyPassphrase = subscriberCredential?.GpgPassPhrase?.ToCharArray() ?? Array.Empty<char>();

            if (bufferedInput.CanSeek)
                bufferedInput.Position = 0;

            using var decoderStream = PgpUtilities.GetDecoderStream(bufferedInput);
            PgpObjectFactory pgpFactory = new PgpObjectFactory(decoderStream);

            PgpEncryptedDataList? encryptedDataList = null;
            PgpObject? pgpObject;

            while ((pgpObject = pgpFactory.NextPgpObject()) != null)
            {
                if (pgpObject is PgpEncryptedDataList list)
                {
                    encryptedDataList = list;
                    break;
                }
            }

            if (encryptedDataList == null)
                throw new ArgumentException("No encrypted data found in the input.");

            PgpPrivateKey? privateKey = null;
            PgpPublicKeyEncryptedData? encryptedData = null;

            foreach (PgpPublicKeyEncryptedData encryptedDataItem in encryptedDataList.GetEncryptedDataObjects())
            {
                using var keyStream = new MemoryStream(privateKeyDecoded);
                privateKey = FindPrivateKey(keyStream, encryptedDataItem.KeyId, privateKeyPassphrase);

                if (privateKey != null)
                {
                    encryptedData = encryptedDataItem;
                    break;
                }
            }

            if (privateKey == null)
                throw new ArgumentException("Private key not found in the key file.");

            if (encryptedData == null)
                throw new ArgumentException("No matching encrypted data found.");

            using var decryptedStream = encryptedData.GetDataStream(privateKey);
            var decryptedFactory = new PgpObjectFactory(decryptedStream);

            // Instead of disposing the compressed data stream immediately,
            // we hold a reference to it and dispose it after copying literal data.
            Stream? compressedDataStream = null;
            while ((pgpObject = decryptedFactory.NextPgpObject()) != null)
            {
                if (pgpObject is PgpCompressedData compressedData)
                {
                    compressedDataStream = compressedData.GetDataStream();
                    var compressedFactory = new PgpObjectFactory(compressedDataStream);
                    pgpObject = compressedFactory.NextPgpObject();
                    break;
                }
                else
                {
                    break;
                }
            }

            if (pgpObject is PgpLiteralData literalData)
            {
                using Stream literalStream = literalData.GetInputStream();
                await literalStream.CopyToAsync(output, bufferSize: 8 * 1024 * 1024);
                if (compressedDataStream != null)
                {
                    await compressedDataStream.DisposeAsync();
                }
                return;
            }

            if (compressedDataStream != null)
            {
                await compressedDataStream.DisposeAsync();
            }

            throw new ArgumentException("No valid literal data found in decrypted content.");
        }

        private static PgpPublicKey ReadPublicKey(Stream inputStream)
        {
            inputStream = PgpUtilities.GetDecoderStream(inputStream);
            PgpPublicKeyRingBundle publicKeyRingBundle = new PgpPublicKeyRingBundle(inputStream);
            PgpPublicKey publicKey = GetFirstPublicKey(publicKeyRingBundle);

            if (publicKey == null)
            {
                throw new ArgumentException("No encryption key found in public key ring.");
            }

            return publicKey;
        }

        private static PgpPublicKey GetFirstPublicKey(PgpPublicKeyRingBundle publicKeyRingBundle)
        {
            foreach (PgpPublicKeyRing keyRing in publicKeyRingBundle.GetKeyRings())
            {
                foreach (PgpPublicKey key in keyRing.GetPublicKeys())
                {
                    if (key.IsEncryptionKey)
                    {
                        return key;
                    }
                }
            }

            throw new ArgumentException("No public key found in key ring.");
        }

        private static PgpPrivateKey FindPrivateKey(Stream privateKeyStream, long keyId, char[] passphrase)
        {
            var decoderStream = PgpUtilities.GetDecoderStream(privateKeyStream);

            PgpSecretKeyRingBundle? secretKeyRingBundle =
                new PgpSecretKeyRingBundle(decoderStream);

            PgpSecretKey? secretKey = secretKeyRingBundle.GetSecretKey(keyId);

            if (secretKey != null)
            {
                return secretKey.ExtractPrivateKey(passphrase);
            }

            throw new ArgumentException("No secret key found in key ring.");
        }
    }
}
