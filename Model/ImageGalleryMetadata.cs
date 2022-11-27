using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEH.Model
{
    public class ImageGalleryMetadata
    {
        public GalleryMetadata Metadata { get; set; }
        public byte[] base64Image { get; set; }

        public string base64String 
        {
            get => $"data:image/jpeg;base64, {Convert.ToBase64String(this.base64Image)}";
        }
    }
}
