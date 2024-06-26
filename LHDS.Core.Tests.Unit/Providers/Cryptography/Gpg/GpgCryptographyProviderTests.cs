// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Providers.Cryptography;
using LHDS.Core.Providers.Cryptography.Gpg;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Documents
{
    public partial class GpgCryptographyProviderTests
    {
        private readonly SubscriberCredential subscriberCredential;
        private readonly ICryptographyProvider cryptographyProvider;

        public GpgCryptographyProviderTests()
        {
            subscriberCredential = new SubscriberCredential
            {
                GpgPrivateKey = "LS0tLS1CRUdJTiBQR1AgUFJJVkFURSBLRVkgQkxPQ0stLS0tLQoKbElZRVkrSk45eFlKS3dZQkJBSGFSdzhCQVFkQWRaTWxIckpvckQyUmgyZUxhTUQyTlBTVHlvZmM0V1Z1NkdRYQp3TTVneXVmK0J3TUNNVHFudHJ0ajY0ZkhKZWpuTFZnaVlJQjJkOGFuZTNsRmtCS0ZVNkZQaUN5Y0JYbzFmemVCClFkTXFDbkVmdHFzWUdxUXM4QzRhVW5OZUF2c252dkFKeXl5a2VFL0JQNXhvbnREcC8yQW5ZTFFXVEVoRVV5QTgKZEdWemRFQnNhR1J6TG14dlkyRnNQb2lUQkJNV0NnQTdGaUVFRURHNVBEYWtZSHZ5a1FocVZYWm00eGdpZzZVRgpBbVBpVGZjQ0d3TUZDd2tJQndJQ0lnSUdGUW9KQ0FzQ0JCWUNBd0VDSGdjQ0Y0QUFDZ2tRVlhabTR4Z2lnNlZXClhnRC9VVTdWUWJCWTBpcHFkRFE2SFltYjAvNS9ibE9oZEcrZTFBVXBKb3UrSUZnQS9qd0VLcGxXYUtrTm1GZ3kKNUI5Q09zMXNOYUg5Q0RscjBnK2VyR25IdHVZQW5Jc0VZK0pOOXhJS0t3WUJCQUdYVlFFRkFRRUhRQUZESzJqRwpIZ3NzZjQ0WDgyK0dxTnJrdXlhVWM3bitrdzJSSEpHQWkvRVZBd0VJQi80SEF3SnZGc2doaUQ3NkJzY1BWd2ovCkEvTjFZMC9ZTnFIMHJIdnpjcDZVNmk3M205Rjh6ZU1SMXgrajhWU3FmTHBjRUR6UjVNaVF0RzRHSUt2T043engKWmhPNzdTOElES2QzMENYRTVFSk9TL3hQaUhnRUdCWUtBQ0FXSVFRUU1iazhOcVJnZS9LUkNHcFZkbWJqR0NLRApwUVVDWStKTjl3SWJEQUFLQ1JCVmRtYmpHQ0tEcFRUWUFRQ3hrV0hCS09GOHF5SCtXWWZFY25UTUZvMU5wbi9mCmR5bUdkVDRsOXYvMnBnRC9VTVdiR2JJOVRkakxkRGVxTXBPejlrUXk1Q05GQSttMFVVaWdrbGFoeFFRPQo9YVBKdwotLS0tLUVORCBQR1AgUFJJVkFURSBLRVkgQkxPQ0stLS0tLQo=",
                GpgPublicKey = "LS0tLS1CRUdJTiBQR1AgUFVCTElDIEtFWSBCTE9DSy0tLS0tCgptRE1FWStKTjl4WUpLd1lCQkFIYVJ3OEJBUWRBZFpNbEhySm9yRDJSaDJlTGFNRDJOUFNUeW9mYzRXVnU2R1FhCndNNWd5dWUwRmt4SVJGTWdQSFJsYzNSQWJHaGtjeTVzYjJOaGJENklrd1FURmdvQU94WWhCQkF4dVR3MnBHQjcKOHBFSWFsVjJadU1ZSW9PbEJRSmo0azMzQWhzREJRc0pDQWNDQWlJQ0JoVUtDUWdMQWdRV0FnTUJBaDRIQWhlQQpBQW9KRUZWMlp1TVlJb09sVmw0QS8xRk8xVUd3V05JcWFuUTBPaDJKbTlQK2YyNVRvWFJ2bnRRRktTYUx2aUJZCkFQNDhCQ3FaVm1pcERaaFlNdVFmUWpyTmJEV2gvUWc1YTlJUG5xeHB4N2JtQUxnNEJHUGlUZmNTQ2lzR0FRUUIKbDFVQkJRRUJCMEFCUXl0b3hoNExMSCtPRi9OdmhxamE1THNtbEhPNS9wTU5rUnlSZ0l2eEZRTUJDQWVJZUFRWQpGZ29BSUJZaEJCQXh1VHcycEdCNzhwRUlhbFYyWnVNWUlvT2xCUUpqNGszM0Foc01BQW9KRUZWMlp1TVlJb09sCk5OZ0JBTEdSWWNFbzRYeXJJZjVaaDhSeWRNd1dqVTJtZjk5M0tZWjFQaVgyLy9hbUFQOVF4WnNac2oxTjJNdDAKTjZveWs3UDJSRExrSTBVRDZiUlJTS0NTVnFIRkJBPT0KPWVIMEoKLS0tLS1FTkQgUEdQIFBVQkxJQyBLRVkgQkxPQ0stLS0tLQo=",
                GpgPassPhrase = "P@ssw0rd!",
            };

            this.cryptographyProvider = new GpgCryptographyProvider();
        }

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