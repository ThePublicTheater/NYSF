using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using Joes_Pub_MVC_4.Filters;
using Joes_Pub_MVC_4.Models;
using PagedList;
using System.Data;
namespace Joes_Pub_MVC_4.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                if (Nysf.Tessitura.WebClient.Login(model.UserName, model.Password))
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    WebSecurity.Logout();
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            if(Nysf.Tessitura.WebClient.IsLoggedIn())
                Nysf.Tessitura.WebClient.Logout();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Nysf.Tessitura.WebClient.MaintainTessSession();
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.Login(model.UserName, model.Password);
                    ProfilesDBContext ProfileDB = new ProfilesDBContext();
                    Profiles NewProfile = new Profiles();
                    NewProfile.UserName = model.UserName;
                    NewProfile.NyMag = model.NyMagSub;
                    NewProfile.Bio = model.Bio;
                    NewProfile.EmailSub = model.EmailSub;
                    NewProfile.FirstName = model.FirstName;
                    NewProfile.LastName = model.LastName;
                    NewProfile.GenresSerialized = model.Genres;
                    ProfileDB.ProfileList.Add(NewProfile);
                    ProfileDB.SaveChanges();
                    if (Nysf.Tessitura.WebClient.RegisterNewConstituent(model.UserName, model.FirstName, model.LastName,  92, Nysf.Types.Organization.JoesPub))
                    {
                        bool test = Nysf.Tessitura.WebClient.Login(model.UserName, model.Password);
                        if (Nysf.Tessitura.WebClient.SetNewPassword(model.Password))
                        {
                            Nysf.Tessitura.WebClient.Login(model.UserName, model.Password);
                        }
                    }

                    return RedirectToAction("Manage");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }
        
        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            Models.ProfileViewModel ViewMod = new ProfileViewModel(User.Identity.Name);
            if (ViewMod.needscaching)
            {
                List<string> temp;
                ViewMod.CurUserCache.Username = ViewMod.CurUser.UserName;
                ViewMod.CurUserCache.CachedDate = DateTime.Now;
                ViewMod.CurUserCache.DeleteDate = DateTime.Now.AddDays(14);
                ViewMod.CurUserCache.playercache = ControllerContext.RenderPartialViewToString("~/Views/Shared/_MusicPlayer.cshtml", ViewMod.SuggestedSongs);

                temp = new List<string>();
                for (int i = 1; i < ViewMod.NewsCommPages.PageCount; i++)
                {
                    temp.Add(ControllerContext.RenderPartialViewToString("~/Views/Account/_NewsComms.cshtml", ViewMod.NewsComms.ToPagedList(i, 5)));
                }
                ViewMod.CurUserCache.newscommentcache = temp;

                temp = new List<string>();
                for (int i = 1; i < ViewMod.ShowCommPages.PageCount; i++)
                {
                    temp.Add(ControllerContext.RenderPartialViewToString("~/Views/Account/_ShowComms.cshtml", ViewMod.ShowComms.ToPagedList(i, 5)));
                }
                ViewMod.CurUserCache.showcommentcache = temp;

                temp = new List<string>();
                for (int i = 1; i < ViewMod.ArtistCommPages.PageCount; i++)
                {
                    temp.Add(ControllerContext.RenderPartialViewToString("~/Views/Account/_ArtistComms.cshtml", ViewMod.ArtistComms.ToPagedList(i, 5)));
                }
                ViewMod.CurUserCache.artistcommentcache = temp;

                temp = new List<string>();
                for (int i = 1; i < ViewMod.GeneralCommPages.PageCount; i++)
                {
                    temp.Add(ControllerContext.RenderPartialViewToString("~/Views/Account/_GeneralComms.cshtml", ViewMod.GeneralComms.ToPagedList(i, 5)));
                }
                ViewMod.CurUserCache.generalcommentcache = temp;

                temp = new List<string>();
                for (int i = 1; i < ViewMod.SuggestedShowsPages.PageCount; i++)
                {
                    temp.Add(ControllerContext.RenderPartialViewToString("~/Views/Account/_SuggestedShows.cshtml", ViewMod.SuggestedShows.ToPagedList(i, 5)));
                }
                ViewMod.CurUserCache.sshowscache = temp;

                temp = new List<string>();
                for (int i = 1; i < ViewMod.SuggestedArtistsPages.PageCount; i++)
                {
                    temp.Add(ControllerContext.RenderPartialViewToString("~/Views/Account/_SuggestedArtists.cshtml", ViewMod.SuggestedArtists.ToPagedList(i, 5)));
                }
                ViewMod.CurUserCache.sartistcache = temp;

                Utilities.CacheDB.ProfileCaches.Add(ViewMod.CurUserCache);
                Utilities.CacheDB.SaveChanges();
            }
            ViewMod.strings[0] = ViewMod.CurUserCache.playercache;
            ViewMod.strings[1] = ViewMod.CurUserCache.sshowscache[0];
            ViewMod.strings[2] = ViewMod.CurUserCache.sartistcache[0];
            ViewMod.strings[3] = ViewMod.CurUserCache.newscommentcache[0];
            ViewMod.strings[4] = ViewMod.CurUserCache.showcommentcache[0];
            ViewMod.strings[5] = ViewMod.CurUserCache.artistcommentcache[0];
            ViewMod.strings[6] = ViewMod.CurUserCache.generalcommentcache[0];
            return View(ViewMod);
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }
                    try
                    {
                        changePasswordSucceeded = Nysf.Tessitura.WebClient.SetNewPassword(model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        if(!Nysf.Tessitura.WebClient.LoginExists(model.UserName))
                        {
                            if (!Nysf.Tessitura.WebClient.RegisterNewConstituent(model.UserName, model.Fname, model.Lname, 92, Nysf.Types.Organization.JoesPub))
                            {
                                ModelState.AddModelError("UserName", "There was an issue registering your account please try again later.");
                                OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
                                ViewBag.ReturnUrl = returnUrl;
                                return View(model);
                            }
                        }
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);
                        ProfilesDBContext ProfileDB = new ProfilesDBContext();
                        Profiles prof = new Profiles();
                        prof.UserName = model.UserName;
                        prof.Bio = model.Bio;
                        prof.FirstName = model.Fname;
                        prof.LastName = model.Lname;
                        prof.EmailSub = model.Email;
                        prof.NyMag = model.Nymag;
                        string[] l = model.genrestring.Split(';');
                        prof.GenreIDs = new List<int>();
                        foreach (var item in l)
                        {
                            int temp = new int();
                            if (int.TryParse(item, out temp))
                            {
                                prof.GenreIDs.Add(temp);
                            }
                        }
                        ProfileDB.ProfileList.Add(prof);
                        ProfileDB.SaveChanges();
                        return RedirectToAction("Manage");
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }


        public string ChangeSSpage(int topage)
        {
            Models.ProfileViewModel Mod = new Models.ProfileViewModel(User.Identity.Name);
            return Mod.CurUserCache.sshowscache[topage - 1];
        }

        public string ChangeSApage(int topage)
        {
            Models.ProfileViewModel Mod = new Models.ProfileViewModel(User.Identity.Name);
            return Mod.CurUserCache.sartistcache[topage - 1];
        }
        public string ChangeNCpage(int topage)
        {
            Models.ProfileViewModel Mod = new Models.ProfileViewModel(User.Identity.Name);
            return Mod.CurUserCache.newscommentcache[topage - 1];
        }
        public string ChangeSCpage(int topage)
        {
            Models.ProfileViewModel Mod = new Models.ProfileViewModel(User.Identity.Name);
            return Mod.CurUserCache.showcommentcache[topage - 1];
        }
        public string ChangeACpage(int topage)
        {
            Models.ProfileViewModel Mod = new Models.ProfileViewModel(User.Identity.Name);
            return Mod.CurUserCache.artistcommentcache[topage - 1];
        }
        public string ChangeGCpage(int topage)
        {
            Models.ProfileViewModel Mod = new Models.ProfileViewModel(User.Identity.Name);
            return Mod.CurUserCache.generalcommentcache[topage - 1];
        }
      

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
