﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI.WebControls;
using MRS_web.Models.EDM;
using WebGrease.Css.Extensions;

namespace MRS_web.Controllers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MultiPostAttribute : ActionNameSelectorAttribute
    {
        public string NameOfAttributes { get; set; }
        public int countAttribute  { get; set; } 
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            if (controllerContext.HttpContext.Request[NameOfAttributes] == null)
                return false;

            var g = controllerContext.HttpContext.Request[NameOfAttributes].Split(',');

            return g.Length == countAttribute;
        }
    }

    [AuthorizeUser]
    public class AdminController : Controller
    {
        private Models.DataManager _DataManager;

        public AdminController(Models.DataManager DM)
        {
            _DataManager = DM;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DataBase()
        {
            return View();
        }

        public ActionResult Addmeter()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ExtendMeter()
        {
            ViewData["UsersList"] = _DataManager.UserRepo.Users();
            if (ViewData["SelectedUserLogin"] == null)
                ViewData["SelectedUserLogin"] = _DataManager.UserRepo.Users().First().Login;

            ViewData["MetersList"] = _DataManager.UserRepo.GetUser(ViewData["SelectedUserLogin"].ToString()).Meters;


            if(ViewData["SelectedMeterId"] ==null )
                ViewData["SelectedMeterId"] = ((IEnumerable<Meter>)ViewData["MetersList"]).First().ProductionId;

            Meter met =_DataManager.MetRepo.GetMeter((long)ViewData["SelectedMeterId"]) ;

            ViewData["Meter"] = met;
            ViewData["MeterName"] = met.Name;

            return View();
        }
        
        // UserCombobox
        public ActionResult ExtendMeter(string[] action)
        {
            ViewData["SelectedUserLogin"] = action[0];

            return ExtendMeter();
        }

        // UserCombobox || MeterCombobox
        [HttpPost]
        [MultiPost(countAttribute = 2, NameOfAttributes = "action")]
        public ActionResult ExtendMeter(string[] action, object notUsed)
        {
            ViewData["SelectedUserLogin"] = action[0];

            if (long.TryParse(action[1], out long prodId))
            {
                if (_DataManager.MetRepo.GetMeter(prodId).User.Login != action[0])
                    return ExtendMeter(new[] {action[0]});

                ViewData["SelectedMeterId"] = prodId;
            }

            return ExtendMeter();
        }

        // NewDateButton
        [HttpPost]
        [MultiPost(countAttribute = 3, NameOfAttributes = "action")]
        public ActionResult ExtendMeter(string[] action, DateTime? InputDate)
        {
            ViewData["NewDate"] = InputDate?.ToString("yyyy-MM-dd");

            if (!long.TryParse(action[1], out long prodId))
                return ExtendMeter();

            if (InputDate != null)
            {
                if (InputDate <= ((InstalledMeter)_DataManager.MetRepo.GetMeter(prodId)).ExpirationDate || InputDate <= DateTime.Now)
                    ModelState.AddModelError("Date", "Дата не должна быть меньше или равна текущей дате или дате следующей проверки");
            }
            else ModelState.AddModelError("Date", "Заполните дату");

            if (!ModelState.IsValid)
                return ExtendMeter(new []{action[0], action[1]}, notUsed:null);

            return RedirectToAction("Index");
        }

        public ActionResult DeleteMeter()
        {
            return View();
        }
    }
}