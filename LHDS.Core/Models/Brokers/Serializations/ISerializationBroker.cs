// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHDS.Core.Models.Brokers.Serializations
{
    public interface ISerializationBroker
    {
        ValueTask<string> Serialize<T>(T obj);
        ValueTask<T> Deserialize<T>(string json);
    }
}
