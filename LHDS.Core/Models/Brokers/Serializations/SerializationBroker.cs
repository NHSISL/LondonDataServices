// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace LHDS.Core.Models.Brokers.Serializations
{
    public class SerializationBroker : ISerializationBroker
    {
        public T Deserialize<T>(string json) =>
            throw new NotImplementedException();

        public string Serialize<T>(T obj) =>
            throw new NotImplementedException();
    }
}
