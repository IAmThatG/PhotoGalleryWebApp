using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoGallery.web.ViewModel
{
    public class AlbumViewModel
    {
        [Required]
        public string AlbumTitle { get; set; }

        public string AlbumDescription { get; set; }
    }
}