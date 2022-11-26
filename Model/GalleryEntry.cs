using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotEH.Model
{
    public class GalleryEntry
    {
        public int GalleryId { get; set; }
        public string GalleryToken { get; set; }
        public string Title { get; set; }

        public string Link 
        {
            get
            {
                return $"https://e-hentai.org/g/{GalleryId}/{GalleryToken}/";
            }
            set
            {
                var regex = new Regex(@"https:\/\/e[a-z -]hentai\.org\/g\/([0-9]+)\/([a-z0-9]+)\/");
                var groups = regex.Match(value).Groups;
                this.GalleryId = int.Parse(groups[1].Value);
                this.GalleryToken = groups[2].Value;
            }
        }
        //an e-hentai gallery link usually looks like this-
        //https://e-hentai.org/g/galleryid/gallerytoken/
        public GalleryEntry(string title, string uri) 
        {
            this.Title = title;
            this.Link = uri;
        }
    }
}
