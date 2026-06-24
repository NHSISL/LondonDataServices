// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Brokers.TempLocations
{
    public interface ITempLocationBroker
    {
        string GetTempDirectory();
        string GetHomeDirectory();
        string GetTempFilePath(string fileName);
        string GetHomeFilePath(string fileName);
        string GetUniqueTempFilePath(string extension = ".tmp");
        string GetUniqueHomeFilePath(string extension = ".tmp");
    }
}
