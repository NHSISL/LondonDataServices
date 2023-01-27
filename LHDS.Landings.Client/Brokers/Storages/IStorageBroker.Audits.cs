using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Audits;

namespace LHDS.Landings.Client.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Audit> InsertAuditAsync(Audit audit);
        IQueryable<Audit> SelectAllAudits();
    }
}
