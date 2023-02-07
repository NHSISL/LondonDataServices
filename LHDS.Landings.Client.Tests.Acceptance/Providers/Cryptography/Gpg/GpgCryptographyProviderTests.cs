// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using LHDS.Landings.Client.Providers.Cryptography;
using LHDS.Landings.Client.Providers.Cryptography.Gpg;
using Microsoft.Extensions.Configuration;
using Tynamix.ObjectFiller;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Documents
{
    public partial class GpgCryptographyProviderTests
    {
        private readonly IGpgCryptographyProviderSettings gpgCryptographyProviderSettings;
        private readonly IConfiguration inMemoryConfiguration;
        private readonly ICryptographyProvider cryptographyProvider;

        public GpgCryptographyProviderTests()
        {
            var appSettingsStub = new Dictionary<string, string> {
                {"cryptography:gpgPrivateKey", "LS0tLS1CRUdJTiBQR1AgUFJJVkFURSBLRVkgQkxPQ0stLS0tLQoKbElZRVkzWVJhUllKS3dZQkJBSGFSdzhCQVFkQUdFdHdrR0pvb0hsWThXNmNPK0czYWlQY0szdEJ4cHk4TDNyYgptbDhNVFlqK0J3TUM1UTFYMFJ5STFRSEhpb0gyWE9LZy9FbHRURUNaNTlxcjBOMW9iY2NmMDRRSlUzU0xrSUdZCjRwRDhUYzlxSkhXRzI2cnVkbE42MGM3UXhIZWh2aytlRzhFWUpvRmM3NTVIOGdxcThGL2xRTFF2UTNWdWJHbG0KWm1Vc0lFUmhkbWxrSUR4a1lYWnBaQzVqZFc1c2FXWm1aVEpBYm1Wc1kzTjFMbTVvY3k1MWF6NkltUVFURmdvQQpRUlloQkpiS0hsR3B0bmxaK2ZIcmdLc1RSWitJNDh0cEJRSmpkaEZwQWhzREJRa0R3OGxYQlFzSkNBY0NBaUlDCkJoVUtDUWdMQWdRV0FnTUJBaDRIQWhlQUFBb0pFS3NUUlorSTQ4dHBFRVFBL0FvTXVXOElNblhUR0RmbFFsWncKVWZOUkNMYzhaVzF6UThOMmJXQkZPWWVQQVFDODRXV2VqTnFPcXFOb2N6MXBLQVV1Qmg3OFFHQmJZRmRRVDg5NgpzYzhJQjV5TEJHTjJFV2tTQ2lzR0FRUUJsMVVCQlFFQkIwQ281U0ZGQzZoVlgrZVBjNFVRN1BQZFVJejNoR0hOCkNCYVlaS1kvQ2x6akd3TUJDQWYrQndNQzVFelFqWk5lenQ3SGl5cVQrbXF4aUJxVEZNcnNUYVo2c0FTZnhGQ3oKMVZnV2ZjRm1QMkhtWXFiWXRJSkc0QWllRitNdU4xc3puVXZUdEJxMnp0UUVsUGozaWJETG1mZzNxT2VhRTlzSgpzWWgrQkJnV0NnQW1GaUVFbHNvZVVhbTJlVm41OGV1QXF4TkZuNGpqeTJrRkFtTjJFV2tDR3d3RkNRUER5VmNBCkNna1FxeE5GbjRqankybVU5UUVBbk5venM4UzBicGx0TGIzaER3b0ZGdVk5M2R6SURSQ09yZnNqUkpaTS95b0IKQVBDWngzWmh4Z0hBdHdEK0lPRkg5Y205MlYzeHJrY25NOXY3VHNDMFlkZ00KPU9nckgKLS0tLS1FTkQgUEdQIFBSSVZBVEUgS0VZIEJMT0NLLS0tLS0K"},
                {"cryptography:gpgPublicKey", "LS0tLS1CRUdJTiBQR1AgUFVCTElDIEtFWSBCTE9DSy0tLS0tCgptRE1FWTNZUmFSWUpLd1lCQkFIYVJ3OEJBUWRBR0V0d2tHSm9vSGxZOFc2Y08rRzNhaVBjSzN0QnhweThMM3JiCm1sOE1UWWkwTDBOMWJteHBabVpsTENCRVlYWnBaQ0E4WkdGMmFXUXVZM1Z1YkdsbVptVXlRRzVsYkdOemRTNXUKYUhNdWRXcytpSmtFRXhZS0FFRVdJUVNXeWg1UnFiWjVXZm54NjRDckUwV2ZpT1BMYVFVQ1kzWVJhUUliQXdVSgpBOFBKVndVTENRZ0hBZ0lpQWdZVkNna0lDd0lFRmdJREFRSWVCd0lYZ0FBS0NSQ3JFMFdmaU9QTGFSQkVBUHdLCkRMbHZDREoxMHhnMzVVSldjRkh6VVFpM1BHVnRjMFBEZG0xZ1JUbUhqd0VBdk9GbG5vemFqcXFqYUhNOWFTZ0YKTGdZZS9FQmdXMkJYVUUvUGVySFBDQWU0T0FSamRoRnBFZ29yQmdFRUFaZFZBUVVCQVFkQXFPVWhSUXVvVlYvbgpqM09GRU96ejNWQ005NFJoelFnV21HU21Qd3BjNHhzREFRZ0hpSDRFR0JZS0FDWVdJUVNXeWg1UnFiWjVXZm54CjY0Q3JFMFdmaU9QTGFRVUNZM1lSYVFJYkRBVUpBOFBKVndBS0NSQ3JFMFdmaU9QTGFaVDFBUUNjMmpPenhMUnUKbVcwdHZlRVBDZ1VXNWozZDNNZ05FSTZ0K3lORWxrei9LZ0VBOEpuSGRtSEdBY0MzQVA0ZzRVZjF5YjNaWGZHdQpSeWN6Mi90T3dMUmgyQXc9Cj1PZ3Y0Ci0tLS0tRU5EIFBHUCBQVUJMSUMgS0VZIEJMT0NLLS0tLS0K"},
                {"cryptography:gpgPassphrase", "Vh68@KIiQ7^JM@l1vVRpmVHZpolmAhmHAnd!nPa0MdBkKT8p$N&XRU63!CakUMfVHwe9K@q7bszmUQyR@pOqFfdnKvoT^xA&3ZG"},
            };

            this.inMemoryConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            this.gpgCryptographyProviderSettings = new GpgCryptographyProviderSettings(inMemoryConfiguration);
            this.cryptographyProvider = new GpgCryptographyProvider(this.gpgCryptographyProviderSettings);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();
    }
}