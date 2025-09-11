using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POS_display.wpf.ViewModel
{
    public class FMDkas_posd : FMDbase
    {
        #region Methods
        internal override async Task Add2Dbarcode(Model.fmd model)
        {
            if (CurrentPosdRow.Flags.HasFlag(Enumerator.ProductFlag.FmdException))
                throw new Exception("Yra pažymeta, kad produktas neturi 2D barkodo arba jis yra pažeistas");
            if (model.serialNumber.Contains(model.productCode) || model.serialNumber.Length > 20)
                throw new Exception("2D kodas turi būti skenuojamas viršutiniame laukelyje!");
            if (fmd_models_log.Any(a => a.productCodeScheme != model.productCodeScheme || a.productCode != model.productCode))
            {
                var productIdFromBC = await DB.recipe.getProductIdFromBarcode(model.productCode);
                if (CurrentPosdRow.productid != productIdFromBC)
                    throw new Exception("Ši pakuotė priklauso kitai prekei");
            }
            if (fmd_models_log.Count(c => c.productCodeScheme == model.productCodeScheme && c.productCode == model.productCode && c.serialNumber == model.serialNumber && c.type == "supply") <= 0)
                throw new Exception("Ši pakuotė nebuvo parduota!");
            if (fmd_models.Count(c => c.productCodeScheme == model.productCodeScheme && c.productCode == model.productCode && c.serialNumber == model.serialNumber && c.type == "decommission") > 0)
                throw new Exception("Ši pakuotė jau nuskenuota");
            model.type = "decommission";
            fmd_models.Insert(0, model);
            if (!string.IsNullOrEmpty(model.alertId))
                PerformFMDReport(model);
            else
                PerformFMDNotification(model);
            NotifyPropertyChanged("fmd_models");
            NotifyPropertyChanged("IsAlertIdShown");
        }
        #endregion

        public List<wpf.Model.fmd> fmd_models_log { get; set; }
    }
}
