using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POS_display.wpf.ViewModel
{
    public class FMDposd : FMDbase
    {
        #region Methods
        internal override async Task Add2Dbarcode(Model.fmd model)
        {
            if (CurrentPosdRow.Flags.HasFlag(Enumerator.ProductFlag.FmdException))
                throw new Exception("Yra pažymeta, kad produktas neturi 2D barkodo arba jis yra pažeistas");
            if (model.serialNumber.Contains(model.productCode) || model.serialNumber.Length > 20)
                throw new Exception("2D kodas turi būti skenuojamas viršutiniame laukelyje!");
            if (fmd_models.Any(a => a.type != "verify"))
                throw new Exception("Neįmanoma keisti duomenų, nes atsiskaitymo operacija jau buvo atlikta!");
            if (fmd_models.Count == 0 || fmd_models.Any(a => a.productCodeScheme != model.productCodeScheme || a.productCode != model.productCode))
            {
                var productIdFromBC = await DB.recipe.getProductIdFromBarcode(model.productCode);
                if (productIdFromBC <= 0)
                    throw new Exception("Nerastas pakuotės barkodas. Kreipkitės į IT");
                if (CurrentPosdRow.productid != productIdFromBC)
                    throw new Exception("Ši pakuotė priklauso kitai prekei");
            }
            if (fmd_models.Count(c => c.productCodeScheme == model.productCodeScheme && c.productCode == model.productCode && c.serialNumber == model.serialNumber) > 0)
                throw new Exception("Ši pakuotė jau nuskenuota");
            model.type = GetTransactionType();
            model.id = await DB.POS.CreateFMDtrans(model);
            model = await VerifySinglePack(model);
            if (model == null)
                throw new Exception("Nepavyksta pridėti pakuotės. Patikrinkite, ar jos nėra kvite!");
            fmd_models.Insert(0, model);
            if (!string.IsNullOrEmpty(model.alertId))
                PerformFMDReport(model);
            else
                PerformFMDNotification(model);
            NotifyPropertyChanged("fmd_models");
            NotifyPropertyChanged("LeftAmount");
            NotifyPropertyChanged("IsAlertIdShown");
            NotifyPropertyChanged("IsEnabled");
        }
        #endregion
    }
    
}
