using PhotoGallery.core.Interfaces.Managers;
using PhotoGallery.core.Managers;
using PhotoGallery.core.Models;
using PhotoGallery.Data;
using PhotoGallery.Data.Repositories;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System;
using PhotoGallery.web.ViewModel;

namespace PhotoGallery.web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IAlbumManager<AlbumModel> _albumManager;
        private DataContext _context;
        
        public HomeController()
        {
            _context = new DataContext();
            _albumManager = new AlbumManager(new AlbumRepository(_context));
        }

        // GET: Home
        public ActionResult Index(bool? alert = false)
        {
            var homePageModel = new HomePageModel();
            if (alert.GetValueOrDefault())
            {
                ViewBag.AlertMsg = TempData[Alert.AlertMsgKey];
                ViewBag.AlertType = TempData[Alert.AlertTypeKey];
            }

            var userID = User.Identity.GetID();
            homePageModel.Albums = _albumManager.GetAlbums(userID);
            return View(homePageModel);
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