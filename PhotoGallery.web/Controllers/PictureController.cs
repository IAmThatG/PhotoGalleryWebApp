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
            AlbumPageModel albumPageModel = new AlbumPageModel();
            albumPageModel.AlbumID = albumID;

            if (alert.GetValueOrDefault())
            {
                ViewBag.AlertMsg = TempData[Alert.AlertMsgKey];
                ViewBag.AlertType = TempData[Alert.AlertTypeKey];
            }

            try
            {
                pictures = _pictureManager.GetAlbumPictures(albumID);
                albumPageModel.Pictures = pictures;
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
            return View(albumPageModel);
        }

        //Post: Add picture
        //note: If each album has a directory, then the file path will be the directory path plus the filename
        [HttpPost]
        public ActionResult Index(AddPictureModel addPictureModel)
        {
            if (ModelState.IsValid)
            {
                //ImageFile imageFile = new ImageFile(addPictureModel.PictureFile);
                //imageFile.ServerFileName = Server.MapPath($"~/Pictures/{imageFile.ImageFieNameWithExt}");
                //imageFile.PerformUpload();

                //get the full path of the file to be uploaded
                var fileName = addPictureModel.PictureFile.FileName;

                //get the image name from the fileName sent by the browser without its extension
                var imageFileName = Path.GetFileNameWithoutExtension(fileName);

                //get the file extension
                var fileExtension = Path.GetExtension(fileName);

                //validate the extention by comparing it with the allowed extensions
                var isValidExtension = ".jpg,.png,.jpeg,.JPG,.PNG,.JPEG".Split(',').Contains(fileExtension);

                //if extension is a valid image, copy to output stream and save copied file path to database
                if (isValidExtension)
                {
                    //create image processor
                    var imageProcessor = new ImageService();

                    //scale picture retaining a max aspect ratio of 400px
                    var imageInputStream = imageProcessor.ScaleWidth(addPictureModel.PictureFile.InputStream, 400);

                    //get the name of the image with our new extension
                    var imageFileNameWithExt = $"{imageFileName}.png";

                    //get the file path we want to upload to
                    var filePath = Server.MapPath($"~/Pictures/{imageFileNameWithExt}");

                    //open an output stream for saving file
                    using (var outputStream = System.IO.File.Create(filePath))
                    {
                        //copy image to the output stream
                        imageInputStream.CopyTo(outputStream);
                    }

                    //Save picture in DB
                    var isSaved = SavePicture(addPictureModel, imageFileName);
                    if (isSaved)
                    {
                        //if saving to DB is successful redirect to index in order to load picture and display success msg
                        TempData[Alert.AlertMsgKey] = "Picture Uploaded Successfully";
                        TempData[Alert.AlertTypeKey] = Alert.AlertSuccess;
                        return RedirectToAction("Index", "Picture", new { albumID = addPictureModel.AlbumID, alert = true });
                    }
                }
                else
                {
                    TempData[Alert.AlertMsgKey] = "Error!!!...Picture must be jpeg, jpg or png";
                    TempData[Alert.AlertTypeKey] = Alert.AlertDanger;
                    return RedirectToAction("Index", "Picture", new { albumID = addPictureModel.AlbumID, alert = true });
                }
            }
            //if ModelState is invalid or saving picture to DB fails
            TempData[Alert.AlertMsgKey] = "Ooops!!!...Couldn't save picture";
            TempData[Alert.AlertTypeKey] = Alert.AlertDanger;
            return RedirectToAction("Index", "Picture", new { albumID = addPictureModel.AlbumID, alert = true });
        }

        /// <summary>
        /// Saves data about the picture to the DB
        /// </summary>
        /// <param name="addPictureModel"></param>
        /// <param name="imageFileName"></param>
        /// <returns></returns>
        private bool SavePicture(AddPictureModel addPictureModel, string fileNameWithoutExt)
        {
            var pictureModel = new PictureModel
            {
                AlbumID = addPictureModel.AlbumID,
                PictureTitle = addPictureModel.Title,
                FileName = fileNameWithoutExt,
                FilePath = $"~/Pictures/{fileNameWithoutExt}.png",
                FileSize = addPictureModel.PictureFile.ContentLength,
            };
            return _pictureManager.AddPicture(pictureModel, addPictureModel.AlbumID);
        }

        //method to display Add Picture Partial view
        public PartialViewResult AddPartial(int albumID)
        {
            AddPictureModel pictureModel = new AddPictureModel();
            pictureModel.AlbumID = albumID;
            return PartialView(pictureModel);
        }

        //POST: Rename Picture
        public ActionResult Rename(PictureModel picture)
        {
            //TODO: Check if ModelState is valid
            if (ModelState.IsValid)
            {
                //TODO: Tell picture manager to rename picture
                var isRenamed = _pictureManager.RenamePicture(picture);

                //TODO: If rename is successfull, do the following
                if (isRenamed)
                {
                    //TODO: Return to index with success alert msg
                    TempData[Alert.AlertMsgKey] = "Picture has been renamed successfully";
                    TempData[Alert.AlertTypeKey] = Alert.AlertSuccess;
                    return RedirectToAction("Index", "Picture", new { albumID = picture.AlbumID, alert = true });
                }
                //TODO: On failure to rename, return to index with error alert
                TempData[Alert.AlertMsgKey] = "Sorry, picture couldn't be renamed. Please try again";
                TempData[Alert.AlertTypeKey] = Alert.AlertDanger;
                return RedirectToAction("Index", "Picture", new { albumID = picture.AlbumID, alert = true });
            }
            //TODO: If ModelState isn't valid, return to index with error alert
            TempData[Alert.AlertMsgKey] = "Sorry, picture couldn't be renamed. Please try again";
            TempData[Alert.AlertTypeKey] = Alert.AlertDanger;
            return RedirectToAction("Index", "Picture", new { albumID = picture.AlbumID, alert = true });
        }
       
        //POST: Delete Picture
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //Find picture by ID
            var pictureModel = _pictureManager.GetPictureByID(id);

            //TODO: Tell PictureManager to delete Picture
            var isDeleted = _pictureManager.DeletePicture(id);

            //TODO: If picture is deleted, do the following
            if (isDeleted)
            {
                //TODO: Delete picture from file
                //Find the File to be deleted
                var filePath = Server.MapPath(pictureModel.FilePath);

                //Delete the picture from the file.
                System.IO.File.Delete(filePath);

                //TODO: Return to index page with success alert
                TempData[Alert.AlertMsgKey] = "Picture Deleted Successfully";
                TempData[Alert.AlertTypeKey] = Alert.AlertSuccess;
                return RedirectToAction("Index", "Picture", new { albumID = pictureModel.AlbumID, alert = true });
            }

            //TODO: If picture failed to delete, return to index page with danger alert
            TempData[Alert.AlertMsgKey] = "Sorry, Picture couldn't be deleted. Pls try again.";
            TempData[Alert.AlertTypeKey] = Alert.AlertSuccess;
            return RedirectToAction("Index", "Picture", new { albumID = pictureModel.AlbumID, alert = true });
        }

        //Dispose of DbContext resource.
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