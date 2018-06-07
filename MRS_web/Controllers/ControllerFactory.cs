using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MRS_web.Controllers
{
    public class ControllerFactory: DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return Activator.CreateInstance(typeof(HomeController), new Models.DataManager()) as IController;

            return Activator.CreateInstance(controllerType, new Models.DataManager()) as IController;
        }
    }
}