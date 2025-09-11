using POS_display.Repository.Pos;
using POS_display.Utils.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace POS_display.wpf.ViewModel
{
    public abstract class FMDbase : BaseViewModel
    {
        #region Commands
        private ICommand _ParseCommand;
        public ICommand ParseCommand
        {
            get
            {
                return _ParseCommand ?? (_ParseCommand = new BaseCommand(ParseEvent));
            }
        }

        private ICommand _ManualInputCommand;
        public ICommand ManualInputCommand
        {
            get
            {
                return _ManualInputCommand ?? (_ManualInputCommand = new BaseCommand(ManualInputEvent));
            }
        }

        private ICommand _DeleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return _DeleteCommand ?? (_DeleteCommand = new BaseCommand(DeleteEvent));
            }
        }
        #endregion
        #region Events
        public async void ParseEvent(object sender)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(code2d))
                    throw new Exception("");
                var fmd_model = new Model.fmd()
                {
                    posDetail = CurrentPosdRow,
                    Barcode2D = code2d,
                };
                fmd_model.Parse2Dbarcode();
                if (string.IsNullOrWhiteSpace(fmd_model.productCodeScheme) || string.IsNullOrWhiteSpace(fmd_model.productCode) || string.IsNullOrWhiteSpace(fmd_model.serialNumber))
                    throw new Exception($"Neteisingai nuskaitytas 2D kodas\nSchema: {fmd_model.productCodeScheme}\nKodas: {fmd_model.productCode}\nNuoseklusis nr.: {fmd_model.serialNumber}\nSerija: {fmd_model.batchId}\n Galiojimas: {fmd_model.expiryDate}");
                await Add2Dbarcode(fmd_model);
            });
            code2d = "";
        }

        public async void ManualInputEvent(object Result)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(serialNumber))
                    throw new Exception("");
                var fmd_model = new Model.fmd()
                {
                    posDetail = CurrentPosdRow,
                    productCodeScheme = SelectedBarcodeType,
                    productCode = productCode,
                    serialNumber = serialNumber
                };
                await Add2Dbarcode(fmd_model);
                serialNumber = "";
            });
        }

        public async void DeleteEvent(object Result)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (fmd_models.Any(a => a.type != "verify"))
                    throw new Exception("Neįmanoma keisti duomenų, nes atsiskaitymo operacija jau buvo atlikta!");
                if (helpers.alert(Enumerator.alert.confirm, "Ar tikrai norite panaikinti šį įrašą?", true))
                {
                    var success = await DB.POS.deleteFMDitem(selected_fmd_model.id);
                    if (success < 0)
                        throw new Exception("Negalima panaikinti šios eilutės!");
                    fmd_models.Remove(selected_fmd_model);
                    NotifyPropertyChanged("fmd_models");
                    NotifyPropertyChanged("IsAlertIdShown");
                    NotifyPropertyChanged("DocumentNumberReadOnly");
                    NotifyPropertyChanged("SubmitEnabled");
                    NotifyPropertyChanged("LeftAmount");
                    NotifyPropertyChanged("IsEnabled");
                }
            });
        }
        #endregion
        #region Abstract
        internal abstract Task Add2Dbarcode(Model.fmd model);
        #endregion
        #region Methods 
        public void PerformFMDReport(Model.fmd fmdModel)
        {
            ExecuteWithWait(() =>
            {
                var wpf = new wpf.View.FMDAlertReport();
                wpf.DataContext = new FMDAlertReportViewModel(wpf, fmdModel);

                using (Popups.wpf_dlg pfForm = new Popups.wpf_dlg(wpf, "FMD Pranešimas"))
                {
                    pfForm.HideControlBox();
                    pfForm.SetNotResizeable();
                    pfForm.Location = helpers.middleScreen(Program.Display1, pfForm);
                    pfForm.Activate();
                    pfForm.BringToFront();
                    pfForm.ShowDialog();
                }
            },false);
        }

        public void PerformFMDNotification(Model.fmd fmdModel)
        {
            if (fmdModel.posDetail?.HasRestriction ?? false)
            {
                helpers.alert(Enumerator.alert.error, 
                    "Patikrinkite pakuotės duomenis!\nPardavimas bus blokuojamas.\nPašalinkite prekę ir bandykite vėl");
            }
        }

        public async Task<Model.fmd> VerifySinglePack(Model.fmd fmdModel)
        {
            return await PerformFMDAsync(
                fmdModel, 
                async (pack, manual) =>
                {
                    return await Session.FMDclient.VerifySinglePackAsync(pack, manual);
                });
        }

        public async Task<Model.fmd> ChangeStateSinglePackAsync(Model.fmd fmdModel, FMD.Model.State state, string referenceNumber = "")
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            PosRepository posRepository = new PosRepository();

            stopwatch.Restart();
            var created = await posRepository.CreateFMDtrans(new Model.fmd()
            {
                posDetail = fmdModel.posDetail,
                Pack = fmdModel.Pack,
                type = GetTransactionType(state),
                referencenumber = referenceNumber
            });
            Serilogger.GetLogger().Information($"[PerformFMD] KAS ID: {Session.SystemData.kas_client_id}" +
                $" POSD.ID: {fmdModel?.posDetail?.id} CreateFMDtrans took {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            var modelNew = (await posRepository.GetFMDItem(created.ToLong())).FirstOrDefault();
            Serilogger.GetLogger().Information($"[PerformFMD] KAS ID: {Session.SystemData.kas_client_id}" +
                $" POSD.ID: {fmdModel?.posDetail?.id} GetFMDItem took {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            var result = await PerformFMDAsync(
                modelNew,
                async (pack, manual) =>
                {
                    return await Session.FMDclient.ChangeStateSinglePackAsync(pack, manual, state);
                });
            Serilogger.GetLogger().Information($"[PerformFMD] KAS ID: {Session.SystemData.kas_client_id}" +
                $" POSD.ID: {fmdModel?.posDetail?.id} PerformFMDAsync took {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Stop();
            Serilogger.GetLogger().Information($"[PerformFMD] KAS ID: {Session.SystemData.kas_client_id}" +
                $" POSD.ID: {fmdModel?.posDetail?.id} ChangeStateSinglePackAsync method completed in {stopwatch.ElapsedMilliseconds} ms");

            return result;
        }

        public async Task<Model.fmd> PerformFMDAsync(Model.fmd model, Func<FMD.Model.Pack, bool, Task<FMD.Model.SinglePackResponse>> fmdFunctionToExcute)
        {
            if (fmdFunctionToExcute != null && model != null
                && string.IsNullOrWhiteSpace(model.Response.operationCode) //no response currently
                && !string.IsNullOrWhiteSpace(model.productCode) && !string.IsNullOrWhiteSpace(model.serialNumber))//existing pack
            {
                PosRepository posRepository = new PosRepository();
                bool manual = (string.IsNullOrWhiteSpace(model.batchId) && string.IsNullOrWhiteSpace(model.expiryDate));
                model.Response = await fmdFunctionToExcute(model.Pack, manual);
                await posRepository.UpdateFMDtrans(model);
            }
            return model;
        }

        internal string GetTransactionType(FMD.Model.State? state = null)
        {
            if (state == null)
                return "verify";
            else if (state == FMD.Model.State.Supplied)
                return "supply";
            else
                return "decommission";
        }
        #endregion
        #region Properties
        private Items.posd _CurrentPosdRow = new Items.posd();
        public Items.posd CurrentPosdRow
        {
            get
            {
                _CurrentPosdRow.fmd_model = fmd_models;
                return _CurrentPosdRow;
            }
            set
            {
                _CurrentPosdRow = value;
                ProductName = _CurrentPosdRow.barcodename;
                fmd_models = _CurrentPosdRow.fmd_model;
                NotifyPropertyChanged("CurrentPosdRow");
            }
        }

        private string _code2d;
        public string code2d
        {
            get
            {
                return _code2d;
            }
            set
            {
                _code2d = value;
                NotifyPropertyChanged("code2d");
            }
        }

        private string _codeType;
        public string codeType
        {
            get
            {
                return _codeType;
            }
            set
            {
                _codeType = value;
                NotifyPropertyChanged("codeType");
            }
        }

        private string _serialNumber;
        public string serialNumber
        {
            get
            {
                return _serialNumber;
            }
            set
            {
                _serialNumber = value;
                NotifyPropertyChanged("serialNumber");
            }
        }

        private string _ProductName;
        public string ProductName
        {
            get
            {
                return _ProductName;
            }
            set
            {
                _ProductName = value;
                NotifyPropertyChanged("ProductName");
            }
        }

        public bool Barcode2DException
        {
            get
            {
                return CurrentPosdRow.Flags.HasFlag(Enumerator.ProductFlag.FmdException);
            }
            set
            {
                if (value)
                    CurrentPosdRow.Flags |= Enumerator.ProductFlag.FmdException;
                else
                    CurrentPosdRow.Flags &= ~Enumerator.ProductFlag.FmdException;

                Serilogger.GetLogger().Information($"POS header ID: {CurrentPosdRow?.hid} Detail ID: {CurrentPosdRow?.id} has been marked FMD exception as" +
                    $" {CurrentPosdRow.Flags.HasFlag(Enumerator.ProductFlag.FmdException)}");
            }
        }

        public virtual bool IsEnabled 
        {
            get 
            { 
                return (_CurrentPosdRow?.qty ?? 0) > (fmd_models?.Count ?? 0);
            }
        }

        public string LeftAmount
        {
            get
            {
                int left_qty = (int)Math.Ceiling(_CurrentPosdRow.qty - fmd_models.Count);
                return left_qty > 0 ? "Liko: " + left_qty : "";
            }
        }

        private List<wpf.Model.fmd> _fmd_models;
        public List<wpf.Model.fmd> fmd_models
        {
            get
            {
                if (_fmd_models == null)
                    _fmd_models = new List<Model.fmd>();
                return _fmd_models;
            }
            set
            {
                _fmd_models = value.OrderByDescending(o => o.id).ToList();
                NotifyPropertyChanged("fmd_models");
                NotifyPropertyChanged("IsAlertIdShown");
            }
        }

        private wpf.Model.fmd _selected_fmd_model;
        public virtual wpf.Model.fmd selected_fmd_model
        {
            get
            {
                return _selected_fmd_model;
            }
            set
            {
                _selected_fmd_model = value;
                NotifyPropertyChanged("selected_fmd_model");
            }
        }

        private List<string> _BarcodeType;
        public List<string> BarcodeType
        {
            get
            {
                if (_BarcodeType == null)
                {
                    _BarcodeType = new List<string>
                    {
                        Parser2D.types2Dcode.gtin.ToString(),
                        Parser2D.types2Dcode.ppn.ToString()
                    };
                }
                return _BarcodeType;
            }
        }

        private string _SelectedBarcodeType = Parser2D.types2Dcode.gtin.ToString();
        public string SelectedBarcodeType
        {
            get
            {
                return _SelectedBarcodeType;
            }
            set
            {
                _SelectedBarcodeType = value;
                NotifyPropertyChanged("SelectedBarcodeType");
            }
        }

        private string _productCode;
        public string productCode
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_productCode))
                    _productCode = CurrentPosdRow?.barcode;
                return _productCode;
            }
            set
            {
                _productCode = value;
                NotifyPropertyChanged("productCode");
            }
        }

        public bool IsAlertIdShown
        {
            get
            {
                return fmd_models.Count(c => !string.IsNullOrWhiteSpace(c.Response.alertId)) > 0;
            }
        }
        #endregion
    }
    
}
