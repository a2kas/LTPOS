using TamroUtilities.HL7.Models;
using System.Collections.Generic;
using System.Linq;

namespace POS_display.wpf.ViewModel
{
    public class RecipeListViewModel : BaseViewModel
    {
        public RecipeListViewModel(RecipeListDto recipeListDto)
        {
            RecipeList = recipeListDto?.RecipeList?.OrderBy(el => el.Status == "active" ? 1 : el.Status == "onhold" ? 2 : el.Status == "completed" ? 3 : 4)?.ToList() ?? new List<RecipeDto>();
        }

        #region Variables
        private List<RecipeDto> _RecipeList;
        public List<RecipeDto> RecipeList
        {
            get
            {
                return _RecipeList;
            }
            set
            {
                _RecipeList = value;
                NotifyPropertyChanged("RecipeList");
            }
        }
        #endregion
    }
}
