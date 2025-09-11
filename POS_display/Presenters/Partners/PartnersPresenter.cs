using AutoMapper;
using POS_display.Exceptions;
using POS_display.Models.Partner;
using POS_display.Repository.Partners;
using POS_display.Repository.Pos;
using POS_display.Views.Partners;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_display.Presenters.Partners
{
    public class PartnersPresenter : IPartnersPresenter
    {
        #region Members
        private readonly IPartnersView _view;
        private readonly IPartnerRepository _partnerRepository;
        private readonly IPosRepository _posRepository;
        private readonly IMapper _mapper;
        private List<PartnerViewData> _partnersData;
        private const int PageSize = 15;
        private int _currentPageIndex;
        private int _lastPageIndex;
        private AutoCompleteStringCollection _autoCompleteStringCollection;
        private const string _autoCompleteFilterKey = "name";
        #endregion

        #region Constructor
        public PartnersPresenter(
            IPartnersView view,
            IPosRepository posRepository,
            IPartnerRepository partnerRepository,
            IMapper mapper)
        {
            _view = view ?? throw new ArgumentNullException();
            _partnerRepository = partnerRepository ?? throw new ArgumentNullException();
            _posRepository = posRepository ?? throw new ArgumentNullException();
            _mapper = mapper ?? throw new ArgumentNullException();
        }
        #endregion

        #region Public methods
        public async Task Init()
        {
            await _posRepository.UpdateSession("Kreditorių/Debitorių lentelė", 2);
            Dictionary<string, string> filterValues = new Dictionary<string, string>
            {
                {"name", "pavadinimą"},
                {"name:ecode:tcode:address:old_ecode_scala", "pavadinimą, įm. kodą, adresą, scala k."},
                {"ecode", "įmonės kodą"},
                {"tcode", "įmonės PVM kodą"},
                {"address", "adresą"},
                {"debtortypename", "pirkėjų grupę"},
                {"descrip", "komentarą"},
                {"old_ecode_scala", "scala kodą"},
            };

            _view.FilterByValues.DataSource = new BindingSource(filterValues, null);
            _view.FilterByValues.DisplayMember = "Value";
            _view.FilterByValues.ValueMember = "Key";
            _autoCompleteStringCollection = new AutoCompleteStringCollection();
            Reset();
        }

        public async Task LoadPartners()
        {
            Reset();

             _partnersData = _mapper.Map<List<PartnerViewData>>(await _partnerRepository.GetDebtors(GetFilter()));

            SetPageData();
        }

        public void FocusPartner(long partnerId)
        {
            int partnerIndex = _partnersData.FindIndex(p => p.Id == partnerId);

            if (partnerIndex >= 0)
            {
                int page = (partnerIndex / PageSize) + 1;

                if (page != _currentPageIndex)
                {
                    _currentPageIndex = page;
                    SetPageData();
                }

                foreach (DataGridViewRow row in _view.PartnersGridView.Rows)
                {
                    if ((row.DataBoundItem as PartnerViewData).Id == partnerId)
                    {
                        _view.PartnersGridView.ClearSelection();
                        row.Selected = true;
                        _view.PartnersGridView.FirstDisplayedScrollingRowIndex = row.Index;
                        break;
                    }
                }
            }
            else
            {
                throw new PartnerException("Partner not found in the current data");
            }
        }

        public void LoadFilterAutoCompleteData()
        {
            if (_partnersData != null && _partnersData.Count > 0)
            {
                _autoCompleteStringCollection.AddRange(_partnersData.Select(e => e.Name).ToArray());
                _view.FilterValue.AutoCompleteCustomSource = _autoCompleteStringCollection;
                _view.FilterValue.AutoCompleteMode = AutoCompleteMode.Suggest;
                _view.FilterValue.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }
        }

        public void SetFilterAutoCompleteAvailability()
        {
            if (_view.FilterByValues.SelectedValue.ToString().ToLowerInvariant() == _autoCompleteFilterKey)
                _view.FilterValue.AutoCompleteCustomSource = _autoCompleteStringCollection;
            else
                _view.FilterValue.AutoCompleteCustomSource = null;
        }

        public void SetNextPage()
        {
            _currentPageIndex++;

            _view.FirstPage.Enabled = true;
            _view.PreviousPage.Enabled = true;

            SetPageData();
        }

        public void SetPreviousPage()
        {
            _currentPageIndex--;

            _view.NextPage.Enabled = true;
            _view.LastPage.Enabled = true;

            SetPageData();
        }

        public void SetFirstPage()
        {
            _currentPageIndex = 1;

            _view.PreviousPage.Enabled = false;
            _view.NextPage.Enabled = true;
            _view.LastPage.Enabled = true;
            _view.FirstPage.Enabled = false;

            SetPageData();
        }

        public void SetLastPage()
        {
            _currentPageIndex = (int)Math.Ceiling(Convert.ToDecimal(_partnersData.Count) / PageSize);

            _view.FirstPage.Enabled = true;
            _view.PreviousPage.Enabled = true;
            _view.NextPage.Enabled = false;
            _view.LastPage.Enabled = false;

            SetPageData();
        }

        public void Reset()
        {
            _currentPageIndex = 1;
            _lastPageIndex = 1;
            _partnersData = new List<PartnerViewData>();
        }

        public void EnableControls()
        {
            _view.FindButton.Enabled = true;
            _view.FilterByValues.Enabled = true;
            _view.CloseButton.Enabled = true;
            _view.NewPartner.Enabled = true;
            _view.EditPartner.Enabled = GetFocusedPartner() != null;

            if (_currentPageIndex == (int)Math.Ceiling(Convert.ToDecimal(_partnersData.Count) / PageSize))
            {
                _view.NextPage.Enabled = false;
                _view.LastPage.Enabled = false;
            }
            else
            {
                _view.NextPage.Enabled = true;
                _view.LastPage.Enabled = true;
            }

            if (_currentPageIndex <= 1)
            {
                _view.PreviousPage.Enabled = false;
                _view.FirstPage.Enabled = false;
            }
            else
            {
                _view.PreviousPage.Enabled = true;
                _view.FirstPage.Enabled = true;
            }
        }

        public PartnerViewData GetFocusedPartner()
        {
            if (_view.PartnersGridView.SelectedRows.Count > 0)
            {
                var selectedRow = _view.PartnersGridView.SelectedRows;
                return selectedRow[0].DataBoundItem as PartnerViewData;
            }
            return null;
        }
        public void ClearFilter() 
        {
            _view.FilterValue.Text = string.Empty;
        }
        #endregion

        #region Private methods
        private void SetPageData()
        {
            if (_partnersData.Count > 0)
            {
                int startIndex = (_currentPageIndex - 1) * PageSize;
                _view.CurrentPageData = _partnersData.Skip(startIndex).Take(PageSize).ToList();
                _lastPageIndex = (int)Math.Ceiling(Convert.ToDecimal(_partnersData.Count) / PageSize);
                _view.RecordStatus.Text = _currentPageIndex + " / " + _lastPageIndex;
            }
            else
            {
                _view.CurrentPageData = new List<PartnerViewData>();
                _view.RecordStatus.Text = _currentPageIndex + " / " + _currentPageIndex;
                _currentPageIndex = 0;
            }

            EnableControls();
        }

        private PartnerFilterModel GetFilter() 
        {
            return new PartnerFilterModel()
            {
                SearchByKey = _view.FilterByValues.SelectedValue.ToString(),
                Value = _view.FilterValue.Text
            };
        }
        #endregion
    }
}
