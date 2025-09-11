
using System;

namespace POS_display.wpf
{
    public interface IRecipeEdit
    {
        DateTime ValidFrom { get; set; }
        DateTime SalesDate { get; set; }
        DateTime RecipeDate { get; set; }
        DateTime PastTillDate { get; set; }
        int Ext { get; set; }
        decimal RecipeValid { get; set; }
    }
}
