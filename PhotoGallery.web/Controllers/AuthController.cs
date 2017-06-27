using Microsoft.Owin.Security;
using PhotoGallery.core.Interfaces.Managers;
using PhotoGallery.core.Managers;
using PhotoGallery.core.Models;
using PhotoGallery.Data;
using PhotoGallery.Data.Repositories;
using PhotoGallery.web.ViewModel;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace PhotoGallery.web.Controllers
{
    public class AuthController : Controller
    {
        private IUserManager<UserModel> _userManager;
        private DataContext _context;

        //inject all necessary dependencies
        public AuthController()
        {
            _context = new DataContext();
            _userManager = new UserManager(new UserRepository(_context));
        }

        // GET: Login
        [HttpGet]
        public ActionResult Login(string returnUrl, string email, bool? alert = false)
        {
            LoginModel loginModel = new LoginModel();

            //Assign LoginModel properties
            loginModel.ReturnUrl = returnUrl;
            loginModel.Email = email;

            if (alert.GetValueOrDefault())
            {
                ViewBag.AlertMsg = TempData["AlertMsg"];
                ViewBag.AlertType = TempData["AlertType"];
            }
            return View(loginModel);
        }

        //POST: Login
        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            RedirectResult redirectResult = null;

            try
            {
                if (ModelState.IsValid)
                {
                    //convert LoginModel obj to UserModel obj
                    var userModel = new UserModel { Email = loginModel.Email, Password = loginModel.Password };

                    //Validate User
                    var validUser = _userManager.ValidateUser(userModel);

                    //SignIn Valid User
                    SignIn(validUser, loginModel.RememberMe);
                }
                else
                    return View("Login");
            }
            catch (Exception e)
            {
                //log error msg in errorLog file
                Console.WriteLine($"{e.Message} ---> {e.Source}");

                //send an error notice to the view
                TempData["AlertMsg"] = "Invalid user. Pls Register";
                TempData["AlertType"] = Alert.AlertDanger;

                return RedirectToAction("Login", "Auth", new { returnUrl = loginModel.ReturnUrl, loginModel.Email, alert = true });
            }

            //if SignIn is succcessfull, redirect User to returnUrl
            if (Url.IsLocalUrl(loginModel.ReturnUrl))
                redirectResult = Redirect(loginModel.ReturnUrl);

            return redirectResult;
        }

        private void SignIn(UserModel validUser, bool rememberMe)
        {
            var auth = Request.GetOwinContext().Authentication;

            var claims = new ClaimsIdentity(new List<Claim> {
                new Claim(ClaimTypes.Email, validUser.Email),
                new Claim(ClaimTypes.Name, validUser.Firstname),
                new Claim(ClaimTypes.NameIdentifier, validUser.UserID.ToString())
            }, AuthTypes.ApplicationCookie);

            var authProperties = new AuthenticationProperties { IsPersistent = rememberMe };

            var identity = new ClaimsIdentity(claims);

            auth.SignIn(authProperties, identity);
        }

        private void SignOut()
        {
            var auth = Request.GetOwinContext().Authentication;
            auth.SignOut(AuthTypes.ApplicationCookie);
        }

        //Logout User
        [HttpGet]
        public ActionResult LogOut()
        {
            SignOut();
            return RedirectToAction("Login", new { returnUrl = "/Home/Index" });
        }

        //Post: register

        /*Notice that Register action method doesn't have a parameter for passing LoginModel obj because if LoinModel is passed,
         * the ModelState.IsValid will check if LoginModel properties are valid and we do not want that for Registration.
         * I took this route because I wanted to experiment multiple forms of distinct view models in a single view
         * and this single view can only accept one model.
         * LoginModel is the view's acceptable model but with a property that maps to RegistrationModel. 
         * Experiment was Successful after putting a lot of work hours trying to crack this.
         */
        [HttpPost]
        public ActionResult Register(RegistrationModel regModel, string returnUrl)
        {
            UserModel registeredUser;
            RedirectResult redirectResult = null;

            //assign the value returnUrl to RegistrationModel's ReturnUrl property
            regModel.ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                //register user and return the registered user
                var userModel = new UserModel
                {
                    Firstname = regModel.Firstname,
                    Lastname = regModel.Lastname,
                    Email = regModel.Email,
                    Password = regModel.Password
                };

                registeredUser = _userManager.RegisterUser(userModel);

                //if user registration is successful, go ahead and login the user
                if (registeredUser != null)
                    SignIn(registeredUser, true);
                else
                {
                    //if registration isn't successfull display error msg in view
                    TempData["AlertMsg"] = "You have failed to register. Pls try again";
                    TempData["AlertType"] = Alert.AlertDanger;

                    ViewBag.AlertMsg = TempData["AlertMsg"];
                    ViewBag.AlertType = TempData["AlertType"];

                    return View("Login");
                }
            }
            else
            {
                TempData["AlertMsg"] = "Registration Failed. Pls fill registration form appropriately";
                TempData["AlertType"] = Alert.AlertDanger;

                ViewBag.AlertMsg = TempData["AlertMsg"];
                ViewBag.AlertType = TempData["AlertType"];

                return View("Login");
            }

            if (Url.IsLocalUrl(regModel.ReturnUrl))
                redirectResult = Redirect(regModel.ReturnUrl);

            return redirectResult;
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