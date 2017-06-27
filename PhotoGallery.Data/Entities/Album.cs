using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGallery.Data.Entities
{
    public class Album
    {
        public int AlbumID { get; set; }

        [Required]
        public string AlbumTitle { get; set; }

        public string AlbumDescription { get; set; }
        public int PictureCount { get; set; }
        public int UserID { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; } = new HashSet<Picture>();
    }
}
