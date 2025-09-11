using System.Collections.Generic;

namespace POS_display.Views.Erecipe
{
    public interface IRecipeFilterView
    {
        void Init();
        Dictionary<Enumerator.RecipeFilterValue, string> FilterValues { get; }
    }
}
