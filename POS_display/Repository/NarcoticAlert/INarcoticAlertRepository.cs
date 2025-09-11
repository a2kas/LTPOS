using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_display.Models.NarcoticAlert;

namespace POS_display.Repository.NarcoticAlert
{
    public interface INarcoticAlertRepository
    {
        Task Load();
        Task<List<ATCCodifier>> GetATCCodifiersByATC(string atc);
        Task<List<ATCCodifier>> GetATCCodifiers();
    }
}
