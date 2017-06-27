using PhotoGallery.core.Interfaces.Managers;
using PhotoGallery.core.Managers;
using PhotoGallery.core.Models;
using PhotoGallery.Data;
using PhotoGallery.Data.Repositories;
using PhotoGallery.web.Utilities;
using PhotoGallery.web.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoGallery.web.Controllers
{
    [Authorize]
    public class PictureController : Controller
    {
        private IPictureManager<PictureModel> _pictureManager;
        private DataContext _context;

        public PictureController()
        {
            _context = new DataContext();
            _pictureManager = new PictureManager(new PictureRepository(_context));
        }

        // GET: Pictures
        [HttpGet]
        public ActionResult Index(int albumID, bool? alert = false)
        {
            var pictures = new List<PictureModel>();
            PictureViewModel pictureViewModel = new PictureViewModel();
            pictureViewModel.AlbumID = albumID;

            if (alert.GetValueOrDefault())
            {
                ViewBag.AlertMsg = TempData[Alert.AlertMsgKey];
                ViewBag.AlertType = TempData[Alert.AlertTypeKey];
            }

            try
            {
                pictures = _pictureManager.GetAlbumPictures(albumID);
                pictureViewModel.Pictures = pictures;
            }
            catch (Exception e)
            {
                //log error msg in errorLog file
                Console.WriteLine($"{e.Message} ---> {e.Source}");
                
                //send an error notice to the view
                TempData[Alert.AlertMsgKey] = "Ooops!...Pictures couldn't be fetched. Pls contact DBAdmin";
                TempData[Alert.AlertTypeKey] = "danger";
                return RedirectToAction("Index", "Home");
            }
            return View(pictureViewModel);
        }

        //Post: Add picture
        //note: If each album has a directory, then the file path will be the directory path plus the filename
        [HttpPost]
        public ActionResult Index(AddPictureModel addPictureModel)
        {
            if (ModelState.IsValid)
            {
                //get the path of the file
                var fileName = addPictureModel.PictureFile.FileName;

                //get the image name extension from the fileName sent by the browser
                var imageFileName = Path.GetFileNameWithoutExtension(fileName);
                
                var fileExtension = Path.GetExtension(fileName);

                //validate the extention by comparing it with the allowed extensions
                var isValidExtension = ".jpg,.png,.jpeg,.JPG,.PNG,.JPEG".Split(',').Contains(fileExtension);

                //if extension is a valid image, copy to output stream and save copied file path to database
                if (isValidExtension)
                {
                    //create image processor
                    var imageProcessor = new ImageService();

                    //use image processor to scale picture retaining a max aspect ratio of 400px
                    var image = imageProcessor.ScaleWidth(addPictureModel.PictureFile.InputStream, 400);
                    
                    //get folder location of our pictures folder
                    var folderPath = Server.MapPath("~/Pictures/");

                    var imageFileNameWithExt = $"{imageFileName}.png";

                    //combine path to get full file name
                    var filePath = Path.Combine(folderPath, imageFileNameWithExt);

                    //open an output stream for saving file
                    using (var outputStream = System.IO.File.Create(filePath))
                    {
                        //copy image to the output stream
                        image.CopyTo(outputStream);
                    }

                    //Save picture in DB
                    var isSaved = SavePicture(addPictureModel, imageFileNameWithExt);

                    //if save is unsuccessful, return view with error msg.
                    if(isSaved == false)
                    {
                        TempData[Alert.AlertMsgKey] = "Ooops!!!...Couldn't save picture";
                        TempData[Alert.AlertTypeKey] = Alert.AlertDanger;

                        return RedirectToAction("Index", "Picture", new { albumID = addPictureModel.AlbumID, alert = true });
                    }
                }
            }
            else
            {
                TempData[Alert.AlertMsgKey] = "Error!!!...Picture must be jpeg, jpg or png";
                TempData[Alert.AlertTypeKey] = Alert.AlertDanger;
                return RedirectToAction("Index", "Picture", new { albumID = addPictureModel.AlbumID, alert = true });
            }

            //if saving to DB is successful redirect to index in order to load picture
            TempData[Alert.AlertMsgKey] = "Picture Uploaded Successfully";
            TempData[Alert.AlertTypeKey] = Alert.AlertSuccess;
            return RedirectToAction("Index", "Picture", new { albumID = addPictureModel.AlbumID, alert = true });
        }

        private bool SavePicture(AddPictureModel addPictureModel, string imageFileName)
        {
            var pictureModel = new PictureModel
            {
                AlbumID = addPictureModel.AlbumID,
                PictureTitle = addPictureModel.Title,
                FileName = Path.GetFileNameWithoutExtension(addPictureModel.PictureFile.FileName),
                FilePath = $"~/Pictures/{imageFileName}",
                FileSize = addPictureModel.PictureFile.ContentLength,
            };
            return _pictureManager.AddPicture(pictureModel, addPictureModel.AlbumID);
        }

        //method to handle image processing
        public Image ProcessImage(Stream inputImageStream)
        {
            return Image.FromStream(inputImageStream);
        }

        //method to display Add Picture Partial view
        public PartialViewResult AddPartial(int albumID)
        {
            AddPictureModel pictureModel = new AddPictureModel();
            pictureModel.AlbumID = albumID;
            return PartialView(pictureModel);
        }

        //POST: Add Picture

        //GET: Edit Picture

        //POST: Edit Picture

        //GET: Delete Picture

        //POST: Delete Picture

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}