using DotEH.Model;
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

        public async Task PerformSearch()
        {
            this.searching = true;
            this.metadata.AddRange(await searchingService.DoSearch(queryStr));
            this.searching = false;
        }
    }
}
