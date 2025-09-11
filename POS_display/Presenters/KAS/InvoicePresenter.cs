using POS_display.Models.KAS;
using POS_display.Models.Partner;
using POS_display.Repository.KAS;
using POS_display.Repository.Partners;
using POS_display.Views.KAS;
using System;
using System.Threading.Tasks;

namespace POS_display.Presenters.KAS
{
    public class InvoicePresenter : BasePresenter, IInvoicePresenter
    {
        #region Members
        private readonly IInvoiceView _view;
        private readonly IKASRepository _kasRepository = null;
        private readonly IPartnerRepository _partnerRepository = null;
        private const int _buyer = 7419;
        private const string _dateFormat = "yyyy.MM.dd";
        #endregion

        #region Constructor
        public InvoicePresenter(IInvoiceView view, IKASRepository kasRepository, IPartnerRepository partnerRepository)
        {
            _view = view;
            _kasRepository = kasRepository;
            _partnerRepository = partnerRepository;
        }
        #endregion

        #region Public methods
        public async Task Init(PosHeader posHeader)
        {
            _view.CheckNo.Text = posHeader.CheckNo;
            _view.CheckDate.Text = posHeader.DocumentDate.Date.ToString(_dateFormat);
            _view.DocumentDate.Text = DateTime.Now.Date.ToString(_dateFormat);
            long sfNumber = await _kasRepository.GetSFNumber(new DateTime(2020, 12, 01));
            _view.DocumentNo.Text = sfNumber.ToString();
            ChequePresent chequePresent = await _kasRepository.GetBENUMTransaction(posHeader.Id.ToLong(), _buyer);
            await SetPartnerDataByScala(chequePresent);
        }

        public async Task SetPartnerDataByScala(ChequePresent chequePresent) 
        {
            if (chequePresent != null)
            {
                Partner partner = await _partnerRepository.GetPartnerByScala(chequePresent.Buyer.ToString());
                if (partner != null)
                {
                    _view.DebtorEcode.ReadOnly = true;
                    _view.SelDebtor.Enabled = false;
                    _view.DebtorEcode.Text = partner.ECode;
                    _view.DebtorName.Text = partner.Name;
                    _view.CreditorId = partner.Id;
                }
            }
        }

        public async Task<bool> CheckSFHeaderExist(string invoice_no) 
        {
            return await _kasRepository.CheckSFHeader(invoice_no);
        }

        public async Task<Partner> LoadPartnerData(string ecode)
        {
            return await _partnerRepository.GetPartner(_view.DebtorEcode.Text);
        }

        public void SetPartnerData(Partner partner)
        {
            _view.DebtorEcode.Text = partner.ECode;
            _view.DebtorName.Text = partner.Name;
            _view.CreditorId = partner.Id;
        }

        public void EnableSaving()
        {
            if (_view.DocumentNo.Text.Replace('.', ',').ToDecimal() > 0 && !_view.DebtorEcode.Text.Equals(""))
                _view.Save.Enabled = true;
            else
                _view.Save.Enabled = false;
        }
        #endregion
    }
}
