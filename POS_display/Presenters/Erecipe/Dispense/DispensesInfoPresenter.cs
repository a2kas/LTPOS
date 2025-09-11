using AutoMapper;
using POS_display.Models.Recipe;
using POS_display.Views.Erecipe.Dispense;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POS_display.Presenters.Erecipe.Dispense
{
    public class DispensesInfoPresenter : IDispensesInfoPresenter
    {
        #region Members
        private readonly IDispensesView _view;
        private readonly IMapper _mapper;
        private const string QtyLeftToDispnenseText = "Liko išduoti: {0}";
        private const string FormHeaderText = "{0} - Recepto išdavimai";
        #endregion

        #region Constructor
        public DispensesInfoPresenter(IDispensesView view, IMapper mapper)
        {
            _view = view ?? throw new ArgumentNullException();
            _mapper = mapper ?? throw new ArgumentNullException();
        }
        #endregion

        #region Public methods
        public void SetData(Items.eRecipe.Recipe eRecipeItem)
        {
            _view.DispensesInfo = string.Format(QtyLeftToDispnenseText, eRecipeItem.QtyLeftDispense);
            _view.FormHeaderText = string.Format(FormHeaderText, eRecipeItem.eRecipe_RecipeNumber);

            if (eRecipeItem?.DispenseList?.DispenseList == null || !eRecipeItem.DispenseList.DispenseList.Any())
                return;

            var dispenses = _mapper.Map<List<MainDispenseData>>(eRecipeItem.DispenseList.DispenseList);
            _view.Dispenses.DataSource = dispenses.OrderByDescending(e => e.DateDueDate).ToList();
        }
        #endregion
    }
}
