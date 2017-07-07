using PhotoGallery.web.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PhotoGallery.web.Utilities
{
    public class ImageFile : IUploadable
    {
        private HttpPostedFileBase _postedImageFile;
        public string ClientFileName
        {
            get { return _postedImageFile.FileName; }
            private set { }
        }
        public string ServerFileName
        {
            get { return ServerFileName; }
            set { ServerFileName = value; }
        }
        public string ImageFileName
        {
            get { return Path.GetFileNameWithoutExtension(ClientFileName); }
            private set { }
        }
        //The image filename with our new extension
        public string ImageFieNameWithExt
        {
            get { return $"{ImageFileName}.png"; }
            private set { }
        }

        public ImageFile(HttpPostedFileBase postedImageFile)
        {
            _postedImageFile = postedImageFile;
        }

        public bool ValidateExtension()
        {
            var extension = Path.GetExtension(ClientFileName);
            var validExtensions = ".jpg,.png,.jpeg,.JPG,.PNG,.JPEG";
            return validExtensions.Split(',').Contains(extension);
        }

        public void PerformUpload()
        {
            if (ValidateExtension())
            {
                //create image processor
                var imageProcessor = new ImageService();

                //scale picture retaining a max aspect ratio of 400px
                var imageInputStream = imageProcessor.ScaleWidth(_postedImageFile.InputStream, 400);

                //open an output stream for saving file
                using (var outputStream = File.Create(ServerFileName))
                {
                    //copy image to the output stream
                    imageInputStream.CopyTo(outputStream);
                }
            }
        }
    }
}