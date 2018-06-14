using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using MRS_web.Models;
using MRS_web.Models.EDM;

namespace MRS_web.Controllers
{
    [AuthorizeUser]
    public class DatabaseController : Controller
    {
        DataManager _DataManager;

        public DatabaseController(DataManager dm)
        {
            _DataManager = dm;
        }

        public ActionResult MetersList(string userLogin="")
        {
            ViewData["Meters"] = ((User)Session["User"]).AdminPrivileges? _DataManager.MetRepo.Meters(): ((User)Session["User"]).Meters;
            if (((User) Session["User"]).AdminPrivileges && !userLogin.IsNullOrWhiteSpace())
                ViewData["Meters"] = _DataManager.UserRepo.GetUser(userLogin)?.Meters;

            return View();
        }

        public ActionResult Meter(long MeterId)
        {
            User user = (User) Session["User"];
            Meter met = _DataManager.MetRepo.GetMeter(MeterId);

            if (!user.AdminPrivileges && met.User != user)
                return View("Error");

            ViewData["MeterName"] = met.Name;
            ViewData["Meter"] = met;

            return View();
        }

        public ActionResult UserList()
        {
            ViewData["Users"] = _DataManager.UserRepo.Users();

            return View();
        }

        public ActionResult UserInfo(string UserLogin)
        {
            ViewData["User"] = _DataManager.UserRepo.GetUser(UserLogin);

            return View();
        }

        public ActionResult Tariffes()
        {
            ViewData["Tariffes"] = _DataManager.TarRepo.Tariffs();

            return View();
        }

        public ActionResult TimeSpans(int TariffId)
        {
            Tariff tar = _DataManager.TarRepo.GetTariff(TariffId);

            ViewData["TariffId"] = TariffId;
            ViewData["TariffName"] = tar.Name;
            ViewData["TimeSpans"] = tar.TimeSpans;

            return View();
        }

        public ActionResult Types()
        {
            ViewData["Types"] = _DataManager.TypeRepo.Types();

            return View();
        }

        public ActionResult ParametersList()
        {
            ViewData["Parameters"] = _DataManager.ParRepo.Parametrs();

            return View();
        }

        public ActionResult Parameters(long MeterId)
        {
            User user = (User)Session["User"];
            Meter met = _DataManager.MetRepo.GetMeter(MeterId);

            if (!user.AdminPrivileges && met.User != user)
                return View("Error");

            ViewData["MeterName"] = met.Name;
            ViewData["Parameters"] = met.Parametrs;

            return View();
        }

        public ActionResult Readings(long MeterId)
        {
            User user = (User)Session["User"];
            Meter met = _DataManager.MetRepo.GetMeter(MeterId);

            if (!user.AdminPrivileges && met.User != user)
                return View("Error");

            ViewData["MeterName"] = met.Name;
            ViewData["ReadingsUnit"] = met.Type.Unit;
            ViewData["Readings"] = met.Readings;

            return View();
        }

        public ActionResult Documents(long MeterId)
        {
            User user = (User)Session["User"];
            Meter met = _DataManager.MetRepo.GetMeter(MeterId);

            if (!user.AdminPrivileges && met.User != user)
                return View("Error");

            ViewData["MeterName"] = met.Name;
            ViewData["Documents"] = met.Documents;

            return View();
        }
    }
}