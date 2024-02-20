// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Keys;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;

namespace LHDS.Core.Brokers.CryptographyKeys
{
    public class GpgKeyBroker : ICryptographyKeyBroker
    {
        public ValueTask<Key> GenerateKeys(string publicKeyComment)
        {
            var publicKeyPath = @"C:\Users\d_cun\OneDrive\Desktop\AZ SSH Files\pub.asc";
            var privateKeyPath = @"C:\Users\d_cun\OneDrive\Desktop\AZ SSH Files\priv.asc";
            RsaKeyPairGenerator rsaKeyPairGenerator = new RsaKeyPairGenerator();
            rsaKeyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
            AsymmetricCipherKeyPair keyPair = rsaKeyPairGenerator.GenerateKeyPair();

            // Create the PGP key pair
            PgpKeyPair pgpKeyPair = new PgpKeyPair(PublicKeyAlgorithmTag.RsaGeneral, keyPair, DateTime.UtcNow);

            // Create the PGP key ring generator
            PgpSignatureSubpacketGenerator subpacketGenerator = new PgpSignatureSubpacketGenerator();

            subpacketGenerator.SetKeyFlags(
                false, PgpKeyFlags.CanSign | PgpKeyFlags.CanEncryptCommunications | PgpKeyFlags.CanEncryptStorage);

            subpacketGenerator.SetPreferredSymmetricAlgorithms(false, new[] { (int)SymmetricKeyAlgorithmTag.Aes256 });
            subpacketGenerator.SetPreferredHashAlgorithms(false, new[] { (int)HashAlgorithmTag.Sha256 });
            PgpSignatureSubpacketVector subpacketVector = subpacketGenerator.Generate();

            PgpKeyRingGenerator keyRingGenerator = new PgpKeyRingGenerator(
                PgpSignature.DefaultCertification, 
                pgpKeyPair, "test", SymmetricKeyAlgorithmTag.Aes256, "test".ToCharArray(), true, 
                subpacketVector, null, new SecureRandom());

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

            // Write the private key to a string
            string privateKey;
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (ArmoredOutputStream armoredStream = new ArmoredOutputStream(outputStream))
                {
                    secretKeyRing.Encode(armoredStream);
                }
                privateKey = Encoding.UTF8.GetString(outputStream.ToArray());
            }

            Key returnedKey =  new Key
            {
                Base64PrivateKey = publicKey,
                Base64PublicKey = privateKey
            };

            return ValueTask.FromResult(returnedKey);
        }

        public ValueTask<Key> GenerateKeys() =>
            this.GenerateKeys("");


    }
}
