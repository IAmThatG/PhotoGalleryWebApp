using PhotoGallery.core.Managers;
using PhotoGallery.core.Models;
using PhotoGallery.Data;
using PhotoGallery.Data.Repositories;
using PhotoGallery.web.ViewModel;
using System;
using System.Web.Mvc;

namespace PhotoGallery.web.Controllers
{
    [Authorize]
    public class AlbumController : Controller
    {
        private AlbumManager _albumManager;
        private DataContext _context;

        public AlbumController()
        {
            _context = new DataContext();
            _albumManager = new AlbumManager(new AlbumRepository(_context));
        }

        // GET: Album
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        //GET: Create New Album
        //[HttpGet]
        //public ActionResult Create(bool? alert = false)
        //{
        //    //set alert ViewBag values 
        //    if (alert.GetValueOrDefault())
        //    {
        //        ViewBag.AlertMsg = TempData["AlertMsg"];
        //        ViewBag.AlertType = TempData["AlertType"];
        //    }

        //    return View();
        //}

        //POST: Create a New Album
        [HttpPost]
        public ActionResult Create(HomePageModel homePageModel)
        {
            var createdAlbum = new AlbumModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var albumModel = new AlbumModel();
                    albumModel.Assign(homePageModel.Album);
                    var userID = User.Identity.GetID();
                    createdAlbum = _albumManager.CreateAlbum(albumModel, userID);
                    if(createdAlbum == null)
                    {
                        //send an error notice to the view
                        TempData[Alert.AlertMsgKey] = "Ooops!!!...Can't create an already existing album";
                        TempData[Alert.AlertTypeKey] = Alert.AlertDanger;
                        return RedirectToAction("Index", "Home", new { alert = true });
                    }
                }
                else
                {
                    //send an error notice to the view
                    TempData[Alert.AlertMsgKey] = "Pls provide a title for the album";
                    TempData[Alert.AlertTypeKey] = Alert.AlertDanger;
                    return RedirectToAction("Index", "Home", new { alert = true });
                }
            }
            catch (Exception e)
            {
                //log error msg in errorLog file
                Console.WriteLine($"{e.Message} ---> {e.Source}");

                //send an error notice to the view
                TempData[Alert.AlertMsgKey] = "Sory!!!...Couldn't Create Album. Pls Try Again.";
                TempData[Alert.AlertTypeKey] = Alert.AlertDanger;

                return RedirectToAction("Index", "Home", new { alert = true });
            }
        
            TempData[Alert.AlertMsgKey] = "Album Created Successfully";
            TempData[Alert.AlertTypeKey] = Alert.AlertSuccess;
            return RedirectToAction("Index", "Picture", new { albumID = createdAlbum.AlbumID, alert = true});
        }

        //POST: Edit Album
        [HttpPost]
        public ActionResult Edit(AlbumModel albumModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userID = User.Identity.GetID();
                    var isAlbumEdited = _albumManager.EditAlbum(albumModel, userID);
                    if (isAlbumEdited)
                    {
                        TempData[Alert.AlertMsgKey] = "Album Edited Successfully...";
                        TempData[Alert.AlertTypeKey] = Alert.AlertSuccess;
                    }
                }
                catch (Exception e)
                {
                    //write error msg to log file
                    Console.WriteLine($"{e.Message} ---> {e.Source}");

                    //Display error msg
                    TempData[Alert.AlertMsgKey] = "Ooops!!!...failed to edit album";
                    TempData[Alert.AlertTypeKey] = Alert.AlertDanger;
                }
            }
            else
            {
                TempData[Alert.AlertMsgKey] = "Pls fill form appropriately";
                TempData[Alert.AlertTypeKey] = Alert.AlertDanger;
            }

            return RedirectToAction("Index", "Home", new { alert = true });
        }

        //POST: Delete Album
        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var isAlbumDeleted = _albumManager.DeleteAlbum(id);
                    if (isAlbumDeleted)
                    {
                        TempData[Alert.AlertMsgKey] = "Album Deleted Successfully...";
                        TempData[Alert.AlertTypeKey] = Alert.AlertSuccess;
                    }
                }
                catch (Exception e)
                {
                    //write error to log file
                    Console.WriteLine($"{e.Message} ---> {e.Source}");

                    //display error msg to user
                    TempData[Alert.AlertMsgKey] = "Sorry, Failed to delete album...";
                    TempData[Alert.AlertTypeKey] = Alert.AlertDanger;
                }
            }
            return RedirectToAction("Index", "Home", new { alert = true });
        }

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