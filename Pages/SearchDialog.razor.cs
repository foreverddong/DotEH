using DotEH.Model;
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

        public string tagString { get; set; }

        public List<string> addedTags { get; set; } = new();

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

        protected override async Task OnInitializedAsync()
        {
            await this.tagStorage.LoadTagsAsync();
        }

        public async Task<IEnumerable<string>> TagSearch(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return tagStorage.Storage.Tags;
            }
            if (!tagStorage.Storage.Tags.Any())
            {
                return tagStorage.Storage.Tags;
            }
            return tagStorage.Storage.Tags.Where(t => t.Contains(value));
        }

        public async Task AddTag()
        {
            await tagStorage.AddTagOrSkip(tagString);
            this.addedTags.Add(tagString);
            this.tagString = "";
        }

        public void CloseDialog()
        {
            Dialog.Cancel();
        }

        public void ConfirmSearch()
        {
            Dialog.Close(DialogResult.Ok<SearchParameter>(new SearchParameter(this.buttonStates, this.addedTags, this.queryStr)));
        }
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
