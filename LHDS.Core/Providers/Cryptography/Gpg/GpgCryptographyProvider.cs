// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;

namespace LHDS.Core.Providers.Cryptography.Gpg
{
    public class GpgCryptographyProvider : ICryptographyProvider
    {
        public async ValueTask<byte[]> EncryptAsync(byte[] data, SubscriberCredential subscriberCredential)
        {
            // Parse ASCII-armored public key
            PgpPublicKey publicKey;
            using (Stream keyIn = new MemoryStream(Encoding.UTF8.GetBytes(subscriberCredential.GpgPublicKey)))
            {
                var decoderStream = PgpUtilities.GetDecoderStream(keyIn);
                var publicKeyRingBundle = new PgpPublicKeyRingBundle(decoderStream);
                publicKey = GetFirstPublicKey(publicKeyRingBundle);
            }

            using (Stream inputFileStream = new MemoryStream(data))
            using (MemoryStream encryptedFileStream = new MemoryStream())
            {
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

        public async ValueTask<byte[]> DecryptAsync(byte[] data, SubscriberCredential subscriberCredential)
        {
            // Decode ASCII-armored private key
            PgpPrivateKey privateKey;
            char[] privateKeyPassphrase = subscriberCredential.GpgPassPhrase.ToCharArray();

            using (Stream privateKeyStream = new MemoryStream(Encoding.UTF8.GetBytes(subscriberCredential.GpgPrivateKey)))
            {
                var decoderStream = PgpUtilities.GetDecoderStream(privateKeyStream);
                var privateKeyRingBundle = new PgpSecretKeyRingBundle(decoderStream);
                privateKey = GetPrivateKey(privateKeyRingBundle, privateKeyPassphrase);
            }

            using (Stream encryptedFileStream = new MemoryStream(data))
            {
                PgpObjectFactory pgpFactory = new PgpObjectFactory(PgpUtilities.GetDecoderStream(encryptedFileStream));
                PgpEncryptedDataList encryptedDataList = (PgpEncryptedDataList)pgpFactory.NextPgpObject();

                PgpPublicKeyEncryptedData encryptedData = null;

                foreach (PgpPublicKeyEncryptedData encryptedDataItem in encryptedDataList.GetEncryptedDataObjects())
                {
                    if (encryptedDataItem.KeyId == privateKey.KeyId)
                    {
                        encryptedData = encryptedDataItem;
                        break;
                    }
                }

                if (encryptedData == null)
                {
                    throw new ArgumentException("No encrypted data found for the provided private key.");
                }

                Stream decryptedStream = encryptedData.GetDataStream(privateKey);
                PgpObjectFactory decryptedFactory = new PgpObjectFactory(decryptedStream);
                PgpObject pgpObject = decryptedFactory.NextPgpObject();

                if (pgpObject is PgpCompressedData)
                {
                    PgpCompressedData compressedData = (PgpCompressedData)pgpObject;
                    PgpObjectFactory compressedFactory = new PgpObjectFactory(compressedData.GetDataStream());
                    pgpObject = compressedFactory.NextPgpObject();
                }

                byte[] decryptedData = null;

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

        private PgpPrivateKey GetPrivateKey(PgpSecretKeyRingBundle secretKeyRingBundle, char[] passphrase)
        {
            foreach (PgpSecretKeyRing keyRing in secretKeyRingBundle.GetKeyRings())
            {
                foreach (PgpSecretKey key in keyRing.GetSecretKeys())
                {
                    if (key.IsSigningKey)
                        continue;

                    var privateKey = key.ExtractPrivateKey(passphrase);
                    if (privateKey != null)
                        return privateKey;
                }
            }
            throw new ArgumentException("No private key found in the provided key ring bundle.");
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

            return null;
        }

        private static PgpPrivateKey FindPrivateKey(Stream privateKeyStream, long keyId, char[] passphrase)
        {
            var decoderStream = PgpUtilities.GetDecoderStream(privateKeyStream);

            PgpSecretKeyRingBundle secretKeyRingBundle =
                new PgpSecretKeyRingBundle(decoderStream);

            PgpSecretKey secretKey = secretKeyRingBundle.GetSecretKey(keyId);

            if (secretKey != null)
            {
                return secretKey.ExtractPrivateKey(passphrase);
            }

            return null;
        }
    }
}
