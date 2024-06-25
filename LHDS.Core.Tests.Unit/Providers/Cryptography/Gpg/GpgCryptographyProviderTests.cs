// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Providers.Cryptography;
using LHDS.Core.Providers.Cryptography.Gpg;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Unit.Providers.Cryptography.Gpg
{
    public partial class GpgCryptographyProviderTests
    {
        private readonly ICryptographyProvider cryptographyProvider;

        public GpgCryptographyProviderTests() =>
            this.cryptographyProvider = new GpgCryptographyProvider();

        static byte[] ReadAllBytesFromStream(Stream stream)
        {
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static SubscriberCredential CreateRandomSubscriberCredential() =>
            CreateSubscriberCredentialFiller().Create();

        private static Filler<SubscriberCredential> CreateSubscriberCredentialFiller()
        {
            var filler = new Filler<SubscriberCredential>();
            string user = Guid.NewGuid().ToString();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(subscriberCredential => subscriberCredential.CreatedBy).Use(user)
                .OnProperty(subscriberCredential => subscriberCredential.UpdatedBy).Use(user);

            return filler;
        }
    }
}