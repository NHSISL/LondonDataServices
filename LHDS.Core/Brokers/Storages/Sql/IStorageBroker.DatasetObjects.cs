// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSetObjects;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<DataSetObject> InsertDataSetObjectAsync(DataSetObject dataSetObject);
        IQueryable<DataSetObject> SelectAllDataSetObjects();
        ValueTask<DataSetObject> SelectDataSetObjectByIdAsync(Guid dataSetObjectId);
        ValueTask<DataSetObject> UpdateDataSetObjectAsync(DataSetObject dataSetObject);
        ValueTask<DataSetObject> DeleteDataSetObjectAsync(DataSetObject dataSetObject);
    }
}