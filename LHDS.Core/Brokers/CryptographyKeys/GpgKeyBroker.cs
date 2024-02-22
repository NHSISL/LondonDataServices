// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.CryptographicKeys;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;

namespace LHDS.Core.Brokers.CryptographyKeys
{
    public class GpgKeyBroker : ICryptographyKeyBroker
    {
        public async ValueTask<CryptographicKey> GenerateKeys(string? publicKeyComment = "")
        {
            RsaKeyPairGenerator rsaKeyPairGenerator = new RsaKeyPairGenerator();
            rsaKeyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
            AsymmetricCipherKeyPair keyPair = rsaKeyPairGenerator.GenerateKeyPair();
            PgpKeyPair pgpKeyPair = new PgpKeyPair(PublicKeyAlgorithmTag.RsaGeneral, keyPair, DateTime.UtcNow);
            PgpSignatureSubpacketGenerator subpacketGenerator = new PgpSignatureSubpacketGenerator();

            subpacketGenerator.SetKeyFlags(
                false, PgpKeyFlags.CanSign | PgpKeyFlags.CanEncryptCommunications | PgpKeyFlags.CanEncryptStorage);

            subpacketGenerator.SetPreferredSymmetricAlgorithms(false, new[] { (int)SymmetricKeyAlgorithmTag.Aes256 });
            subpacketGenerator.SetPreferredHashAlgorithms(false, new[] { (int)HashAlgorithmTag.Sha256 });
            PgpSignatureSubpacketVector subpacketVector = subpacketGenerator.Generate();

            PgpKeyRingGenerator keyRingGenerator = new PgpKeyRingGenerator(
                certificationLevel: PgpSignature.DefaultCertification,
                masterKey: pgpKeyPair,
                id: "test",
                SymmetricKeyAlgorithmTag.Aes256,
                "test".ToCharArray(),
                true,
                subpacketVector,
                null,
                new SecureRandom());

            PgpPublicKeyRing publicKeyRing = keyRingGenerator.GeneratePublicKeyRing();
            PgpSecretKeyRing secretKeyRing = keyRingGenerator.GenerateSecretKeyRing();
            string publicKey;
            string privateKey;

            using (MemoryStream outputStream = new MemoryStream())
            {
                using (ArmoredOutputStream armoredStream = new ArmoredOutputStream(outputStream))
                {
                    publicKeyRing.Encode(armoredStream);
                }
                publicKey = Encoding.UTF8.GetString(outputStream.ToArray());
            }

            using (MemoryStream outputStream = new MemoryStream())
            {
                using (ArmoredOutputStream armoredStream = new ArmoredOutputStream(outputStream))
                {
                    secretKeyRing.Encode(armoredStream);
                }
                privateKey = Encoding.UTF8.GetString(outputStream.ToArray());
            }

            CryptographicKey returnedKey = new CryptographicKey
            {
                Base64PrivateKey = publicKey,
                Base64PublicKey = privateKey
            };

            return await ValueTask.FromResult(returnedKey);
        }
    }
}
