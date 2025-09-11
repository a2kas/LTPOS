using POS_display.Repository.Pos;
using POS_display.Views;
using System.Threading.Tasks;

namespace POS_display.Presenters
{
    public class BasePresenter : IBasePresenter
    {
        #region Members
        private IPosRepository _posRepository;
        private IBaseView _view;
        #endregion

        #region Constructor
        public BasePresenter()
        {
            _posRepository = new PosRepository();
        }
        public BasePresenter(IBaseView view) 
        {
            _view = view;
            _posRepository = new PosRepository();
        }
        #endregion

        #region Properties
        public IPosRepository PosRepository 
        {
            get { return _posRepository; }
            set { _posRepository = value; }
        }
        #endregion

        #region Public methods
        public async Task UpdateSession(string action, decimal f_mode)
        {
            await _posRepository.UpdateSession(action, f_mode);
        }
        #endregion
    }
}
