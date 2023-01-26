using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Downloads;

namespace LHDS.Landings.Client.Services.Foundations.Downloads
{
    public interface IDownloadService
    {
        ValueTask<Download> AddDownloadAsync(Download download);
        IQueryable<Download> RetrieveAllDownloads();
        ValueTask<Download> RetrieveDownloadByIdAsync(Guid downloadId);
    }
}