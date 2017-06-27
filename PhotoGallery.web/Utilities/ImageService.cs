using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace PhotoGallery.web.Utilities
{
    public class ImageService
    {
        #region Scale Stream
        public Stream ScaleWidth(Stream imageStream, int maxWidth)
        {
            var output = new MemoryStream();

            //Get Image from Stream
            using (var image = Image.FromStream(imageStream))
            {
                //Scale Image by Height and Save
                var newImage = ScaleHeight(image, maxWidth);
                newImage.Save(output, ImageFormat.Png);
            }

            //Reset Stream Pointer
            output.Seek(0, SeekOrigin.Begin);
            return output;
        }
        public Stream ScaleHeight(Stream imageStream, int maxHeight)
        {
            //Create Output Stream
            var output = new MemoryStream();

            //Get Image from Stream
            using (var image = Image.FromStream(imageStream))
            {
                //Scale Image by Height and Save
                var newImage = ScaleHeight(image, maxHeight);
                newImage.Save(output, ImageFormat.Png);
            }

            //Reset Stream Pointer
            output.Seek(0, SeekOrigin.Begin);
            return output;
        }
        #endregion

        #region Scale Image
        public Image ScaleWidth(Image image, int maxWidth)
        {
            var ratio = (double)maxWidth / image.Width;

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(newImage))
            {
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.SmoothingMode = SmoothingMode.HighSpeed;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        public Image ScaleHeight(Image image, int maxHeight)
        {
            var ratio = (double)maxHeight / image.Height;

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(newImage))
            {
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.SmoothingMode = SmoothingMode.HighSpeed;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }
        #endregion

        #region Crop Stream
        public Stream Crop(Stream imageStream, int height)
        {
            //Create Output Stream
            var output = new MemoryStream();

            //Get Image from Stream
            using (var image = Image.FromStream(imageStream))
            {
                //Scale Image by Height and Save
                var newImage = Crop(image, height);
                newImage.Save(output, ImageFormat.Png);
            }

            //Reset Stream Pointer
            output.Seek(0, SeekOrigin.Begin);
            return output;
        }

        public Stream Crop(Stream imageStream, int width, int height)
        {
            //Create Output Stream
            var output = new MemoryStream();

            //Get Image from Stream
            using (var image = Image.FromStream(imageStream))
            {
                //Scale Image by Height and Save
                var newImage = Crop(image, width, height);
                newImage.Save(output, ImageFormat.Png);
            }

            //Reset Stream Pointer
            output.Seek(0, SeekOrigin.Begin);
            return output;
        }
        #endregion

        #region Crop Image
        public Image Crop(Image image, int height)
        {
            return Crop(image, height, height);
        }

        public Image Crop(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentW;
                destY = (int)((Height - (sourceHeight * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentH;
                destX = (int)((Width - (sourceWidth * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
        #endregion
    }
}