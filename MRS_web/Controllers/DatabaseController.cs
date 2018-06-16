using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using MainLib;
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
        private void ExportResponce(string fileName)
        {
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", $"attachment; filename={fileName}");
            Response.TransmitFile(Server.MapPath($"~/Output_Excel/{fileName}"));
            Response.End();
        }

        public ActionResult MetersList(string userLogin="")
        {
            ViewData["Meters"] = ((User)Session["User"]).AdminPrivileges? _DataManager.MetRepo.Meters(): ((User)Session["User"]).Meters;
            if (((User) Session["User"]).AdminPrivileges && !userLogin.IsNullOrWhiteSpace())
                ViewData["Meters"] = _DataManager.UserRepo.GetUser(userLogin)?.Meters;

            ViewData["UserLogin"] = userLogin;

            return View();
        }
        public void ExportMeterList(string userLogin = "")
        {
            List<Meter> list = (((User)Session["User"]).AdminPrivileges ? _DataManager.MetRepo.Meters() : ((User)Session["User"]).Meters).ToList();
            if (((User)Session["User"]).AdminPrivileges && !userLogin.IsNullOrWhiteSpace())
                list = _DataManager.UserRepo.GetUser(userLogin)?.Meters.ToList();

            string filename = "meterList";

            DataManager.ExportToExcel(filename, DataManager.GetDataTables(list));

            ExportResponce(filename);
        }

        public ActionResult Meter(long MeterId)
        {
            User user = (User) Session["User"];
            Meter met = _DataManager.MetRepo.GetMeter(MeterId);

            if (!user.AdminPrivileges && met.User.Login != user.Login)
                return View("Error");

            ViewData["MeterName"] = met.Name;
            ViewData["Meter"] = met;

            return View();
        }
        public void ExportMeter(long MeterId)
        {
            User user = (User)Session["User"];
            Meter met = _DataManager.MetRepo.GetMeter(MeterId);

            if (!user.AdminPrivileges && met.User.Login != user.Login)
                throw new MemberAccessException();

            string filename = "meter";

            DataManager.ExportToExcel(filename, DataManager.GetDataTables(new []{met}, met.Documents, met.Readings,met.Parametrs));

            ExportResponce(filename);
        }

        public ActionResult UserList()
        {
            ViewData["Users"] = _DataManager.UserRepo.Users();

            return View();
        }
        public void ExportUserList()
        {
            string fileName = "userList";

            DataManager.ExportToExcel(fileName, DataManager.GetDataTables(_DataManager.UserRepo.Users())); 
            ExportResponce(fileName);
        }

        public ActionResult UserInfo(string UserLogin="")
        {
            if (UserLogin == null)
                return HttpNotFound();

            if (!UserLogin.IsNullOrWhiteSpace() && !(Session["User"] as User).AdminPrivileges)
                return HttpNotFound();

            ViewData["User"] = UserLogin.IsNullOrWhiteSpace()? Session["User"] : _DataManager.UserRepo.GetUser(UserLogin);

            return View();
        }
        public void ExportUser(string UserLogin = "")
        {
            if (UserLogin == null)
                throw new MemberAccessException();

            if (!UserLogin.IsNullOrWhiteSpace() && !(Session["User"] as User).AdminPrivileges)
                throw new MemberAccessException();

            User us = UserLogin.IsNullOrWhiteSpace() ? Session["User"] as User : _DataManager.UserRepo.GetUser(UserLogin);

            string fileName = "user";
            DataManager.ExportToExcel(fileName, DataManager.GetDataTables(new []{us},us?.Meters));
            ExportResponce(fileName);
        }

        public ActionResult Tariffes()
        {
            ViewData["Tariffes"] = _DataManager.TarRepo.Tariffs();

            return View();
        }
        public void ExportTariffList()
        {
            string fileName = "tariffList";
            DataManager.ExportToExcel(fileName, DataManager.GetDataTables(_DataManager.TarRepo.Tariffs()));
            ExportResponce(fileName);
        }

        public ActionResult TimeSpans(int TariffId)
        {
            Tariff tar = _DataManager.TarRepo.GetTariff(TariffId);

            ViewData["TariffId"] = TariffId;
            ViewData["TariffName"] = tar.Name;
            ViewData["TimeSpans"] = tar.TimeSpans;

            return View();
        }
        public void ExportTimeSpanList(int TariffId)
        {
            Tariff tar = _DataManager.TarRepo.GetTariff(TariffId);

            string fileName = "timespanList";
            DataManager.ExportToExcel(fileName, DataManager.GetDataTables(new []{tar}));
            ExportResponce(fileName);
        }

        public ActionResult TypeList()
        {
            ViewData["Types"] = _DataManager.TypeRepo.Types();

            return View();
        }
        public void ExportTypesList()
        {
            string fileName = "typeList";
            DataManager.ExportToExcel(fileName, DataManager.GetDataTables(_DataManager.TypeRepo.Types()));
            ExportResponce(fileName);
        }

        public ActionResult ParameterList()
        {
            ViewData["Parameters"] = _DataManager.ParRepo.Parametrs();

            return View();
        }
        public void ExportParameterList()
        {
            string fileName = "parametrList";
            DataManager.ExportToExcel(fileName, DataManager.GetDataTables(_DataManager.ParRepo.Parametrs()));
            ExportResponce(fileName);
        }

        public ActionResult Parameters(long MeterId)
        {
            User user = (User)Session["User"];
            Meter met = _DataManager.MetRepo.GetMeter(MeterId);

            if (!user.AdminPrivileges && met.User.Login != user.Login)
                return View("Error");

            ViewData["MeterName"] = met.Name;
            ViewData["Parameters"] = met.Parametrs;

            return View();
        }
        public void ExportParameters(long MeterId)
        {
            User user = (User)Session["User"];
            Meter met = _DataManager.MetRepo.GetMeter(MeterId);

            if (!user.AdminPrivileges && met.User.Login != user.Login)
                throw new MemberAccessException();

            string fileName = "parameters";
            DataManager.ExportToExcel(fileName,DataManager.GetDataTables(met.Parametrs));
            ExportResponce(fileName);
        }

        public ActionResult Readings(long MeterId)
        {
            User user = (User)Session["User"];
            Meter met = _DataManager.MetRepo.GetMeter(MeterId);

            if (!user.AdminPrivileges && met.User.Login != user.Login)
                return View("Error");

            ViewData["MeterName"] = met.Name;
            ViewData["ReadingsUnit"] = met.Type.Unit;
            ViewData["Readings"] = met.Readings;

            return View();
        }
        public void ExportReadings(long MeterId)
        {
            User user = (User)Session["User"];
            Meter met = _DataManager.MetRepo.GetMeter(MeterId);

            if (!user.AdminPrivileges && met.User.Login != user.Login)
                throw new MemberAccessException();

            string fileName = "readings";
            DataManager.ExportToExcel(fileName, DataManager.GetDataTables(met.Readings));
            ExportResponce(fileName);
        }

        public ActionResult DocumentList(long MeterId)
        {
            User user = (User)Session["User"];
            Meter met = _DataManager.MetRepo.GetMeter(MeterId);

            if (!user.AdminPrivileges && met.User.Login != user.Login)
                return View("Error");

            ViewData["MeterName"] = met.Name;
            ViewData["Documents"] = met.Documents;

            return View();
        }
        public void ExportDocuments(long MeterId)
        {
            User user = (User)Session["User"];
            Meter met = _DataManager.MetRepo.GetMeter(MeterId);

            if (!user.AdminPrivileges && met.User.Login != user.Login)
                throw new MemberAccessException();

            string fileName = "documents";
            DataManager.ExportToExcel(fileName, DataManager.GetDataTables(met.Documents));
            ExportResponce(fileName);
        }
    }
}