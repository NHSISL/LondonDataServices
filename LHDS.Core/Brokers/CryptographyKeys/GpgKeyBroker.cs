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
        public string CryptographyType => "GPG";

        public async ValueTask<CryptographicKey> GenerateKeys(string comment, string password, string name, string email)
        {
            RsaKeyPairGenerator rsaKeyPairGenerator = new RsaKeyPairGenerator();
            rsaKeyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
            AsymmetricCipherKeyPair keyPair = rsaKeyPairGenerator.GenerateKeyPair();

            PgpKeyPair pgpKeyPair = new PgpKeyPair(
                algorithm: PublicKeyAlgorithmTag.RsaGeneral,
                keyPair,
                time: DateTime.UtcNow);

            PgpSignatureSubpacketGenerator subpacketGenerator = new PgpSignatureSubpacketGenerator();

            subpacketGenerator.SetKeyFlags(
                isCritical: false,
                flags: PgpKeyFlags.CanSign | PgpKeyFlags.CanEncryptCommunications | PgpKeyFlags.CanEncryptStorage);

            subpacketGenerator.SetPreferredSymmetricAlgorithms(
                isCritical: false,
                algorithms: new[] { (int)SymmetricKeyAlgorithmTag.Aes256 });

            subpacketGenerator.SetPreferredHashAlgorithms(
                isCritical: false,
                algorithms: new[] { (int)HashAlgorithmTag.Sha256 });

            PgpSignatureSubpacketVector subpacketVector = subpacketGenerator.Generate();
            var userId = $"{name} {comment} <{email}>";

            PgpKeyRingGenerator keyRingGenerator = new PgpKeyRingGenerator(
                certificationLevel: PgpSignature.DefaultCertification,
                masterKey: pgpKeyPair,
                id: userId,
                encAlgorithm: SymmetricKeyAlgorithmTag.Aes256,
                passPhrase: password.ToCharArray(),
                useSha1: true,
                hashedPackets: subpacketVector,
                unhashedPackets: null,
                rand: new SecureRandom());

            PgpPublicKeyRing publicKeyRing = keyRingGenerator.GeneratePublicKeyRing();
            PgpSecretKeyRing secretKeyRing = keyRingGenerator.GenerateSecretKeyRing();

            string publicKey;

            using (MemoryStream outputStream = new MemoryStream())
            {
                using (ArmoredOutputStream armoredStream = new ArmoredOutputStream(outputStream))
                {
                    publicKeyRing.Encode(armoredStream);
                }
                publicKey = Encoding.UTF8.GetString(outputStream.ToArray());
            }

            string privateKey;

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
                PrivateKey = privateKey,
                PublicKey = publicKey,
                Passphrase = password,
            };

            return await ValueTask.FromResult(returnedKey);
        }
    }
}
