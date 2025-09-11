using Qas.EHealth.Models;
using System.Collections.Generic;
using System.Linq;

namespace POS_display.wpf.ViewModel
{
    public class VaccineOrderListViewModel : BaseViewModel
    {
        public VaccineOrderListViewModel(VaccineOrderListDto patientOrderListDto)
        {
            OrderList = patientOrderListDto?.OrderList?.OrderBy(el => el.Date)?.ToList() ?? new List<VaccineOrderDto>();
        }

        #region Variables
        private List<VaccineOrderDto> _orderList;
        public List<VaccineOrderDto> OrderList
        {
            get
            {
                return _orderList;
            }
            set
            {
                _orderList = value;
                NotifyPropertyChanged("OrderList");
            }
        }
        #endregion
    }
}
