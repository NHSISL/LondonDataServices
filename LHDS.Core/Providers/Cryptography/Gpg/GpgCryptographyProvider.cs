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
            byte[] publicKeyBytes = Convert.FromBase64String(subscriberCredential.GpgPublicKey ?? "");
            int bufferSize = 65536;

            using (Stream publicKeyStream = new MemoryStream(publicKeyBytes))
            {
                PgpPublicKey publicKey = ReadPublicKey(publicKeyStream);

                var encryptedDataGenerator = new PgpEncryptedDataGenerator(
                        encAlgorithm: SymmetricKeyAlgorithmTag.Cast5,
                        withIntegrityPacket: true,
                        random: new SecureRandom());

                encryptedDataGenerator.AddMethod(publicKey);

                using (Stream encryptedStream = encryptedDataGenerator.Open(output, new byte[bufferSize]))
                {
                    var compressedDataGenerator = new PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip);
                    Stream compressedStream = compressedDataGenerator.Open(encryptedStream);

                    var literalDataGenerator = new PgpLiteralDataGenerator();
                    Stream literalStream = literalDataGenerator.Open(
                        outStr: compressedStream,
                        format: PgpLiteralData.Binary,
                        name: "",
                        length: input.Length,
                        modificationTime: DateTime.UtcNow);

                    input.Position = 0;
                    await input.CopyToAsync(literalStream, bufferSize);

                    literalStream.Close();
                    compressedStream.Close();
                }
            }
        }

        public async ValueTask DecryptAsync(Stream input, Stream output, SubscriberCredential subscriberCredential)
        {
            byte[] privateKeyBytes = Convert.FromBase64String(subscriberCredential.GpgPrivateKey ?? "");
            char[] privateKeyPassphrase = subscriberCredential?.GpgPassPhrase?.ToCharArray() ?? Array.Empty<char>();

            using (Stream privateKeyStream = new MemoryStream(privateKeyBytes))
            {
                input.Position = 0;
                PgpObjectFactory pgpFactory = new PgpObjectFactory(PgpUtilities.GetDecoderStream(input));
                PgpEncryptedDataList encryptedDataList = (PgpEncryptedDataList)pgpFactory.NextPgpObject();

                PgpPrivateKey? privateKey = null;
                PgpPublicKeyEncryptedData? encryptedData = null;

                foreach (PgpPublicKeyEncryptedData encryptedDataItem in encryptedDataList.GetEncryptedDataObjects())
                {
                    privateKey = FindPrivateKey(
                        privateKeyStream: privateKeyStream,
                        keyId: encryptedDataItem.KeyId,
                        passphrase: privateKeyPassphrase);

                    if (privateKey != null)
                    {
                        encryptedData = encryptedDataItem;
                        break;
                    }
                }

                if (privateKey == null) throw new ArgumentException("Private key not found in the key file");
                if (encryptedData == null) throw new ArgumentException("No encrypted data found in the message");

                using (Stream decryptedStream = encryptedData.GetDataStream(privateKey))
                {
                    PgpObjectFactory decryptedFactory = new PgpObjectFactory(decryptedStream);
                    PgpObject pgpObject = decryptedFactory.NextPgpObject();

                    if (pgpObject is PgpCompressedData compressedData)
                    {
                        using (Stream compressedStream = compressedData.GetDataStream())
                        {
                            PgpObjectFactory compressedFactory = new PgpObjectFactory(compressedStream);
                            pgpObject = compressedFactory.NextPgpObject();
                        }
                    }

                    if (pgpObject is PgpLiteralData literalData)
                    {
                        using (Stream literalStream = literalData.GetInputStream())
                        {
                            await literalStream.CopyToAsync(output, 65536); // 64KB buffer
                        }
                    }
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
