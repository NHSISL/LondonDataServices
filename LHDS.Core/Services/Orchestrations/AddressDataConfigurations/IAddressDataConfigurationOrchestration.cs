// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Services.Orchestrations.AddressDataConfigurations
{
    public interface IAddressDataConfigurationOrchestration
    {
        ValueTask<bool> LoadConfiguration();
    }
}
