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
        private IEnumerable<GalleryMetadata> metadata { get; set; }

        public async Task PerformSearch()
        {
           this.metadata = await searchingService.DoSearch(queryStr);
        }
    }
}
