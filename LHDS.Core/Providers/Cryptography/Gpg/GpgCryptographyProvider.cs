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
        [Obsolete]
        public async ValueTask<byte[]> EncryptAsync(byte[] data, SubscriberCredential subscriberCredential)
        {
            var publicKeyDecoded = Convert.FromBase64String(subscriberCredential.GpgPublicKey ?? "");

            using (Stream inputFileStream = new MemoryStream(data))
            using (Stream publicKeyFileStream = new MemoryStream(publicKeyDecoded))
            using (MemoryStream encryptedFileStream = new MemoryStream())
            {
                PgpPublicKey publicKey = ReadPublicKey(publicKeyFileStream);

                PgpEncryptedDataGenerator encryptedDataGenerator =
                    new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Cast5, true, new SecureRandom());

                encryptedDataGenerator.AddMethod(publicKey);
                Stream encryptedStream = encryptedDataGenerator.Open(encryptedFileStream, new byte[1 << 16]);

                PgpCompressedDataGenerator compressedDataGenerator =
                    new PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip);

                Stream compressedStream = compressedDataGenerator.Open(encryptedStream);
                PgpLiteralDataGenerator literalDataGenerator = new PgpLiteralDataGenerator();

                Stream literalStream = literalDataGenerator
                    .Open(
                        outStr: compressedStream,
                        format: PgpLiteralData.Binary,
                        name: string.Empty,
                        length: inputFileStream.Length,
                        modificationTime: DateTime.UtcNow);

                inputFileStream.CopyTo(literalStream);
                literalStream.Close();
                compressedStream.Close();
                encryptedStream.Close();
                var result = encryptedFileStream.ToArray();

                return await ValueTask.FromResult(result);
            }
        }

        [Obsolete]
        public async ValueTask<byte[]> DecryptAsync(byte[] data, SubscriberCredential subscriberCredential)
        {
            var privateKeyDecoded = Convert.FromBase64String(subscriberCredential.GpgPrivateKey ?? "");
            char[] privateKeyPassphrase = subscriberCredential?.GpgPassPhrase?.ToCharArray() ?? Array.Empty<char>();

            using (Stream encryptedFileStream = new MemoryStream(data))
            using (Stream privateKeyFileStream = new MemoryStream(privateKeyDecoded))
            {
                PgpObjectFactory pgpFactory = new PgpObjectFactory(PgpUtilities.GetDecoderStream(encryptedFileStream));
                PgpEncryptedDataList encryptedDataList = (PgpEncryptedDataList)pgpFactory.NextPgpObject();

                PgpPrivateKey? privateKey = null;
                PgpPublicKeyEncryptedData? encryptedData = null;

                foreach (PgpPublicKeyEncryptedData encryptedDataItem in encryptedDataList.GetEncryptedDataObjects())
                {
                    privateKey = FindPrivateKey(
                        privateKeyStream: privateKeyFileStream,
                        keyId: encryptedDataItem.KeyId,
                        passphrase: privateKeyPassphrase);

                    if (privateKey != null)
                    {
                        encryptedData = encryptedDataItem;
                        break;
                    }
                }

                if (privateKey == null)
                {
                    throw new ArgumentException("Private key not found in the key file");
                }

                if (encryptedData == null)
                {
                    throw new ArgumentException("No encrypted data found in the message");
                }

                Stream decryptedStream = encryptedData.GetDataStream(privateKey);
                PgpObjectFactory decryptedFactory = new PgpObjectFactory(inputStream: decryptedStream);
                PgpObject pgpObject = decryptedFactory.NextPgpObject();

                if (pgpObject is PgpCompressedData)
                {
                    PgpCompressedData compressedData = (PgpCompressedData)pgpObject;
                    PgpObjectFactory compressedFactory = new PgpObjectFactory(compressedData.GetDataStream());
                    pgpObject = compressedFactory.NextPgpObject();
                }

                byte[] decryptedData = ""u8.ToArray();

                if (pgpObject is PgpLiteralData)
                {
                    PgpLiteralData literalData = (PgpLiteralData)pgpObject;

                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        Stream literalStream = literalData.GetInputStream();
                        literalStream.CopyTo(outputStream);
                        decryptedData = outputStream.ToArray();
                    }
                }

                return await ValueTask.FromResult(decryptedData);
            }
        }

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
            var privateKeyDecoded = Convert.FromBase64String(subscriberCredential.GpgPrivateKey ?? "");
            char[] privateKeyPassphrase = subscriberCredential?.GpgPassPhrase?.ToCharArray() ?? Array.Empty<char>();

            using (Stream privateKeyFileStream = new MemoryStream(privateKeyDecoded))
            {
                input.Position = 0;
                PgpObjectFactory pgpFactory = new PgpObjectFactory(PgpUtilities.GetDecoderStream(input));
                PgpEncryptedDataList encryptedDataList = (PgpEncryptedDataList)pgpFactory.NextPgpObject();

                PgpPrivateKey? privateKey = null;
                PgpPublicKeyEncryptedData? encryptedData = null;

                foreach (PgpPublicKeyEncryptedData encryptedDataItem in encryptedDataList.GetEncryptedDataObjects())
                {
                    privateKey = FindPrivateKey(
                        privateKeyStream: privateKeyFileStream,
                        keyId: encryptedDataItem.KeyId,
                        passphrase: privateKeyPassphrase);

                    if (privateKey != null)
                    {
                        encryptedData = encryptedDataItem;
                        break;
                    }
                }

                if (privateKey == null)
                {
                    throw new ArgumentException("Private key not found in the key file");
                }

                if (encryptedData == null)
                {
                    throw new ArgumentException("No encrypted data found in the message");
                }

                Stream decryptedStream = encryptedData.GetDataStream(privateKey);
                PgpObjectFactory decryptedFactory = new PgpObjectFactory(inputStream: decryptedStream);
                PgpObject pgpObject = decryptedFactory.NextPgpObject();

                if (pgpObject is PgpCompressedData)
                {
                    PgpCompressedData compressedData = (PgpCompressedData)pgpObject;
                    PgpObjectFactory compressedFactory = new PgpObjectFactory(compressedData.GetDataStream());
                    pgpObject = compressedFactory.NextPgpObject();
                }

                if (pgpObject is PgpLiteralData)
                {
                    PgpLiteralData literalData = (PgpLiteralData)pgpObject;
                    Stream literalStream = literalData.GetInputStream();
                    await literalStream.CopyToAsync(output);
                }
            }
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
