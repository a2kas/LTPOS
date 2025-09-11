using Microsoft.Extensions.DependencyInjection;
using POS_display.Models.Partner;
using POS_display.Presenters.Partners;
using POS_display.Repository.Partners;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamroutilities.Client;

namespace POS_display.Views.Partners
{
    public partial class PartnerEditorView : FormBase, IPartnerEditorView, IBaseView
    {
        #region Members
        private readonly PartnerEditorPresenter _partnerEditorPresenter;
        private PartnerEditConfig _partnerEditConfig;
        #endregion

        #region Constructor
        public PartnerEditorView()
        {
            InitializeComponent();
            _partnerEditorPresenter = new PartnerEditorPresenter(
                this, 
                new PartnerRepository(), 
                Program.ServiceProvider.GetRequiredService<ITamroClient>());
        }
        #endregion

        #region Properties
        public long Id 
        { 
            get;
            set; 
        }

        public TextBox PartnerName 
        {
            get => tbName;
            set => tbName = value;
        }

        public ComboBox BuyerType
        {
            get => cbBuyerType;
            set => cbBuyerType = value;
        }

        public ComboBox SupplierType
        {
            get => cbSupplierType;
            set => cbSupplierType = value;
        }

        public TextBox Fax
        {
            get => tbFax;
            set => tbFax = value;
        }

        public TextBox Phone
        {
            get => tbPhone;
            set => tbPhone = value;
        }

        public TextBox PostIndex
        {
            get => tbPostIndex;
            set => tbPostIndex = value;
        }

        public TextBox Email
        {
            get => tbEmail;
            set => tbEmail = value;
        }

        public TextBox CountryCode
        {
            get => tbCountryCode;
            set => tbCountryCode = value;
        }

        public TextBox VatCode
        {
            get => tbVatCode;
            set => tbVatCode = value;
        }

        public ComboBox Type
        {
            get => cbType;
            set => cbType = value;
        }

        public TextBox CompanyCode
        {
            get => tbCompanyCode;
            set => tbCompanyCode = value;
        }

        public TextBox Comment
        {
            get => tbComment;
            set => tbComment = value;
        }

        public TextBox Address
        {
            get => tbAddress;
            set => tbAddress = value;
        }

        public TextBox City
        {
            get => tbCity;
            set => tbCity = value;
        }

        public Button Save
        {
            get => btnSave;
            set => btnSave = value;
        }

        public PartnerEditConfig PartnerEditConfig 
        {
            get => _partnerEditConfig;
            set => _partnerEditConfig = value;
        }
        #endregion


        #region Public methods
        public async Task Init()
        {
            await ExecuteWithWaitAsync(async () => await _partnerEditorPresenter.Init());
        }

        public async Task LoadData(decimal partnerId = 0m) 
        {
            await ExecuteWithWaitAsync(async () => await _partnerEditorPresenter.Load(partnerId));
        }
        #endregion

        #region Actions
        private async void btnSave_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () => 
            {
                await _partnerEditorPresenter.Save();
                DialogResult = DialogResult.OK;
            });
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExecuteWithWait(() => _partnerEditorPresenter.SetValues());
        }
        #endregion
    }
}
