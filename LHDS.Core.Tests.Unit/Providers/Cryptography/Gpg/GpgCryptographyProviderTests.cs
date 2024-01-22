// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using LHDS.Core.Models.Configurations;
using LHDS.Core.Providers.Cryptography;
using LHDS.Core.Providers.Cryptography.Gpg;
using Microsoft.Extensions.Configuration;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Documents
{
    public partial class GpgCryptographyProviderTests
    {
        private readonly IGpgCryptographyProviderSettings gpgCryptographyProviderSettings;
        private readonly IConfiguration inMemoryConfiguration;
        private readonly ICryptographyProvider cryptographyProvider;

        public GpgCryptographyProviderTests()
        {
            var appSettingsStub = new Dictionary<string, string> {
                {"cryptography:privateKey", "LS0tLS1CRUdJTiBQR1AgUFJJVkFURSBLRVkgQkxPQ0stLS0tLQoKbElZRVkrSk45eFlKS3dZQkJBSGFSdzhCQVFkQWRaTWxIckpvckQyUmgyZUxhTUQyTlBTVHlvZmM0V1Z1NkdRYQp3TTVneXVmK0J3TUNNVHFudHJ0ajY0ZkhKZWpuTFZnaVlJQjJkOGFuZTNsRmtCS0ZVNkZQaUN5Y0JYbzFmemVCClFkTXFDbkVmdHFzWUdxUXM4QzRhVW5OZUF2c252dkFKeXl5a2VFL0JQNXhvbnREcC8yQW5ZTFFXVEVoRVV5QTgKZEdWemRFQnNhR1J6TG14dlkyRnNQb2lUQkJNV0NnQTdGaUVFRURHNVBEYWtZSHZ5a1FocVZYWm00eGdpZzZVRgpBbVBpVGZjQ0d3TUZDd2tJQndJQ0lnSUdGUW9KQ0FzQ0JCWUNBd0VDSGdjQ0Y0QUFDZ2tRVlhabTR4Z2lnNlZXClhnRC9VVTdWUWJCWTBpcHFkRFE2SFltYjAvNS9ibE9oZEcrZTFBVXBKb3UrSUZnQS9qd0VLcGxXYUtrTm1GZ3kKNUI5Q09zMXNOYUg5Q0RscjBnK2VyR25IdHVZQW5Jc0VZK0pOOXhJS0t3WUJCQUdYVlFFRkFRRUhRQUZESzJqRwpIZ3NzZjQ0WDgyK0dxTnJrdXlhVWM3bitrdzJSSEpHQWkvRVZBd0VJQi80SEF3SnZGc2doaUQ3NkJzY1BWd2ovCkEvTjFZMC9ZTnFIMHJIdnpjcDZVNmk3M205Rjh6ZU1SMXgrajhWU3FmTHBjRUR6UjVNaVF0RzRHSUt2T043engKWmhPNzdTOElES2QzMENYRTVFSk9TL3hQaUhnRUdCWUtBQ0FXSVFRUU1iazhOcVJnZS9LUkNHcFZkbWJqR0NLRApwUVVDWStKTjl3SWJEQUFLQ1JCVmRtYmpHQ0tEcFRUWUFRQ3hrV0hCS09GOHF5SCtXWWZFY25UTUZvMU5wbi9mCmR5bUdkVDRsOXYvMnBnRC9VTVdiR2JJOVRkakxkRGVxTXBPejlrUXk1Q05GQSttMFVVaWdrbGFoeFFRPQo9YVBKdwotLS0tLUVORCBQR1AgUFJJVkFURSBLRVkgQkxPQ0stLS0tLQo="},
                {"cryptography:publicKey", "LS0tLS1CRUdJTiBQR1AgUFVCTElDIEtFWSBCTE9DSy0tLS0tCgptRE1FWStKTjl4WUpLd1lCQkFIYVJ3OEJBUWRBZFpNbEhySm9yRDJSaDJlTGFNRDJOUFNUeW9mYzRXVnU2R1FhCndNNWd5dWUwRmt4SVJGTWdQSFJsYzNSQWJHaGtjeTVzYjJOaGJENklrd1FURmdvQU94WWhCQkF4dVR3MnBHQjcKOHBFSWFsVjJadU1ZSW9PbEJRSmo0azMzQWhzREJRc0pDQWNDQWlJQ0JoVUtDUWdMQWdRV0FnTUJBaDRIQWhlQQpBQW9KRUZWMlp1TVlJb09sVmw0QS8xRk8xVUd3V05JcWFuUTBPaDJKbTlQK2YyNVRvWFJ2bnRRRktTYUx2aUJZCkFQNDhCQ3FaVm1pcERaaFlNdVFmUWpyTmJEV2gvUWc1YTlJUG5xeHB4N2JtQUxnNEJHUGlUZmNTQ2lzR0FRUUIKbDFVQkJRRUJCMEFCUXl0b3hoNExMSCtPRi9OdmhxamE1THNtbEhPNS9wTU5rUnlSZ0l2eEZRTUJDQWVJZUFRWQpGZ29BSUJZaEJCQXh1VHcycEdCNzhwRUlhbFYyWnVNWUlvT2xCUUpqNGszM0Foc01BQW9KRUZWMlp1TVlJb09sCk5OZ0JBTEdSWWNFbzRYeXJJZjVaaDhSeWRNd1dqVTJtZjk5M0tZWjFQaVgyLy9hbUFQOVF4WnNac2oxTjJNdDAKTjZveWs3UDJSRExrSTBVRDZiUlJTS0NTVnFIRkJBPT0KPWVIMEoKLS0tLS1FTkQgUEdQIFBVQkxJQyBLRVkgQkxPQ0stLS0tLQo="},
                {"cryptography:passphrase", "P@ssw0rd!"},
            };

            this.inMemoryConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            var cryptographyProviderSettings = inMemoryConfiguration.GetSection("cryptography").Get<GpgCryptographyProviderSettings>();
            ValidateCryptographyProviderSettings(cryptographyProviderSettings);

            this.gpgCryptographyProviderSettings = cryptographyProviderSettings;
            this.cryptographyProvider = new GpgCryptographyProvider(this.gpgCryptographyProviderSettings);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static void ValidateCryptographyProviderSettings(GpgCryptographyProviderSettings cryptographyProviderSettings)
        {
            Validate(
                (Rule: IsInvalid(cryptographyProviderSettings.PrivateKey),
                    Parameter: "cryptography__privateKey"),

                (Rule: IsInvalid(cryptographyProviderSettings.PublicKey),
                    Parameter: "cryptography__publicKey"),

                (Rule: IsInvalid(cryptographyProviderSettings.Passphrase),
                    Parameter: "cryptography__passphrase"));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Configuration value does not exist"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidConfigurationException = new InvalidConfigurationException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidConfigurationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidConfigurationException.ThrowIfContainsErrors();
        }
    }
}