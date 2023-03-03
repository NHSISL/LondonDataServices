// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace LHDS.Functions.Landings.Emis
{
    public class DecryptionEventFunction
    {
        private readonly ILogger _logger;

        public DecryptionEventFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DecryptionEventFunction>();
        }

        [Function("DecryptionEventFunction")]
        public void Run([BlobTrigger("emislanding/encrypted/{name}", Connection = "BlobStorage")] string myBlob, string name)
        {
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {myBlob}");
        }
    }
}
