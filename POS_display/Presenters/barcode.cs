using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_display.Repository.SalesOrder;
using POS_display.Repository.Barcode;

namespace POS_display.Presenters
{
    public class barcode : DB_Base
    {
        private Views.Ibarcode _view;
        private Models.Barcode _model;
        private readonly SalesOrderRepository _salesOrderRepository;
        private readonly BarcodeRepository _barcodeRepository;
        
        public barcode(Views.Ibarcode view, Models.Barcode model)
        {
            _view = view;
            _model = model;
            _salesOrderRepository = new SalesOrderRepository();
            _barcodeRepository = new BarcodeRepository();
        }

        public async Task ScanBarcode()
        {
            _model.PosdId = -1;
            if (_model.IsInPrices)
            {
                if (_view.display2_timer <= 0)
                {
                    if (string.IsNullOrWhiteSpace(_model.NpakId))
                    {
                        if (!helpers.alert(Enumerator.alert.confirm, "Ši prekė nerasta vaistų kainų sąraše.\nRašykite laišką į servicedesk.lt@tamro.com\nAr vistiek norite parduoti prekę?", true))
                            _model.BarcodeStr = "";
                    }
                    else
                    {
                        var selected_product_id = Program.Display2.ExecuteFromRemote(_model);
                        if (Program.Display2.PricesVM.FirstPrescriptionUncomp)
                            _model.CompPercent = 0;
                        if (selected_product_id <= 0)
                            _model.BarcodeStr = "";
                        else if (selected_product_id != _model.ProductId)
                        {
                            _model.CheapestPrescription = Program.Display2.PricesVM.gvPricesSelectedCheapest;
                            _model.FirstPrescriptionReason = Program.Display2.PricesVM.FirstPrescriptionReason;
                            _model.BarcodeStr = "";
                            var vm = new wpf.ViewModel.SubmitBarcode()
                            {
                                ProductName = "Skenuokite prekę",
                            };
                            var wpf = new wpf.View.SubmitBarcode()
                            {
                                DataContext = vm
                            };
                            using (var d = new POS_display.popups.wpf_dlg(wpf, "Prekės pasirinkimas"))
                            {
                                d.Location = helpers.middleScreen(Program.Display1, d);
                                d.ShowDialog();
                                if (d.DialogResult == System.Windows.Forms.DialogResult.OK)
                                {
                                    _model.BarcodeStr = vm.Barcode;
                                    await GetDataFromBarcode();
                                }
                            }
                        }
                    }
                }
                else
                {
                    helpers.alert(Enumerator.alert.warning, "Blokuojamas receptinių vaistų pardavimas\nLiko laiko " + _view.display2_timer + "s");
                    _model.BarcodeStr = "";
                }
            }

            if (!string.IsNullOrWhiteSpace(_model.BarcodeStr))
            {
                _model.PosdId = await DB.POS.CreatePosdPos(_view.PoshItem.Id, _model.BarcodeStr, _model.Mode, 0);

                var creditoriId = await DB.POS.GetPrepTransCreditor(_view.PoshItem.Id);
                
                if(creditoriId != 0)
                 await DB.POS.asyncCreatePrepTrans(_model.PosdId, creditoriId);

            }
        }

        public async Task GetDataFromBarcode()
        {
            _model.Mode = 0;
            _model.EAN = _model.BarcodeStr;
            _model.QtyStr = "";
            if (_model.BarcodeStr?.IndexOf('*') > 0)
            {
                _model.QtyStr = _model.BarcodeStr.Substring(0, _model.BarcodeStr.IndexOf('*')).ToUpper();
                _model.EAN = _model.BarcodeStr.Substring(_model.BarcodeStr.IndexOf('*') + 1);
            }
            _model.FmdModel = new wpf.Model.fmd
            {
                Barcode2D = _model.EAN
            };
            _model.FmdModel.Parse2Dbarcode();
            if (!string.IsNullOrWhiteSpace(_model.FmdModel.productCodeScheme))
            {
                _model.EAN = _model.FmdModel.productCode;
                _model.BarcodeStr = $"{_model.FmdModel.productCodeScheme};{_model.EAN};{_model.FmdModel.serialNumber};{_model.FmdModel.batchId};{_model.FmdModel.expiryDate}";
                if (_model.QtyStr != "")
                    _model.BarcodeStr = $"{_model.QtyStr}*{_model.BarcodeStr}";
                _model.Mode = 6;
            }

            var bd = await _barcodeRepository.GetBarcodeData(_model.EAN);
            if (bd != null)
            {
                _model.ProductId = bd.ProductID;
                _model.BarcodeID = bd.BarcodeID;
            }

            _model.NpakId = (from item in Session.Generics.ItemsAllList
                             where item.kas_id == _model.ProductId //&& item.rekname == "C"
                             select item).DefaultIfEmpty(new Items.Prices.GenericItem() { NpakId = "" }).First().NpakId;
            _model.Dosage = await DB.POS.getBarcodeRatio(_model.ProductId);
            if (_model.QtyStr != "")
            {
                if (_model.QtyStr.IndexOf("D") > -1)
                    _model.Dosage = _model.QtyStr.Replace("D", "").ToDecimal();
                else
                    _model.Dosage = _model.QtyStr.ToDecimal() * _model.Dosage;
            }
            _model.Dosage = Math.Round(_model.Dosage, 2, MidpointRounding.AwayFromZero);//todo?
            _model.Gr4 = await DB.POS.getGr4(_model.ProductId);
            _model.SalesOrderProduct = await _salesOrderRepository.GetSalesOrderProduct(_model.ProductId);
        }
    }
}
