using Microsoft.Extensions.DependencyInjection;
using POS_display.Models.TLK;
using POS_display.Repository.Barcode;
using POS_display.Utils.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tamroutilities.Client;
using static POS_display.Enumerator;

namespace POS_display.wpf.ViewModel
{
    public class SubmitBarcode : BaseViewModel
    {
        private IAsyncCommand _SubmitBarcodeCommand;
        public IAsyncCommand SubmitBarcodeCommand => _SubmitBarcodeCommand ?? (_SubmitBarcodeCommand = new BaseAsyncCommand(ScanBarcode));

        private async Task ScanBarcode()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                BarcodeStr = Barcode;
                if (Barcode.Contains("*"))
                {
                    string[] parts = Barcode.Split('*');
                    if (parts.Length > 1)
                    {
                        BarcodeStr = parts[1];
                    }
                }

                if (IsRecipeCompensated && RecipeType == "mpp" && !SecondScreenScan)
                {
                    var barcodeData = await new BarcodeRepository().GetBarcodeData(BarcodeStr);
                    if (barcodeData.ProductID != SelectedItemProductId)
                    {
                        Barcode = string.Empty;
                        helpers.alert(Enumerator.alert.error, "Įskenuota prekė nesutinka su prekė, kuri buvo pasirinkta iš sąrašo!");
                        return;
                    }

                    var mppBarcodeStatus = await GetMppBarcodeStatusByNpakid7(BarcodeStr, Npakid7);
                    if (mppBarcodeStatus == MppBarcodeStatus.ThereIsNoInList)
                    {
                        MppBarcode = string.Empty;
                        CloseEvent(System.Windows.Forms.DialogResult.OK);
                    }
                    else if (mppBarcodeStatus == MppBarcodeStatus.IsInListValid) 
                    {
                        MppBarcode = BarcodeStr;
                        CloseEvent(System.Windows.Forms.DialogResult.OK);
                    }
                    else if (mppBarcodeStatus == MppBarcodeStatus.IsInListInvalid)
                    {
                        if (helpers.alert(Enumerator.alert.error, $"Išduodami kompensuojamo MPP receptą įskenavote nekompensuojamą barkodą. Ar norite skenuoti kitą?", true))
                        {
                            Barcode = string.Empty;
                            return;
                        }
                        else
                        {
                            CloseEvent(System.Windows.Forms.DialogResult.Cancel);
                        }
                    }
                    else
                        CloseEvent(System.Windows.Forms.DialogResult.OK);
                }
                else 
                {
                    CloseEvent(System.Windows.Forms.DialogResult.OK);
                }
            });
        }

        #region Variables
        private string _barcode;
        #endregion

        #region Properties

        public string MppBarcode { get; set; } = string.Empty;
        public string BarcodeStr { get; set; }
        public string ProductName { get; set; }
        public bool IsRecipeCompensated { get; set; }
        public string RecipeType { get; set; }
        public decimal Npakid7 { get; set; }
        public bool SecondScreenScan { get; set; } = false;
        public decimal SelectedItemProductId { get; set; }

        public string Barcode
        {
            get { return _barcode; }
            set
            {
                if (_barcode != value)
                {
                    _barcode = value;
                    NotifyPropertyChanged(nameof(Barcode));
                }
            }
        }
        #endregion


        #region Private
        private async Task<MppBarcodeStatus> GetMppBarcodeStatusByNpakid7(string barcode, decimal npakid7)
        {
            try
            {
                if (Session.getParam("ERECIPE", "V2") == "0")
                    return MppBarcodeStatus.Unknown;

                var tamroClient = Program.ServiceProvider.GetRequiredService<ITamroClient>();
                var response = await tamroClient.GetAsync<List<BarcodeModel>>(string.Format(Session.CKasV1GetVlkBarcodes, npakid7));
                if (response == null || response.Count == 0)
                    return MppBarcodeStatus.ThereIsNoInList;

                var currentDate = DateTime.Now;

                return response.Any(e => e.Code == barcode && e.StartDate < currentDate && e.EndDate > currentDate) ? 
                    MppBarcodeStatus.IsInListValid :
                    MppBarcodeStatus.IsInListInvalid;
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Information($"[ValidateBarcodeByNpakid7] Pharmacy: {Session.SystemData.kas_client_id}; Error: {ex.Message}");
                return MppBarcodeStatus.Unknown;
            }
        }
        #endregion
    }
}
