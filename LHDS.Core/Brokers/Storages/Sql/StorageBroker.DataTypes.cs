// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.DataTypes;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<DataType> DataTypes { get; set; }
    }
}
