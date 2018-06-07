using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace MRS_web.Controllers
{
    public class HomeController : Controller
    {
        private Models.DataManager _DataManager;

        public HomeController(Models.DataManager DM)
        {
            _DataManager = DM;
        }

        [AcceptVerbs((HttpVerbs.Get))]
        public ActionResult SignIn(bool signOut = false)
        {
            HttpCookie aCookie = Request.Cookies["userInfo"];
            
            if (signOut)
            {
                Session.Remove("User");

                if (aCookie != null)
                {
                    aCookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(aCookie);
                }
            }

            if (aCookie != null && aCookie.Expires >= DateTime.Now)
                return SignIn(aCookie.Values["UserLogin"], aCookie.Values["UserPass"],"on"); 

            return View();
        }

        [AcceptVerbs((HttpVerbs.Post))]
        public ActionResult SignIn(string Login, string Password, string SaveMe)
        {
            bool Save = SaveMe != null && SaveMe == "on";

            if (string.IsNullOrEmpty(Login))
                ModelState.AddModelError("Login", "Логин не заполнен");

            if (string.IsNullOrEmpty(Password))
                ModelState.AddModelError("Password", "Пароль не заполнен");

            if (!ModelState.IsValid) return View();

            Models.EDM.User user = _DataManager.UserRepo.GetUser(Login);

            if (user == null)
                ModelState.AddModelError("UserLogin", "Пользователь не найден");

            if (!ModelState.IsValid) return View();

            if (user?.Password != Password)
                ModelState.AddModelError("UserPassword", "Пароль введён неверно");

            if (ModelState.IsValid)
            {
                Session["User"] = user;

                HttpCookie aCookie = Request.Cookies["userInfo"];

                if ((aCookie == null || aCookie.Expires < DateTime.Now) && Save)
                {
                    aCookie = new HttpCookie("userInfo");
                    aCookie.Values["UserLogin"] = user.Login;
                    aCookie.Values["UserPass"] = user.Password;
                    aCookie.Values["LastVisit"] = DateTime.Now.ToString();
                    aCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(aCookie);
                }

                if (user.AdminPrivileges)
                    return RedirectToAction("Index", "Admin");

                return RedirectToAction("Index", "Admin");
            }
            
            return View();
        }
    }
}