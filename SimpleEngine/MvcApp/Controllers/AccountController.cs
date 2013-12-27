//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;
//using Microsoft.Owin.Security;


using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using MvcApp.Models.User;

namespace MvcApp.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(RegisterUserModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser() { UserName = model.Username };
        //        var result = await UserManager<User>.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            await SignInAsync(user, isPersistent: false);
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            //AddErrors(result);
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        //{
        //    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

        //    var identity = await UserManager.CreateIdentityAsync(
        //       user, DefaultAuthenticationTypes.ApplicationCookie);

        //    AuthenticationManager.SignIn(
        //       new AuthenticationProperties()
        //       {
        //           IsPersistent = isPersistent
        //       }, identity);
        //}

        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LogOff()
        //{
        //    AuthenticationManager.SignOut();
        //    return RedirectToAction("Index", "Home");
        //}

       

        

        //protected void SignOut(object sender, EventArgs e)
        //{
        //    var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
        //    authenticationManager.SignOut();
        //    Response.Redirect("~/Login.aspx");
        //}

        [HttpGet]
        public ActionResult Register()
        {
            var model = new RegisterUserModel
            {
                Username = "name",
                Password = "password",
                Email = "mail",
                Msg = "message"
            };
            return View("Register", model);
        }

        [HttpPost]
        public ActionResult Register(RegisterUserModel model)
        {
            if (ModelState.IsValid)
            {
                // Default UserStore constructor uses the default connection string named: DefaultConnection
                var userStore = new UserStore<IdentityUser>();
                var manager = new UserManager<IdentityUser>(userStore);
                var user = new IdentityUser() { UserName = model.Username };

                IdentityResult result = manager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    model.Msg = string.Format("User {0} was created successfully!", user.UserName);
                }
                else
                {
                    model.Msg = result.Errors.FirstOrDefault();
                }
            }
            return View("Register", model);
        }

        //private async Task SignInAsync(IdentityUser user, bool isPersistent)
        //{
        //    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        //    var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        //    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        //}

        [HttpGet]
        public ActionResult SignIn()
        {
            var model = new SignInModel
            {
                Login = "name",
                Password = "password"
            };
            return View("SignIn", model);
        }

        // user list : 
        // username:password
        [HttpPost]
        public ActionResult SignIn(SignInModel model)
        {
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);

            var user = userManager.Find(model.Login, model.Password);
            //userManager.GetLoginsAsync()

            if (user != null)
            {
                //Request.Cookies.Add(new HttpCookie("username", model.Login));
                String playerId = PlayerNameToStr(model.Login);
                Response.Cookies.Add(new HttpCookie("playerId", playerId));
            //    Request.GetOwinContext();
            //    HttpContext.
            //    var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            //    //var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                //authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);

                //var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                //HttpContext..
                //var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                //authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);

                return RedirectToAction("ShowBoard", "Test");
            }
            else
            {
                ViewBag.Msg = "Invalid username or password.";
            }

            return View("SignIn", model);
        }

        // todo: remake with authentication =\
        // todo: temporary add limit for creates open games
        // todo: temporary add clearing for finished games
        private String PlayerNameToStr(string shortName)
        {
            String res = "0";
            switch (shortName)
            {
                case "player1":
                    res = "1";
                    break;
                case "player2":
                    res = "2";
                    break;
                case "player3":
                    res = "3";
                    break;
                case "player4":
                    res = "4";
                    break;
            }
            return res;
        }
    }
}


//using System;
//using System.Transactions;
//using System.Web.Mvc;
//using System.Web.Security;
//using Microsoft.Web.WebPages.OAuth;
//using WebMatrix.WebData;
//using LearningRus.Filters;
//using LearningRus.Models;

//namespace LearningRus.Controllers
//{
//    [InitializeSimpleMembership]
//    public class AccountController : Controller
//    {
//        public static readonly string[] AnonymousActions = { "Login", "Register" };

//        public ActionResult Login(string returnUrl)
//        {
//            ViewBag.ReturnUrl = returnUrl;
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Login(LoginModel model, string returnUrl)
//        {
//            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, model.RememberMe))
//            {
//                return RedirectToLocal(returnUrl);
//            }

//            ModelState.AddModelError("", "The user name or password provided is incorrect.");
//            return View(model);
//        }

