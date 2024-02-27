// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Providers.Cryptography.Gpg;

namespace LHDS.Core.Providers.Cryptography.Builders
{
    public class GpgProviderRegistrationBuilder
    {
        private readonly List<ICryptographyProvider> cryptographyProviderRegistrations;

        public GpgProviderRegistrationBuilder()
        {
            cryptographyProviderRegistrations = new List<ICryptographyProvider>();
        }

        public void AddGpgCryptographyProvider()
        {
            var gpgCryptographyProvider = new GpgCryptographyProvider();

            if (cryptographyProviderRegistrations.Count > 0)
            {
                throw new Exception("Only one cryptography provider can be registered at a time");
            }

            cryptographyProviderRegistrations.Add(gpgCryptographyProvider);
        }

        public List<ICryptographyProvider> Build()
        {
            return cryptographyProviderRegistrations;
        }
    }
}
