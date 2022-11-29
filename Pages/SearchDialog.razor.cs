using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Colors = MudBlazor.Colors;

namespace DotEH.Pages
{
    public partial class SearchDialog
    {
        [CascadingParameter]
        private MudDialogInstance Dialog { get; set; }

        public string queryStr { get; set; }

        public List<CategoryButtonState> buttonStates { get; init; } = new()
        {
            new CategoryButtonState { Name = "Doujinshi" , Color = Colors.DeepOrange.Darken4},
            new CategoryButtonState { Name = "Manga" , Color = Colors.Orange.Darken2},
            new CategoryButtonState { Name = "Artist CG" , Color = Colors.DeepOrange.Lighten2},
            new CategoryButtonState { Name = "Game CG" , Color = Colors.LightGreen.Darken2},
            new CategoryButtonState { Name = "Western" , Color = Colors.Lime.Darken2},
            new CategoryButtonState { Name = "Non-H" , Color = Colors.Cyan.Accent3},
            new CategoryButtonState { Name = "Image Set" , Color = Colors.LightBlue.Darken3},
            new CategoryButtonState { Name = "Cosplay" , Color = Colors.DeepPurple.Darken2},
            new CategoryButtonState { Name = "Asian Porn" , Color = Colors.Purple.Accent2},
            new CategoryButtonState { Name = "Misc" , Color = Colors.Grey.Lighten1},
        };
    }

    public class CategoryButtonState
    {
        public string Name { get; init; }
        public bool Enabled { get; set; } = true;

        public string Color { get; init; }

        public readonly string DisabledColor = Colors.Grey.Darken3;


        public string Style 
        {
            get
            {
                return $"color:{(Enabled ? Colors.Grey.Lighten5 : Colors.Grey.Darken2)}; background:{(Enabled ? Color : DisabledColor)};";
            }
        }
        
    }
}