//        public ActionResult LogOff()
//        {
//            WebSecurity.Logout();

//            return RedirectToAction("Index", "ExerciseEditor");
//        }

//        public ActionResult Register()
//        {
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Register(RegisterModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }

//            try
//            {
//                WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
//                WebSecurity.Login(model.UserName, model.Password);
//                return RedirectToAction("Index", "ExerciseEditor");
//            }
//            catch (MembershipCreateUserException e)
//            {
//                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
//            }

//            return View(model);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Disassociate(string provider, string providerUserId)
//        {
//            var ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
//            ManageMessageId? message = null;

//            // Only disassociate the account if the currently logged in user is the owner
//            if (ownerAccount != User.Identity.Name)
//            {
//                return RedirectToAction("Manage", new {Message = message});
//            }
//            // Use a transaction to prevent the user from deleting their last login credential
//            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
//            {
//                var hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
//                if (!hasLocalAccount && OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count <= 1)
//                {
//                    return RedirectToAction("Manage", new {Message = message});
//                }
//                OAuthWebSecurity.DeleteAccount(provider, providerUserId);
//                scope.Complete();
//                message = ManageMessageId.RemoveLoginSuccess;
//            }

//            return RedirectToAction("Manage", new { Message = message });
//        }

//        public ActionResult Manage(ManageMessageId? message)
//        {
//            ViewBag.StatusMessage =
//                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
//                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
//                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
//                : "";
//            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
//            ViewBag.ReturnUrl = Url.Action("Manage");
//            return View();
//        }

//        [ValidateAntiForgeryToken]
//        [HttpPost]
//        public ActionResult Manage(LocalPasswordModel model)
//        {
//            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
//            ViewBag.HasLocalPassword = hasLocalAccount;
//            ViewBag.ReturnUrl = Url.Action("Manage");
//            if (hasLocalAccount)
//            {
//                if (ModelState.IsValid)
//                {
//                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
//                    bool changePasswordSucceeded;
//                    try
//                    {
//                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
//                    }
//                    catch (Exception)
//                    {
//                        changePasswordSucceeded = false;
//                    }

//                    if (changePasswordSucceeded)
//                    {
//                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
//                    }
//                    else
//                    {
//                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
//                    }
//                }
//            }
//            else
//            {
//                // User does not have a local password so remove any validation errors caused by a missing
//                // OldPassword field
//                ModelState state = ModelState["OldPassword"];
//                if (state != null)
//                {
//                    state.Errors.Clear();
//                }

//                if (ModelState.IsValid)
//                {
//                    try
//                    {
//                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
//                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
//                    }
//                    catch (Exception)
//                    {
//                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
//                    }
//                }
//            }

//            return View(model);
//        }

//        #region Helpers
//        private ActionResult RedirectToLocal(string returnUrl)
//        {
//            if (Url.IsLocalUrl(returnUrl))
//            {
//                return Redirect(returnUrl);
//            }
//            else
//            {
//                return RedirectToAction("Index", "ExerciseEditor");
//            }
//        }

//        public enum ManageMessageId
//        {
//            ChangePasswordSuccess,
//            SetPasswordSuccess,
//            RemoveLoginSuccess,
//        }

//        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
//        {
//            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
//            // a full list of status codes.
//            switch (createStatus)
//            {
//                case MembershipCreateStatus.DuplicateUserName:
//                    return "User name already exists. Please enter a different user name.";

//                case MembershipCreateStatus.DuplicateEmail:
//                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

//                case MembershipCreateStatus.InvalidPassword:
//                    return "The password provided is invalid. Please enter a valid password value.";

//                case MembershipCreateStatus.InvalidEmail:
//                    return "The e-mail address provided is invalid. Please check the value and try again.";

//                case MembershipCreateStatus.InvalidAnswer:
//                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

//                case MembershipCreateStatus.InvalidQuestion:
//                    return "The password retrieval question provided is invalid. Please check the value and try again.";

//                case MembershipCreateStatus.InvalidUserName:
//                    return "The user name provided is invalid. Please check the value and try again.";

//                case MembershipCreateStatus.ProviderError:
//                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

//                case MembershipCreateStatus.UserRejected:
//                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

//                default:
//                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
//            }
//        }
//        #endregion
//    }
//}
