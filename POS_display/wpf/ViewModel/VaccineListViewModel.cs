using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TamroUtilities.HL7.Models;

namespace POS_display.wpf.ViewModel
{
    public class VaccineListViewModel : BaseViewModel
    {
        #region Commands
        public BaseAsyncCommand RefreshCommand { get; set; }
        public BaseAsyncCommand SignCommand { get; set; }
        public BaseAsyncCommand PageIndexChangedCommand { get; set; }
        #endregion

        public VaccineListViewModel()
        {
            SignCommand = new BaseAsyncCommand(Sign);
            RefreshCommand = new BaseAsyncCommand(Refresh);
            PageIndexChangedCommand = new BaseAsyncCommand(PageIndexChanged);
        }

        private async Task GetVaccineList()
        {
            string dtFrom = DateFrom.ToString("yyyy-MM-dd");
            string dtTo = DateTo.ToString("yyyy-MM-dd");
            Enumerator.VaccineDocFilterValue vaccineDocFilterValue = DocType.ToEnum<Enumerator.VaccineDocFilterValue>();
            VaccinationDataListDto vaccineList = await Session.eRecipeUtils.GetVaccinationData<VaccinationDataListDto>(
                "",
                FilterPractitionerId,
                FilterOrganizationId,
                (int)vaccineDocFilterValue,
                Status,
                DocStatus,
                GridSize,
                PageIndex,
                dtFrom,
                dtTo,
                null);
            if ((vaccineList?.VaccinationDataList?.Count ?? 0) <= 0)
                throw new Exception("Nepavyksta gauti paskyrimo ir išdavimo dokumentų pagal pasirinktus kriterijus");
            VaccineListDto = vaccineList;
            NotifyPropertyChanged("IsEnabled");
        }

        #region Command definitions
        private async Task Sign()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (SelectedVaccineItems.Count == 0)
                    throw new Exception("Nepasirinkta nei viena eilutė!");
                var count_successful = 0;
                var first_vaccine = SelectedVaccineItems.First();
                var signed_first = await Session.eRecipeUtils.SignVaccine(first_vaccine.CompositionId);
                   count_successful++;
                if (count_successful == SelectedVaccineItems.Count)
                    await GetVaccineList();
                foreach (var el in SelectedVaccineItems.Where(gv => gv.CompositionId != first_vaccine.CompositionId))
                {
                    ExecuteAsyncAction(async () =>
                    {
                        await Session.eRecipeUtils.SignVaccine(el.CompositionId);
                        count_successful++;
                        if (count_successful == SelectedVaccineItems.Count)
                            await GetVaccineList();
                    });
                }
            });
        }

        private async Task Refresh()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                Session.eRecipeUtils.cts = new System.Threading.CancellationTokenSource();
                await GetVaccineList();
            });
        }

        private async Task PageIndexChanged()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if ((VaccineListDto?.VaccinationDataList?.Count ?? 0) <= 0)
                    throw new Exception("");
                var pageIndex = PageIndex;
                await GetVaccineList();
                pageIndex = 1;
                SetVaccineList(pageIndex);
            });
        }

        private void SetVaccineList(int pageIndex)
        {
            var toSkip = (pageIndex - 1) * Session.eRecipeGridSize;
            VaccineList = (from el in VaccineListDto?.VaccinationDataList select new wpf.Model.VaccineListModel(el)).Skip(toSkip).Take(Session.eRecipeGridSize).ToList();
        }
        #endregion

        #region Variables
        private VaccinationDataListDto _vaccineListDto;
        public VaccinationDataListDto VaccineListDto
        {
            get
            {
                return _vaccineListDto;
            }
            set
            {
                _vaccineListDto = value;
                decimal total = _vaccineListDto?.Total ?? 0;
                PageCount = (int)Math.Ceiling((decimal)total / Session.eRecipeGridSize);
                SetVaccineList(PageIndex);
            }
        }

        private List<Model.VaccineListModel> _vaccineList;
        public List<Model.VaccineListModel> VaccineList
        {
            get
            {
                return _vaccineList;
            }
            set
            {
                _vaccineList = value;
                NotifyPropertyChanged("VaccineList");
            }
        }

        public bool IsEnabled
        {
            get
            {
                return VaccineList?.Count > 0;
            }
        }



        private List<Model.VaccineListModel> SelectedVaccineItems
        {
            get
            {
                return VaccineList.Where(w => w.selected == true).ToList();
            }
        }
        public string PatientId { get; set; }
        public string FilterPractitionerId { get; set; }
        public string FilterOrganizationId { get; set; }
        public string Status { get; set; }
        public string DocType { get; set; }
        public string DocStatus { get; set; }
        public int GridSize { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        private int _PageIndex;
        public int PageIndex
        {
            get
            {
                return _PageIndex;
            }
            set
            {
                _PageIndex = value;
                NotifyPropertyChanged("PageIndex");
            }
        }

        private int _PageCount;
        public int PageCount {
            get
            {
                return _PageCount;
            }
            set
            {
                _PageCount = value;
                NotifyPropertyChanged("PageCount");
            }
        }
        #endregion
    }
}
