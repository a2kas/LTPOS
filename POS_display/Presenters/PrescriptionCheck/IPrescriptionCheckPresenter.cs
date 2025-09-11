using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_display.Presenters.PrescriptionCheck
{
    public interface IPrescriptionCheckPresenter
    {
        Task Init();

		Task<decimal> Save();

		void ValueChangesCalculation(object sender);

		void SetTillDateDisplay(object sender);

		void SetValidFromDisplay(object sender);

		void QuantityKeyPress(object sender, KeyPressEventArgs e);

		void NumberKeyPress(object sender, KeyPressEventArgs e);
	}
}
