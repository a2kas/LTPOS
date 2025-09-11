using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_display.Repository.Price;

namespace POS_display.Popups
{
    public partial class SandBox : Form
    {
        #region Members
        private readonly PriceRepository _priceRepository = null;
        #endregion

        public SandBox()
        {
            InitializeComponent();
            _priceRepository = new PriceRepository();
        }

        public async void PriceTest()
        {
            decimal VKBPrice100 = 6.09M;
            decimal VKBPrice90 = 5.48M;
            decimal VKBPrice80 = 4.87M;
            decimal VKBPrice50 = 3.05M;
            decimal kas_id = 10000111657;

            //decimal t0 = await DB.prices.GetSalesPriceWithDiscount(kas_id);
            //decimal t1 = await DB.prices.GetQty(10000000024, 10000000107);
            //decimal t2 = await DB.prices.GetCompPriceWithDiscount(kas_id, Session.PriceClass);
            //decimal t3 = await DB.prices.GetSalesPriceComp(kas_id, VKBPrice100, Session.PriceClass);
            //decimal t4 = await DB.prices.GetSalesPriceComp(kas_id, VKBPrice90, Session.PriceClass);
            //decimal t5 = await DB.prices.GetSalesPriceComp(kas_id, VKBPrice80, Session.PriceClass);
            //decimal t6 = await DB.prices.GetSalesPriceComp(kas_id, VKBPrice50, Session.PriceClass);
            //string t7 = await DB.prices.GetATCCode(kas_id);
            //decimal t8 = await DB.prices.GetVatFromStock(kas_id);

            decimal t9 = await _priceRepository.GetCompPriceWithDiscount(kas_id);
            decimal t10 = await _priceRepository.SearchProductQty(kas_id);
            decimal t11 = await _priceRepository.GetCompPriceWithDiscount(kas_id);
            decimal t12 = await _priceRepository.GetSalesPriceComp(kas_id, VKBPrice100, Session.PriceClass);
            decimal t13 = await _priceRepository.GetSalesPriceComp(kas_id, VKBPrice90, Session.PriceClass);
            decimal t14 = await _priceRepository.GetSalesPriceComp(kas_id, VKBPrice80, Session.PriceClass);
            decimal t15 = await _priceRepository.GetSalesPriceComp(kas_id, VKBPrice50, Session.PriceClass);
            string t16 = await _priceRepository.GetATCCode(kas_id);
            decimal t17 = await _priceRepository.GetVatFromStock(kas_id);


        }
    }
}
