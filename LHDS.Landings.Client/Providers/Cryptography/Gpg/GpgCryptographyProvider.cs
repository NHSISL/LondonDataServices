// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using LHDS.Landings.Client.Providers.Cryptography.Gpg;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;

namespace LHDS.Landings.Client.Providers.Cryptography
{
    public class GpgCryptographyProvider : ICryptographyProvider
    {
        private readonly IGpgCryptographyProviderSettings gpgCryptographyProviderSettings;
        public GpgCryptographyProvider(IGpgCryptographyProviderSettings gpgCryptographyProviderSettings)
        {
            this.gpgCryptographyProviderSettings = gpgCryptographyProviderSettings;
        }

        public async ValueTask<byte[]> EncryptAsync(byte[] data)
        {
            var publicKeyDecoded = Convert.FromBase64String(this.gpgCryptographyProviderSettings.PublicKey);

            using (Stream publicKeyStream = new MemoryStream(publicKeyDecoded))
            {
                PgpPublicKey publicKey = ReadPublicKey(publicKeyStream);

                using (MemoryStream encryptedDataStream = new MemoryStream())
                {
                    PgpCompressedDataGenerator compressor =
                        new PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip);

                    PgpEncryptedDataGenerator encryptedDataGenerator =
                        new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Cast5, true, new SecureRandom());

                    encryptedDataGenerator.AddMethod(publicKey);

                    using (Stream encryptedDataWriter =
                        encryptedDataGenerator.Open(compressor.Open(encryptedDataStream), new byte[1 << 16]))
                    {
                        encryptedDataWriter.Write(data, 0, data.Length);
                    }

                    encryptedDataStream.Position = 0;
                    return await ValueTask.FromResult(encryptedDataStream.ToArray());
                }
            }
        }

        public async ValueTask<byte[]> DecryptAsync(byte[] data)
        {
            var privateKeyDecoded = Convert.FromBase64String(this.gpgCryptographyProviderSettings.PrivateKey);
            char[] privateKeyPassphrase = this.gpgCryptographyProviderSettings.Passphrase.ToCharArray();

            using (Stream encryptedFileStream = new MemoryStream(data))
            using (Stream privateKeyFileStream = new MemoryStream(privateKeyDecoded))
            {
                PgpObjectFactory pgpFactory = new PgpObjectFactory(PgpUtilities.GetDecoderStream(encryptedFileStream));
                PgpEncryptedDataList encryptedDataList = (PgpEncryptedDataList)pgpFactory.NextPgpObject();

                PgpPrivateKey privateKey = null;
                PgpPublicKeyEncryptedData encryptedData = null;

                // Find the correct private key to use for decryption
                foreach (PgpPublicKeyEncryptedData encryptedDataItem in encryptedDataList.GetEncryptedDataObjects())
                {
                    privateKey = FindPrivateKey(privateKeyFileStream, encryptedDataItem.KeyId, privateKeyPassphrase);
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

                // Decrypt the file
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
