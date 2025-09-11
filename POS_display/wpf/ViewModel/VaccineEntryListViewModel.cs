using POS_display.Models.Recipe;
using TamroUtilities.HL7.Models;
using System.Collections.Generic;
using System.Linq;

namespace POS_display.wpf.ViewModel
{
    public class VaccineEntryListViewModel : BaseViewModel
    {
        public VaccineEntryListViewModel(List<VaccinationEntry> vaccinesEntries)
        {
            VaccineEntries = vaccinesEntries?
                .Where(el=> el.Prescription != null)?
                .OrderByDescending(el => el.Prescription.Date)?
                .ToList() ?? new List<VaccinationEntry>();
        }

        #region Variables
        private List<VaccinationEntry> _vaccineEntries;
        public List<VaccinationEntry> VaccineEntries
        {
            get
            {
                return _vaccineEntries;
            }
            set
            {
                _vaccineEntries = value;
                NotifyPropertyChanged("VaccineEntries");
            }
        }
        #endregion
    }
}
