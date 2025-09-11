using Microsoft.Extensions.DependencyInjection;
using POS_display.Models.HomeMode;
using POS_display.Models.TransactionService.Stock;
using POS_display.Presenters.Barcode;
using POS_display.Repository.Barcode;
using POS_display.Repository.HomeMode;
using POS_display.Repository.Pos;
using POS_display.Repository.SalesOrder;
using POS_display.Views.HomeMode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamroutilities.Client;

namespace POS_display.Presenters
{
    public class BarcodePresenter : IBarcodePresenter
    {
        private Views.IBarcodeView _view;
        private Models.Barcode _model;
        private readonly IBarcodeRepository _barcodeRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IPosRepository _posRepository;
        private readonly IHomeModeRepository _homeModeRepository;
        private readonly ITamroClient _tamroClient;
        public BarcodePresenter(Views.IBarcodeView view, Models.Barcode model)
        {
            _view = view;
            _model = model;
            _barcodeRepository = new BarcodeRepository();
            _salesOrderRepository = new SalesOrderRepository();
            _posRepository = new PosRepository();
            _homeModeRepository = new HomeModeRepository();
            _tamroClient = Program.ServiceProvider.GetRequiredService<ITamroClient>();

        }

        public async Task ScanBarcode()
        {
            _model.PosdId = -1;
            if (_model.IsInPrices && !Session.ExclusiveProducts.ContainsKey(_model.ProductId))
            {
                if (_view.display2_timer <= 0)
                {
                    if (_model.ProductIdBySecondScreen == 0m)
                    {
                        var selectedProduct = _model.SelectedItem is null ? Program.Display2.ExecuteFromRemote(_model) : _model.SelectedItem;
                        if (Program.Display2.PricesVM.FirstPrescriptionUncomp)
                            _model.CompPercent = 0;

                        if (selectedProduct?.ProductId <= 0)
                        {
                            _model.BarcodeStr = string.Empty;
                        }
                        else if (Session.HomeMode) 
                        {
                            using (HomeModeQuantityView homeModeQtyView = new HomeModeQuantityView(selectedProduct))
                            {
                                homeModeQtyView.ShowDialog();

                                if (homeModeQtyView.DialogResult == DialogResult.OK)
                                {
                                    _model.HomeModeQuantities = homeModeQtyView.GetQuantites();
                                    var qtyStr = $@"D{Math.Round(_model.HomeModeQuantities.HomeQuantityByRatio + _model.HomeModeQuantities.RealQuantityByRatio, 4)
                                        .ToString().Replace(",", ".")}";     
                                    
                                    string barcodeStr = await _posRepository.GetBarcodeByProductId(selectedProduct?.ProductId ?? -1);
                                    _model.BarcodeStr = !string.IsNullOrWhiteSpace(barcodeStr) ? $"{qtyStr}*{barcodeStr}" : "";
                                    _model.NpakId7 = selectedProduct?.NpakId7 ?? 0m;
                                    _model.ProductIdBySecondScreen = selectedProduct?.ProductId ?? 0m;
                                    _model.QtyStr = qtyStr;
                                    _model.CheapestPrescription = Program.Display2.PricesVM.gvPricesSelectedCheapest;
                                    _model.FirstPrescriptionReason = Program.Display2.PricesVM.FirstPrescriptionReason;
                                }
                            }
                        }
                        else if (selectedProduct?.ProductId != _model.ProductId)
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
                            using (var d = new Popups.wpf_dlg(wpf, "Prekės pasirinkimas"))
                            {
                                d.Location = helpers.middleScreen(Program.Display1, d);
                                d.ShowDialog();
                                if (d.DialogResult == DialogResult.OK)
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

            if (_model.ProductIdBySecondScreen != 0 && _model.ProductIdBySecondScreen != _model.ProductId)
            {
                helpers.alert(Enumerator.alert.error, "Įskenuota prekė nesutinka su prekė, kuri buvo pasirinkta iš sąrašo!");
                _model.BarcodeStr = "";
            }

            if (Session.WoltMode && !Session.WoltProducts.Contains(_model.ProductId.ToLong()))
            {
                helpers.alert(Enumerator.alert.warning, "Šis produktas negali būti parduodamas per WOLT!");
                _model.BarcodeStr = "";
            }

            if (Session.HomeMode && !string.IsNullOrWhiteSpace(_model.BarcodeStr))
            {
                var productFlags = Session.ProductFlags.ContainsKey(_model.ProductId) ? 
                    Session.ProductFlags[_model.ProductId] :
                    Enumerator.ProductFlag.None;

                if (!productFlags.HasFlag(Enumerator.ProductFlag.NotReserveQty))
                {
                    helpers.alert(Enumerator.alert.error, "Ši prekė negali būti parduodama į namus");
                    _model.BarcodeStr = "";
                }
                else if (_model.HomeModeQuantities.HomeQuantity != Math.Floor(_model.HomeModeQuantities.HomeQuantity))
                {
                    helpers.alert(Enumerator.alert.error, "Pristatant prekes į namus, galima parduoti tik visą prekės pakuotę");
                    _model.BarcodeStr = "";
                }
                else if (!await IsEnoughTamroQty(_model))
                {
                    helpers.alert(Enumerator.alert.error, "Nepakankamas prekės likutis didmenoje");
                    _model.BarcodeStr = "";
                }
            }

            if (!string.IsNullOrWhiteSpace(_model.BarcodeStr))
            {
                _model.PosdId = await DB.POS.CreatePosdPos(_view.PoshItem.Id, _model.BarcodeStr, _model.Mode, 0);

                var creditoriId = await DB.POS.GetPrepTransCreditor(_view.PoshItem.Id);
                
                if (creditoriId != 0)
                 await DB.POS.asyncCreatePrepTrans(_model.PosdId, creditoriId);

                if (Session.HomeMode)                
                    await _homeModeRepository.CreateHomeDeliveryDetail(_view.PoshItem.Id, _model.PosdId, _model.HomeModeQuantities);                

                if (_model.PosdId > 0 && _model.ProductIdBySecondScreen != 0m)
                    Program.Display2.StartTimer();

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
                _model.EAN = _model.BarcodeStr.Substring(_model.BarcodeStr.IndexOf('*') + 1).Trim();
            }
            _model.FmdModel = new wpf.Model.fmd();
            _model.FmdModel.Barcode2D = _model.EAN;
            _model.FmdModel.Parse2Dbarcode();
            if (!string.IsNullOrWhiteSpace(_model.FmdModel.productCodeScheme))
            {
                _model.SerialNumber = _model.FmdModel.serialNumber;
                _model.EAN = _model.FmdModel.productCode;
                _model.BarcodeStr = $"{_model.FmdModel.productCodeScheme};{_model.EAN};{_model.FmdModel.serialNumber};{_model.FmdModel.batchId};{_model.FmdModel.expiryDate}";
                if (_model.QtyStr != "")
                    _model.BarcodeStr = $"{_model.QtyStr}*{_model.BarcodeStr}";
                _model.Mode = 6;
            }

            var bd = await _barcodeRepository.GetBarcodeData(_model.EAN.Trim());
            if (bd != null)
            {
                _model.ProductId = bd.ProductID;
                _model.BarcodeID = bd.BarcodeID;
            }
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
            _model.IsSalesOrderProduct = await _salesOrderRepository.GetSalesOrderProduct(_model.ProductId.ToLong());
            _model.HasPriceChange = await _barcodeRepository.HasPriceChange(_model.ProductId.ToLong());
        }

        public async Task<bool> IsEnoughTamroQty(Models.Barcode model) 
        {
            decimal.TryParse(Session.getParam("HOMEMODE", "BUFFERSIZE"), out decimal bufferSize);
            var result = await _tamroClient.GetAsync<List<StockViewModel>>(
                     string.Format(Session.TransactionV3GetLTCountryStocks,
                                   helpers.BuildQueryString("LocalItemCodes=", new List<string>() { model.ProductId.ToString() }) +
                                   "&StockType=WholeSale"));

            var stockModel = result.FirstOrDefault();
            if (stockModel != null) 
            {
                return (model.Qty + bufferSize) < stockModel.Wholesale?.FirstOrDefault()?.TotalQty.ToDecimal();
            }
            return false;
        }
    }
}
