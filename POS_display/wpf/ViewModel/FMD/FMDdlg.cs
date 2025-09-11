using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POS_display.wpf.ViewModel
{
    public class FMDdlg : FMDbase
    {
        private ICommand _SubmitCommand;
        public ICommand SubmitCommand
        {
            get
            {
                return _SubmitCommand ?? (_SubmitCommand = new BaseCommand(SubmitEvent));
            }
        }

        #region Methods
        internal override async Task Add2Dbarcode(Model.fmd model)
        {
            if (CurrentPosdRow.Flags.HasFlag(Enumerator.ProductFlag.FmdException))
                throw new Exception("Yra pažymeta, kad produktas neturi 2D barkodo arba jis yra pažeistas");
            if (model.serialNumber.Contains(model.productCode) || model.serialNumber.Length > 20)
                throw new Exception("2D kodas turi būti skenuojamas viršutiniame laukelyje!");
            if (fmd_models.Count(c => c.productCodeScheme == model.productCodeScheme && c.productCode == model.productCode && c.serialNumber == model.serialNumber) > 0)
                throw new Exception("Ši pakuotė jau nuskenuota");
            model.type = GetTransactionType();
            model.id = await DB.POS.CreateFMDtrans(model);
            model = await VerifySinglePack(model);
            fmd_models.Insert(0, model);
            if (!string.IsNullOrEmpty(model.alertId))
                PerformFMDReport(model);
            else
                PerformFMDNotification(model);
            NotifyPropertyChanged("fmd_models");
            NotifyPropertyChanged("IsAlertIdShown");
            NotifyPropertyChanged("SubmitEnabled");
            NotifyPropertyChanged("DocumentNumberReadOnly");
        }

        public async new Task<Model.fmd> VerifySinglePack(Model.fmd fmdModel)
        {
            if (string.IsNullOrWhiteSpace(DocumentNumber))
                throw new Exception("Nenurodytas dokumento numeris.");
            fmdModel.referencenumber = DocumentNumber;
            return await base.VerifySinglePack(fmdModel);
        }
        #endregion

        #region Override
        public override bool IsEnabled { get { return true; } }
        #endregion

        #region Events
        public async void SubmitEvent(object sender)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(DocumentNumber))
                    throw new Exception("Nenurodytas dokumento numeris.");
                if (SelectedState == null)
                    throw new Exception("Nepasirinktas būsimas statusas.");
                foreach (var m in fmd_models)
                {
                    var resp = await ChangeStateSinglePackAsync(m, (FMD.Model.State)SelectedState, DocumentNumber);
                    m.Response = resp.Response;
                }
                if (fmd_models.Any(a => a.Response.Success == false))
                {
                    helpers.alert(Enumerator.alert.warning, "Ne visoms pakuotėms buvo pakeistas statusas.");
                    fmd_models = fmd_models.Where(w => w.Response.Success == false).ToList();
                }
                else
                {
                    helpers.alert(Enumerator.alert.info, "Operacija atlikta sėkmingai");
                    CloseEvent(System.Windows.Forms.DialogResult.OK);
                }
            });
        }
        #endregion
        private wpf.Model.fmd _selected_fmd_model;
        public override wpf.Model.fmd selected_fmd_model
        {
            get
            {
                return _selected_fmd_model;
            }
            set
            {
                _selected_fmd_model = value;
                ProductName = "";
                using (System.Data.DataTable dt = DB.recipe.getDataFromBarcode2(value?.productCode))
                {
                    if (dt?.Rows?.Count > 0)
                        ProductName = dt.Rows[0]["name"]?.ToString() ?? "";
                }
                NotifyPropertyChanged("selected_fmd_model");
            }
        }

        private string _DocumentNumber;
        public string DocumentNumber
        {
            get
            {
                return _DocumentNumber;
            }
            set
            {
                _DocumentNumber = value;
                NotifyPropertyChanged("DocumentNumber");
            }
        }

        private Dictionary<FMD.Model.State, string> _StateList;
        public Dictionary<FMD.Model.State, string> StateList
        {
            get
            {
                if (_StateList == null)
                {
                    _StateList = new Dictionary<FMD.Model.State, string>()
                    {
                        { FMD.Model.State.Destroyed, "Sunaikintas" },
                        { FMD.Model.State.Active, "Aktyvus" },
                        { FMD.Model.State.Sample, "Laboratoriniams tyrimams" },
                        { FMD.Model.State.Supplied, "Išduotas" }
                    };
                }

                return _StateList;
            }
        }


        private FMD.Model.State? _SelectedState;
        public FMD.Model.State? SelectedState
        {
            get
            {
                return _SelectedState;
            }
            set
            {
                _SelectedState = value;
                NotifyPropertyChanged("SelectedState");
                NotifyPropertyChanged("SubmitEnabled");
            }
        }

        public bool SubmitEnabled
        {
            get
            {
                return true;
                if (SelectedState == null || fmd_models.Count == 0)
                    return false;
                else if (SelectedState == FMD.Model.State.Active)
                    return !fmd_models.Any(a => a.state == null || (a.state_enum != FMD.Model.State.Supplied && a.state_enum != FMD.Model.State.Sample));
                else
                    return !fmd_models.Any(a => a.state == null || a.state_enum != FMD.Model.State.Active);
            }
        }

        public bool DocumentNumberReadOnly
        {
            get
            {
                return fmd_models.Count > 0;
            }
        }
    }
}
