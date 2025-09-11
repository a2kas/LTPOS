using POS_display.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_display.Display.Views
{
    public interface IDisplay1View
    {
        Items.posh PoshItem { get; set; }
        Items.posd currentPosdRow { get; set; }
        Task BindGrid();

        Task TurnOffHomeMode();
        Task TurnOffWoltMode();
        Task DeletePosDetail(Items.posd posDetail);

        event EventHandler btnPosdFMD_Event;
        event EventHandler btnPosdFMDdlg_Event;
        event EventHandler CurrentCellChanged_Event;

        DataGridView RecomendationsGridView { get; }
        LoaderUserControl LoaderControl { get; }
        Button PersonalPharmacistButton { get; }
        Label CRMDataLoadStatus { get; }
        Button CRMDataReload { get; }
        ToolStripStatusLabel UserPrioritiesRatio { get; }

        Button HomeModeButton { get; }
        Button WoltModeButton { get; }
    }
}
