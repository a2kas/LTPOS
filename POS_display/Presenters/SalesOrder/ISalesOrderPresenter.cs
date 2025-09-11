using POS_display.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace POS_display.Presenters.SalesOrder
{
    public interface ISalesOrderPresenter
    {
        void EnableControls();

        Task LoadClientData(string jsonData);

        Task TransferFromRemotePharmacy();

        string ValidateClientData();

        void ClearCientDataForm();

        void EnableInputFields(bool apply);

        FlowDocument CreateAgreementDocument();

        Task UpdatePosDetailID(long posDetailID);

        Task<string> SaveClientData();

        void SetFocusedClient(ClientData client);
    }
}
