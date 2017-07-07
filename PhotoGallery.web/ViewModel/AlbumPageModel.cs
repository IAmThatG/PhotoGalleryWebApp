using PhotoGallery.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoGallery.web.ViewModel
{
    /// <summary>
    /// Our AlbumPageModel must have an AlbumID so that the albumID can be
    /// passed on adding a new picture.
    /// </summary>
    public class AlbumPageModel
    {
        public int AlbumID { get; set; }
        public IList<PictureModel> Pictures { get; set; }
        public PictureModel Picture { get; set; }
    }
}