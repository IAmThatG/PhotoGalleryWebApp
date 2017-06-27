using PhotoGallery.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoGallery.web.ViewModel
{
    public class HomePageModel
    {
        public IList<AlbumModel> Albums { get; set; }
        public AlbumViewModel Album { get; set; }
    }
}