using POS_display.Models.ECRReports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_display.Repository.ECRReports
{
    public class ECRReportsRepository : BaseRepository, IECRReportsRepository
    {
        public async Task<IList<ECRReport>> Get()
        {
            return await Task.FromResult(new List<ECRReport>
            {
                new ECRReport() { Id = "1", Command = "Atidaryti stalčių" },
                new ECRReport() { Id = "3", Command = "X ataskaita" },
                new ECRReport() { Id = "5", Command = "Pinigų įdėjimas" },
                new ECRReport() { Id = "7", Command = "Suminė periodinė ataskaita" },
                new ECRReport() { Id = "8", Command = "Detali periodinė ataskaita" },
                new ECRReport() { Id = "11", Command = "Nustatyti datą ir laiką" },
                new ECRReport() { Id = "12", Command = "Paskutinio kvito kopija" },
                new ECRReport() { Id = "6", Command = "Pinigų išėmimas" },
                new ECRReport() { Id = "4", Command = "Z ataskaita" },
                new ECRReport() { Id = "13", Command = "Avanso įdėjimas" },
                new ECRReport() { Id = "14", Command = "Avanso įdėjimas kortele" },
            });
        }
    }
}
