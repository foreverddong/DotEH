using DotEH.Model;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEH.Pages
{
    public partial class Index
    {
        private string queryStr { get; set; }
        private List<GalleryMetadata> metadata { get; set; } = new List<GalleryMetadata>();
        private bool searching = false;
        private SearchParameter search;

        public async Task PerformSearch()
        {
            this.searching = true;
            StateHasChanged();
            this.metadata.AddRange(await searchingService.DoSearch(search));
            this.searching = false;
            StateHasChanged();
        }

        private async void OpenSearchDialog()
        {
            var dialog = dialogService.Show<SearchDialog>("Searching", new DialogOptions { CloseOnEscapeKey = false, DisableBackdropClick = true }) ;
            var result = await dialog.GetReturnValueAsync<SearchParameter>();
            if (result == null)
            {
                return;
            }
            this.searchingService.ClearSearch();
            this.search = result;
            await this.PerformSearch();
        }
    }
}
