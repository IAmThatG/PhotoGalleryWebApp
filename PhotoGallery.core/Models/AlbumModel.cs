using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGallery.core.Models
{
    public class AlbumModel
    {
        public int AlbumID { get; set; }
        [Required]
        public string AlbumTitle { get; set; }
        public string AlbumDescription { get; set; }
        public int PictureCount { get; set; }
        public int UserID { get; set; }

        public UserModel User { get; set; }
        public ICollection<PictureModel> Pictures { get; set; }
    }
}
