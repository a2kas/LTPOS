using POS_display.Repository.NarcoticAlert;
using POS_display.Views.NarcoticAlert;
using System.Threading.Tasks;

namespace POS_display.Presenters.NarcoticAlert
{
	public class NarcoticAlertPresenter : BasePresenter
    {
        #region Members
        private readonly INarcoticAlertView _view;
        private readonly INarcoticAlertRepository _narcoticAlertRepository = null;
        #endregion

        #region Constructor
        public NarcoticAlertPresenter(INarcoticAlertView view, INarcoticAlertRepository narcoticAlertRepository)
        {
            _view = view;
            _narcoticAlertRepository = narcoticAlertRepository;
        }
        #endregion

        #region Public methods
        public async Task Init(Enumerator.DrugType drugType, string atc)
        {           
            _view.DrugMaterials.DataSource = await _narcoticAlertRepository.GetATCCodifiersByATC(atc);
			_view.Header.Text = $"Išduodamas {(drugType == Enumerator.DrugType.NARC ? "narkotinis" : "psichotropinis")} vaistinis preparatas !!!";
			_view.Notification.SelectedText = "Šių preparatų leidžiama išrašyti:\n\n";
			_view.Notification.SelectedText = "* Injekcinių ar infuzinių vaistinių preparatų – ne ilgesniam kaip 15 dienų kursui;\n";
            _view.Notification.SelectedText = "* Neinjekcinių ir neinfuzinių vaistinių preparatų – ne ilgesniam kaip 30 dienų kursui.";
        }
        #endregion
    }
}
