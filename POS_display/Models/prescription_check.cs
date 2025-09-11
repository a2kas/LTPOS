using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Models
{
    public class prescription_check
    {
        #region Constructor
        public prescription_check(Items.posd current_row)
        {
            currentPosdRow = current_row;
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("Recepto čekis ", 2);
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
        #endregion
        #region Model
        public string RecipeValid { get; set; }
        public Items.posd currentPosdRow { get; set; }
        #endregion
        #region Business Logic
        public async Task PrintPrescriptionCheck()
        {
            await Session.FP550.OpenNonFiscal();
            await Session.FP550.PrintNonFiscal(currentPosdRow.barcodename);
            if (RecipeValid != "")
                await Session.FP550.PrintNonFiscal("PAKANKA: " + RecipeValid);
            await Session.FP550.PrintNonFiscal("IŠDUOT. KIEKIS: " + currentPosdRow.qty.ToString());
            await Session.FP550.PrintNonFiscal("KAINA: " + currentPosdRow.sum.ToString());
            await Session.FP550.PrintNonFiscal("Vaistai išduoti: ");
            await Session.FP550.PrintNonFiscal("    " + Session.User.postname);
            await Session.FP550.PrintNonFiscal("    " + Session.User.DisplayName);
            await Session.FP550.CloseNonFiscal();
        }
        #endregion
    }
}
