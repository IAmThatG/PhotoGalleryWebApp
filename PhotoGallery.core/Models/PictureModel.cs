using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGallery.core.Models
{
    public class PictureModel
    {
        public int PictureID { get; set; }
        public string PictureTitle { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileMime { get; set; }
        public long FileSize { get; set; }
        public int AlbumID { get; set; }

        public AlbumModel Album { get; set; }
    }
}