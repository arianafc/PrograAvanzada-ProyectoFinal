using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class DashboardController : Controller
    {
        [HttpGet]
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}