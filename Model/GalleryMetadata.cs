using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEH.Model
{

    public class GalleryMetadata
    {
        public int gid { get; set; }
        public string token { get; set; }
        public string archiver_key { get; set; }
        public string title { get; set; }
        public string title_jpn { get; set; }
        public string category { get; set; }
        public string thumb { get; set; }
        public string uploader { get; set; }
        public string posted { get; set; }
        public string filecount { get; set; }
        public long filesize { get; set; }
        public bool expunged { get; set; }
        public string rating { get; set; }
        public string torrentcount { get; set; }
        public Torrent[] torrents { get; set; }
        public string[] tags { get; set; }
        public string parent_gid { get; set; }
        public string parent_key { get; set; }
        public string first_gid { get; set; }
        public string first_key { get; set; }
    }

    public class Torrent
    {
        public string hash { get; set; }
        public string added { get; set; }
        public string name { get; set; }
        public string tsize { get; set; }
        public string fsize { get; set; }
    }

}
