using System.Collections.Generic;
using System.Threading.Tasks;
using POS_display.Models.ECRReports;

namespace POS_display.Repository.ECRReports
{
    public interface IECRReportsRepository
    {
        Task<IList<ECRReport>> Get();
    }
}
