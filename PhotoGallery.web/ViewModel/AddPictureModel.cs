using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PhotoGallery.web.ViewModel
{
    public class AddPictureModel
    {
        public int AlbumID { get; set; }
        public string Title { get; set; }

        [Required]
        public HttpPostedFileBase PictureFile { get; set; }
    }
}